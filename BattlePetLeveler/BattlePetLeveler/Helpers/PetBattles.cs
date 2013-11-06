/* PetBattles created by AknA and Wigglez */

#region Namespaces
using System.Collections.Generic;
using Styx.WoWInternals;
#endregion

namespace BattlePetLeveler.Helpers {
    public class PetBattles {
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
        ///     Accepts a pending PvP duel invitation.
        /// </summary> 
        /// <remarks>
        ///     <para>Called in the StaticPopup PET_BATTLE_PVP_DUEL_REQUESTED.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.AcceptPVPDuel
        /// </remarks>
        public static void AcceptPVPDuel() {
            Lua.DoString("C_PetBattles.AcceptPVPDuel()");
        }

        /// <summary>
        ///     Accepts a pending PvP match from the queue.
        /// </summary> 
        /// <remarks>
        ///     <para>Called by the button PetBattleQueueReadyFrame.AcceptButton:Click().</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.AcceptQueuedPVPMatch
        /// </remarks>
        public static void AcceptQueuedPVPMatch() {
            Lua.DoString("C_PetBattles.AcceptQueuedPVPMatch()");
        }

        /// <summary>
        ///     Returns whether or not the active pet can be swapped out in the pet battle.
        /// </summary> 
        /// <returns>
        ///     <para>usable = Boolean - True if active pet can swap out, false otherwise</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.CanActivePetSwapOut
        /// </remarks>
        public static bool CanActivePetSwapOut() {
            return Lua.GetReturnVal<bool>("return C_PetBattles.CanActivePetSwapOut()", 0);
        }

        /// <summary>
        ///     Declines a pending PvP pet battle duel invitation.
        /// </summary> 
        /// <remarks>
        ///     <para>Called in the StaticPopup PET_BATTLE_PVP_DUEL_REQUESTED.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.CancelPVPDuel
        /// </remarks>
        public static void CancelPVPDuel() {
            Lua.DoString("C_PetBattles.CancelPVPDuel");
        }

        /// <summary>
        ///     Returns whether or not the pet can swap in to the active position.
        /// </summary>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>usable = Boolean - True if the pet is able to swap in, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.CanPetSwapIn
        /// </remarks>
        public static bool CanPetSwapIn(int petIndex) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetBattles.CanPetSwapIn({0})", petIndex), 0);
        }

        /// <summary>
        ///     Changes the active pet out for a different pet in a pet battle.
        /// </summary>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.ChangePet
        /// </remarks>
        public static void ChangePet(int petIndex) {
            Lua.DoString(string.Format("C_PetBattles.ChangePet({0})", petIndex));
        }

        /// <summary>
        ///     Declines a pending PvP match from the queue.
        /// </summary> 
        /// <remarks>
        ///     <para>Called by the button PetBattleQueueReadyFrame.DeclineButton:Click().</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.DeclineQueuedPVPMatch
        /// </remarks>
        public static void DeclineQueuedPVPMatch() {
            Lua.DoString("C_PetBattles.DeclineQueuedPVPMatch()");
        }

        /// <summary>
        ///     Forfeits the current Pet Battle immediately.
        /// </summary>
        /// <remarks>
        ///     <para>Triggers Events:</para>
        ///     <para>PET_BATTLE_FINAL_ROUND, immediately after entered</para>
        ///     <para>PET_BATTLE_OVER, immediately after entered</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.ForfeitGame
        /// </remarks>
        public static void ForfeitGame() {
            Lua.DoString("C_PetBattles.ForfeitGame()");
        }

        /// <summary>
        ///     Returns the effect info for a pet's ability.
        /// </summary>
        /// <param name="abilityID">The ID of this ability</param>
        /// <param name="turnIndex">The turn in question for this ability. This can be obtained from C_PetBattles.GetAbilityProcTurnIndex.</param>
        /// <param name="effectIndex">The effect in question for this ability. This can be obtained from the unparsed description for this ability with extensive string manipulation, but currently no ability has more than 7 effects, so loops work too.</param>
        /// <param name="effectName">One of the effect names from C_PetBattles.GetAllEffectNames.</param>
        /// <returns>
        ///     <para>value = Number - The information you requested for a specific element of a certain effect of a certain ability at a specific turn.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetAbilityEffectInfo
        /// </remarks>
        public static int GetAbilityEffectInfo(int abilityID, int turnIndex, int effectIndex, string effectName) {
            return
                Lua.GetReturnVal<int>(
                    string.Format("return C_PetBattles.GetAbilityEffectInfo({0}, {1}, {2}, {3})", abilityID, turnIndex,
                        effectIndex, effectName), 0);
        }

        /// <summary>
        ///     Returns ability information for an ability in a slot on a pet that you own.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <param name="abilityIndex">Number - Accepted values are 1-3, corresponding to the ability in the slot with that number.</param>
        /// <returns>
        ///     <para>id = Number - The ID of the ability returned back.</para>
        ///     <para>name = String - The name of the ability.</para>
        ///     <para>icon = String - The full path to the ability's icon.</para>
        ///     <para>maxCooldown - Number = The normal cooldown period for the ability.</para>
        ///     <para>unparsedDescription = String - The ability's description in its pure and unparsed form.</para>
        ///     <para>numTurns - Number = Duration of the ability. This is typically 1, but some abilities last multiple rounds.</para>
        ///     <para>petType - Number = Returned values are 1-10, based on the pet's type.</para>
        ///     <para>noStrongWeakHints = Boolean - True if the ability should not show Strong/Weak indicators, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetAbilityInfo
        /// </remarks>
        public static List<string> GetAbilityInfo(int petOwner, int petIndex, int abilityIndex) {
            return
                Lua.GetReturnValues(string.Format("return C_PetBattles.GetAbilityInfo({0}, {1}, {2})", petOwner,
                    petIndex, abilityIndex));
        }

