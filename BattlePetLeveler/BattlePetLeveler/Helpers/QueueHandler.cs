/* BotBase created by AknA and Wigglez */

#region Namespaces

using System.Linq;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace BattlePetLeveler.Helpers {
    public class QueueHandler : BattlePetLeveler {
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

        public static bool IsQueuable() {
            // Are we doing something that can stop us from accepting queue?
            if(!Character.Me.IsValid) {
                BPLLog("Not able to accept queue due to being invalid (not in world).");
                return false;
            }

            if(Character.Me.InVehicle || Character.Me.IsOnTransport) {
                BPLLog("Not able to accept queue due to being in a vehicle or on a transport.");
                return false;
            }

            if(Character.Me.IsGhost || Character.Me.IsDead) {
                BPLLog("Not able to accept queue due to being a ghost or dead.");
                return false;
            }

            if(Character.Me.Combat || Character.Me.IsActuallyInCombat) {
                BPLLog("Not able to accept queue due to being in combat.");
                return false;
            }

            // Get any wow unit (if I have aggro or my pet has aggro)
            if(ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Any(unit => unit.Aggro || unit.PetAggro)) {
                BPLLog("Not able to accept queue due to aggro.");
                return false;
            }

            return true;
        }

        public static bool IsQueuePopped() {
            return Lua.GetReturnVal<bool>("return select(1, C_PetBattles.GetPVPMatchmakingInfo()) == 'proposal'", 0);
        }

        public static bool IsInQueue() {
            return Lua.GetReturnVal<bool>("return select(1, C_PetBattles.GetPVPMatchmakingInfo()) == 'queued'", 0);
        }

        public static void LeaveQueueCommand() {
            Lua.DoString("C_PetBattles.StopPVPMatchmaking()");
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================
    }
}
