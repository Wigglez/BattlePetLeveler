#region Information
/* BotBase created by AknA and Wigglez */

#endregion

#region Namespaces

using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using BattlePetLeveler.Convenience;
using BattlePetLeveler.GUI;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using ProfileManager = Styx.CommonBot.Profiles.ProfileManager;

#endregion

namespace BattlePetLeveler {
    #region OBJECTIVE
    /////////////////////////////////////////////////////////////////////////////
    // OBJECTIVE - Player Leveling using Battle Pet System
    // 2 accounts, 2 characters
    // both accounts have a certain set of battle pets (25-1-1)
    // character to level always wins battle, character that helps always loses
    // both characters queue up for battle
    // when in battle, each person swaps from lvl 25 pet to lvl 1
    // 60 seconds elapsed time during battle
    // loser forfeits, winner gains exp
    // Both queue again after battle
    /////////////////////////////////////////////////////////////////////////////

    #endregion

    public class BattlePetLeveler : BotBase {
        #region Constants
        // ===========================================================
        // Constants
        // ===========================================================

        public const int ThrottleTimerCount = 4;

        #endregion

        #region Fields
        // ===========================================================
        // Fields
        // ===========================================================

        private static int _treeLogicStep;

        private static readonly Stopwatch PulseTimerStopwatch = new Stopwatch();
        private static readonly Stopwatch LeaveQueueTimerStopwatch = new Stopwatch();
        private static readonly Stopwatch LoserForfeitTimerStopwatch = new Stopwatch();
        private static readonly Stopwatch WinnerForfeitTimerStopwatch = new Stopwatch();
        private static readonly Stopwatch RequeueTimerStopwatch = new Stopwatch();

        private static ThrottleTimer[] _throttleTimers = new ThrottleTimer[ThrottleTimerCount];

        public const string LeaveQueueTimerString = "Leave queue timer";
        public const string LoserForfeitTimerString = "Loser forfeit timer";
        public const string WinnerForfeitTimerString = "Winner forfeit timer";
        public const string RequeueTimerString = "Requeue timer";

        private static int _characterCurrentXp;
        private static int _characterLastXp;

        private static readonly int MyPetXp = GetPetCurrentXp(1, 1);
        private static readonly int EnemyPetXp = GetPetCurrentXp(2, 1);
        private static readonly int MyPetLevel = GetPetLevel(1, 1);
        private static readonly int EnemyPetLevel = GetPetLevel(2, 1);

        private Composite _root;

        #endregion

        #region Constructors
        // ===========================================================
        // Constructors
        // ===========================================================

        #endregion

        #region Getter & Setter
        // ===========================================================
        // Getter & Setter
        // ===========================================================

        public static string BPLStatusText { get; set; }

        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static ThrottleTimer[] ThrottleTimers {
            get { return _throttleTimers; }
            set { _throttleTimers = value; }
        }

        #endregion

        #region Methods for/from SuperClass/Interfaces
        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // The name of the BotBase
        public override string Name { get { return "BattlePetLeveler"; } }

        // Only used in botbases
        public override PulseFlags PulseFlags { get { return PulseFlags.All; } }

        public override Composite Root { get { return _root ?? (_root = CreateRoot()); } }

        public override Form ConfigurationForm { get { return new BattlePetLevelerGUI(); } }

        public override void Start() {
            ProfileManager.LoadEmpty();

            _treeLogicStep = 0;

            BPLStatusText = "{TimerName}: {TimeRemaining} ({TimeDuration})";

            for(var i = 0; i < ThrottleTimerCount; i++) {
                _throttleTimers[i] = new ThrottleTimer("", 0);
            }

            _throttleTimers[0].TimerName = LeaveQueueTimerString;
            _throttleTimers[1].TimerName = LoserForfeitTimerString;
            _throttleTimers[2].TimerName = WinnerForfeitTimerString;
            _throttleTimers[3].TimerName = RequeueTimerString;

            BPLLog("Initialization complete.");
        }

        public override void Stop() {
            TreeRoot.GoalText = string.Empty;
            TreeRoot.StatusText = string.Empty;

            // Clear the throttle timers
            foreach(var t in _throttleTimers) {
                t.TimerName = "";
                t.Time = 0;
            }

            LeaveQueueCommand();

            BPLLog("Shutdown complete.");
        }

