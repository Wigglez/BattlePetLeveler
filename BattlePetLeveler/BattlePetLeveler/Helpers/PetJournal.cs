/* PetJournal created by AknA and Wigglez */

#region Namespace

using System;
using System.Collections.Generic;
using System.Data;
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
        public static void AddAllPetSourcesFilter() {
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

        /// <summary>
        ///     Returns information about the number of battle pets.
        /// </summary>
        /// <returns>
        ///     <para>numPets = Number - Total number of pets available</para>
        ///     <para>numOwned = Number - Number of pets currently owned</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetNumPets
        /// </remarks>
        public static List<string> GetNumPets() {
            return Lua.GetReturnValues("return C_PetJournal.GetNumPets()");
        }

        /// <summary>
        ///     Returns information about the number of pet sources.
        /// </summary>
        /// <returns>
        ///     <para>numSources = Number - Number of pet sources available</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetNumPetSources
        /// </remarks>
        public static int GetNumPetSources() {
            return Lua.GetReturnVal<int>("return C_PetJournal.GetNumPetSources()", 0);
        }

        /// <summary>
        ///     Returns information about the number of pet types.
        /// </summary>
        /// <returns>
        ///     <para>numTypes = Number - Number of pet types available</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetNumPetTypes
        /// </remarks>
        public static int GetNumPetTypes() {
            return Lua.GetReturnVal<int>("return C_PetJournal.GetNumPetTypes()", 0);
        }

        /// <summary>
        ///     Returns a string describing how many battle pets of a specific species you've collected.
        /// </summary>
        /// <param name="speciesID">Number - Battle pet species ID.</param>
        /// <returns>
        ///     <para>ownedString = String - a description of how many pets of this species you've collected, e.g. "|cFFFFD200Collected (1/3)", or nil if you haven't collected any.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetOwnedBattlePetString
        /// </remarks>
        public static int GetOwnedBattlePetString(int speciesID) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetJournal.GetAbilityEffectInfo({0})", speciesID), 0);
        }

        /// <summary>
        ///     Returns information about a battle pet ability.
        /// </summary>
        /// <param name="abilityID">Number - battle pet ability ID, as returned by C_PetJournal.GetPetAbilityList, e.g. 362 for [Howl].</param>
        /// <returns>
        ///     <para>name = String - Ability name, e.g. "Howl"</para>
        ///     <para>icon = String - Ability icon texture path, e.g. "INTERFACE\ICONS\ABILITY_SHAMAN_FREEDOMWOLF.BLP"</para>
        ///     <para>type = Number - Battle pet type this ability belongs to, as an index into the global PET_TYPE_SUFFIX, e.g. 8 for "Beast"</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetAbilityInfo
        /// </remarks>
        public static List<string> GetPetAbilityInfo(int abilityID) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.GetPetAbilityInfo({0})", abilityID));
        }

        /// <summary>
        ///     Returns pet battle abilities available to a particular battle pet species.
        /// </summary>
        /// <param name="speciesID">Number - Battle pet species ID to query the abilities of.</param>
        /// <param name="idTable">Optional Table - Table that will be used to return ability ID information; a new table will be created if this argument is omitted.</param>
        /// <param name="levelTable">Optional Table - Table that will be used to return ability level requirement information; a new table will be created if this argument is omitted.</param>
        /// <returns>
        ///     <para>idTable = Table - An array of ability IDs available to the battle pet species.</para>
        ///     <para>levelTable = Table - An array of levels at which the corresponding ability in the idTable becomes available to the species.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetAbilityList
        /// </remarks>
        public static List<string> GetPetAbilityList(int speciesID, DataTable idTable, DataTable levelTable) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.GetPetAbilityList({0}, {1}, {2})", speciesID, idTable, levelTable));
        }

        /// <summary>
        ///     Returns information about a battle pet.
        /// </summary>
        /// <param name="index">Number - Numeric index of the pet in the Pet Journal, ascending from 1.</param>
        /// <returns>
        ///     <para>petID = String - Unique identifier for this specific pet</para>
        ///     <para>speciesID = Number - Identifier for the pet species</para>
        ///     <para>owned = Boolean - Whether the pet is owned by the player</para>
        ///     <para>customName = String - Name assigned by the player or nil if unnamed</para>
        ///     <para>level = Number - The pet's current battle level</para>
        ///     <para>favorite = Boolean - Whether the pet is marked as a favorite</para>
        ///     <para>isRevoked = Boolean - True if the pet is revoked; false otherwise.</para>
        ///     <para>speciesName = String - Name of the pet species ("Albino Snake", "Blue Mini Jouster", etc.)</para>
        ///     <para>icon = String - Full path for the species' icon</para>
        ///     <para>petType = Number - Index of the species' petType.</para>
        ///     <para>companionID = Number - NPC ID for the summoned companion pet.</para>
        ///     <para>tooltip = String - Section of the tooltip that provides location information</para>
        ///     <para>description = String - Section of the tooltip that provides pet description ("flavor text")</para>
        ///     <para>isWild = Boolean - For pets in the player's possession, true if the pet was caught in the wild. For pets not in the player's possession, true if the pet can be caught in the wild.</para>
        ///     <para>canBattle = Boolean - True if this pet can be used in battles, false otherwise.</para>
        ///     <para>isTradeable = Boolean - True if this pet can be traded, false otherwise.</para>
        ///     <para>isUnique = Boolean - True if this pet is unique, false otherwise.</para>
        ///     <para>obtainable = Boolean - True if this pet can be obtained, false otherwise (only false for tamer pets and developer/test pets).</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetInfoByIndex
        /// </remarks>
        public static List<string> GetPetInfoByIndex(int index) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.GetPetInfoByIndex({0})", index));
        }

        /// <summary>
        ///     Returns information about a battle pet.
        /// </summary>
        /// <param name="petID">String - Unique identifier for this specific pet</param>
        /// <returns>
        ///     <para>speciesID = Number - Identifier for the pet species</para>
        ///     <para>customName = String - Name assigned by the player or nil if unnamed</para>
        ///     <para>level = Number - The pet's current battle level</para>
        ///     <para>xp = Number - The pet's current xp</para>
        ///     <para>maxXp = Number - The pet's maximum xp</para>
        ///     <para>displayID = Number - The display ID of the pet</para>
        ///     <para>isFavorite = Boolean - Whether the pet is marked as a favorite</para>
        ///     <para>name = String - Name of the pet species ("Albino Snake", "Blue Mini Jouster", etc.)</para>
        ///     <para>icon = String - Full path for the species' icon</para>
        ///     <para>petType = Number - Index of the species' petType.</para>
        ///     <para>creatureID = Number - NPC ID for the summoned companion pet</para>
        ///     <para>sourceText = String - Section of the tooltip that provides location information</para>
        ///     <para>description = String - Section of the tooltip that provides pet description ("flavor text")</para>
        ///     <para>isWild = Boolean - For pets in the player's possession, true if the pet was caught in the wild. For pets not in the player's possession, true if the pet can be caught in the wild.</para>
        ///     <para>canBattle = Boolean - True if this pet can be used in battles, false otherwise.</para>
        ///     <para>tradeable = Boolean - True if this pet can be traded, false otherwise.</para>
        ///     <para>unique = Boolean - True if this pet is unique, false otherwise.</para>
        ///     <para>obtainable = Boolean - True if this pet can be obtained, false otherwise (only false for tamer pets and developer/test pets).</para>
        /// </returns>
        /// <remarks>
        ///     <para>Information about the player's battle pets is available after UPDATE_SUMMONPETS_ACTION has fired.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetInfoByPetID
        /// </remarks>
        public static List<string> GetPetInfoByPetID(int petID) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.GetPetInfoByPetID({0})", petID));
        }

        /// <summary>
        ///     Returns information about a pet species.
        /// </summary>
        /// <param name="speciesID">Number - identifier for the pet species</param>
        /// <returns>
        ///     <para>speciesName = String - Name of the pet species ("Albino Snake", "Blue Mini Jouster", etc.)</para>
        ///     <para>speciesIcon = String - Full path for the species' icon</para>
        ///     <para>petType = Number - Index of the species' pet type.</para>
        ///     <para>companionID = Number - NPC ID for the summoned companion pet.</para>
        ///     <para>tooltipSource = String - Section of the species tooltip that provides location information</para>
        ///     <para>tooltipDescription = String - Section of the species tooltip that provides pet description ("flavor text")</para>
        ///     <para>isWild = Boolean - For pets in the player's possession, true if the pet was caught in the wild. For pets not in the player's possession, true if the pet can be caught in the wild.</para>
        ///     <para>canBattle = Boolean - True if this pet can be used in battles, false otherwise.</para>
        ///     <para>isTradeable = Boolean - True if this pet can be traded, false otherwise.</para>
        ///     <para>isUnique = Boolean - True if this pet is unique, false otherwise.</para>
        ///     <para>obtainable = Boolean - True if this pet can be obtained, false otherwise (only false for tamer pets and developer/test pets).</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetInfoBySpeciesID
        /// </remarks>
        public static List<string> GetPetInfoBySpeciesID(int speciesID) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.GetPetInfoBySpeciesID({0})", speciesID));
        }

        /// <summary>
        ///     Returns information about a slot in your battle pet team.
        /// </summary>
        /// <param name="slotIndex">Number - battle pet slot index, an integer between 1 and 3. Values outside this range throw an error.</param>
        /// <returns>
        ///     <para>petGUID = String - GUID of the battle pet currently in this slot.</para>
        ///     <para>ability1 = Number - Ability ID of the first (level 1/10) ability selected for the battle pet in this slot.</para>
        ///     <para>ability2 = Number - Ability ID of the second (level 2/15) ability selected for the battle pet in this slot.</para>
        ///     <para>ability3 = Number - Ability ID of the third (level 4/20) ability selected for the battle pet in this slot.</para>
        ///     <para>locked = Boolean - false if the player can place a battle pet in this slot, true otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     <para>Ability IDs are returned even for slots that are not yet unlocked by a low-level battle pet.</para>
        ///     <para>Slots are locked until the player has earned the necessary achievement/skill. The first slot is unlocked by learning [Battle Pet Training].</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetLoadOutInfo
        /// </remarks>
        public static List<string> GetPetLoadOutInfo(int slotIndex) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.GetPetLoadOutInfo({0})", slotIndex));
        }

        /// <summary>
        ///     Returns the index of the currently active sort parameter.
        /// </summary>
        /// <returns>
        ///     <para>sortParameter = Number - currently active ordering for C_PetJournal.GetPetInfoByIndex, e.g. 1 for sorting by name.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetSortParameter
        /// </remarks>
        public static int GetPetSortParameter() {
            return Lua.GetReturnVal<int>("return C_PetJournal.GetPetSortParameter()", 0);
        }

        /// <summary>
        ///     Returns a pet's stats from the Pet Journal
        /// </summary>
        /// <param name="petID">String - GUID of pet in Pet Journal (different than speciesID and displayID)</param>
        /// <returns>
        ///     <para>health = Number - Current health of the pet. Zero or negative if the pet is dead.</para>
        ///     <para>maxHealth = Number - Maximum health of the pet</para>
        ///     <para>power = Number</para>
        ///     <para>speed = Number</para>
        ///     <para>rarity = Number - 1: "Poor", 2: "Common", 3: "Uncommon", 4: "Rare", 5: "Epic", 6: "Legendary"</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetStats
        /// </remarks>
        public static List<string> GetPetStats(string petID) {
            return Lua.GetReturnValues(string.Format("return C_PetJournal.GetPetStats('{0}')", petID));
        }

        /// <summary>
        ///     Returns the average level of pets in your battle pet team.
        /// </summary>
        /// <returns>
        ///     <para>avgLevel = Number - Average level of pets in your current battle pet team.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.GetPetTeamAverageLevel
        /// </remarks>
        public static int GetPetTeamAverageLevel() {
            return Lua.GetReturnVal<int>("return C_PetJournal.GetPetTeamAverageLevel()", 0);
        }

        /// <summary>
        ///     Returns information about a battle pet.
        /// </summary>
        /// <returns>
        ///     <para>summonedPetGUID = String - GUID identifying the currently-summoned battle pet, or nil if no battle pet is summoned.</para>
        /// </returns>
        /// <remarks>
        ///     <para>Blizzard has moved all petIDs over to the "petGUID" system, but left all of their functions using the petID terminology (not the petGUID terminology) except for this one. For consistency, the term "petID" should continue to be used.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.GetSummonedPetGUID
        /// </remarks>
        public static string GetSummonedPetGUID() {
            return Lua.GetReturnVal<string>("return C_PetJournal.GetSummonedPetGUID()", 0);
        }

        /// <summary>
        ///     Returns whether the player can queue for PvP pet battles.
        /// </summary>
        /// <returns>
        ///     <para>isEnabled = Boolean - true if you can queue for a PvP pet battle, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     <para>You may not queue for PvP pet battles until you've unlocked all three battle pet slots.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.IsFindBattleEnabled
        /// </remarks>
        public static bool IsFindBattleEnabled() {
            return Lua.GetReturnVal<bool>("return C_PetJournal.IsFindBattleEnabled()", 0);
        }

        /// <summary>
        ///     Returns true if the selected flag is unchecked.
        /// </summary>
        /// <param name="flag">Number - Bitfield for each flag (LE_PET_JOURNAL_FLAG_COLLECTED: Pets you have collected, LE_PET_JOURNAL_FLAG_FAVORITES: Pets you have set as favorites, LE_PET_JOURNAL_FLAG_NOT_COLLECTED: Pets you have not collected)</param>
        /// <returns>
        ///     <para>isFiltered = Boolean - True if the filter is unchecked, false if the filter is checked</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.IsFlagFiltered
        /// </remarks>
        public static bool IsFlagFiltered(int flag) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.IsFlagFiltered({0})", flag), 0);
        }

        /// <summary>
        ///     Returns true if the pet source is unchecked.
        /// </summary>
        /// <param name="index">Number - Index (from 1 to GetNumPetSources()) of all available pet sources</param>
        /// <returns>
        ///     <para>isFiltered = Boolean - True if the filter is unchecked, false if the filter is checked</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.IsPetSourceFiltered
        /// </remarks>
        public static bool IsPetSourceFiltered(int index) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.IsPetSourceFiltered({0})", index), 0);
        }

        /// <summary>
        ///     Returns true if the pet type is unchecked.
        /// </summary>
        /// <param name="index">Number - Index (from 1 to GetNumPetTypes()) of all available pet types</param>
        /// <returns>
        ///     <para>isFiltered = Boolean - True if the filter is unchecked, false if the filter is checked</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.IsPetTypeFiltered
        /// </remarks>
        public static bool IsPetTypeFiltered(int index) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.IsPetTypeFiltered({0})", index), 0);
        }

        /// <summary>
        ///     Returns true if you can release the pet.
        /// </summary>
        /// <param name="petID">String - Unique identifier for this specific pet</param>
        /// <returns>
        ///     <para>canRelease = Boolean - True if you can release the pet</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.PetCanBeReleased
        /// </remarks>
        public static bool PetCanBeReleased(string petID) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.PetCanBeReleased('{0}')", petID), 0);
        }

        /// <summary>
        ///     Returns whether a battle pet in your collection is capturable (i.e. a wild pet).
        /// </summary>
        /// <param name="petID">String - GUID of a battle pet in your collection, e.g. "0x0000000000067932"</param>
        /// <returns>
        ///     <para>isCapturable = Boolean - true if the pet can be captured through wild pet battles, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.PetIsCapturable
        /// </remarks>
        public static bool PetIsCapturable(string petID) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.PetIsCapturable('{0}')", petID), 0);
        }

        /// <summary>
        ///     Returns whether a battle pet in your collection is marked as a favorite.
        /// </summary>
        /// <param name="petGUID">String - GUID of a battle pet in your collection.</param>
        /// <returns>
        ///     <para>isFavorite = Boolean - true if this pet is marked as a favorite, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.PetIsFavorite
        /// </remarks>
        public static bool PetIsFavorite(string petGUID) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.PetIsFavorite('{0}')", petGUID), 0);
        }

        /// <summary>
        ///     Returns whether the specified battle pet is injured and unable to participate in battles.
        /// </summary>
        /// <param name="petID">String - Battle pet GUID of a pet in your collection.</param>
        /// <returns>
        ///     <para>isHurt = Boolean - true if the specified pet is injured and cannot fight, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     <para>Pets are injured by dying in pet battles, and may be cured by using [Revive Battle Pets], a [Battle Pet Bandage], or by visiting a Stable master.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.PetIsHurt
        /// </remarks>
        public static bool PetIsHurt(string petID) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.PetIsHurt('{0}')", petID), 0);
        }

        /// <summary>
        ///     Returns whether or not the pet is revoked.
        /// </summary>
        /// <param name="petID">String - Unique identifier for this specific pet</param>
        /// <returns>
        ///     <para>isRevoked = Boolean - true if the pet is revoked.</para>
        /// </returns>
        /// <remarks>
        ///     <para>Revoked pets are pets that have been stripped from the player in every fashion except for their name. They remain in the Pet Journal, but they cannot be summoned or used in battle. In addition, the rarity border and level icon will not appear around and over the pet's name in the Pet Journal's scrolling list.</para>
        ///     <para> </para>
        ///     <para>This function returns true for Blizzard Pet Store pets on the PTR, which suggests that isRevoked is only ever true for pets that cost money and have not been authorized for a specific World of Warcraft account. This mechanic is likely in place to prevent characters from transferring with an unused Blizzard Pet Store pet to a different account that does not have access to that pet.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.PetIsRevoked
        /// </remarks>
        public static bool PetIsRevoked(string petID) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.PetIsRevoked('{0}')", petID), 0);
        }

        /// <summary>
        ///     Returns whether a battle pet in your collection is part of your current battle pet team.
        /// </summary>
        /// <param name="petID">String - GUID of a battle pet in your collection.</param>
        /// <returns>
        ///     <para>isSlotted = Boolean - true if the battle pet is part of your current team (loadout), false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.PetIsSlotted
        /// </remarks>
        public static bool PetIsSlotted(string petID) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.PetIsSlotted('{0}')", petID), 0);
        }

        /// <summary>
        ///     Returns true if you can summon this pet.
        /// </summary>
        /// <param name="petID">String - Unique identifier for this specific pet</param>
        /// <returns>
        ///     <para>isSummonable = Boolean - Returns true if you can summon this pet.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.PetIsSummonable
        /// </remarks>
        public static bool PetIsSummonable(string petID) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.PetIsSummonable('{0}')", petID), 0);
        }

        /// <summary>
        ///     Returns whether or not a pet from the Pet Journal is tradable.
        /// </summary>
        /// <param name="petID">String - GUID of pet in Pet Journal (different than speciesID and displayID)</param>
        /// <returns>
        ///     <para>isTradable = Boolean</para>
        /// </returns>
        /// <remarks>
        ///     <para>This is used by Blizzard to generate the red "Not Tradable" text on Pet Journal tooltips when false or nil.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.PetIsTradable
        /// </remarks>
        public static bool PetIsTradable(string petID) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetJournal.PetIsTradable('{0}')", petID), 0);
        }

        /// <summary>
        ///     Places a battle pet onto the mouse cursor.
        /// </summary>
        /// <param name="petID">String - GUID of a battle pet in your collection.</param>
        /// <remarks>
        ///     <para>The function places a specific battle pet in your collection on your cursor.</para>
        ///     <para>Battle pets on your cursor can be placed on your action bars.</para>
        ///     <para>Attempting to pick up a battle pet that's already on the cursor clears the cursor instead.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.PickupPet
        /// </remarks>
        public static void PickupPet(string petID) {
            Lua.DoString(string.Format("C_PetJournal.PickupPet('{0}')", petID));
        }

        /// <summary>
        ///     Releases the pet.
        /// </summary>
        /// <param name="petID">String - Unique identifier for this specific pet</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.ReleasePetByID
        /// </remarks>
        public static void ReleasePetByID(string petID) {
            Lua.DoString(string.Format("C_PetJournal.ReleasePetByID('{0}')", petID));
        }

        /// <summary>
        ///     Selects a battle pet ability to make available in battle.
        /// </summary>
        /// <param name="slotIndex">Number - battle pet slot index, integer between 1 and 3.</param>
        /// <param name="spellIndex">Number - ability slot index, integer between 1 and 3.</param>
        /// <param name="petSpellID">Number - pet ability ID to select for the spellIndex slot of the pet in the slotIndex battle pet slot.</param>
        /// <remarks>
        ///     <para>While this function allows you to select abilities outside of those specified by C_PetJournal.GetPetAbilityList (or assign those abilities to other ability slots), those abilities will not be usable in pet battles.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.SetAbility
        /// </remarks>
        public static void SetAbility(int slotIndex, int spellIndex, int petSpellID) {
            Lua.DoString(string.Format("C_PetJournal.SetAbility({0}, {1}, {2})", slotIndex, spellIndex, petSpellID));
        }

        /// <summary>
        ///     Sets a custom name for the pet.
        /// </summary>
        /// <param name="petID">String - Unique identifier for this specific pet</param>
        /// <param name="customName">String - Custom name for the pet</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.SetCustomName
        /// </remarks>
        public static void SetCustomName(int petID, int customName) {
            Lua.DoString(string.Format("C_PetJournal.SetCustomName({0}, {1})", petID, customName));
        }

        /// <summary>
        ///     Sets (or clears) the pet as a favorite.
        /// </summary>
        /// <param name="petID">String - Unique identifier for this specific pet</param>
        /// <param name="value">Number - 0: Pet is not a favorite, 1: Pet is a favorite</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.SetFavorite
        /// </remarks>
        public static void SetFavorite(int petID, int value) {
            Lua.DoString(string.Format("C_PetJournal.SetFavorite({0}, {1})", petID, value));
        }

        /// <summary>
        ///     Sets the flags in the filter menu.
        /// </summary>
        /// <param name="flag">Number - Bitfield for each flag (LE_PET_JOURNAL_FLAG_COLLECTED: Pets you have collected, LE_PET_JOURNAL_FLAG_FAVORITES: Pets you have set as favorites, LE_PET_JOURNAL_FLAG_NOT_COLLECTED: Pets you have not collected)</param>
        /// <param name="value">Boolean - True to set the flag, false to clear the flag</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.SetFlagFilter
        /// </remarks>
        public static void SetFlagFilter(int flag, bool value) {
            Lua.DoString(string.Format("C_PetJournal.SetFlagFilter({0}, {1})", flag, value));
        }

        /// <summary>
        ///     Places the specified pet into a battle pet slot.
        /// </summary>
        /// <param name="slotIndex">Number - Battle pet slot index, integer between 1 and 3.</param>
        /// <param name="petID">String - Battle pet GUID of a pet in your collection to move into the battle pet slot.</param>
        /// <remarks>
        ///     <para>If the pet specified by petID is already in a battle pet slot, the pets are exchanged.</para>
        ///     <para>When a pet is moved from the journal to a battle pet slot, its abilities are reset to the level 1/2/4 choices.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.SetPetLoadOutInfo
        /// </remarks>
        public static void SetPetLoadOutInfo(int slotIndex, string petID) {
            Lua.DoString(string.Format("C_PetJournal.SetPetLoadOutInfo({0}, '{1}')", slotIndex, petID));
        }

        /// <summary>
        ///     Changes the battle pet ordering in the pet journal.
        /// </summary>
        /// <param name="sortParameter">Number - Index of the ordering type that should be applied to C_PetJournal.GetPetInfoByIndex returns; one of the following global numeric values: LE_SORT_BY_NAME, LE_SORT_BY_LEVEL, LE_SORT_BY_RARITY, LE_SORT_BY_PETTYPE</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.SetPetSortParameter
        /// </remarks>
        public static void SetPetSortParameter(int sortParameter) {
            Lua.DoString(string.Format("C_PetJournal.SetPetSortParameter({0})", sortParameter));
        }

        /// <summary>
        ///     Sets the pet source in the filter menu.
        /// </summary>
        /// <param name="index">Number - Index (from 1 to GetNumPetSources()) of all available pet sources</param>
        /// <param name="value">Boolean - True to set the pet source, false to clear the pet source</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.SetPetSourceFilter
        /// </remarks>
        public static void SetPetSourceFilter(int index, bool value) {
            Lua.DoString(string.Format("C_PetJournal.SetPetSourceFilter({0}, {1})", index, value));
        }

        /// <summary>
        ///     Sets the pet type in the filter menu.
        /// </summary>
        /// <param name="index">Number - Index (from 1 to GetNumPetTypes()) of all available pet types</param>
        /// <param name="value">Boolean - True to set the pet type, false to clear the pet type</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.SetPetTypeFilter
        /// </remarks>
        public static void SetPetTypeFilter(int index, bool value) {
            Lua.DoString(string.Format("C_PetJournal.SetPetTypeFilter({0}, {1})", index, value));
        }

        /// <summary>
        ///     Sets the search filter in the pet journal.
        /// </summary>
        /// <param name="text">String - Search text for the pet journal</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetJournal.SetSearchFilter
        /// </remarks>
        public static void SetSearchFilter(string text) {
            Lua.DoString(string.Format("C_PetJournal.SetSearchFilter('{0}')", text));
        }

        /// <summary>
        ///     Summons (or dismisses) a pet.
        /// </summary>
        /// <param name="petID">String - GUID of the battle pet to summon. If the pet is already summoned, it will be dismissed.</param>
        /// <remarks>
        ///     <para>You can dismiss the currently-summoned battle pet by running:</para>
        ///     <code>
        ///         C_PetJournal.SummonPetByGUID(C_PetJournal.GetSummonedPetGUID())
        ///     </code>
        ///     <para>Note that this will throw an error if you do not have a pet summoned.</para>
        ///     <para>Blizzard has moved all petIDs over to the "petGUID" system, but left all of their functions using the petID terminology (not the petGUID terminology) except for this one. For consistency, the term "petID" should continue to be used.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.SummonPetByGUID
        /// </remarks>
        public static void SummonPetByGUID(string petID) {
            Lua.DoString(string.Format("C_PetJournal.SummonPetByGUID('{0}')", petID));
        }

        /// <summary>
        ///     Summons a random battle pet companion.
        /// </summary>
        /// <param name="allPets">Boolean - true to summon any pet, false to summon one of your favorite pets.</param>
        /// <remarks>
        ///     <para>This function is also available via slash commands: <c>/randompet</c> or <c>/randomfavoritepet</c></para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetJournal.SetSearchFilter
        /// </remarks>
        public static void SetSearchFilter(bool allPets) {
            Lua.DoString(string.Format("C_PetJournal.SetSearchFilter({0})", allPets));
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================
    }
}
