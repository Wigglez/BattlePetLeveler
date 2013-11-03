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

namespace BattlePetLeveler
{
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

    public class BattlePetLeveler : BotBase
    {
        #region Constants
        // ===========================================================
        // Constants
        // ===========================================================

        private const int _throttleTimerCount = 4;

        #endregion

        #region Fields
        // ===========================================================
        // Fields
        // ===========================================================

        private static int _treeLogicStep;

        private static readonly Stopwatch PulseTimer = new Stopwatch();
        private static readonly Stopwatch LeaveQueueTimer = new Stopwatch();
        private static readonly Stopwatch LoserForfeitTimer = new Stopwatch();
        private static readonly Stopwatch WinnerForfeitTimer = new Stopwatch();
        private static readonly Stopwatch RequeueTimer = new Stopwatch();

        private static ThrottleTimer[] _throttleTimers;

        public const string _leaveQueueTimer = "Leave queue timer";
        public const string _loserForfeitTimer = "Loser forfeit timer";
        public const string _winnerForfeitTimer = "Winner forfeit timer";
        public const string _requeueTimer = "Requeue timer";
        
        private Composite _Root;

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

        public static ThrottleTimer[] ThrottleTimers
        {
            get { return _throttleTimers; }
            set { _throttleTimers = value; }
        }

