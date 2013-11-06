/* PetJournal created by AknA and Wigglez */

#region Namespace
using System.Collections.Generic;
using Styx.WoWInternals;
#endregion

namespace BattlePetLeveler.Helpers {
    public class PetJournal {
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

        /// <summary>
        ///     Enables all pet sources in the filter menu.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.AddAllPetSourcesFilter
        /// </remarks>
        public static void AddAllPetSourcesFilter()
        {
            Lua.DoString("C_PetJournal.AddAllPetSourcesFilter()");
        }

        /// <summary>
        ///     Enables all pet types in the filter menu.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.AddAllPetTypesFilter
        /// </remarks>
        public static void AddAllPetTypesFilter() {
            Lua.DoString("C_PetJournal.AddAllPetTypesFilter()");
        }

        /// <summary>
        ///     Puts the pet into a cage.
        /// </summary>
        /// <param name="petID">String - Unique identifier for this specific pet</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.CagePetByID
        /// </remarks>
        public static void CagePetByID(string petID) {
            Lua.DoString(string.Format("C_PetJournal.CagePetByID('{0}')", petID));
        }

        /// <summary>
        ///     Clears all pet sources in the filter menu.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.ClearAllPetSourcesFilter
        /// </remarks>
        public static void ClearAllPetSourcesFilter() {
            Lua.DoString("C_PetJournal.ClearAllPetSourcesFilter()");
        }

        /// <summary>
        ///     Clears all pet types in the filter menu.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.ClearAllPetTypesFilter
        /// </remarks>
        public static void ClearAllPetTypesFilter() {
            Lua.DoString("C_PetJournal.ClearAllPetTypesFilter()");
        }

        /// <summary>
        ///     Clears the search box in the pet journal.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.ClearSearchFilter
        /// </remarks>
        public static void ClearSearchFilter() {
            Lua.DoString("C_PetJournal.ClearSearchFilter()");
        }

        /// <summary>
        ///     Returns pet species and GUID by pet name.
        /// </summary>
        /// <param name="petName">String - Name of the pet to find species/GUID of.</param>
        /// <returns>
        ///     <para>speciesId = Number - Species ID of the first battle pet (or species) with the specified name, nil if no such pet exists.</para>
        ///     <para>petGUID = String - GUID of the first battle pet collected by the player with the specified name, nil if the player has not collected any pets with the specified name.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.FindPetIDByName
        /// </remarks>
        public static List<string> FindPetIDByName(string petName) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.FindPetIDByName('{0}')", petName));
        }

        /// <summary>
        ///     Returns a chat link for the specified battle pet.
        /// </summary>
        /// <param name="petID">String - GUID specifying a battle pet in your collection.</param>
        /// <returns>
        ///     <para>link = String - A chat link to the specified battle pet; nil if there is no pet with the specified petID in your collection.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetBattlePetLink
        /// </remarks>
        public static int GetBattlePetLink(string petID) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetJournal.GetBattlePetLink('{0}')", petID), 0);
        }

        /// <summary>
        ///     Returns the number of collected battle pets of a particular species.
        /// </summary>
        /// <param name="speciesId">Number - Battle pet species ID to query, e.g. 635 for Adder battle pets.</param>
        /// <returns>
        ///     <para>numCollected = Number - Number of battle pets of the queried species the player has collected.</para>
        ///     <para>limit = Number - Maximum number of battle pets of the queried species the player may collect.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetNumCollectedInfo
        /// </remarks>
        public static List<string> GetNumCollectedInfo(int speciesId) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.GetNumCollectedInfo({0})", speciesId));
        }


        // Lua.DoString("C_PetBattles.ForfeitGame()");
        // return Lua.GetReturnVal<bool>("return C_PetBattles.ShouldShowPetSelect()", 0);
        // return Lua.GetReturnValues(string.Format("return C_PetBattles.GetAbilityState({0}, {1}, {2})", petOwner, petIndex, actionIndex));
        // return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetAbilityEffectInfo({0}, {1}, {2}, {3})", abilityID, turnIndex, effectIndex, effectName), 0);

        /*

        C_PetJournal.GetNumPets - This function is not yet documented
        C_PetJournal.GetNumPetSources - This function is not yet documented
        C_PetJournal.GetNumPetTypes - This function is not yet documented
        C_PetJournal.GetOwnedBattlePetString - This function is not yet documented
        C_PetJournal.GetPetAbilityInfo - This function is not yet documented
        C_PetJournal.GetPetAbilityList - This function is not yet documented
        C_PetJournal.GetPetInfoByIndex - This function is not yet documented
        C_PetJournal.GetPetInfoByPetID - Retreives information about a battle pet from its GUID
        C_PetJournal.GetPetInfoBySpeciesID - This function is not yet documented
        C_PetJournal.GetPetLoadOutInfo - Returns pet and spell IDs
        C_PetJournal.GetPetSortParameter - This function is not yet documented
        C_PetJournal.GetPetStats - Retrieves the stats of a battle pet from its GUID
        C_PetJournal.GetPetTeamAverageLevel - This function is not yet documented
        C_PetJournal.GetSummonedPetGUID - This function is not yet documented
        C_PetJournal.IsFindBattleEnabled - This function is not yet documented
        C_PetJournal.IsFlagFiltered - This function is not yet documented
        C_PetJournal.IsJournalReadOnly - This function is not yet documented
        C_PetJournal.IsJournalUnlocked - This function is not yet documented
        C_PetJournal.IsPetSourceFiltered - This function is not yet documented
        C_PetJournal.IsPetTypeFiltered - This function is not yet documented
        C_PetJournal.PetCanBeReleased - This function is not yet documented
        C_PetJournal.PetIsCapturable - This function is not yet documented
        C_PetJournal.PetIsFavorite - This function is not yet documented
        C_PetJournal.PetIsHurt - This function is not yet documented
        C_PetJournal.PetIsLockedForConvert - This function is not yet documented
        C_PetJournal.PetIsRevoked - This function is not yet documented
        C_PetJournal.PetIsSlotted - This function is not yet documented
        C_PetJournal.PetIsSummonable - This function is not yet documented
        C_PetJournal.PetIsTradable - This function is not yet documented
        C_PetJournal.PickupPet - This function is not yet documented
        C_PetJournal.ReleasePetByID - This function is not yet documented
        C_PetJournal.SetAbility - Set battle pet ability
        C_PetJournal.SetCustomName - This function is not yet documented
        C_PetJournal.SetFavorite - This function is not yet documented
        C_PetJournal.SetFlagFilter - This function is not yet documented
        C_PetJournal.SetPetLoadOutInfo - Setup battle pet team
        C_PetJournal.SetPetSortParameter - This function is not yet documented
        C_PetJournal.SetPetSourceFilter - This function is not yet documented
        C_PetJournal.SetPetTypeFilter - This function is not yet documented
        C_PetJournal.SetSearchFilter - This function is not yet documented
        C_PetJournal.SummonPetByGUID - This function is not yet documented
        C_PetJournal.SummonRandomPet - This function is not yet documented
        */

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================
    }
}
