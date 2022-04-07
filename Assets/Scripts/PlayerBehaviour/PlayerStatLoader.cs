using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Player
{
    public class PlayerStatLoader : MonoBehaviour
    {
        public static List<Character> characters = new List<Character>();
        public static List<Board> boards = new List<Board>();

        public static void LoadStats(PlayerBehaviour playerBehaviour)
        {
            // Determine Character & Board
        }

        private static void LoadCharacterStats(PlayerBehaviour playerBehaviour)
        {
            
            // Load into Level stat first
            /*
            playerBehaviour.statsLevel1.acceleration = ;

            playerBehaviour.statsLevel1.acceleration;
            playerBehaviour.statsLevel1.cruisingSpeed;

            playerBehaviour.statsLevel1.boostSpeed;

            playerBehaviour.statsLevel1.driftBoostSpeed;

            playerBehaviour.statsLevel1.turnrate;

            playerBehaviour.statsLevel1.maxAir;
            playerBehaviour.statsLevel1.passiveAirDrain;
            playerBehaviour.statsLevel1.driftAirCost;
            playerBehaviour.statsLevel1.boostCost;
            playerBehaviour.statsLevel1.tornadoCost;
            */
    }

        private static void LoadBoardStats()
        {

        }
    }
}
