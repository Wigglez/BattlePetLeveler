/* BotBase created by AknA and Wigglez */

#region Namespaces

using System;
using BattlePetLeveler.GUI;
using Styx.CommonBot;
using Styx.WoWInternals;
#endregion

namespace BattlePetLeveler.Helpers {
    public class PriorityTreeState : BattlePetLeveler {
        // ===========================================================
        // Constants
        // ===========================================================

        public enum State {
            SETTINGS_INCORRECT,
            NOT_QUEUED,
            QUEUED,
            PROPOSAL,
            BATTLE_STARTED,
            BATTLE_ENDED
        };

        // ===========================================================
        // Fields
        // ===========================================================

        public static State TreeState = State.SETTINGS_INCORRECT;

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

        public static void TreeStateHandler() {
            switch(TreeState) {
                #region State.SETTINGS_INCORRECT
                case State.SETTINGS_INCORRECT:
                    // If the user didn't select anything
                    if(string.IsNullOrEmpty(BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox)) {
                        BPLLog("Go into Bot Config and configure the settings accordingly.");
                        TreeRoot.Stop("Stopped the bot due to a settings issue.");
                    } else {
                        TreeState = State.NOT_QUEUED;
                    }
                    break;
                #endregion

                #region State.NOT_QUEUED
                case State.NOT_QUEUED:
                    // If we are not in queue, we are queuable, and our pulse timer is active
                    if(!QueueHandler.IsInQueue()) {
                        if(QueueHandler.IsQueuable() && ThrottleTimer.CheckThrottleTimer(ThrottleTimer.PulseTimerStopwatch, 1000, "not_create_pulse")) {

                            // We queue up
                            BPLLog("Queuing.");
                            Lua.DoString("C_PetBattles.StartPVPMatchmaking()");

                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create the queue timer
                            ThrottleTimer.LeaveQueue();
                        }
                    } else {
                        // Move on to the next step if we are in the queue
                        TreeState = State.QUEUED;
                    }
                    break;
                #endregion

                #region State.QUEUED
                case State.QUEUED:
                    // If the queue did not pop
                    if(!QueueHandler.IsQueuePopped()) {
                        // If the queue timer is running
                        if(ThrottleTimer.TimerStopwatch.IsRunning) {
                            // Check the queue timer
                            ThrottleTimer.LeaveQueue();
                        } else {
                            // Otherwise if the queue timer isn't running (it expired)
                            // Leave the queue
                            BPLLog("Left queue due to long queue time.");

                            QueueHandler.LeaveQueueCommand();

                            // Start over and requeue
                            TreeState = State.NOT_QUEUED;
                        }
                    } else {
                        // Move to the next step if the queue did pop
                        TreeState = State.PROPOSAL;
                    }
                    break;
                #endregion

                #region State.PROPOSAL
                case State.PROPOSAL:
                    // Check for proposal
                    if(QueueHandler.IsQueuePopped()) {
                        // We accept the queue after a random amount of time (1.5 - 3 sec)
                        if(ThrottleTimer.CheckThrottleTimer(ThrottleTimer.PulseTimerStopwatch, RandomNumber.GenerateRandomInt(1500, 3000), "not_create_pulse")) {
                            PetBattles.AcceptQueuedPVPMatch();
                            BPLLog("Accepted queue, waiting on opponent to accept.");
                        }
                    }

                    if(PetBattles.IsInBattle()) {
                        // Reset the queue timer
                        ThrottleTimer.TimerStopwatch.Reset();

                        if(PetBattles.ShouldShowPetSelect()) {
                            // Check if the pet is usable
                            if(PetBattles.CanPetSwapIn(1)) {
                                // Change pet to the pet
                                PetBattles.ChangePet(1);
                            }
                        }

                        if(PetBattles.IsWaitingOnOpponent()) {
                            BPLLog("Waiting on opponent to choose a pet.");
                        } else {
                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            switch(BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox) {
                                case "Winner":
                                    // Create winner started timer
                                    ThrottleTimer.WinnerForfeit();
                                    break;

                                case "Loser":
                                    // Create loser started timer
                                    ThrottleTimer.LoserForfeit();
                                    break;

                                case "Win Trade":
                                    switch(BattlePetLevelerSettings.Instance.BPLLevelingTypeComboBox) {
                                        case "Character Leveling":
                                            Character.CharacterCurrentXp = Character.GetCharacterCurrentXp();

                                            if(Character.CharacterCurrentXp > Character.CharacterLastXp) {
                                                // Create loser started timer
                                                ThrottleTimer.LoserForfeit();
                                            } else {
                                                // Create winner started timer
                                                ThrottleTimer.WinnerForfeit();
                                            }
                                            break;

                                        case "Pet Leveling":
                                            // Make sure we create a new timer
                                            ThrottleTimer.WaitTimerCreated = false;

                                            ThrottleTimer.CreateThrottleTimer(ThrottleTimer.TimerStopwatch, 5000, 10000,
                                                ThrottleTimer.WinnerForfeitTimerString);
                                            break;
                                    }
                                    break;
                            }
                            // Move to the next step
                            TreeState = State.BATTLE_STARTED;
                        }
                    }
                    break;
                #endregion

                #region State.BATTLE_STARTED
                case State.BATTLE_STARTED:
                    // Get current experience for player pet
                    var getMyPetXp = PetBattles.GetXP(1, 1);
                    var myPetXp = Convert.ToInt32(getMyPetXp[0]);

                    // Get current experience for enemy pet
                    var getEnemyPetXp = PetBattles.GetXP(2, 1);
                    var enemyPetXp = Convert.ToInt32(getEnemyPetXp[0]);

                    var myPetLevel = PetBattles.GetLevel(1, 1);
                    var enemyPetLevel = PetBattles.GetLevel(2, 1);


                    if(PetBattles.ShouldShowPetSelect()) {
                        // Check if the pet is usable
                        if(PetBattles.CanPetSwapIn(1)) {
                            // Change pet to the pet
                            PetBattles.ChangePet(1);
                        }
                    }

                    // If the battle started timer is still running
                    if(ThrottleTimer.TimerStopwatch.IsRunning) {
                        switch(BattlePetLevelerSettings.Instance.BPLCharacterTypeComboBox) {
                            case "Winner":
                                // Check the timer
                                ThrottleTimer.WinnerForfeit();

                                // If the loser forfeits
                                if(!PetBattles.IsInBattle()) {
                                    ThrottleTimer.TimerStopwatch.Reset();
                                }
                                break;

                            case "Loser":
                                // Check the timer
                                ThrottleTimer.LoserForfeit();
                                break;

                            case "Win Trade":
                                switch(BattlePetLevelerSettings.Instance.BPLLevelingTypeComboBox) {
                                    case "Character Leveling":
                                        switch(ThrottleTimer.TimerStringName) {
                                            case ThrottleTimer.WinnerForfeitTimerString:
                                                ThrottleTimer.WinnerForfeit();
                                                break;
                                            case ThrottleTimer.LoserForfeitTimerString:
                                                ThrottleTimer.LoserForfeit();
                                                break;
                                        }

                                        // If the loser forfeits
                                        if(!PetBattles.IsInBattle()) {
                                            ThrottleTimer.TimerStopwatch.Reset();
                                        }
                                        break;

                                    case "Pet Leveling":
                                        if(myPetLevel > enemyPetLevel) {
                                            PetBattles.ForfeitGame();
                                            ThrottleTimer.TimerStopwatch.Reset();
                                        }
                                        if((myPetLevel == enemyPetLevel) && (myPetXp > enemyPetXp)) {
                                            PetBattles.ForfeitGame();
                                            ThrottleTimer.TimerStopwatch.Reset();
                                        }
                                        if((myPetLevel == enemyPetLevel) && (myPetXp == enemyPetXp)) {
                                            PetBattles.ForfeitGame();
                                            ThrottleTimer.TimerStopwatch.Reset();
                                        }

                                        // Check the timer
                                        ThrottleTimer.WinnerForfeit();

                                        // If the loser forfeits
                                        if(!PetBattles.IsInBattle()) {
                                            ThrottleTimer.TimerStopwatch.Reset();
                                        }
                                        break;
                                }
                                break;
                        }
                    } else {
                        if(PetBattles.IsInBattle()) {
                            // Forfeit the match
                            BPLLog("Forfeiting.");
                            PetBattles.ForfeitGame();
                        } else {
                            Character.CharacterLastXp = Character.CharacterCurrentXp;

                            // Make sure we create a new timer
                            ThrottleTimer.WaitTimerCreated = false;

                            // Create the battle ended timer
                            ThrottleTimer.Requeue();

                            // Move to the next step
                            TreeState = State.BATTLE_ENDED;
                        }
                    }
                    break;
                #endregion

                #region State.BATTLE_ENDED
                case State.BATTLE_ENDED:
                    if(ThrottleTimer.TimerStopwatch.IsRunning) {
                        ThrottleTimer.Requeue();
                    } else {
                        TreeState = State.NOT_QUEUED;
                    }
                    break;
                #endregion
            }
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================
    }
}





