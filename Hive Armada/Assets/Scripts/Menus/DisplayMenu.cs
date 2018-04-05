﻿//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// DisplayMenu controls interactions with the Display Menu.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Display Menu.
    /// </summary>
    public class DisplayMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Reference to Bloom Toggle.
        /// </summary>
        public Toggle bloomToggle;

        /// <summary>
        /// Reference to Color Blind Mode Toggle.
        /// </summary>
        public Toggle colorBlindModeToggle;

        /// <summary>
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Variables used to make sure that audio
        /// doesn't play over itself
        /// </summary>
        private int backCounter = 0;

        private int bloomCounter = 0;

        private int colorBlindModeCounter = 0;

        /// <summary>
        /// Find references. 
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            bloomToggle.isOn = reference.optionsValues.bloom;
            colorBlindModeToggle.isOn = reference.optionsValues.colorBlindMode;
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetDisplayPlayerPrefs();
            transitionManager.Transition(backMenuGO);
        }

        /// <summary>
        /// Change bloom setting based on bloomToggle value.
        /// </summary>
        public void SetBloom(bool isOn)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetBloom(isOn);
        }

        /// <summary>
        /// Change color blind mode setting based on colorBlindModeToggle value.
        /// </summary>
        public void SetColorBlindMode(bool isOn)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetColorBlindMode(isOn);
        }
    }
}
