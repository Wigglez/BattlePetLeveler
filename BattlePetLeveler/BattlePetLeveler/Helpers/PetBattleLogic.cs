using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePetLeveler.Helpers {
    public class PetBattleLogic {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

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

        public static void UseRandomAbility() {
            var randNum = RandomNumber.GenerateRandomInt(1, 3);

            var getCanUseAbility = PetBattles.GetAbilityState(1, 1, randNum);

            if (getCanUseAbility[0] == "0")
            {
                PriorityTreeState.UsedRandomAbility = false;
            }
            else
            {
                BattlePetLeveler.BPLLog("used " + randNum);
                PriorityTreeState.UsedRandomAbility = true;
                PetBattles.UseAbility(randNum);
            }

            PriorityTreeState.UsedRandomAbility = true;
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

    }
}