        #endregion

        #region Methods
        // ===========================================================
        // Methods
        // ===========================================================

        #region Convenience

        public static void BPLLog(string message, params object[] args) {
            Logging.Write(Colors.DeepSkyBlue, "[BPL]: " + message, args);
        }

        #endregion

        #region Queue Handling

        public static bool IsQueuable() {
            // Are we doing something that can stop us from accepting queue?
            if(!Me.IsValid) {
                BPLLog("Not able to accept queue due to being invalid (not in world).");
                return false;
            }

            if(Me.InVehicle || Me.IsOnTransport) {
                BPLLog("Not able to accept queue due to being in a vehicle or on a transport.");
                return false;
            }

            if(Me.IsGhost || Me.IsDead) {
                BPLLog("Not able to accept queue due to being a ghost or dead.");
                return false;
            }

            if(Me.Combat || Me.IsActuallyInCombat) {
                BPLLog("Not able to accept queue due to being in combat.");
                return false;
            }

            // Get any wow unit (if I have aggro or my pet has aggro)
            if(ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Any(unit => unit.Aggro || unit.PetAggro)) {
                BPLLog("Not able to accept queue due to aggro.");
                return false;
            }

            return true;
        }

        public static bool IsQueuePopped() {
            return Lua.GetReturnVal<bool>("return select(1, C_PetBattles.GetPVPMatchmakingInfo()) == 'proposal'", 0);
        }

        public static bool IsInQueue() {
            return Lua.GetReturnVal<bool>("return select(1, C_PetBattles.GetPVPMatchmakingInfo()) == 'queued'", 0);
        }

        public static void LeaveQueueCommand() {
            Lua.DoString("C_PetBattles.StopPVPMatchmaking()");
        }

        public static void LeaveQueue() {
            // 3-5 min timer to wait now that we are queued
            if(!LeaveQueueTimerStopwatch.IsRunning) {
                ThrottleTimer.CreateThrottleTimer(LeaveQueueTimerStopwatch, 180000, 300000, LeaveQueueTimerString);
            } else {
                ThrottleTimer.CheckThrottleTimer(LeaveQueueTimerStopwatch, _throttleTimers[0].Time, LeaveQueueTimerString);
            }
        }

        public static void LoserForfeit() {
            // 61-65 sec timer to wait now that the battle has started
            if(!LoserForfeitTimerStopwatch.IsRunning) {
                ThrottleTimer.CreateThrottleTimer(LoserForfeitTimerStopwatch, 61000, 65000, LoserForfeitTimerString);
            } else {
                ThrottleTimer.CheckThrottleTimer(LoserForfeitTimerStopwatch, _throttleTimers[1].Time, LoserForfeitTimerString);
            }
        }

        public static void WinnerForfeit() {
            // 80-85 sec timer to wait now that the battle has started
            if(!WinnerForfeitTimerStopwatch.IsRunning) {
                ThrottleTimer.CreateThrottleTimer(WinnerForfeitTimerStopwatch, 80000, 85000, WinnerForfeitTimerString);
            } else {
                ThrottleTimer.CheckThrottleTimer(WinnerForfeitTimerStopwatch, _throttleTimers[2].Time, WinnerForfeitTimerString);
            }
        }

        public static void Requeue() {
            // 8-12 sec timer to wait now that the battle has ended
            if(!RequeueTimerStopwatch.IsRunning) {
                ThrottleTimer.CreateThrottleTimer(RequeueTimerStopwatch, 8000, 12000, RequeueTimerString);
            } else {
                ThrottleTimer.CheckThrottleTimer(RequeueTimerStopwatch, _throttleTimers[3].Time, RequeueTimerString);
            }
        }

        #endregion

        #region Pet Battle

        public static bool ShowPetFrame() {
            var petSelectFrame = Lua.GetReturnVal<bool>("return C_PetBattles.ShouldShowPetSelect()", 0);
            return petSelectFrame;
        }

        public static void CheckAndChangePet() {
            // Check if the pet is usable
            if(PetUsable(1)) {
                // Change pet to the first pet
                ChangePetCommand(1);
            }
        }

        public static bool IsInPetBattle() {
            var inPetBattle = Lua.GetReturnVal<bool>("return C_PetBattles.IsInBattle()", 0);
            return inPetBattle;
        }

