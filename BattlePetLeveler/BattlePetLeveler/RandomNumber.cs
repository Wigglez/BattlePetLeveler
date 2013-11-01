using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattlePetLeveler
{
    public class RandomNumber
    {
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

        public RandomNumber()
        {

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

        /**
	     * Returns a psuedo-random number between min and max, inclusive. The
	     * difference between min and max can be at most
	     * <code>Integer.MAX_VALUE - 1</code>.
	     */

        public static int generateRandomInt(int pMinVal, int pMaxVal)
        {
            _mRandom = new Random();

            // nextInt is normally exclusive of the top value (pMaxVal), so add 1 to
            // make it inclusive

            int randomNumber = _mRandom.Next((pMaxVal - pMinVal) + 1) + pMinVal;

            return randomNumber;
        }

        public static float generateRandomFloat(float pMinVal, float pMaxVal)
        {
            _mRandom = new Random();

            // nextfloat is normally exclusive of the top value (pMaxVal), so add 1
            // to
            // make it inclusive

            int rndFlt = generateRandomInt((int) (pMinVal*100),
                (int) (pMaxVal*100));

            float randomNumber = (float) rndFlt/100.0f;

            return randomNumber;
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================
    }
}