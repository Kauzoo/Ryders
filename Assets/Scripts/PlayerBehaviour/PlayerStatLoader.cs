using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Player
{
    public class PlayerStatLoader : MonoBehaviour
    {
        public GameObject boardListPrefab;
        public GameObject characterListPrefab;

        public void LoadStats(PlayerBehaviour playerBehaviour)
        {
            // Determine Character & Board
            var boardListObject = Instantiate<GameObject>(boardListPrefab);
            var characterListObject = Instantiate<GameObject>(characterListPrefab);
            var boardList = boardListObject.GetComponent<BoardListScript>().boardPrefabList;
            var characterList = characterListObject.GetComponent<CharacterListScript>().characterPrefabList;

            Character character = null;
            foreach(GameObject characterObj in characterList)
            {
                if(characterObj.TryGetComponent<Character>(out Character characterIter))
                {
                    if(playerBehaviour.character == characterIter.character)
                    {
                        character = characterIter;
                        break;
                    }
                }
                else
                {
                    Debug.LogWarning("Character List contains object without character script");
                }
            }
            if(character == null)
            {
                Debug.LogError("Character not found");
            }

            Board board = null;
            foreach (GameObject boardObj in boardList)
            {
                if (boardObj.TryGetComponent<Board>(out Board boardIter))
                {
                    if (playerBehaviour.board == boardIter.board)
                    {
                        board = boardIter;
                        break;
                    }
                }
                else
                {
                    Debug.LogWarning("Board List contains object without board script");
                }
            }
            if (board == null)
            {
                Debug.LogError("Board not found");
            }

            // Load into Level stat first
            /*
             * LEVEL 1
             */
            // Speed
            playerBehaviour.statsLevel1.acceleration = character.characterStatsMovement.AccelerationLvl1;
            playerBehaviour.statsLevel1.cruisingSpeed = character.characterStatsMovement.TopSpeedLvl1;
            // Boost
            playerBehaviour.statsLevel1.boostSpeed = board.GearStatsLevel1.BoostSpeed;
            // Drift
            playerBehaviour.statsLevel1.driftBoostSpeed = board.GearStatsLevel1.SpeedGainedFromDriftDash;
            // Cornering
            playerBehaviour.statsLevel1.turnrate = character.characterStatsMovement.CorneringLvl1;
            // Air
            playerBehaviour.statsLevel1.maxAir = board.GearStatsLevel1.MaxAir;
            playerBehaviour.statsLevel1.passiveAirDrain = board.GearStatsLevel1.PassiveAirDrain;
            playerBehaviour.statsLevel1.driftAirCost = board.GearStatsLevel1.DriftAirCost;
            playerBehaviour.statsLevel1.boostCost = board.GearStatsLevel1.BoostCost;
            playerBehaviour.statsLevel1.tornadoCost = board.GearStatsLevel1.TornadoCost;

            /*
             * LEVEL 2
             */
            // Speed
            playerBehaviour.statsLevel2.acceleration = character.characterStatsMovement.AccelerationLvl2;
            playerBehaviour.statsLevel2.cruisingSpeed = character.characterStatsMovement.TopSpeedLvl2;
            // Boost
            playerBehaviour.statsLevel2.boostSpeed = board.GearStatsLevel2.BoostSpeed;
            // Drift
            playerBehaviour.statsLevel2.driftBoostSpeed = board.GearStatsLevel2.SpeedGainedFromDriftDash;
            // Cornering
            playerBehaviour.statsLevel2.turnrate = character.characterStatsMovement.CorneringLvl2;
            // Air
            playerBehaviour.statsLevel2.maxAir = board.GearStatsLevel2.MaxAir;
            playerBehaviour.statsLevel2.passiveAirDrain = board.GearStatsLevel2.PassiveAirDrain;
            playerBehaviour.statsLevel2.driftAirCost = board.GearStatsLevel2.DriftAirCost;
            playerBehaviour.statsLevel2.boostCost = board.GearStatsLevel2.BoostCost;
            playerBehaviour.statsLevel2.tornadoCost = board.GearStatsLevel2.TornadoCost;

            /*
             * LEVEL 3
             */
            // Speed
            playerBehaviour.statsLevel3.acceleration = character.characterStatsMovement.AccelerationLvl3;
            playerBehaviour.statsLevel3.cruisingSpeed = character.characterStatsMovement.TopSpeedLvl3;
            // Boost
            playerBehaviour.statsLevel3.boostSpeed = board.GearStatsLevel3.BoostSpeed;
            // Drift
            playerBehaviour.statsLevel3.driftBoostSpeed = board.GearStatsLevel3.SpeedGainedFromDriftDash;
            // Cornering
            playerBehaviour.statsLevel3.turnrate = character.characterStatsMovement.CorneringLvl3;
            // Air
            playerBehaviour.statsLevel3.maxAir = board.GearStatsLevel3.MaxAir;
            playerBehaviour.statsLevel3.passiveAirDrain = board.GearStatsLevel3.PassiveAirDrain;
            playerBehaviour.statsLevel3.driftAirCost = board.GearStatsLevel3.DriftAirCost;
            playerBehaviour.statsLevel3.boostCost = board.GearStatsLevel3.BoostCost;
            playerBehaviour.statsLevel3.tornadoCost = board.GearStatsLevel3.TornadoCost;

            /*
             * OTHER
             */
            // Speed
            playerBehaviour.movementVars.minSpeed = board.movementVars.MinSpeed;
            playerBehaviour.movementVars.fastAcceleration = board.movementVars.FastAcceleration;
            playerBehaviour.movementVars.deceleration = board.movementVars.deceleration;
            playerBehaviour.movementVars.corneringDeceleration = board.movementVars.corneringDeceleration;
            // Boost
            //playerBehaviour.movementVars.boostDuration = board.movementVars.boo


        }
    }
}
