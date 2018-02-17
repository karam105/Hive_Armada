﻿//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// UIHover controls hovering interactions with the UIPointer. This activates
// active sprite versions of various UI icons when the UIPointer is hovering
// over it. In a selected state, the active sprite version remains active
// regardless of hovering.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Facilitate interaction with UIPointer.
    /// </summary>
    public class UIHover : MonoBehaviour
    {
        /// <summary>
        /// Reference to gameobject with image of active sprite.
        /// </summary>
        public GameObject activeSpriteImageGO;

        /// <summary>
        /// Reference to audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Reference to audio clip for source.
        /// </summary>
        public AudioClip hoverClip;

        /// <summary>
        /// State of whether UI is being hovered over.
        /// </summary>
        private bool isHovering = false;

        /// <summary>
        /// State of whether UI has been selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// Prevent hovering on enable.
        /// </summary>
        private void OnEnable()
        {
            EndHover();
        }

        /// <summary>
        /// Activate Active Image and play hover clip.
        /// </summary>
        public void Hover()
        {
            if (!isHovering && !isSelected)
            {
                isHovering = true;
                activeSpriteImageGO.SetActive(true);
                source.PlayOneShot(hoverClip);
            }
        }

        /// <summary>
        /// Deactivate Active Image.
        /// </summary>
        public void EndHover()
        {
            if (!isSelected)
            {
                activeSpriteImageGO.SetActive(false);
                isHovering = false;
            }
        }

        /// <summary>
        /// Activate selected state.
        /// </summary>
        public void Select()
        {
            isSelected = true;
            activeSpriteImageGO.SetActive(true);
        }

        /// <summary>
        /// Deactivate selected state.
        /// </summary>
        public void EndSelect()
        {
            isSelected = false;
            activeSpriteImageGO.SetActive(false);
        }
    }
}
