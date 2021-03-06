﻿//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// OptionsValues saves changes made to options through options menu and 
// implements game adjustments using values for sounds, display, etc. Values
// are stored as variables and as PlayerPrefs. Note: PlayerPrefs do not 
// support booleans, so conversion is used between boolean and stored int
// values (0 = false, 1 = true). In addition, audio sources require the
// VolumeAdjustment component in order to update volume level.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Store options values.
    /// </summary>
    public class OptionsValues : MonoBehaviour
    {
        [Header("Sound")]
        /// <summary>
        /// Master volume level.
        /// </summary>
        public float masterVolume;

        /// <summary>
        /// Default master volume level.
        /// </summary>
        public float defaultMasterVolume;

        /// <summary>
        /// Music volume level.
        /// </summary>
        public float musicVolume;

        /// <summary>
        /// Default music volume level.
        /// </summary>
        public float defaultMusicVolume;

        /// <summary>
        /// Music volume level.
        /// </summary>
        public float fxVolume;

        /// <summary>
        /// Default master volume level.
        /// </summary>
        public float defaultFXVolume;

        /// <summary>
        /// Dialogue volume level.
        /// </summary>
        public float dialogueVolume;

        /// <summary>
        /// Default dialogue volume level.
        /// </summary>
        public float defaultDialogueVolume;

        /// <summary>
        /// References to Volume Adjustment components.
        /// </summary>
        private VolumeAdjustment[] volumeAdjustments;

        [Header("Display")]
        /// <summary>
        /// State of whether bloom is on.
        /// </summary>
        public bool bloom;

        /// <summary>
        /// Default bloom.
        /// </summary>
        public bool defaultBloom;

        /// <summary>
        /// Type of color blind mode.
        /// </summary>
        public ColorBlindMode.Mode colorBlindMode;

        /// <summary>
        /// Default color blind mode.
        /// </summary>
        public ColorBlindMode.Mode defaultColorBlindMode;

        [Header("Gameplay")]
        /// <summary>
        /// State of whether aim assist is on.
        /// </summary>
        public bool aimAssist;

        /// <summary>
        /// Default aimAssist.
        /// </summary>
        public bool defaultAimAssist;

        /// <summary>
        /// State of whether score display is on.
        /// </summary>
        public bool scoreDisplay;

        /// <summary>
        /// Default score.
        /// </summary>
        public bool defaultScoreDisplay;

        [Header("References")]
        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to player camera gameObject.
        /// </summary>
        public GameObject cameraGO;

        /// <summary>
        /// Reference to ShipLoadout.
        /// </summary>
        private GameSettings gameSettings;

        /// <summary>
        /// State of whether the Awake function has finished execution.
        /// </summary>
        private bool awakeFinished = false;

        /// <summary>
        /// Find references. Get PlayerPrefs value and set game settings.
        /// </summary>
        private void Awake()
        {
            GetPlayerPrefs();
            reference = FindObjectOfType<ReferenceManager>();
            volumeAdjustments = FindObjectsOfType<VolumeAdjustment>();
            //gameSettings = reference.gameSettings;
            gameSettings = FindObjectOfType<GameSettings>();
            SetAwakeGameValues();
        }

        /// <summary>
        /// Store value for masterVolume.
        /// </summary>
        /// <param name="masterVolume">New masterVolume value.</param>
        public void SetMasterVolume(float masterVolume)
        {
            this.masterVolume = masterVolume;
            AudioListener.volume = masterVolume;
        }

        /// <summary>
        /// Store value for musicVolume.
        /// </summary>
        /// <param name="musicVolume">New masterVolume value.</param>
        public void SetMusicVolume(float musicVolume)
        {
            this.musicVolume = musicVolume;
            foreach (VolumeAdjustment volumeAdjustment in volumeAdjustments)
            {
                if (volumeAdjustment.category == VolumeAdjustment.AudioSourceCategory.Music)
                {
                    volumeAdjustment.UpdateVolume();
                }
            }
        }

        /// <summary>
        /// Store value for FXVolume.
        /// </summary>
        /// <param name="fxVolume">New fxVolume value.</param>
        public void SetFXVolume(float fxVolume)
        {
            this.fxVolume = fxVolume;
            foreach (VolumeAdjustment volumeAdjustment in volumeAdjustments)
            {
                if (volumeAdjustment.category == VolumeAdjustment.AudioSourceCategory.FX)
                {
                    volumeAdjustment.UpdateVolume();
                }
            }
        }

        /// <summary>
        /// Store value for dialogueVolume.
        /// </summary>
        /// <param name="dialogueVolume">New dialogue value.</param>
        public void SetDialogueVolume(float dialogueVolume)
        {
            this.dialogueVolume = dialogueVolume;
            foreach (VolumeAdjustment volumeAdjustment in volumeAdjustments)
            {
                if (volumeAdjustment.category == VolumeAdjustment.AudioSourceCategory.Dialogue)
                {
                    volumeAdjustment.UpdateVolume();
                }
            }
        }

        /// <summary>
        /// Change and store value for bloom.
        /// </summary>
        /// <param name="bloom">New bloom value.</param>
        public void SetBloom(bool bloom)
        {
            this.bloom = bloom;
            cameraGO.GetComponent<PostProcessingBehaviour>().profile.bloom.enabled = bloom;
        }

        /// <summary>
        /// Change and store value for colorBlindMode.
        /// </summary>
        /// <param name="colorBlindMode">New colorBlindMode value.</param>
        public void SetColorBlindMode(ColorBlindMode.Mode colorBlindMode)
        {
            this.colorBlindMode = colorBlindMode;
            gameSettings.colorBlindMode = colorBlindMode;
            reference.colorBlindMode.SetMode(colorBlindMode);
        }

        /// <summary>
        /// Change and store value for aim assist.
        /// </summary>
        /// <param name="aimAssist">New aim assist value.</param>
        public void SetAimAssist(bool aimAssist)
        {
            this.aimAssist = aimAssist;
            gameSettings.aimAssist = aimAssist;
        }

        /// <summary>
        /// Change and store value for scoreDisplay.
        /// </summary>
        /// <param name="scoreDisplay">New scoreDisplay value.</param>
        public void SetScoreDisplay(bool scoreDisplay)
        {
            this.scoreDisplay = scoreDisplay;
            gameSettings.scoreDisplay = scoreDisplay;
        }

        /// <summary>
        /// Set player prefs for sound values.
        /// </summary>
        public void SetSoundPlayerPrefs()
        {
            PlayerPrefs.SetFloat("masterVolume", masterVolume);
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.SetFloat("fxVolume", fxVolume);
            PlayerPrefs.SetFloat("dialogueVolume", dialogueVolume);
        }

        /// <summary>
        /// Set player prefs for display values.
        /// </summary>
        public void SetDisplayPlayerPrefs()
        {
            PlayerPrefs.SetInt("bloom", Convert.ToInt32(bloom));
            PlayerPrefs.SetInt("colorBlindMode", Convert.ToInt32(colorBlindMode));
        }

        /// <summary>
        /// Set player prefs for gameplay values.
        /// </summary>
        public void SetGameplayPlayerPrefs()
        {
            PlayerPrefs.SetInt("aimAssist", Convert.ToInt32(aimAssist));
            PlayerPrefs.SetInt("scoreDisplay", Convert.ToInt32(scoreDisplay));
        }

        /// <summary>
        /// Set all player prefs.
        /// </summary>
        public void SetPlayerPrefs()
        {
            SetSoundPlayerPrefs();
            SetDisplayPlayerPrefs();
            SetGameplayPlayerPrefs();
        }

        /// <summary>
        /// Get all player prefs, or default values if prefs not yet set.
        /// </summary>
        private void GetPlayerPrefs()
        {
            //Sound
            masterVolume = PlayerPrefs.GetFloat("masterVolume", defaultMasterVolume);
            musicVolume = PlayerPrefs.GetFloat("musicVolume", defaultMusicVolume);
            fxVolume = PlayerPrefs.GetFloat("fxVolume", defaultFXVolume);
            dialogueVolume = PlayerPrefs.GetFloat("dialogueVolume", defaultDialogueVolume);

            //Display
            bloom = Convert.ToBoolean(PlayerPrefs.GetInt("bloom", 
                Convert.ToInt32(defaultBloom)));
            colorBlindMode = (ColorBlindMode.Mode)PlayerPrefs.GetInt("colorBlindMode", Convert.ToInt32(defaultColorBlindMode));

            //Gameplay
            aimAssist = Convert.ToBoolean(PlayerPrefs.GetInt("aimAssist", 
                Convert.ToInt32(defaultAimAssist)));
            scoreDisplay = Convert.ToBoolean(PlayerPrefs.GetInt("scoreDisplay", 
                Convert.ToInt32(defaultScoreDisplay)));
        }

        /// <summary>
        /// Set in-game values for objects/components that do not update independently.
        /// </summary>
        private void SetAwakeGameValues()
        {
            //Display
            SetBloom(bloom);
            SetColorBlindMode(colorBlindMode);

            //Gameplay
            SetAimAssist(aimAssist);
            SetScoreDisplay(scoreDisplay);
        }
    }
}