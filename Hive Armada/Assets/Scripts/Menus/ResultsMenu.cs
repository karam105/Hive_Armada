﻿//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Results Menu displays the results of the the just completed run to the 
// player.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Display run results to player.
    /// </summary>
    public class ResultsMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject continueMenuGO;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Refernece to IridiumSystem.
        /// </summary>
        private IridiumSystem iridiumSystem;

        /// <summary>
        /// Percent of score used to calculate Iridium gain.
        /// </summary>
        public float iridiumScoreMultiplier = 0.2f;

        /// <summary>
        /// Reference to PlayerStats.
        /// </summary>
        private PlayerStats stats;

        /// <summary>
        /// Reference to Text GameObjecft for victory/defeat.
        /// </summary>
        public GameObject victoryDefeatTextGO;

        /// <summary>
        /// Message to display if player wins.
        /// </summary>
        public string victoryMessage;

        /// <summary>
        /// Color to apply to victory text.
        /// </summary>
        public Color victoryColor;

        /// <summary>
        /// Message to display if player loses.
        /// </summary>
        public string defeatMessage;

        /// <summary>
        /// Color to apply to defeat text.
        /// </summary>
        public Color defeatColor;

        /// <summary>
        /// Reference to Text GameObject for score stat.
        /// </summary>
        public GameObject scoreTextGO;

        /// <summary>
        /// Reference to Iridium text GO.
        /// </summary>
        public GameObject iridiumTextGO;

        /// <summary>
        /// Reference to Text GameObject for time stat.
        /// </summary>
        public GameObject timeTextGO;

        /// <summary>
        /// Reference to Text GameObject for kills stat.
        /// </summary>
        public GameObject killsTextGO;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to GameSettings.
        /// </summary>
        private GameSettings gameSettings;

        /// <summary>
        /// Variable used to make sure that audio
        /// doesn't play over itself
        /// </summary>
        private int continueCounter = 0;

        /// <summary>
        /// Find references. Get and set results values. Reset stats totals. 
        /// Calculate Iridium totals.
        /// </summary>
        void Awake()
        {
            iridiumSystem = FindObjectOfType<IridiumSystem>();
            reference = FindObjectOfType<ReferenceManager>();
            gameSettings = reference.gameSettings;
            stats = reference.statistics;

            int iridiumScoreAmount = (int)(stats.totalScore * 0.2);

            if (stats.won)
            {
                victoryDefeatTextGO.GetComponent<Text>().text = victoryMessage;
                victoryDefeatTextGO.GetComponent<Text>().color = victoryColor;
            }
            else
            {
                victoryDefeatTextGO.GetComponent<Text>().text = defeatMessage;
                victoryDefeatTextGO.GetComponent<Text>().color = defeatColor;
            }

            if (gameSettings.selectedGameMode == GameSettings.GameMode.SoloNormal)
            {
                scoreTextGO.GetComponent<Text>().text = "Score: " + stats.totalScore;

                int iridiumShootablesSpawnedAmount = iridiumSystem.GetSpawnedShootablesAmount();
                int iridiumShootablesObtainedAmount = iridiumSystem.GetObtainedShootablesAmount();

                iridiumTextGO.GetComponent<Text>().text = "Iridium: " +
                    iridiumScoreAmount + ", " + iridiumShootablesObtainedAmount + " obtained / " +
                    iridiumShootablesSpawnedAmount + " spawned";
            }

            else if (gameSettings.selectedGameMode == GameSettings.GameMode.SoloInfinite)
            {
                TimeSpan time = TimeSpan.FromSeconds(stats.totalAliveTime);

                string timeOutput;
                if (time.Seconds < 60)
                {
                    timeOutput = string.Format("{0:D2} sec", time.Seconds);
                }
                else if (time.Minutes < 60)
                {
                    timeOutput = string.Format("{0:D2} min {1:D2} sec", time.Minutes, time.Seconds);
                }
                else
                {
                    timeOutput = string.Format("{0:D2} hrs {1:D2} min {2:D2} sec", time.Hours, time.Minutes, time.Seconds);
                }

                timeTextGO.GetComponent<Text>().text = "Time: " + timeOutput;
            }

            //wavesTextGO.GetComponent<Text>().text = "Waves: " + stats.waves;
            
            if (killsTextGO)
            {
                killsTextGO.GetComponent<Text>().text = "Kills: " + stats.totalEnemiesKilled;
            }

            stats.ResetTotals();
            iridiumSystem.ResetShootablesAmounts();
            iridiumSystem.AddIridium(iridiumScoreAmount);
            iridiumSystem.WriteIridiumFile();
			transitionManager.currMenu = gameObject;
        }

        /// <summary>
        /// Continue button pressed; navigates to Main Menu.
        /// Reset stat values.
        /// </summary>
        public void PressContinue()
        {
            source.PlayOneShot(clips[0]);
            continueCounter += 1;
            if (continueCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            transitionManager.Transition(continueMenuGO);
        }
    }
}

