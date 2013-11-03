#region Information
/* BotBase created by AknA and Wigglez */

#endregion

#region Namespaces

using System;

#endregion

namespace BattlePetLeveler.Convenience
{
    public class RandomNumber
    {
        #region Constants
        // ===========================================================
        // Constants
        // ===========================================================

        #endregion

        #region Fields
        // ===========================================================
        // Fields
        // ===========================================================

        private static Random _mRandom;



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

        /*
	     * Returns a psuedo-random number between min and max, inclusive. The
	     * difference between min and max can be at most
	     * <code>Integer.MAX_VALUE - 1</code>.
	     */

        public static int generateRandomInt(int pMinVal, int pMaxVal)
        {
            _mRandom = new Random();

            // nextInt is normally exclusive of the top value (pMaxVal), so add 1 to
            // make it inclusive

            var randomNumber = _mRandom.Next((pMaxVal - pMinVal) + 1) + pMinVal;

            return randomNumber;
        }

        public static float generateRandomFloat(float pMinVal, float pMaxVal)
        {
            _mRandom = new Random();

            // nextfloat is normally exclusive of the top value (pMaxVal), so add 1
            // to
            // make it inclusive

            var rndFlt = generateRandomInt((int) (pMinVal*100),
                (int) (pMaxVal*100));

            var randomNumber = rndFlt/100.0f;

            return randomNumber;
        }



        #endregion

        #region Inner and Anonymous Classes
        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        #endregion
    }
}