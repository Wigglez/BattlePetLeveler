/* PetBattleLeveler BotBase created by AknA and Wigglez */

using System;
using System.Diagnostics;
using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using ProfileManager = Styx.CommonBot.Profiles.ProfileManager;

namespace BattlePetLeveler
{
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


    public class BattlePetLeveler : BotBase
    {
        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        #region Fields

        private static readonly ThrottleTimer _throttleTimer = new ThrottleTimer();

        private static readonly Stopwatch _pulseTimer = new Stopwatch();
        private static readonly Stopwatch _queueTimer = new Stopwatch();
        private static readonly Stopwatch _proposalTimer = new Stopwatch();
        private static readonly Stopwatch _battleStartedTimer = new Stopwatch();
        private static readonly Stopwatch _battleEndedTimer = new Stopwatch();

        private static int _throttleTimerCount;
        private static ThrottleTimer[] _throttleTimers;

        private static string _queue;
        private static string _proposal;
        private static string _battleStarted;
        private static string _battleEnded;

        private Composite _root;
        #endregion

        #region Properties

        public static ThrottleTimer[] ThrottleTimers
        {
            get { return _throttleTimers; }
            set { _throttleTimers = value; }
        }

        public static int ThrottleTimerCount 
        {
            get { return _throttleTimerCount; }
            set { _throttleTimerCount = value; }
        }
        #endregion

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        #region Properties
        public static LocalPlayer Me { get { return StyxWoW.Me; } }
        #endregion

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        #region Overrides
        // The name of the BotBase
        public override string Name { get { return "BattlePetLeveler"; } }

        // Only used in botbases
        public override PulseFlags PulseFlags { get { return PulseFlags.All; } }

        public override Composite Root { get { return _root ?? (_root = CreateRoot()); } }

        public override void Start()
        {
            ProfileManager.LoadEmpty();

            _throttleTimerCount = 4;

            _proposal = "proposal";
            _battleStarted = "battleStarted";
            _battleEnded = "battleEnded";
            _queue = "queue";

            _throttleTimers = new ThrottleTimer[_throttleTimerCount];

            for (var i = 0; i < _throttleTimerCount; i++)
            {
                _throttleTimers[i] = new ThrottleTimer("", 0);
            }
            
            _throttleTimers[0].TimerName = _proposal;
            _throttleTimers[1].TimerName = _battleStarted;
            _throttleTimers[2].TimerName = _battleEnded;
            _throttleTimers[3].TimerName = _queue;

            Logging.Write("[BPL]: Initialization complete.");
        }

        public override void Stop()
        {
            // Clear the throttle timers
            foreach (var t in _throttleTimers)
            {
                t.TimerName = "";
                t.Time = 0;
            }

            LeaveQueue();

            Logging.Write("[BPL]: Shutdown complete.");
        }

        #endregion

        // ===========================================================
        // Methods
        // ===========================================================