        public static int ThrottleTimerCount
        {
            get { return _throttleTimerCount; }
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

        public override Composite Root { get { return _Root ?? (_Root = CreateRoot()); } }

        public override Form ConfigurationForm { get { return new BattlePetLevelerGUI(); } }

        public override void Start()
        {
            ProfileManager.LoadEmpty();

            _treeLogicStep = 0;

            BPLStatusText = "{TimerName}: {TimeRemaining} ({TimeDuration})";

            _throttleTimers = new ThrottleTimer[_throttleTimerCount];

            for (var i = 0; i < _throttleTimerCount; i++)
            {
                _throttleTimers[i] = new ThrottleTimer("", 0);
            }

            _throttleTimers[0].TimerName = _leaveQueueTimer;
            _throttleTimers[1].TimerName = _loserForfeitTimer;
            _throttleTimers[2].TimerName = _winnerForfeitTimer;
            _throttleTimers[3].TimerName = _requeueTimer;


            BPLlog("Initialization complete.");
        }

        public override void Stop()
        {
            TreeRoot.GoalText = string.Empty;
            TreeRoot.StatusText = string.Empty;

            // Clear the throttle timers
            foreach (var t in _throttleTimers)
            {
                t.TimerName = "";
                t.Time = 0;
            }

            LeaveQueueCommand();

            BPLlog("Shutdown complete.");
        }

        #endregion

        #region Methods
        // ===========================================================
        // Methods
        // ===========================================================

        #region Convenience

        public static void BPLlog(string message, params object[] args)
        {
            Logging.Write(Colors.DeepSkyBlue, "[BPL]: " + message, args);
        }

        public static void LeaveQueue()
        {
            // 3-5 min timer to wait now that we are queued
            if (!LeaveQueueTimer.IsRunning)
            {
                ThrottleTimer.CreateThrottleTimer(LeaveQueueTimer, 180000, 300000, _leaveQueueTimer);
            }
            else
            {
                ThrottleTimer.CheckThrottleTimer(LeaveQueueTimer, _throttleTimers[0].Time, _leaveQueueTimer);
            }
        }

        public static void LoserForfeit()
        {
            // 65-70 sec timer to wait now that the battle has started
            if (!LoserForfeitTimer.IsRunning)
            {
                ThrottleTimer.CreateThrottleTimer(LoserForfeitTimer, 65000, 70000, _loserForfeitTimer);
            }
            else
            {
                ThrottleTimer.CheckThrottleTimer(LoserForfeitTimer, _throttleTimers[1].Time, _loserForfeitTimer);
            }
        }

        public static void WinnerForfeit()
        {
            // 120-130 sec timer to wait now that the battle has started
            if (!WinnerForfeitTimer.IsRunning)
            {
                ThrottleTimer.CreateThrottleTimer(WinnerForfeitTimer, 120000, 130000, _winnerForfeitTimer);
            }
            else
            {
                ThrottleTimer.CheckThrottleTimer(WinnerForfeitTimer, _throttleTimers[2].Time, _winnerForfeitTimer);
            }
        }

        public static void Requeue()
        {
            // 15-20 sec timer to wait now that the battle has ended
            if (!RequeueTimer.IsRunning)
            {
                ThrottleTimer.CreateThrottleTimer(RequeueTimer, 15000, 20000, _requeueTimer);
            }
            else
            {
                ThrottleTimer.CheckThrottleTimer(RequeueTimer, _throttleTimers[3].Time, _requeueTimer);
            }
        }

        #endregion

        #region Queue Handling

        public static bool IsQueuable()
        {
            // Are we doing something that can stop us from accepting queue?
            if (!Me.IsValid)
            {
                BPLlog("Not able to accept queue due to being invalid (not in world).");
                return false;
            }

            if(Me.InVehicle || Me.IsOnTransport)
            {
                BPLlog("Not able to accept queue due to being in a vehicle or on a transport.");
                return false;
            }

            if (Me.IsGhost || Me.IsDead)
            {
                BPLlog("Not able to accept queue due to being a ghost or dead.");
                return false;
            }

            if (Me.Combat || Me.IsActuallyInCombat)
            {
                BPLlog("Not able to accept queue due to being in combat.");
                return false;
            }

            // Get any wow unit (if I have aggro or my pet has aggro)
            if (ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Any(unit => unit.Aggro || unit.PetAggro))
            {
                BPLlog("Not able to accept queue due to aggro.");
                return false;
            }

            return true;
        }

        public static bool IsQueuePopped()
        {
            return Lua.GetReturnVal<bool>("return select(1, C_PetBattles.GetPVPMatchmakingInfo()) == 'proposal'", 0);
        }

        public static bool IsInQueue()
        {
            return Lua.GetReturnVal<bool>("return select(1, C_PetBattles.GetPVPMatchmakingInfo()) == 'queued'", 0);
        }

        public static void LeaveQueueCommand()
        {
            Lua.DoString("C_PetBattles.StopPVPMatchmaking()");
        }

        #endregion

        #region Pet Battle

        public static bool ShowPetFrame()
        {
            var petSelectFrame = Lua.GetReturnVal<bool>("return C_PetBattles.ShouldShowPetSelect()", 0);
            return petSelectFrame;
        }

        public static void CheckAndChangePet() {
            // Check if the pet is usable
            if (PetUsable(1))
            {
                // Change pet to the first pet
                ChangePetCommand(1);
            }
            else if (PetUsable(2))
            {
                // Change pet to the second pet
                ChangePetCommand(2);
            }
            else if (PetUsable(3))
            {
                // Change pet to the third pet
                ChangePetCommand(3);
            }
        }

        public static bool IsInPetBattle()
        {
            var inPetBattle = Lua.GetReturnVal<bool>("return C_PetBattles.IsInBattle()", 0);
            return inPetBattle;
        }

        public static bool PetUsable(int pPetIndex)
        {
            var usable = Lua.GetReturnVal<bool>(string.Format("return C_PetBattles.CanPetSwapIn({0})", pPetIndex), 0);
            return usable;
        }

        public static void ChangePetCommand(int pPetIndex)
        {
            Lua.DoString(string.Format("C_PetBattles.ChangePet({0})", pPetIndex));
        }

        public static bool WaitingOnOpponent()
        {
            var waiting = Lua.GetReturnVal<bool>("return C_PetBattles.IsWaitingOnOpponent()", 0);
            return waiting;
        }

        public static void ForfeitCommand()
        {
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

        private static void LoserPriorityTree()
        {
            switch (_treeLogicStep)
            {
                // Not queued state
                case 0:
                    // If we are not in queue, we are queuable, and our pulse timer is active
                    if (!IsInQueue())
                    {
                        if (IsQueuable() && ThrottleTimer.CheckThrottleTimer(PulseTimer, 1000, "not_create_pulse"))
                        {
                            // We queue up
                            BPLlog("Queuing.");
                            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");

                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create the queue timer
                            LeaveQueue();
                        }
                    }
                    // Otherwise if we are in the queue, move on
                    else
                    {
                        // Move on to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Queued state
                case 1:
                    // If the queue did not pop
                    if (!IsQueuePopped())
                    {
                        // If the queue timer is running
                        if (LeaveQueueTimer.IsRunning)
                        {
                            // Check the queue timer
                            LeaveQueue();
                        }
                        else
                        {
                            // Otherwise if the queue timer isn't running (it expired)
                            // Leave the queue
                            BPLlog("Left queue due to long queue time.");

                            LeaveQueueCommand();

                            // Start over and requeue
                            _treeLogicStep = 0;
                        }
                    }
                    // Otherwise if the queue did pop
                    else
                    {
                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Proposal state
                case 2:
                    // Check for proposal
                    if (IsQueuePopped())
                    {
                        // If we still need to accept the proposal
                        if (ThrottleTimer.CheckThrottleTimer(PulseTimer, RandomNumber.generateRandomInt(1500, 3000),
                            "not_create_pulse"))
                        {
                            // Then we accept the queue after a random amount of time (1.5 - 3 sec)
                            Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch()");
                            BPLlog("Accepted queue.");
                        }
                    }

                    if (IsInPetBattle())
                    {
                        // Reset the queue timer
                        LeaveQueueTimer.Reset();

                        if (ShowPetFrame())
                        {
                            // Check which pet is usable and change to one that can be active
                            CheckAndChangePet();
                        }

                        if (WaitingOnOpponent())
                        {
                            BPLlog("Waiting on opponent to choose a pet.");
                        }
                        else
                        {
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
                    if (ShowPetFrame())
                    {
                        // Check which pet is usable and change to one that can be active
                        CheckAndChangePet();
                    }

                    // If the battle started timer is still running
                    if (LoserForfeitTimer.IsRunning)
                    {
                        // Check the timer
                        LoserForfeit();
                    } 
                    // Otherwise the timer has expired
                    else
                    {
                        // Forfeit the match
                        BPLlog("Forfeiting.");
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
                    if (RequeueTimer.IsRunning)
                    {
                        Requeue();
                    }
                    else 
                    {
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

        private static void WinnerPriorityTree()
        {
            switch (_treeLogicStep)
            {
                // Not queued state
                case 0:
                    // If we are not in queue, we are queuable, and our pulse timer is active
                    if (!IsInQueue())
                    {
                        if (IsQueuable() && ThrottleTimer.CheckThrottleTimer(PulseTimer, 1000, "not_create_pulse"))
                        {
                            // We queue up
                            BPLlog("Queuing.");
                            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");

                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create the queue timer
                            LeaveQueue();
                        }
                    }
                    // Otherwise if we are in the queue, move on
                    else
                    {
                        // Move on to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Queued state
                case 1:
                    // If the queue did not pop
                    if (!IsQueuePopped())
                    {
                        // If the queue timer is running
                        if (LeaveQueueTimer.IsRunning)
                        {
                            // Check the queue timer
                            LeaveQueue();
                        }
                        else
                        {
                            // Otherwise if the queue timer isn't running (it expired)
                            // Leave the queue
                            BPLlog("Left queue due to long queue time.");

                            LeaveQueueCommand();

                            // Start over and requeue
                            _treeLogicStep = 0;
                        }
                    }
                    // Otherwise if the queue did pop
                    else
                    {
                        // Move to the next step
                        _treeLogicStep++;
                    }

                    break;

                // Proposal state
                case 2:
                    // Check for proposal
                    if (IsQueuePopped())
                    {
                        // If we still need to accept the proposal
                        if (ThrottleTimer.CheckThrottleTimer(PulseTimer, RandomNumber.generateRandomInt(1500, 3000),
                            "not_create_pulse"))
                        {
                            // Then we accept the queue after a random amount of time (1.5 - 3 sec)
                            Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch()");
                            BPLlog("Accepted queue.");
                        }
                    }

                    if (IsInPetBattle())
                    {
                        // Reset the queue timer
                        LeaveQueueTimer.Reset();

                        if (ShowPetFrame())
                        {
                            // Check which pet is usable and change to one that can be active
                            CheckAndChangePet();
                        }

                        if (WaitingOnOpponent())
                        {
                            BPLlog("Waiting on opponent to choose a pet.");
                        }
                        else
                        {
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
                    if (ShowPetFrame())
                    {
                        // Check which pet is usable and change to one that can be active
                        CheckAndChangePet();
                    }

                    // If the battle started timer is still running
                    if (WinnerForfeitTimer.IsRunning)
                    {
                        // Check the timer
                        WinnerForfeit();
                    }
                    // Otherwise the timer has expired
                    else
                    {
                        // Forfeit the match
                        BPLlog("Forfeiting.");
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
                    if (RequeueTimer.IsRunning)
                    {
                        Requeue();
                    }
                    else
                    {
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

        private static Composite CreateRoot()
        {
            return new PrioritySelector(
                new Decorator(context => string.IsNullOrEmpty(BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox),
                    new Action(context => BPLlog("Go into bot settings and select a character type."))
                ),
                new Decorator(context => BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox == "Winner",
                    new Action(context => WinnerPriorityTree())
                ),
                new Decorator(context => BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox == "Loser",
                    new Action(context => LoserPriorityTree())
                )
            );
        }

        #endregion

        #endregion

        #endregion
    }
}
