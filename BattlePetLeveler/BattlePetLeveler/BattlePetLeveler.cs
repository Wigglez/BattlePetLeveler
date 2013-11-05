/* BotBase created by AknA and Wigglez */

#region Namespaces

using System.Windows.Forms;
using System.Windows.Media;
using BattlePetLeveler.GUI;
using BattlePetLeveler.Helpers;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;
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
        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        private static Composite _root;

        public static string BPLStatusText;

        public static ThrottleTimer[] ThrottleTimers = new ThrottleTimer[ThrottleTimer.ThrottleTimerCount];

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        public override string Name { get { return "BattlePetLeveler"; } }

        public override PulseFlags PulseFlags { get { return PulseFlags.All; } }

        public override Composite Root { get { return _root ?? (_root = CreateRoot()); } }

        public override Form ConfigurationForm { get { return new BattlePetLevelerGUI(); } }

        public override void Start() {
            ProfileManager.LoadEmpty();

            BPLStatusText = "{TimerName}: {TimeRemaining} ({TimeDuration})";

            for(var i = 0; i < ThrottleTimer.ThrottleTimerCount; i++) {
                ThrottleTimers[i] = new ThrottleTimer("", 0);
            }

            ThrottleTimers[0].TimerName = ThrottleTimer.LeaveQueueTimerString;
            ThrottleTimers[1].TimerName = ThrottleTimer.LoserForfeitTimerString;
            ThrottleTimers[2].TimerName = ThrottleTimer.WinnerForfeitTimerString;
            ThrottleTimers[3].TimerName = ThrottleTimer.RequeueTimerString;

            BPLLog("Initialization complete.");
        }

        public override void Stop() {
            TreeRoot.GoalText = string.Empty;
            TreeRoot.StatusText = string.Empty;

            // Clear the throttle timers
            foreach(var t in ThrottleTimers) {
                t.TimerName = "";
                t.Time = 0;
            }

            QueueHandler.LeaveQueueCommand();

            BPLLog("Shutdown complete.");
        }

        // ===========================================================
        // Methods
        // ===========================================================

        public static void BPLLog(string message, params object[] args) {
            Logging.Write(Colors.DeepSkyBlue, "[BPL]: " + message, args);
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static Composite CreateRoot() {
            return new PrioritySelector(
               new Action(context => PriorityTreeState.TreeStateHandler())
            );
        }
    }
}
