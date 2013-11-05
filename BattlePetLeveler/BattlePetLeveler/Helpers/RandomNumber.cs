/* BotBase created by AknA and Wigglez */

#region Namespaces

using System;

#endregion

namespace BattlePetLeveler.Helpers {
    public class RandomNumber : BattlePetLeveler {
        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        private static Random _mRandom;

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        /*
	     * Returns a psuedo-random number between min and max, inclusive. The
	     * difference between min and max can be at most
	     * <code>Integer.MAX_VALUE - 1</code>.
	     */

        public static int GenerateRandomInt(int pMinVal, int pMaxVal) {
            _mRandom = new Random();

            // nextInt is normally exclusive of the top value (pMaxVal), so add 1 to
            // make it inclusive

            var randomNumber = _mRandom.Next((pMaxVal - pMinVal) + 1) + pMinVal;

            return randomNumber;
        }

        public static float GenerateRandomFloat(float pMinVal, float pMaxVal) {
            _mRandom = new Random();

            // nextfloat is normally exclusive of the top value (pMaxVal), so add 1
            // to
            // make it inclusive

            var rndFlt = GenerateRandomInt((int)(pMinVal * 100),
                (int)(pMaxVal * 100));

            var randomNumber = rndFlt / 100.0f;

            return randomNumber;
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}