        /// <summary>
        ///     Returns very detailed information about a specific ability.
        /// </summary>
        /// <param name="abilityID">Number - ID of the ability.</param>
        /// <returns>
        ///     <para>id = Number - The ID of the ability returned back.</para>
        ///     <para>name = String - The name of the ability.</para>
        ///     <para>icon = String - The full path to the ability's icon.</para>
        ///     <para>maxCooldown = Number - The normal cooldown period for the ability.</para>
        ///     <para>unparsedDescription - String - The ability's description in its pure and unparsed form.</para>
        ///     <para>numTurns = Number - Duration of the ability. This is typically 1, but some abilities last multiple rounds.</para>
        ///     <para>petType = Number - Returned values are 1-10, based on the pet's type.</para>
        ///     <para>noStrongWeakHints = Boolean - True if the ability should not show Strong/Weak indicators, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     <para>Despite falling under the realm of C_PetBattles, this function is incredibly useful outside Pet Battles too because it provides FAR more information than its C_PetJournal counterpart - C_PetJournal.GetPetAbilityInfo.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.GetAbilityInfoByID
        /// </remarks>
        public static List<string> GetAbilityInfoByID(int abilityID) {
            return Lua.GetReturnValues(string.Format("return C_PetBattles.GetAbilityInfo({0})", abilityID));
        }

        /// <summary>
        ///     Returns the turn index for a specific ability and proc type.
        /// </summary>
        /// <param name="abilityID">Number - ID of the ability.</param>
        /// <param name="procType">Number - Index corresponding to a proc type.</param>
        /// <returns>
        ///     <para>turnIndex = Number - Number of rounds that the ability has been in play.</para>
        /// </returns>
        /// <remarks>
        ///     <para>Proc types are defined in FrameXML/Constants.lua. They are currently:</para>
        ///     <para>PET_BATTLE_EVENT_ON_APPLY = 0;</para>
        ///     <para>PET_BATTLE_EVENT_ON_DAMAGE_TAKEN = 1</para>
        ///     <para>PET_BATTLE_EVENT_ON_DAMAGE_DEALT = 2</para>
        ///     <para>PET_BATTLE_EVENT_ON_HEAL_TAKEN = 3</para>
        ///     <para>PET_BATTLE_EVENT_ON_HEAL_DEALT = 4</para>
        ///     <para>PET_BATTLE_EVENT_ON_AURA_REMOVED = 5</para>
        ///     <para>PET_BATTLE_EVENT_ON_ROUND_START = 6</para>
        ///     <para>PET_BATTLE_EVENT_ON_ROUND_END = 7</para>
        ///     <para>PET_BATTLE_EVENT_ON_TURN = 8</para>
        ///     <para>PET_BATTLE_EVENT_ON_ABILITY = 9</para>
        ///     <para>PET_BATTLE_EVENT_ON_SWAP_IN = 10</para>
        ///     <para>PET_BATTLE_EVENT_ON_SWAP_OUT = 11</para>
        ///     <para>Since there are only 12 possible values for procType, you can usually afford to just do a loop such as for procType = 0, 11 do instead of finding out the actual procType.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.GetAbilityProcTurnIndex
        /// </remarks>
        public static int GetAbilityProcTurnIndex(int abilityID, int procType) {
            return
                Lua.GetReturnVal<int>(
                    string.Format("return C_PetBattles.GetAbilityProcTurnIndex({0}, {1})", abilityID, procType), 0);
        }

        /// <summary>
        ///     Returns information about a pet's ability's current state in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <param name="actionIndex">Number - Accepted values are 1-3, corresponding to the ability buttons from left to right.</param>
        /// <returns>
        ///     <para>isUsable = Boolean - if ability can be used</para>
        ///     <para>currentCooldown = Number - turns until ability can be used (if not usable due to cooldown)</para>
        ///     <para>currentLockdown = Number - turns until ability can be used (if not usable due to lockdowns, such as [Nevermore])</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetAbilityState
        /// </remarks>
        public static List<string> GetAbilityState(int petOwner, int petIndex, int actionIndex) {
            return
                Lua.GetReturnValues(string.Format("return C_PetBattles.GetAbilityState({0}, {1}, {2})", petOwner,
                    petIndex, actionIndex));
        }

        /// <summary>
        ///     Returns the modification state for an ability.
        /// </summary>
        /// <param name="abilityID">Number - ID of the ability.</param>
        /// <param name="stateID">Number - ID of a state for Pet Battles.</param>
        /// <returns>
        ///     <para>abilityStateMod = Number - The ability's modification state.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetAbilityStateModification
        /// </remarks>
        public static int GetAbilityStateModification(int abilityID, int stateID) {
            return
                Lua.GetReturnVal<int>(
                    string.Format("return C_PetBattles.GetAbilityStateModification({0}, {1})", abilityID, stateID), 0);
        }

