﻿//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// ExtrasMenu controls interactions with the Extras Menu.
//
//=============================================================================

using System.Collections;
using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Contains navigation functions for the Shop and Bestiary menus.
    /// </summary>
    public class ExtrasMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to Shop Menu.
        /// </summary>
        public GameObject shopMenuGO;

        /// <summary>
        /// Reference to player transform for Shop Menu.
        /// </summary>
        public Transform shopTransform;

        /// <summary>
        /// Reference to Bestiary Menu.
        /// </summary>
        public GameObject bestiaryMenuGO;

        /// <summary>
        /// Reference to player transform for Bestiary Menu.
        /// </summary>
        public Transform bestiaryTransform;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        private bool isInteractable = false;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        private void OnEnable()
        {
            StartCoroutine(InteractDelay());
        }

        private void OnDisable()
        {
            isInteractable = false;
        }

        private IEnumerator InteractDelay()
        {
            yield return new WaitForSeconds(Utility.interactDelay);

            isInteractable = true;
        }

        /// <summary>
        /// Start button pressed. Navigate to Shop Menu.
        /// </summary>
        public void PressShop()
        {
            if (!isInteractable)
            {
                return;
            }

            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            FindObjectOfType<RoomTransport>().Transport(shopTransform, gameObject, shopMenuGO);
        }

        /// <summary>
        /// Bestiary button pressed. Navigate to Bestiary Menu.
        /// </summary>
        public void PressBestiary()
        {
            if (!isInteractable)
            {
                return;
            }

            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            FindObjectOfType<RoomTransport>().Transport(bestiaryTransform, gameObject, bestiaryMenuGO);
        }

        /// <summary>
        /// Credits button pressed. 
        /// </summary>
        public void PressCredits()
        {
            if (!isInteractable)
            {
                return;
            }

            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

        }

        /// <summary>
        /// Back button pressed; navigates to Main Menu.
        /// </summary>
        public void PressBack()
        {
            if (!isInteractable)
            {
                return;
            }

            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            transitionManager.Transition(backMenuGO);
        }
    }
}
