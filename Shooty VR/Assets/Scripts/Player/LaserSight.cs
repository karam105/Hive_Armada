﻿//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC 340-01 & CPSC-344-01
// Group Project
//
// This class controls the laser pointer for the player ship. It uses a
// Line Renderer to draw a laser from the front of the player ship to the wall.
// It originally used a cube stretched between the ship and a RayCastHit point,
// but it is easier for the player to see where they are aiming when the laser
// continues through enemies. If not, the laser could potential end just in
// front of the ship and could go unnoticed by the player.
// Doubles as the UI interaction pointer for menus. Can be toggled to menu mode
// where it will interact with UI objects.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Laser sight for the player. Also handles UI interaction.
    /// </summary>
    public class LaserSight : MonoBehaviour
    {
        /// <summary>
        /// Reference to the ship controller
        /// </summary>
        public ShipController shipController;

        /// <summary>
        /// Which mode the ship is in
        /// </summary>
        public ShipController.ShipMode mode;

        /// <summary>
        /// The laser sight itself
        /// </summary>
        private LineRenderer laser;

        /// <summary>
        /// Material for the laser sight
        /// </summary>
        public Material laserMaterial;

        /// <summary>
        /// How thick the laser should be
        /// </summary>
        public float thickness = 0.002f;

        /// <summary>
        /// The object the laser sight is hitting while in Menu mode
        /// </summary>
        private GameObject aimObject;

        /// <summary>
        /// If the aimObject is tagged as button
        /// </summary>
        private bool isButton;

        /// <summary>
        /// Initializes the laser sight's LineRenderer and ship controller reference.
        /// </summary>
        private void Start()
        {
            shipController = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<ShipController>();

            laser = gameObject.AddComponent<LineRenderer>();
            laser.material = laserMaterial;
            laser.shadowCastingMode = ShadowCastingMode.Off;
            laser.receiveShadows = false;
            laser.alignment = LineAlignment.View;
            laser.startWidth = thickness;
            laser.endWidth = thickness;
        }

        // Update is called once per frame
        private void Update()
        {
            RaycastHit hit;

            if (mode == ShipController.ShipMode.Game)
            {
                if (Physics.Raycast(transform.position, transform.forward,
                    out hit, Mathf.Infinity, Utility.roomMask))
                {
                    laser.SetPosition(0, transform.position);
                    laser.SetPosition(1, hit.point);

                    float mag = (transform.position - hit.point).magnitude;
                    laser.endWidth = thickness * Mathf.Max(mag, 1.0f);
                }
                else
                {
                    isButton = false;
                }
            } // end if Game mode
            else if (mode == ShipController.ShipMode.Menu)
            {
                if (Physics.Raycast(transform.position, transform.forward,
                    out hit, Mathf.Infinity, Utility.uiMask))
                {
                    if (hit.collider.gameObject.CompareTag("Button"))
                    {
                        if (!isButton)
                        {
                            isButton = true;
                            if (shipController != null)
                            {
                                try
                                {
                                    shipController.hand.controller.TriggerHapticPulse();
                                }
                                catch (Exception)
                                {
                                    // Do nothing
                                }
                            }
                        }
                    }
                    else
                    {
                        isButton = false;
                    }

                    aimObject = hit.collider.gameObject;
                    isButton = hit.collider.gameObject.CompareTag("Button");

                    laser.SetPosition(0, transform.position);
                    laser.SetPosition(1, hit.point);

                    float mag = (transform.position - hit.point).magnitude;
                    laser.endWidth = thickness * Mathf.Max(mag, 1.0f);
                }
                else if (Physics.Raycast(transform.position, transform.forward,
                    out hit, Mathf.Infinity,
                    Utility.roomMask))
                {
                    aimObject = null;
                    isButton = false;

                    laser.SetPosition(0, transform.position);
                    laser.SetPosition(1, hit.point);

                    float mag = (transform.position - hit.point).magnitude;
                    laser.endWidth = thickness * Mathf.Max(mag, 1.0f);
                }
                else
                {
                    aimObject = null;
                    isButton = false;
                }
            } // end if Menu mode
        }

        /// <summary>
        /// Sent every frame while the trigger is pressed
        /// </summary>
        public void TriggerUpdate()
        {
            if (mode.Equals(ShipController.ShipMode.Menu) && isButton)
            {
                ExecuteEvents.Execute(aimObject,
                    new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }

        /// <summary>
        /// Sets the mode for the laser, whether it should interact with UI or not.
        /// </summary>
        /// <param name="mode"> The ShipMode to use </param>
        public void SetMode(ShipController.ShipMode mode)
        {
            this.mode = mode;
        }
    }
}