        #region Queue Handling
        public static bool IsQueuable()
        {
            // Are we doing something that can stop us from queuing?
            if (!Me.IsValid || Me.InVehicle || Me.IsOnTransport || Me.IsGhost || Me.IsDead || Me.Combat || Me.IsActuallyInCombat) { return false; }

            // Get any wow unit (if I have aggro or my pet has aggro)
            if (ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Any(unit => unit.Aggro || unit.PetAggro)) { return false; }

            // Determine what our current queue status is
            //var matchmakingInfo = Lua.GetReturnVal<string>("return select(1, C_PetBattles.GetPVPMatchmakingInfo())", 0);
            if (IsInQueue() || IsQueuePopped())
            {
                return false;
            }

            if (_queueTimer.IsRunning)
            {
                Queue();
                return false;
            }

            // If the proposal timer is running, then check how much time is left on our current timer
            if (_proposalTimer.IsRunning)
            {
                Proposal();
                return false;
            }

            if (_battleStartedTimer.IsRunning)
            {
                BattleStarted();
                return false;
            }

            if (_battleEndedTimer.IsRunning)
            {
                BattleEnded();
                return false;
            }

            // Waiting on an opponent
            var waiting = Lua.GetReturnVal<bool>("C_PetBattles.IsWaitingOnOpponent()", 0);
            if (waiting) { return false; }

            /* battleState - The current state of the pet battle: (number)
                2 - Battle is beginning
                3 - Battle is in progress
                4 - Waiting for a pet switch
                7 - Battle is ending
             */
            // 
            var battleState = Lua.GetReturnVal<int>("return C_PetBattles.GetBattleState()", 0);
            if (battleState == 2 || battleState == 7) { return false; }

            // In battle
            var isInBattle = Lua.GetReturnVal<bool>("return C_PetBattles.IsInBattle()", 0);
            if (isInBattle) { return false; }

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
        #endregion

        #region Pet Battle
        public static bool ShowPetFrame()
        {
            var petSelectFrame = Lua.GetReturnVal<bool>("return C_PetBattles.ShouldShowPetSelect()", 0);
            return petSelectFrame;
        }

        public static void CheckAndChangePet()
        {
            // Check if the pet is usable
            if (PetUsable(1))
            {
                // Change pet to the first pet
                ChangePet(1);
            }
            else if (PetUsable(2))
            {
                // Change pet to the second pet
                ChangePet(2);
            }
            else if (PetUsable(3))
            {
                // Change pet to the third pet
                ChangePet(3);
            }

            BattleStarted();
        }

        public static void Queue()
        {
            // 3-5 min timer to wait now that we are queued
            if (!_queueTimer.IsRunning)
            {
                ThrottleTimer.CreateThrottleTimer(_queueTimer, 180000, 300000, _queue);
            }
            else
            {
                ThrottleTimer.CheckThrottleTimer(_queueTimer, _throttleTimers[3].Time, _queue);
            }
        }

        public static void Proposal()
        {
            // 31-40 sec timer to wait now that the proposal window is open
            if (!_proposalTimer.IsRunning)
            {
                ThrottleTimer.CreateThrottleTimer(_proposalTimer, 31000, 40000, _proposal);
            }
            else
            {
                ThrottleTimer.CheckThrottleTimer(_proposalTimer, _throttleTimers[0].Time, _proposal);
            }
        }

        public static void BattleStarted()
        {
            // 65-70 sec timer to wait now that the battle has started
            if (!_battleStartedTimer.IsRunning)
            {
                ThrottleTimer.CreateThrottleTimer(_battleStartedTimer, 65000, 70000, _battleStarted);
            }
            else
            {
                ThrottleTimer.CheckThrottleTimer(_battleStartedTimer, _throttleTimers[1].Time, _battleStarted);
            }
        }

        public static void BattleEnded()
        {
            // 10-15 sec timer to wait now that the battle has ended
            if (!_battleEndedTimer.IsRunning)
            {
                ThrottleTimer.CreateThrottleTimer(_battleEndedTimer, 10000, 15000, _battleEnded);
            }
            else
            {
                ThrottleTimer.CheckThrottleTimer(_battleEndedTimer, _throttleTimers[2].Time, _battleEnded);
            }
            
        }

        public static bool PetUsable(int pPetIndex)
        {
            var usable = Lua.GetReturnVal<bool>(string.Format("return C_PetBattles.CanPetSwapIn({0})", pPetIndex), 0);
            return usable;
        }

        public static void ChangePet(int pPetIndex)
        {
            Lua.DoString(string.Format("C_PetBattles.ChangePet({0})", pPetIndex));
        }

        public static int GetActivePet(int pPetIndex)
        {
            var activePet = Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetActivePet({0})", pPetIndex), 0);
            return activePet;
        }

        public static void SkipTurn()
        {
            Lua.DoString("C_PetBattles.SkipTurn()");
        }

        public static void Forfeit()
        {
            BattleEnded();
            Lua.DoString("C_PetBattles.ForfeitGame()");
        }

        public static void LeaveQueue()
        {
            Lua.DoString("C_PetBattles.StopPVPMatchmaking()");
        }

        #endregion

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        #region Root
        private static PrioritySelector CreateRoot() 
        {

            return new PrioritySelector(
                // Check if we can queue
                new Decorator(context => IsQueuable(),
                    // Wait 1000 ms between queue attempts
                    new DecoratorContinue(context => ThrottleTimer.CheckThrottleTimer(_pulseTimer, 1000, "pulse"),
                        new Sequence(
                            // Queue
                            new Action(context => Logging.Write("[BPL]: Queuing.")),
                            new Action(context => Lua.DoString("C_PetBattles.StartPVPMatchmaking()"))
                        )
                    )
                ),
                // Check if we are in queue
                new Decorator(context => IsInQueue(),
                    // Create the queue timer
                    new Action(context => Queue())
                ),
                // If the timer is running, check the time (if it ran out)
                new Decorator(context => _queueTimer.IsRunning,
                    // If true, leaves queue
                    new Sequence(
                        new Action(context => Logging.Write("[BPL]: Left queue due to long queue time.")),
                        new Action(context => LeaveQueue())
                    )
                ),
                // Check if the queue popped
                new Decorator(context => IsQueuePopped(),
                    new Sequence(
                        // We are now in a proposal state, starting the timer
                        new Action(context => Proposal()),
                        // Wait a random amount of time, and since the context is false, it will never be true (enforcing the wait time)
                        new WaitContinue(TimeSpan.FromMilliseconds(2000), context => false, new ActionAlwaysSucceed()),
                        new Action(context => Logging.Write("[BPL]: Accepting queue.")),
                        new Action(context => Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch()"))
                    )
                ),
                // Check if the pet frame is active
                new Decorator(context => ShowPetFrame(),
                    new Sequence(
                        // Check and change the pet depending on if they are usable or not
                        new Action(context => CheckAndChangePet())
                    )
                ),
                new Decorator(context => _battleStartedTimer.IsRunning,
                    new Sequence(
                        new Action(context => BattleStarted()),
                        // Check and change the pet depending on if they are usable or not
                        new Action(context => Logging.Write("[BPL]: Forfeiting.")),
                        new Action(context => Forfeit())
                    )
                    
                )

            );
        }
        #endregion
    }
}
