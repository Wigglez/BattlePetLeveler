
using System;
using System.Diagnostics;
using System.Linq;
using Styx.Common;
using Styx.CommonBot;

namespace BattlePetLeveler
{
    public class ThrottleTimer : BattlePetLeveler
    {
        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        private string _timerName;
        private int _time;

        // ===========================================================
        // Constructors
        // ===========================================================

        public ThrottleTimer()
        {
            _timerName = "";
            _time = 0;
        }

        public ThrottleTimer(string pTimerName, int pTime)
        {
            _timerName = pTimerName;
            _time = pTime;
        }

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        public string TimerName
        {
            get { return _timerName; } 
            set { _timerName = value; }
        }

        public int Time
        {
            get { return _time; }
            set { _time = value; }
        }

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================
          

        // ===========================================================
        // Methods
        // ===========================================================

        public static void CreateThrottleTimer(Stopwatch pTimer, int pFromTime, int pToTime, string pTimerStringName)
        {
            // Generate a random number from a given set of values
            var randNumber = RandomNumber.generateRandomInt(pFromTime, pToTime);

            foreach (var t in ThrottleTimers.Where(t => t._timerName == pTimerStringName))
            {


                t._time = randNumber;
            }
             
            // Check a throttle timer using our currently received timer and the random number
            CheckThrottleTimer(pTimer, randNumber, pTimerStringName);
        }

        public static bool CheckThrottleTimer(Stopwatch pThrottleTimer, int pTime, string pTimerStringName)
        {
            // If the timer isn't running, start it
            if (!pThrottleTimer.IsRunning) { pThrottleTimer.Start(); }
            // If the timer's time is less than or equal to our specified amount of time
            if (pThrottleTimer.ElapsedMilliseconds <= pTime)
            {
                TreeRoot.StatusText = pTimerStringName + ": " + pThrottleTimer.ElapsedMilliseconds + " (" + pTime + ")";

                // Then we return false that we are not queuable
                return false;
            }

            // Otherwise, if the time elapsed is greater than the time we gave, stop and reset the timer
            pThrottleTimer.Stop();
            pThrottleTimer.Reset();
            return true;
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================
    }
}