        /// <summary>
        ///     Returns the index of the active pet for the specified owner in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <returns>
        ///     <para>petIndex = Number - Returned values are always 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetActivePet
        /// </remarks>
        public static int GetActivePet(int petOwner) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetActivePet({0})", petOwner), 0);
        }

        /// <summary>
        ///     Returns the name of an effect parameter for Pet Battles.
        /// </summary>
        /// <returns>
        ///     <para>effectParamString1 = accuracy</para>
        ///     <para>effectParamString2 = auraabilityid</para>
        ///     <para>effectParamString3 = auraid</para>
        ///     <para>effectParamString4 = basechancetosucceed</para>
        ///     <para>effectParamString5 = bonuspoints</para>
        ///     <para>effectParamString6 = bonusstate</para>
        ///     <para>effectParamString7 = boost</para>
        ///     <para>effectParamString8 = casterimmunestate</para>
        ///     <para>effectParamString9 = casterstate</para>
        ///     <para>effectParamString10 = casterstatevalue</para>
        ///     <para>effectParamString11 = chainfailure</para>
        ///     <para>effectParamString12 = chance</para>
        ///     <para>effectParamString13 = dontmiss</para>
        ///     <para>effectParamString14 = duration</para>
        ///     <para>effectParamString15 = duration2</para>
        ///     <para>effectParamString16 = evenmorepoints</para>
        ///     <para>effectParamString17 = healthpercentage</para>
        ///     <para>effectParamString18 = healthpercentthreshold</para>
        ///     <para>effectParamString19 = increasepertoss</para>
        ///     <para>effectParamString20 = index</para>
        ///     <para>effectParamString21 = isperiodic</para>
        ///     <para>effectParamString22 = lockduration</para>
        ///     <para>effectParamString23 = maxallowed</para>
        ///     <para>effectParamString24 = maxpoints</para>
        ///     <para>effectParamString25 = morepoints</para>
        ///     <para>effectParamString26 = newduration</para>
        ///     <para>effectParamString27 = none</para>
        ///     <para>effectParamString28 = overrideindex</para>
        ///     <para>effectParamString29 = percentage</para>
        ///     <para>effectParamString30 = points</para>
        ///     <para>effectParamString31 = pointsincreaseperuse</para>
        ///     <para>effectParamString32 = pointsmax</para>
        ///     <para>effectParamString33 = reportfailsasimmune</para>
        ///     <para>effectParamString34 = requiredcasterpettype</para>
        ///     <para>effectParamString35 = requiredcasterstate</para>
        ///     <para>effectParamString36 = requiredtargetpettype</para>
        ///     <para>effectParamString37 = requiredtargetstate</para>
        ///     <para>effectParamString38 = state</para>
        ///     <para>effectParamString39 = statechange</para>
        ///     <para>effectParamString40 = statemax</para>
        ///     <para>effectParamString41 = statemin</para>
        ///     <para>effectParamString42 = statepoints</para>
        ///     <para>effectParamString43 = statetomultiplyagainst</para>
        ///     <para>effectParamString44 = statetotriggermaxpoints</para>
        ///     <para>effectParamString45 = statevalue</para>
        ///     <para>effectParamString46 = targetimmunestate</para>
        ///     <para>effectParamString47 = targetstate</para>
        ///     <para>effectParamString48 = targetstatevalue</para>
        ///     <para>effectParamString49 = targetteststate</para>
        ///     <para>effectParamString50 = targetteststatevalue</para>
        ///     <para>effectParamString51 = turnstoadvance</para>
        ///     <para>effectParamString52 = unusedweatherstate</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetAllEffectNames
        /// </remarks>
        public static List<string> GetAllEffectNames() {
            return Lua.GetReturnValues("return C_PetBattles.GetAllEffectNames()");
        }

        /// <summary>
        ///     Returns all state IDs.
        /// </summary>
        /// <param name="stateEnv">Optional Table - This table will be filled with all known state IDs, added to the table's index by state name.</param>
        /// <returns>
        ///     <para>Optional Numbers(s) - Each return is the number of a different state value type for Pet Battles.</para>
        ///     <para>stateID1 = STATE_Is_Dead</para>
        ///     <para>stateID2 = STATE_maxHealthBonus</para>
        ///     <para>stateID3 = STATE_Internal_InitialHealth</para>
        ///     <para>stateID4 = STATE_Stat_Kharma</para>
        ///     <para>stateID17 = STATE_Internal_InitialLevel</para>
        ///     <para>stateID18 = STATE_Stat_Power</para>
        ///     <para>stateID19 = STATE_Stat_Stamina</para>
        ///     <para>stateID20 = STATE_Stat_Speed</para>
        ///     <para>stateID21 = STATE_Mechanic_IsPoisoned</para>
        ///     <para>stateID22 = STATE_Mechanic_IsStunned</para>
        ///     <para>stateID23 = STATE_Mod_DamageDealtPercent</para>
        ///     <para>stateID24 = STATE_Mod_DamageTakenPercent</para>
        ///     <para>stateID25 = STATE_Mod_SpeedPercent</para>
        ///     <para>stateID26 = STATE_Ramping_DamageID</para>
        ///     <para>stateID27 = STATE_Ramping_DamageUses</para>
        ///     <para>stateID28 = STATE_Condition_WasDamagedThisTurn</para>
        ///     <para>stateID29 = STATE_untargettable</para>
        ///     <para>stateID30 = STATE_Mechanic_IsUnderground</para>
        ///     <para>stateID31 = STATE_Last_HitTaken</para>
        ///     <para>stateID32 = STATE_Last_HitDealt</para>
        ///     <para>stateID33 = STATE_Mechanic_IsFlying</para>
        ///     <para>stateID34 = STATE_Mechanic_IsBurning</para>
        ///     <para>stateID35 = STATE_turnLock</para>
        ///     <para>stateID36 = STATE_swapOutLock</para>
        ///     <para>stateID40 = STATE_Stat_CritChance</para>
        ///     <para>stateID41 = STATE_Stat_Accuracy</para>
        ///     <para>stateID42 = STATE_Passive_Critter</para>
        ///     <para>stateID43 = STATE_Passive_Beast</para>
        ///     <para>stateID44 = STATE_Passive_Humanoid</para>
        ///     <para>stateID45 = STATE_Passive_Flying</para>
        ///     <para>stateID46 = STATE_Passive_Dragon</para>
        ///     <para>stateID47 = STATE_Passive_Elemental</para>
        ///     <para>stateID48 = STATE_Passive_Mechanical</para>
        ///     <para>stateID49 = STATE_Passive_Magic</para>
        ///     <para>stateID50 = STATE_Passive_Undead</para>
        ///     <para>stateID51 = STATE_Passive_Aquatic</para>
        ///     <para>stateID52 = STATE_Mechanic_IsChilled</para>
        ///     <para>stateID53 = STATE_Weather_BurntEarth</para>
        ///     <para>stateID54 = STATE_Weather_ArcaneStorm</para>
        ///     <para>stateID55 = STATE_Weather_Moonlight</para>
        ///     <para>stateID56 = STATE_Weather_Darkness</para>
        ///     <para>stateID57 = STATE_Weather_Sandstorm</para>
        ///     <para>stateID58 = STATE_Weather_Blizzard</para>
        ///     <para>stateID59 = STATE_Weather_Mud</para>
        ///     <para>stateID60 = STATE_Weather_Rain</para>
        ///     <para>stateID61 = STATE_Weather_Sunlight</para>
        ///     <para>stateID62 = STATE_Weather_LightningStorm</para>
        ///     <para>stateID63 = STATE_Weather_Windy</para>
        ///     <para>stateID64 = STATE_Mechanic_IsWebbed</para>
        ///     <para>stateID65 = STATE_Mod_HealingDealtPercent</para>
        ///     <para>stateID66 = STATE_Mod_HealingTakenPercent</para>
        ///     <para>stateID67 = STATE_Mechanic_IsInvisible</para>
        ///     <para>stateID68 = STATE_unkillable</para>
        ///     <para>stateID69 = STATE_Mechanic_IsObject</para>
        ///     <para>stateID70 = STATE_Special_Plant</para>
        ///     <para>stateID71 = STATE_Add_FlatDamageTaken</para>
        ///     <para>stateID72 = STATE_Add_FlatDamageDealt</para>
        ///     <para>stateID73 = STATE_Stat_Dodge</para>
        ///     <para>stateID74 = STATE_Special_BlockedAttackCount</para>
        ///     <para>stateID75 = STATE_Special_ObjectRedirectionAuraID</para>
        ///     <para>stateID77 = STATE_Mechanic_IsBleeding</para>
        ///     <para>stateID78 = STATE_Stat_Gender</para>
        ///     <para>stateID82 = STATE_Mechanic_IsBlind</para>
        ///     <para>stateID84 = STATE_Cosmetic_Stealthed</para>
        ///     <para>stateID85 = STATE_Cosmetic_WaterBubbled</para>
        ///     <para>stateID87 = STATE_Mod_PetTypeDamageDealtPercent</para>
        ///     <para>stateID88 = STATE_Mod_PetTypeDamageTakenPercent</para>
        ///     <para>stateID89 = STATE_Mod_PetType_ID</para>
        ///     <para>stateID90 = STATE_Internal_CaptureBoost</para>
        ///     <para>stateID91 = STATE_Internal_EffectSucceeded</para>
        ///     <para>stateID93 = STATE_Special_IsCockroach</para>
        ///     <para>stateID98 = STATE_swapInLock</para>
        ///     <para>stateID99 = STATE_Mod_MaxHealthPercent</para>
        ///     <para>stateID100 = STATE_Clone_Active</para>
        ///     <para>stateID101 = STATE_Clone_PBOID</para>
        ///     <para>stateID102 = STATE_Clone_PetAbility1</para>
        ///     <para>stateID103 = STATE_Clone_PetAbility2</para>
        ///     <para>stateID104 = STATE_Clone_PetAbility3</para>
        ///     <para>stateID105 = STATE_Clone_Health</para>
        ///     <para>stateID106 = STATE_Clone_MaxHealth</para>
        ///     <para>stateID107 = STATE_Clone_LastAbilityID</para>
        ///     <para>stateID108 = STATE_Clone_LastAbilityTurn</para>
        ///     <para>stateID113 = STATE_Special_IsCharging</para>
        ///     <para>stateID114 = STATE_Special_IsRecovering</para>
        ///     <para>stateID117 = STATE_Clone_CloneAbilityID</para>
        ///     <para>stateID118 = STATE_Clone_CloneAuraID</para>
        ///     <para>stateID119 = STATE_DarkSimulacrum_AbilityID</para>
        ///     <para>stateID120 = STATE_Special_ConsumedCorpse</para>
        ///     <para>stateID121 = STATE_Ramping_PBOID</para>
        ///     <para>stateID122 = STATE_reflecting</para>
        ///     <para>stateID123 = STATE_Special_BlockedFriendlyMode</para>
        ///     <para>stateID124 = STATE_Special_TypeOverride</para>
        ///     <para>stateID126 = STATE_Mechanic_IsWall</para>
        ///     <para>stateID127 = STATE_Condition_DidDamageThisRound</para>
        ///     <para>stateID128 = STATE_Cosmetic_FlyTier</para>
        ///     <para>stateID129 = STATE_Cosmetic_FetishMask</para>
        ///     <para>stateID136 = STATE_Mechanic_IsBomb</para>
        ///     <para>stateID141 = STATE_Special_IsCleansing</para>
        ///     <para>stateID144 = STATE_Cosmetic_Bigglesworth</para>
        ///     <para>stateID145 = STATE_Internal_HealthBeforeInstakill</para>
        ///     <para>stateID149 = STATE_resilient</para>
        ///     <para>stateID153 = STATE_Passive_Elite</para>
        ///     <para>stateID158 = STATE_Cosmetic_Chaos</para>
        ///     <para>stateID162 = STATE_Passive_Boss</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetAllStates
        /// </remarks>
        public static List<string> GetAllStates(int stateEnv) {
            return Lua.GetReturnValues(string.Format("return C_PetBattles.GetAllStates({0})", stateEnv));
        }

        /// <summary>
        ///     Returns the modifier for an attack launched by a pet against an enemy pet.
        /// </summary>
        /// <param name="petType">Number - Index of the pet's type. Accepted values are 1-10.</param>
        /// <param name="enemyPetType">Number - Index of the enemy pet's type. Accepted values are 1-10.</param>
        /// <returns>
        ///     <para>The returns are currently either: 1 (default), 0.666667 (weak), or 1.5 (strong)</para>
        ///     <para>modifier = Number - The multiplier that should be applied to an attack from petType launched against enemyPetType.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetAttackModifier
        /// </remarks>
        public static int GetAttackModifier(int petType, int enemyPetType) {
            return Lua.GetReturnVal<int>("return C_PetBattles.GetAttackModifier()", 0);
        }

        /// <summary>
        ///     Returns information for an aura in a pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent.</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <param name="auraIndex">Number - The number from an index of all active auras.</param>
        /// <returns>
        ///     <para>auraID = Number - The ability ID of the aura (all auras are abilities too).</para>
        ///     <para>instanceID = Number - A unique identifier for this aura; used for differentiation purposes only.</para>
        ///     <para>turnsRemaining = Number - The number of rounds left for the aura to remain active.</para>
        ///     <para>isBuff = Boolean - True if the aura is displayed to the user, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetAuraInfo
        /// </remarks>
        public static List<string> GetAuraInfo(int petOwner, int petIndex, int auraIndex) {
            return
                Lua.GetReturnValues(string.Format("return C_PetBattles.GetAuraInfo({0}, {1}, {2})", petOwner, petIndex,
                    auraIndex));
        }

        /// <summary>
        ///     Returns the current battle state for the pet battle.
        /// </summary>
        /// <returns>
        ///     <para>battleState = Number - A value representing the state of the battle.</para>
        /// </returns>
        /// <remarks>
        ///     <para>Note that the constants for battle states are not defined in FrameXML. Here are the currently known ones:</para>
        ///     <para>LE_PET_BATTLE_STATE_CREATED = 1</para>
        ///     <para>LE_PET_BATTLE_STATE_WAITING_PRE_BATTLE = 2</para>
        ///     <para>LE_PET_BATTLE_STATE_ROUND_IN_PROGRESS = 3</para>
        ///     <para>LE_PET_BATTLE_STATE_WAITING_FOR_ROUND_PLAYBACK = 4</para>
        ///     <para>LE_PET_BATTLE_STATE_WAITING_FOR_FRONT_PETS = 5</para>
        ///     <para>LE_PET_BATTLE_STATE_CREATED_FAILED = 6</para>
        ///     <para>LE_PET_BATTLE_STATE_FINAL_ROUND = 7</para>
        ///     <para>LE_PET_BATTLE_STATE_FINISHED = 8</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.GetBattleState
        /// </remarks>
        public static int GetBattleState() {
            return Lua.GetReturnVal<int>("return C_PetBattles.GetBattleState()", 0);
        }

        /// <summary>
        ///     Returns the rarity of a specific pet in the current pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>rarity = Number - 1: "Poor", 2: "Common", 3: "Uncommon", 4: "Rare", 5: "Epic", 6: "Legendary"</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetBreedQuality
        /// </remarks>
        public static int GetBreedQuality(int petOwner, int petIndex) {
            return
                Lua.GetReturnVal<int>(
                    string.Format("return C_PetBattles.GetBreedQuality({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the current displayID of a specific pet in the pet battle.
        /// </summary>
        /// 
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>displayID = Number - Display ID of the pet (model/skin combination)</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetDisplayID
        /// </remarks>
        public static int GetDisplayID(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(
                string.Format("return C_PetBattles.GetDisplayID({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the current health of a specific pet in the current pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>health = Number - Current health amount of the pet</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetHealth
        /// </remarks>
        public static int GetHealth(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetHealth({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the current icon of a specific pet in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>icon = String - Full path of the species' icon</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetIcon
        /// </remarks>
        public static string GetIcon(int petOwner, int petIndex) {
            return Lua.GetReturnVal<string>(string.Format("return C_PetBattles.GetIcon({0}, {1})", petOwner, petIndex),
                0);
        }

        /// <summary>
        ///     Returns the level of a specific pet in the current pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>level = Number - Level of the pet</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetLevel
        /// </remarks>
        public static int GetLevel(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetLevel({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the maximum health of a specific pet in the current pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>maxHealth = Number - Maximum health amount of the pet</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetMaxHealth
        /// </remarks>
        public static int GetMaxHealth(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetMaxHealth({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the name(s) of a specific pet in the pet battle.
        /// </summary>
        /// <param name="petOwner">Integer - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Integer - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>name = String - Name of the pet. If the pet has a custom name, it will be returned here. If not, it returns the species name here as well as in the second return.</para>
        ///     <para>speciesName = String - Name of the pet's Species.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetName
        /// </remarks>
        public static List<string> GetName(int petOwner, int petIndex) {
            return Lua.GetReturnValues(string.Format("return C_PetBattles.GetName({0}, {1})", petOwner, petIndex));
        }

        /// <summary>
        ///     Returns the number of active auras affecting the current pet in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>numAuras = Number - Amount of active auras affecting the pet.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetNumAuras
        /// </remarks>
        public static int GetNumAuras(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetNumAuras({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the number of pets that the specified owner has in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <returns>
        ///     <para>numPets = Number - Amount of pets that the current owner has in the pet battle (always 1-3).</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetNumPets
        /// </remarks>
        public static int GetNumPets(int petOwner) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetNumPets({0})", petOwner), 0);
        }

        /// <summary>
        ///     Returns the species ID of a specific pet in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>speciesID = Number - Species ID of the pet</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetPetSpeciesID
        /// </remarks>
        public static int GetPetSpeciesID(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetPetSpeciesID({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the current pet type of a specific pet in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>petType = Number - Returned values are 1-10.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetPetType
        /// </remarks>
        public static int GetPetType(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetPetType({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the ability ID for the best trap available to the player in the pet battle.
        /// </summary>
        /// <returns>
        ///     <para>trapAbilityID = Number - Ability ID of the best trap you have available.</para>
        /// </returns>
        /// <remarks>
        ///     <para>Additional and better traps unlock through achievements, and this function will give different returns as those achievements are accomplished.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.GetPlayerTrapAbility
        /// </remarks>
        public static int GetPlayerTrapAbility() {
            return Lua.GetReturnVal<int>("return C_PetBattles.ShouldShowPetSelect()", 0);
        }

        /// <summary>
        ///     Returns the current Power of a specific pet in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>power = Number - Current amount of Power that the pet has</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetPower
        /// </remarks>
        public static int GetPower(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetPower({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns status information about the current PvP pet battle matchmaking queue.
        /// </summary>
        /// <returns>
        ///     <para>queueState = String - Either "proposal", "queued", "suspended", or nil.</para>
        ///     <para>estimatedTime = Number - The current estimated wait time in seconds, rounded to the nearest second.</para>
        ///     <para>queuedTime = Number - The time that the queue started at, in the same format as GetTime().</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetPVPMatchmakingInfo
        /// </remarks>
        public static List<string> GetPVPMatchmakingInfo() {
            return Lua.GetReturnValues("return C_PetBattles.GetAbilityState()");
        }

        /// <summary>
        ///     Returns the selected action of a specific pet in the pet battle.
        /// </summary>
        /// <returns>
        ///     <para>selectedActionState = Number - 2: Ability, 3: Switch Pet, 4: Trap, 5: Skip turn</para>
        ///     <para>selectedActionIndex = Number - Returned values are 1-3, corresponding to the ability buttons from left to right.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetSelectedAction
        /// </remarks>
        public static List<string> GetSelectedAction() {
            return Lua.GetReturnValues("return C_PetBattles.GetSelectedAction()");
        }

        /// <summary>
        ///     Returns the current Speed of a specific pet in the pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>speed = Number - Current amount of Speed that the pet has</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetSpeed
        /// </remarks>
        public static int GetSpeed(int petOwner, int petIndex) {
            return Lua.GetReturnVal<int>(string.Format("return C_PetBattles.GetSpeed({0}, {1})", petOwner, petIndex), 0);
        }

        /// <summary>
        ///     Returns the value of a specific state for a specific pet in a pet battle.
        /// </summary>
        /// <param name="petOwner">Number - Accepted values are actually 0 through 2, unlike all (?) other Pet Battle functions.</param>
        /// <param name="petIndex">Number - Accepted values are actually 0 through 3, unlike all (?) other Pet Battle functions.</param>
        /// <param name="stateID">Number - The ID of a Pet Battle's specific State.</param>
        /// <returns>
        ///     <para>stateValue = Number - The value of the given state for the given pet.</para>
        /// </returns>
        /// <remarks>
        ///     <para>petOwner and petIndex can accept a value of zero in this function. A petOwner of "0" refers to the states controlled by the Game. For example, to check the weather state, you want to use a petOwner of "0" and a petIndex of "0" (which is the only valid petIndex for a petOwner of "0"). When petOwner is 1 (player) or 2 (enemy), setting the petIndex to "0" also works, but does something different - it refers to any objects (walls, barrels, bombs, mines, missile launchers, etc) or field effects currently in play for that team. For example, the DoT from [Flamethrower] is a field effect ("Turns the battlefield") that persists through pet swaps. To check the associated states, you need to set petOwner to 1 or 2 and petIndex to "0".</para>
        ///     <para> </para>
        ///     <para>Note that some values corresponding to stateIDs do NOT update correctly. For example, stateID 78 "STATE_Stat_Gender" is always 0, regardless of petOwner, petIndex, or the pets in question. This suggests that Blizzard updates stateIDs on an individual basis and probably will not update any stateIDs that they do not actively use in Pet Battles at this time, such as those associated with Poison or Cloning.</para>
        ///     <para> </para>
        ///     <para>Here is an example of a possible use for stateIDs and state values. The following code is reverse engineered from Blizzard's own Battle Pet Ability tooltip parser.</para>
        ///     <para> </para>
        ///     <code>
        ///         <para>local AccuracyModifier</para>
        ///             <para>if (C_PetBattles.GetPetType(petOwner, petIndex) == 7) then -- Elementals aren't affected by weathers.</para>
        ///                 <para>AccuracyModifier = C_PetBattles.GetStateValue(petOwner, petIndex, 41) + C_PetBattles.GetStateValue(petOwner, 0, 41)</para>
        ///             <para>else</para>
        ///                 <para>AccuracyModifier = C_PetBattles.GetStateValue(petOwner, petIndex, 41) + C_PetBattles.GetStateValue(petOwner, 0, 41) + C_PetBattles.GetStateValue(0, 0, 41)</para>
        ///             <para>end</para>
        ///     </code>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.GetStateValue
        /// </remarks>
        public static int GetStateValue(int petOwner, int petIndex, int stateID) {
            return Lua.GetReturnVal<int>("return C_PetBattles.GetStateValue()", 0);
        }

        /// <summary>
        ///     Returns the remaining time and total time for your current turn in a PvP pet battle.
        /// </summary>
        /// <returns>
        ///     <para>timeRemaining = Number - Time in seconds remaining before you are forced to pass your turn</para>
        ///     <para>turnTime = Number - Total time in seconds allotted for you to choose an ability</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetTurnTimeInfo
        /// </remarks>
        public static List<string> GetTurnTimeInfo() {
            return Lua.GetReturnValues("return C_PetBattles.GetTurnTimeInfo()");
        }

        /// <summary>
        ///     Returns the experience values of a specific pet in the current pet battle.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was your 3rd pet when you entered battle, it will always be 3 on the index.</param>
        /// <returns>
        ///     <para>xp = Number - Current experience progress towards the next level of the pet</para>
        ///     <para>maxXp = Number - Total amount of experience required for the pet's current level to increase</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.GetXP
        /// </remarks>
        public static List<string> GetXP(int petOwner, int petIndex) {
            return Lua.GetReturnValues(string.Format("return C_PetBattles.GetXP({0}, {1})", petOwner, petIndex));
        }

        /// <summary>
        ///     Returns whether or not there is a pet battle in progress.
        /// </summary>
        /// <returns>
        ///     <para>inBattle = Boolean - True if in a pet battle, false if not</para>
        /// </returns>
        /// <remarks>
        ///     <para>This function starts to return true when PET_BATTLE_OPENING_START fires. It returns false again after PET_BATTLE_OVER fires.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.IsInBattle
        /// </remarks>
        public static bool IsInBattle() {
            return Lua.GetReturnVal<bool>("return C_PetBattles.IsInBattle()", 0);
        }

        /// <summary>
        ///     Returns whether or not the pet battle player is an NPC.
        /// </summary>
        /// <param name="petOwner">Number - 1: Current player, 2: Opponent</param>
        /// <returns>
        ///     <para>isNPC = Boolean - True if the owner is an NPC, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     <para>The only use of this function currently is to check if it is a tamer battle by using <c>C_PetBattles.IsPlayerNPC(2)</c></para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.IsPlayerNPC
        /// </remarks>
        public static bool IsPlayerNPC(int petOwner) {
            return Lua.GetReturnVal<bool>(string.Format("return C_PetBattles.IsPlayerNPC({0})", petOwner), 0);
        }

        /// <summary>
        ///     Returns whether or not you can skip your turn at the current time.
        /// </summary>
        /// <returns>
        ///     <para>usable = Boolean - True if you can skip your turn, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.IsSkipAvailable
        /// </remarks>
        public static bool IsSkipAvailable() {
            return Lua.GetReturnVal<bool>("return C_PetBattles.IsSkipAvailable()", 0);
        }

        /// <summary>
        ///     Returns whether or not you can skip your turn at the current time.
        /// </summary>
        /// <returns>
        ///     <para>usable = Boolean - True if you can use a trap, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.IsTrapAvailable
        /// </remarks>
        public static bool IsTrapAvailable() {
            return Lua.GetReturnVal<bool>("return C_PetBattles.IsTrapAvailable()", 0);
        }

        /// <summary>
        ///     Returns whether or not you are waiting on your opponent to decide a course of action in a PvP pet battle.
        /// </summary>
        /// <returns>
        ///     <para>isWaiting = Boolean - True if you are waiting on your opponent to choose an action, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.IsWaitingOnOpponent
        /// </remarks>
        public static bool IsWaitingOnOpponent() {
            return Lua.GetReturnVal<bool>("return C_PetBattles.IsWaitingOnOpponent()", 0);
        }

        /// <summary>
        ///     Returns whether or not there is a wild pet battle in progress.
        /// </summary>
        /// <returns>
        ///     <para>inWildBattle = Boolean - True if in a wild pet battle, false if not in a wild pet battle or not in a pet battle at all</para>
        /// </returns>
        /// <remarks>
        ///     <para>This function starts to return true when PET_BATTLE_OPENING_START fires if it is a wild battle. It returns false again after PET_BATTLE_OVER fires.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.IsWildBattle
        /// </remarks>
        public static bool IsWildBattle() {
            return Lua.GetReturnVal<bool>("return C_PetBattles.IsWildBattle()", 0);
        }

        /// <summary>
        ///     Starts the process of reporting one of your opponent's battle pets for having a bad name.
        /// </summary>
        /// <param name="petIndex">Number - Accepted values are 1-3, but the order is based off of the initial order. Which pet is currently active is irrelevant to the index, if it was its 3rd pet when it entered battle, it will always be 3 on the index.</param>
        /// <remarks>
        ///     <para>Needs to be followed up with StaticPopup_Show("CONFIRM_REPORT_BATTLEPET_NAME", name) OR ReportPlayer(PLAYER_REPORT_TYPE_BAD_BATTLEPET_NAME, "pending") to complete the reporting process.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.SetPendingReportBattlePetTarget
        /// </remarks>
        public static void SetPendingReportBattlePetTarget(int petIndex) {
            Lua.DoString(string.Format("C_PetBattles.SetPendingReportBattlePetTarget({0})", petIndex));
        }

        /// <summary>
        ///     Starts the process of reporting the specified battle pet for having a bad name.
        /// </summary>
        /// <param name="unit">String - The UnitId to query (e.g. "player", "party2", "pet", "target" etc.).</param>
        /// <remarks>
        ///     <para>Needs to be followed up with StaticPopup_Show("CONFIRM_REPORT_BATTLEPET_NAME", name) OR ReportPlayer(PLAYER_REPORT_TYPE_BAD_BATTLEPET_NAME, "pending") to complete the reporting process.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.SetPendingReportTargetFromUnit
        ///  </remarks>
        public static void SetPendingReportTargetFromUnit(string unit) {
            Lua.DoString(string.Format("C_PetBattles.SetPendingReportTargetFromUnit('{0}')", unit));
        }

        /// <summary>
        ///     Returns whether or not the UI should show the pet select screen.
        /// </summary>
        /// <returns>
        ///     <para>shouldShow = Boolean - True if the pet selection frame should be shown, false otherwise.</para>
        /// </returns>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.ShouldShowPetSelect
        /// </remarks>
        public static bool ShouldShowPetSelect() {

            return Lua.GetReturnVal<bool>("return C_PetBattles.ShouldShowPetSelect()", 0);
        }

        /// <summary>
        ///     Passes your current turn in the pet battle without using an ability.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.SkipTurn
        /// </remarks>
        public static void SkipTurn() {
            Lua.DoString("C_PetBattles.SkipTurn()");
        }

        /// <summary>
        ///     Challenges the targeted player to a PvP pet battle duel.
        /// </summary>
        /// <remarks>
        ///     <para>Called in the StaticPopup PET_BATTLE_PVP_DUEL_REQUESTED.</para>
        ///     <para> </para>
        ///     http://wowpedia.org/API_C_PetBattles.StartPVPDuel
        /// </remarks>
        public static void StartPVPDuel() {
            Lua.DoString("C_PetBattles.StartPVPDuel()");
        }

        /// <summary>
        ///     Adds the current player to the PvP matchmaking queue.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.StartPVPMatchmaking
        /// </remarks>
        public static void StartPVPMatchmaking() {
            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");
        }

        /// <summary>
        ///     Removes the current player from the PvP matchmaking queue.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.StopPVPMatchmaking
        /// </remarks>
        public static void StopPVPMatchmaking() {
            Lua.DoString("C_PetBattles.StopPVPMatchmaking()");
        }

        /// <summary>
        ///     Uses an ability in the pet battle.
        /// </summary>
        /// <param name="actionIndex"></param>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.UseAbility
        /// </remarks>
        public static void UseAbility(int actionIndex) {
            Lua.DoString(string.Format("C_PetBattles.UseAbility({0})", actionIndex));
        }

        /// <summary>
        ///     Uses a trap to attempt to catch the opponent's active pet in the pet battle.
        /// </summary>
        /// <remarks>
        ///     http://wowpedia.org/API_C_PetBattles.UseTrap
        /// </remarks>
        public static void UseTrap() {
            Lua.DoString("C_PetBattles.UseTrap()");
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================
    }
}

