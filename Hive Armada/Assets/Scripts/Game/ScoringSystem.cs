﻿//=============================================================================
// 
// Perry Sidler & Miguel Gotao
// 1831784 - 2264941
// sidle104@mail.chapman.edu - gotao100@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class contains the basic scoring and combo system. When enemies are killed,
// a combo begins that, upon ending, calculates a point value that is added to the 
// player's score.
// 
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Handles the scoring system in the game.
    /// </summary>
    public class ScoringSystem : MonoBehaviour
    {
        public ReferenceManager reference;

        /// <summary>
        /// The score bank of the currect combo.
        /// </summary>
        private float comboBank;

        /// <summary>
        /// The timer of the current combo.
        /// </summary>
        private float comboTimer;

        /// <summary>
        /// Multiplier value of the current combo.
        /// </summary>
        private float comboMultiplier;

        /// <summary>
        /// Bool dictating whether a combo is active or not.
        /// </summary>
        private bool comboActive;

        public void Start()
        {
            comboTimer = 0;
            comboMultiplier = 1.0f;
            comboActive = false;
        }

        /// <summary>
        /// Main combo function. When the combo timer reacher zero, the bank
        /// is calculated with multiplier and sent to PlayerStats for 
        /// </summary>
        public void StartCombo()
        {
            while(comboActive == true)
            {
                comboTimer -= Time.deltaTime;

                if(comboTimer < 0)
                {
                    comboActive = false;
                    comboBank *= comboMultiplier;
                    int comboOut = (int) comboBank;
                    AddScore(comboOut);
                    comboMultiplier = 1.0f;
                    comboBank = 0;
                    break;
                }
            }
            //do point emitter stuff
        }

        /// <summary>
        /// Adds points to the player's score.
        /// </summary>
        /// <param name="points"> Number of points to add </param>
        public void AddScore(int points)
        {
            reference.statistics.AddScore(points);
        }

        /// <summary>
        /// Get's the player's current score.
        /// </summary>
        /// <returns> The player's score integer </returns>


        //public int GetScore()
        //{
        //    return score;
        //}

        /// <summary>
        /// Starts a new combo. If a combo is currently ongoing,
        /// then extend the current combo and add to the multiplier.
        /// </summary>
        public void ComboIn(int points)
        {
            if(comboActive == false)
            {
                comboActive = true;
                comboTimer = 5;
                comboBank += points;
                StartCombo();
            }
            else if (comboActive == true)
            {
                comboTimer = 5;
                comboMultiplier += 0.1f;
            }
        }
    }
}