        public static bool PetUsable(int pPetIndex) {
            var usable = Lua.GetReturnVal<bool>(string.Format("return C_PetBattles.CanPetSwapIn({0})", pPetIndex), 0);
            return usable;
        }

        public static int GetPetLevel(int pPetOwner, int pPetIndex) {
            /*
            petOwner - 1: Current player, 2: Opponent (number)
            petIndex - Accepted values are 1-3, but the order is based off of the initial order. (number)
            
            Returns:
            level - The level of the pet (number) */
            var level = Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetLevel({0}, {1})", pPetOwner, pPetIndex), 0);
            return level;
        }

        public static int GetCharacterCurrentXp() {
            // XP = UnitXP("player")
            var xp = Lua.GetReturnVal<int>("return UnitXP('player')", 0);
            return xp;
        }

        public static int GetPetCurrentXp(int pPetOwner, int pIndex) {
            // xp, maxXP = C_PetBattles.GetXP(owner, index)
            var xp = Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetXP({0}, {1})", pPetOwner, pIndex), 0);
            return xp;
        }

        public static void ChangePetCommand(int pPetIndex) {
            Lua.DoString(string.Format("C_PetBattles.ChangePet({0})", pPetIndex));
        }

        public static bool WaitingOnOpponent() {
            var waiting = Lua.GetReturnVal<bool>("return C_PetBattles.IsWaitingOnOpponent()", 0);
            return waiting;
        }

        public static void ForfeitCommand() {
            Lua.DoString("C_PetBattles.ForfeitGame()");
        }

        #endregion

        #endregion

        #region Inner and Anonymous Classes
        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        #region PriorityTree

        #region LoserPriorityTree

