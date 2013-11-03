#region Information
/* BotBase created by AknA and Wigglez */

#endregion

#region Namespaces

using System;
using System.Diagnostics;
using System.Linq;
using Styx.Common.Helpers;
using Styx.CommonBot;

#endregion

namespace BattlePetLeveler.Convenience {
    public class ThrottleTimer : BattlePetLeveler {
        #region Constants
        // ===========================================================
        // Constants
        // ===========================================================



        #endregion

        #region Fields
        // ===========================================================
        // Fields
        // ===========================================================

        private string _TimerName;
        private int _Time;
        private static WaitTimer _waitTimer;
        private static string _waitTimerAsString;
        private static bool _waitTimerCreated;



        #endregion

        #region Constructors
        // ===========================================================
        // Constructors
        // ===========================================================

        public ThrottleTimer() {
            _TimerName = "";
            _Time = 0;
        }

        public ThrottleTimer(string pTimerName, int pTime) {
            _TimerName = pTimerName;
            _Time = pTime;
        }



        #endregion

        #region Getter & Setter
        // ===========================================================
        // Getter & Setter
        // ===========================================================

        public string TimerName {
            get { return _TimerName; }
            set { _TimerName = value; }
        }

        public int Time {
            get { return _Time; }
            set { _Time = value; }
        }

        public static bool WaitTimerCreated {
            get { return _waitTimerCreated; }
            set { _waitTimerCreated = value; }
        }



        #endregion

        #region Methods for/from SuperClass/Interfaces
        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================



        #endregion

        #region Methods
        // ===========================================================
        // Methods
        // ===========================================================

        public static void CreateThrottleTimer(Stopwatch pTimer, int pFromTime, int pToTime, string pTimerStringName) {
            // Generate a random number from a given set of values
            var randNumber = RandomNumber.generateRandomInt(pFromTime, pToTime);

            foreach (var t in ThrottleTimers.Where(t => t._TimerName == pTimerStringName)) {
                t._Time = randNumber;
            }

            // Check a throttle timer using our currently received timer and the random number
            CheckThrottleTimer(pTimer, randNumber, pTimerStringName);
        }

        public static bool CheckThrottleTimer(Stopwatch pThrottleTimer, int pTime, string pTimerStringName) {
            // If the timer isn't running, start it
            if (!pThrottleTimer.IsRunning) { pThrottleTimer.Start(); }


            // If the timer's time is less than or equal to our specified amount of time
            if (pThrottleTimer.ElapsedMilliseconds <= pTime) {

                if (pTimerStringName != "not_create_pulse") {
                    FormatAndShowTimer(pTimerStringName);
                }

                return false;
            }

            // Otherwise, if the time elapsed is greater than the time we gave, stop and reset the timer
            ResetTimer(pThrottleTimer);

            // Reset the bool to create a new timer as long as it's not the "pulse" that expired
            if (pTimerStringName != "not_create_pulse") {
                _waitTimerCreated = false;
            }

            return true;
        }

        public static void ResetTimer(Stopwatch pThrottleTimer) {
            if (pThrottleTimer.IsRunning) {
                pThrottleTimer.Reset();
            }
        }



        #endregion

        #region Inner and Anonymous Classes
        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static void FormatAndShowTimer(string pTimerStringName) {
            // Create a new timer if last one has expired or we didn't have one
            if (!_waitTimerCreated) {
                foreach (var t in ThrottleTimers.Where(t => t._TimerName == pTimerStringName)) {
                    _waitTimerCreated = true;

                    // Set up a new timer based on the amount of milliseconds in our current timer
                    _waitTimer = new WaitTimer(new TimeSpan(0, 0, 0, 0, t._Time));
                    // Build the string with the proper format
                    _waitTimerAsString = BuildTimeAsString(_waitTimer.WaitTime);
                    _waitTimer.Reset();
                }
            }
            OutputMessage(pTimerStringName);
        }

        private static string BuildTimeAsString(TimeSpan timeSpan) {
            string formatString;

            // Check if the timeSpan has hours, etc
            if (timeSpan.Hours > 0) {
                // {0:D2} means 0 is the hours, with a maximum digit of 2
                formatString = "{0:D2}h:{1:D2}m:{2:D2}s";
            } else if (timeSpan.Minutes > 0) {
                formatString = "{1:D2}m:{2:D2}s";
            } else {
                formatString = "{2:D2}s";
            }

            return string.Format(formatString, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        private static string SubstituteInMessage(string pTimerStringName, string message) {
            // Replaces a given string with that of requested info
            message = message.Replace("{TimerName}", pTimerStringName);
            message = message.Replace("{TimeRemaining}", BuildTimeAsString(_waitTimer.TimeLeft));
            message = message.Replace("{TimeDuration}", _waitTimerAsString);

            return message;
        }

        private static void OutputMessage(string pTimerStringName) {
            // Timername: 1m:15s (5m:2s)
            TreeRoot.GoalText = (string.IsNullOrEmpty(BPLStatusText) ? "" : SubstituteInMessage(pTimerStringName, BPLStatusText));
            TreeRoot.StatusText = TreeRoot.GoalText;
        }
        #endregion
    }
}
