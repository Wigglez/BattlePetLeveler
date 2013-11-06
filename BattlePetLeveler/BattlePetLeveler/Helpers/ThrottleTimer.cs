/* BotBase created by AknA and Wigglez */

#region Namespaces

using System;
using System.Diagnostics;
using System.Linq;
using Styx.Common.Helpers;
using Styx.CommonBot;

#endregion

namespace BattlePetLeveler.Helpers {
    public class ThrottleTimer : BattlePetLeveler {
        // ===========================================================
        // Constants
        // ===========================================================

        public const int ThrottleTimerCount = 4;

        // ===========================================================
        // Fields
        // ===========================================================

        public static Stopwatch PulseTimerStopwatch = new Stopwatch();
        public static Stopwatch TimerStopwatch = new Stopwatch();

        public const string LeaveQueueTimerString = "Leave queue timer";
        public const string LoserForfeitTimerString = "Loser forfeit timer";
        public const string WinnerForfeitTimerString = "Winner forfeit timer";
        public const string RequeueTimerString = "Requeue timer";

        public string TimerName;
        public int Time;
        public static string TimerStringName;

        public static WaitTimer WaitTimer;
        public static string WaitTimerAsString;
        public static bool WaitTimerCreated;

        // ===========================================================
        // Constructors
        // ===========================================================

        public ThrottleTimer() {
            TimerName = "";
            Time = 0;
        }

        public ThrottleTimer(string pTimerName, int pTime) {
            TimerName = pTimerName;
            Time = pTime;
        }

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static void CreateThrottleTimer(Stopwatch pTimer, int pFromTime, int pToTime, string pTimerStringName) {
            TimerStringName = pTimerStringName;

            // Generate a random number from a given set of values
            var randNumber = RandomNumber.GenerateRandomInt(pFromTime, pToTime);

            foreach(var t in ThrottleTimers.Where(t => t.TimerName == TimerStringName)) {
                t.Time = randNumber;
            }

            // Check a throttle timer using our currently received timer and the random number
            CheckThrottleTimer(pTimer, randNumber, TimerStringName);
        }

        public static bool CheckThrottleTimer(Stopwatch pTimer, int pTime, string pTimerStringName) {

            // If the timer isn't running, start it
            if(!pTimer.IsRunning) { pTimer.Start(); }


            // If the timer's time is less than or equal to our specified amount of time
            if(pTimer.ElapsedMilliseconds <= pTime) {

                if(pTimerStringName != "not_create_pulse") {
                    FormatAndShowTimer(pTimerStringName);
                }

                return false;
            }

            // Otherwise, if the time elapsed is greater than the time we gave, stop and reset the timer
            ResetTimer(pTimer);

            // Reset the bool to create a new timer as long as it's not the "pulse" that expired
            if(pTimerStringName != "not_create_pulse") {
                WaitTimerCreated = false;
            }

            return true;
        }

        public static void ResetTimer(Stopwatch pThrottleTimer) {
            if(pThrottleTimer.IsRunning) {
                pThrottleTimer.Reset();
            }
        }

        public static void LeaveQueue() {
            if(!TimerStopwatch.IsRunning) {
                CreateThrottleTimer(TimerStopwatch, 300000, 600000, LeaveQueueTimerString);
            } else {
                CheckThrottleTimer(TimerStopwatch, ThrottleTimers[0].Time, LeaveQueueTimerString);
            }
        }

        public static void LoserForfeit() {
            if(!TimerStopwatch.IsRunning) {
                CreateThrottleTimer(TimerStopwatch, 61000, 64000, LoserForfeitTimerString);
            } else {
                CheckThrottleTimer(TimerStopwatch, ThrottleTimers[1].Time, LoserForfeitTimerString);
            }
        }

        public static void WinnerForfeit() {
            if(!TimerStopwatch.IsRunning) {
                CreateThrottleTimer(TimerStopwatch, 70000, 73000, WinnerForfeitTimerString);
            } else {
                CheckThrottleTimer(TimerStopwatch, ThrottleTimers[2].Time, WinnerForfeitTimerString);
            }
        }

        public static void Requeue() {
            if(!TimerStopwatch.IsRunning) {
                CreateThrottleTimer(TimerStopwatch, 10000, 13000, RequeueTimerString);
            } else {
                CheckThrottleTimer(TimerStopwatch, ThrottleTimers[3].Time, RequeueTimerString);
            }
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static void FormatAndShowTimer(string pTimerStringName) {
            // Create a new timer if last one has expired or we didn't have one
            if(!WaitTimerCreated) {
                foreach(var t in ThrottleTimers.Where(t => t.TimerName == pTimerStringName)) {
                    WaitTimerCreated = true;

                    // Set up a new timer based on the amount of milliseconds in our current timer
                    WaitTimer = new WaitTimer(new TimeSpan(0, 0, 0, 0, t.Time));
                    // Build the string with the proper format
                    WaitTimerAsString = BuildTimeAsString(WaitTimer.WaitTime);
                    WaitTimer.Reset();
                }
            }
            OutputMessage(pTimerStringName);
        }

        private static string BuildTimeAsString(TimeSpan timeSpan) {
            string formatString;

            // Check if the timeSpan has hours, etc
            if(timeSpan.Hours > 0) {
                // {0:D2} means 0 is the hours, with a maximum digit of 2
                formatString = "{0:D2}h:{1:D2}m:{2:D2}s";
            } else if(timeSpan.Minutes > 0) {
                formatString = "{1:D2}m:{2:D2}s";
            } else {
                formatString = "{2:D2}s";
            }

            return String.Format(formatString, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        private static string SubstituteInMessage(string pTimerStringName, string message) {
            // Replaces a given string with that of requested info
            message = message.Replace("{TimerName}", pTimerStringName);
            message = message.Replace("{TimeRemaining}", BuildTimeAsString(WaitTimer.TimeLeft));
            message = message.Replace("{TimeDuration}", WaitTimerAsString);

            return message;
        }

        private static void OutputMessage(string pTimerStringName) {
            // Timername: 1m:15s (5m:2s)
            TreeRoot.GoalText = (String.IsNullOrEmpty(BPLStatusText) ? "" : SubstituteInMessage(pTimerStringName, BPLStatusText));
            TreeRoot.StatusText = TreeRoot.GoalText;
        }
    }
}