        private static void LoserPriorityTree() {
            switch(_treeLogicStep) {
                // Not queued state
                case 0:
                    // If we are not in queue, we are queuable, and our pulse timer is active
                    if(!IsInQueue()) {
                        if(IsQueuable() && ThrottleTimer.CheckThrottleTimer(PulseTimerStopwatch, 1000, "not_create_pulse")) {
                            // We queue up
                            BPLLog("Queuing.");
                            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");

                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create the queue timer
                            LeaveQueue();
                        }
                    }
                        // Otherwise if we are in the queue, move on
                    else {
                        // Move on to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Queued state
                case 1:
                    // If the queue did not pop
                    if(!IsQueuePopped()) {
                        // If the queue timer is running
                        if(LeaveQueueTimerStopwatch.IsRunning) {
                            // Check the queue timer
                            LeaveQueue();
                        } else {
                            // Otherwise if the queue timer isn't running (it expired)
                            // Leave the queue
                            BPLLog("Left queue due to long queue time.");

                            LeaveQueueCommand();

                            // Start over and requeue
                            _treeLogicStep = 0;
                        }
                    }
                        // Otherwise if the queue did pop
                    else {
                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Proposal state
                case 2:
                    // Check for proposal
                    if(IsQueuePopped()) {
                        // If we still need to accept the proposal
                        if(ThrottleTimer.CheckThrottleTimer(PulseTimerStopwatch, RandomNumber.generateRandomInt(1500, 3000),
                            "not_create_pulse")) {
                            // Then we accept the queue after a random amount of time (1.5 - 3 sec)
                            Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch()");
                            BPLLog("Accepted queue.");
                        }
                    }

                    if(IsInPetBattle()) {
                        // Reset the queue timer
                        LeaveQueueTimerStopwatch.Reset();

                        if(ShowPetFrame()) {
                            // Check which pet is usable and change to one that can be active
                            CheckAndChangePet();
                        }

                        if(WaitingOnOpponent()) {
                            BPLLog("Waiting on opponent to choose a pet.");
                        } else {
                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create battle started timer
                            LoserForfeit();

                            // Move to the next step
                            _treeLogicStep++;
                        }
                    }

                    break;

                // Battle started state
                case 3:
                    if(ShowPetFrame()) {
                        // Check which pet is usable and change to one that can be active
                        CheckAndChangePet();
                    }

                    // If the battle started timer is still running
                    if(LoserForfeitTimerStopwatch.IsRunning) {
                        // Check the timer
                        LoserForfeit();
                    }
                        // Otherwise the timer has expired
                    else {
                        // Forfeit the match
                        BPLLog("Forfeiting.");
                        ForfeitCommand();

                        // Make sure we create a new timer
                        ThrottleTimer.WaitTimerCreated = false;

                        // Create the battle ended timer
                        Requeue();

                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Battle ended state
                case 4:
                    if(RequeueTimerStopwatch.IsRunning) {
                        Requeue();
                    } else {
                        _treeLogicStep = 0;
                    }

                    break;

                // Unused atm
                case 5:

                    break;
            }
        }

        #endregion

        #region WinnerPriorityTree

        private static void WinnerPriorityTree() {
            switch(_treeLogicStep) {
                // Not queued state
                case 0:
                    // If we are not in queue, we are queuable, and our pulse timer is active
                    if(!IsInQueue()) {
                        if(IsQueuable() && ThrottleTimer.CheckThrottleTimer(PulseTimerStopwatch, 1000, "not_create_pulse")) {
                            // We queue up
                            BPLLog("Queuing.");
                            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");

                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create the queue timer
                            LeaveQueue();
                        }
                    }
                        // Otherwise if we are in the queue, move on
                    else {
                        // Move on to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Queued state
                case 1:
                    // If the queue did not pop
                    if(!IsQueuePopped()) {
                        // If the queue timer is running
                        if(LeaveQueueTimerStopwatch.IsRunning) {
                            // Check the queue timer
                            LeaveQueue();
                        } else {
                            // Otherwise if the queue timer isn't running (it expired)
                            // Leave the queue
                            BPLLog("Left queue due to long queue time.");

                            LeaveQueueCommand();

                            // Start over and requeue
                            _treeLogicStep = 0;
                        }
                    }
                        // Otherwise if the queue did pop
                    else {
                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Proposal state
                case 2:
                    // Check for proposal
                    if(IsQueuePopped()) {
                        // If we still need to accept the proposal
                        if(ThrottleTimer.CheckThrottleTimer(PulseTimerStopwatch, RandomNumber.generateRandomInt(1500, 3000),
                            "not_create_pulse")) {
                            // Then we accept the queue after a random amount of time (1.5 - 3 sec)
                            Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch()");
                            BPLLog("Accepted queue.");
                        }
                    }

                    if(IsInPetBattle()) {
                        // Reset the queue timer
                        LeaveQueueTimerStopwatch.Reset();

                        if(ShowPetFrame()) {
                            // Check which pet is usable and change to one that can be active
                            CheckAndChangePet();
                        }

                        if(WaitingOnOpponent()) {
                            BPLLog("Waiting on opponent to choose a pet.");
                        } else {
                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create battle started timer
                            WinnerForfeit();

                            // Move to the next step
                            _treeLogicStep++;
                        }
                    }

                    break;

                // Battle started state
                case 3:
                    if(ShowPetFrame()) {
                        // Check which pet is usable and change to one that can be active
                        CheckAndChangePet();
                    }

                    // If the battle started timer is still running
                    if(WinnerForfeitTimerStopwatch.IsRunning) {
                        // Check the timer
                        WinnerForfeit();

                        // If the looser forfits
                        if(!IsInPetBattle()) {
                            WinnerForfeitTimerStopwatch.Reset();
                        }
                    }
                        // Otherwise the timer has expired
                    else {
                        if(IsInPetBattle()) {
                            // Forfeit the match
                            BPLLog("Forfeiting.");
                            ForfeitCommand();
                        }

                        // Make sure we create a new timer
                        ThrottleTimer.WaitTimerCreated = false;

                        // Create the battle ended timer
                        Requeue();

                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Battle ended state
                case 4:
                    if(RequeueTimerStopwatch.IsRunning) {
                        Requeue();
                    } else {
                        _treeLogicStep = 0;
                    }

                    break;

                // Unused atm
                case 5:

                    break;
            }
        }

        #endregion

        #region CharacterWinTradePriorityTree
        private static void CharacterWinTradePriorityTree() {
            switch(_treeLogicStep) {
                // Not queued state
                case 0:
                    // If we are not in queue, we are queuable, and our pulse timer is active
                    if(!IsInQueue()) {
                        if(IsQueuable() && ThrottleTimer.CheckThrottleTimer(PulseTimerStopwatch, 1000, "not_create_pulse")) {
                            // We queue up
                            BPLLog("Queuing.");
                            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");

                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create the queue timer
                            LeaveQueue();
                        }
                    }
                        // Otherwise if we are in the queue, move on
                    else {
                        // Move on to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Queued state
                case 1:
                    // If the queue did not pop
                    if(!IsQueuePopped()) {
                        // If the queue timer is running
                        if(LeaveQueueTimerStopwatch.IsRunning) {
                            // Check the queue timer
                            LeaveQueue();
                        } else {
                            // Otherwise if the queue timer isn't running (it expired)
                            // Leave the queue
                            BPLLog("Left queue due to long queue time.");

                            LeaveQueueCommand();

                            // Start over and requeue
                            _treeLogicStep = 0;
                        }
                    }
                        // Otherwise if the queue did pop
                    else {
                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Proposal state
                case 2:
                    // Check for proposal
                    if(IsQueuePopped()) {
                        // If we still need to accept the proposal
                        if(ThrottleTimer.CheckThrottleTimer(PulseTimerStopwatch, RandomNumber.generateRandomInt(1500, 3000),
                            "not_create_pulse")) {
                            // Then we accept the queue after a random amount of time (1.5 - 3 sec)
                            Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch()");
                            BPLLog("Accepted queue.");
                        }
                    }

                    if(IsInPetBattle()) {
                        // Reset the queue timer
                        LeaveQueueTimerStopwatch.Reset();

                        if(ShowPetFrame()) {
                            // Check which pet is usable and change to one that can be active
                            CheckAndChangePet();
                        }

                        if(WaitingOnOpponent()) {
                            BPLLog("Waiting on opponent to choose a pet.");
                        } else {
                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            _characterCurrentXp = GetCharacterCurrentXp();
                            if(_characterCurrentXp > _characterLastXp) {
                                // Create battle started timer
                                LoserForfeit();
                            } else {
                                // Create battle started timer
                                WinnerForfeit();
                            }


                            // Move to the next step
                            _treeLogicStep++;
                        }
                    }

                    break;

                // Battle started state
                case 3:
                    if(ShowPetFrame()) {
                        // Check which pet is usable and change to one that can be active
                        CheckAndChangePet();
                    }

                    // If the battle started timer is still running
                    if(WinnerForfeitTimerStopwatch.IsRunning || LoserForfeitTimerStopwatch.IsRunning) {
                        // Check the timers
                        if(WinnerForfeitTimerStopwatch.IsRunning) {
                            WinnerForfeit();
                        }
                        if(LoserForfeitTimerStopwatch.IsRunning) {
                            LoserForfeit();
                        }

                        // If the loser forfeits
                        if(!IsInPetBattle()) {
                            WinnerForfeitTimerStopwatch.Reset();
                            LoserForfeitTimerStopwatch.Reset();
                        }
                    }
                        // Otherwise the timer has expired
                    else {
                        if(IsInPetBattle()) {
                            // Forfeit the match
                            BPLLog("Forfeiting.");
                            ForfeitCommand();
                        }

                        // Make sure we create a new timer
                        ThrottleTimer.WaitTimerCreated = false;

                        _characterLastXp = _characterCurrentXp;

                        // Create the battle ended timer
                        Requeue();

                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Battle ended state
                case 4:
                    if(RequeueTimerStopwatch.IsRunning) {
                        Requeue();
                    } else {
                        _treeLogicStep = 0;
                    }

                    break;

                // Unused atm
                case 5:

                    break;
            }
        }

        #endregion

        #region PetWinTradePriorityTree
        private static void PetWinTradePriorityTree() {
            switch(_treeLogicStep) {
                // Not queued state
                case 0:
                    // If we are not in queue, we are queuable, and our pulse timer is active
                    if(!IsInQueue()) {
                        if(IsQueuable() && ThrottleTimer.CheckThrottleTimer(PulseTimerStopwatch, 1000, "not_create_pulse")) {
                            // We queue up
                            BPLLog("Queuing.");
                            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");

                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create the queue timer
                            LeaveQueue();
                        }
                    }
                        // Otherwise if we are in the queue, move on
                    else {
                        // Move on to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Queued state
                case 1:
                    // If the queue did not pop
                    if(!IsQueuePopped()) {
                        // If the queue timer is running
                        if(LeaveQueueTimerStopwatch.IsRunning) {
                            // Check the queue timer
                            LeaveQueue();
                        } else {
                            // Otherwise if the queue timer isn't running (it expired)
                            // Leave the queue
                            BPLLog("Left queue due to long queue time.");

                            LeaveQueueCommand();

                            // Start over and requeue
                            _treeLogicStep = 0;
                        }
                    }
                        // Otherwise if the queue did pop
                    else {
                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Proposal state
                case 2:
                    // Check for proposal
                    if(IsQueuePopped()) {
                        // If we still need to accept the proposal
                        if(ThrottleTimer.CheckThrottleTimer(PulseTimerStopwatch, RandomNumber.generateRandomInt(1500, 3000),
                            "not_create_pulse")) {
                            // Then we accept the queue after a random amount of time (1.5 - 3 sec)
                            Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch()");
                            BPLLog("Accepted queue.");
                        }
                    }

                    if(IsInPetBattle()) {
                        // Reset the queue timer
                        LeaveQueueTimerStopwatch.Reset();

                        if(ShowPetFrame()) {
                            // Check which pet is usable and change to one that can be active
                            CheckAndChangePet();
                        }

                        if(WaitingOnOpponent()) {
                            BPLLog("Waiting on opponent to choose a pet.");
                        } else {
                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            ThrottleTimer.CreateThrottleTimer(WinnerForfeitTimerStopwatch, 5000, 10000, WinnerForfeitTimerString);

                            // Move to the next step
                            _treeLogicStep++;
                        }
                    }

                    break;

                // Battle started state
                case 3:
                    if(ShowPetFrame()) {
                        // Check which pet is usable and change to one that can be active
                        CheckAndChangePet();
                    }

                    // If the battle started timer is still running
                    if(WinnerForfeitTimerStopwatch.IsRunning) {
                        if(MyPetLevel > EnemyPetLevel) {
                            ForfeitCommand();
                            WinnerForfeitTimerStopwatch.Reset();
                        }
                        if((MyPetLevel == EnemyPetLevel) && (MyPetXp > EnemyPetXp)) {
                            ForfeitCommand();
                            WinnerForfeitTimerStopwatch.Reset();
                        }
                        if((MyPetLevel == EnemyPetLevel) && (MyPetXp == EnemyPetXp)) {
                            ForfeitCommand();
                            WinnerForfeitTimerStopwatch.Reset();
                        }

                        // Check the timer
                        WinnerForfeit();

                        // If the loser forfeits
                        if(!IsInPetBattle()) {
                            WinnerForfeitTimerStopwatch.Reset();
                        }
                    }
                        // Otherwise the timer has expired
                    else {
                        if(IsInPetBattle()) {
                            // Forfeit the match
                            BPLLog("Forfeiting.");
                            ForfeitCommand();
                        }

                        // Make sure we create a new timer
                        ThrottleTimer.WaitTimerCreated = false;

                        // Create the battle ended timer
                        Requeue();

                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Battle ended state
                case 4:
                    if(RequeueTimerStopwatch.IsRunning) {
                        Requeue();
                    } else {
                        _treeLogicStep = 0;
                    }

                    break;

                // Unused atm
                case 5:

                    break;
            }
        }

        #endregion

        #region Root

        private static void PriorityTreeSelection() {
            if(string.IsNullOrEmpty(BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox)) {
                BPLLog("Go into bot settings and configure the settings accordingly.");
            } else
                switch(BattlePetLevelerSettings.Instance.BPLLevelingTypeComboBox) {
                    case "Character Leveling":
                        switch(BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox) {
                            case "Winner":
                                WinnerPriorityTree();
                                break;
                            case "Loser":
                                LoserPriorityTree();
                                break;
                            case "Win Trade":
                                CharacterWinTradePriorityTree();
                                break;
                        }
                        break;
                    case "Pet Leveling":
                        if(BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox == "Win Trade") {
                            PetWinTradePriorityTree();
                        }
                        break;
                }
        }

        private static Composite CreateRoot() {
            return new PrioritySelector(
               new Action(context => PriorityTreeSelection())
            );
        }

        #endregion

        #endregion

        #endregion
    }
}
