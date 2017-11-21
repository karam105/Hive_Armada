﻿//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class handles the ship that the player picks up. It includes all
// functions needed for SteamVR's Player prefab to interact with it. It handles
// firing and switching weapons.
// 
//=============================================================================

using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Hive.Armada.Game;
using Hive.Armada.Player.Weapons;
using SubjectNerd.Utilities;

namespace Hive.Armada.Player
{
    /// <summary>
    /// The controller for the player ship.
    /// </summary>
    [RequireComponent(typeof(Interactable))]
    public class ShipController : MonoBehaviour
    {
        /// <summary>
        /// Modes the ship can be in.
        /// </summary>
        public enum ShipMode
        {
            /// <summary>
            /// Ship can shoot and switch guns in Game mode. Laser Sight is purely for aiming.
            /// </summary>
            Game,

            /// <summary>
            /// Ship can't shoot in Menu mode. The Laser Sight acts as a UI interaction pointer.
            /// </summary>
            Menu
        }

        /// <summary>
        /// Whether or not the player can shoot right now.
        /// </summary>
        [Space(10)]
        private bool canShoot;

        /// <summary>
        /// Index of the currently activated weapon.
        /// </summary>
        private int currentWeapon;

        /// <summary>
        /// If we should wait until LateUpdate to update poses
        /// </summary>
        private bool deferNewPoses;

        /// <summary>
        /// The hand the ship is attached to
        /// </summary>
        [HideInInspector]
        public Hand hand;

        /// <summary>
        /// The Laser Sight on the ship. Reference to switch it between Game and Menu mode.
        /// </summary>
        private LaserSight laserSight;

        /// <summary>
        /// The deferred update position
        /// </summary>
        private Vector3 lateUpdatePos;

        /// <summary>
        /// The deferred update rotation
        /// </summary>
        private Quaternion lateUpdateRot;

        /// <summary>
        /// SteamVR event for applying deferred update poses.
        /// </summary>
        private SteamVR_Events.Action newPosesAppliedAction;

        /// <summary>
        /// Manager with all references we might need.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Which mode is the ship currently in.
        /// </summary>
        public ShipMode shipMode = ShipMode.Menu;

        /// <summary>
        /// Array of the weapons available to the player.
        /// </summary>
        [Header("Weapon Attributes")]
        [Reorderable("Weapon", false)]
        public GameObject[] weapons;

        /// <summary>
        /// Array of the damage for each weapon.
        /// </summary>
        [Reorderable("Weapon", false)]
        public int[] weaponDamage;

        /// <summary>
        /// Array of the fire rate for each weapon.
        /// </summary>
        [Reorderable("Weapon", false)]
        public float[] weaponFireRate;

        /// <summary>
        /// Initializes references to Reference Manager and Laser Sight, sets this
        /// GameObject to the player ship reference in Reference Manager.
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }
            else
            {
                reference.playerShip = gameObject;
                reference.waveManager.Run();
            }

            laserSight = transform.Find("Model").Find("Laser Sight").GetComponent<LaserSight>();
            laserSight.SetMode(ShipMode.Menu);
            newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);
        }

        /// <summary>
        /// Sets the late update pose if we are deferring new poses
        /// </summary>
        private void LateUpdate()
        {
            if (deferNewPoses)
            {
                lateUpdatePos = transform.position;
                lateUpdateRot = transform.rotation;
            }
        }

        /// <summary>
        /// Called when the ship is picked up by a hand. Enables the menus.
        /// </summary>
        /// <param name="attachedHand"> The hand that picked up the ship </param>
        private void OnAttachedToHand(Hand attachedHand)
        {
            hand = attachedHand;

            reference.shipPickup.SetActive(false);
            reference.menuTitle.SetActive(false);
            reference.menuMain.SetActive(true);
            reference.powerUpStatus.BeginTracking();
        }

        /// <summary>
        /// Enables the new poses applied action
        /// </summary>
        private void OnEnable()
        {
            newPosesAppliedAction.enabled = true;
        }

        /// <summary>
        /// Disables the new poses applied action
        /// </summary>
        private void OnDisable()
        {
            newPosesAppliedAction.enabled = false;
        }

        /// <summary>
        /// Updates to the late update pose if we are deferring new poses
        /// </summary>
        private void OnNewPosesApplied()
        {
            if (deferNewPoses)
            {
                // Set object back to previous pose position to avoid jitter
                transform.position = lateUpdatePos;
                transform.rotation = lateUpdateRot;

                deferNewPoses = false;
            }
        }

        /// <summary>
        /// Checks if the ship is shooting or interacting with UI
        /// every frame it is attached to a hand.
        /// </summary>
        /// <param name="hand"> The attached hand </param>
        private void HandAttachedUpdate(Hand hand)
        {
            // Reset transform since we cheated it right after getting poses on previous frame
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            switch (shipMode)
            {
                case ShipMode.Game:
                    if (canShoot)
                    {
                        if (hand.GetStandardInteractionButton())
                        {
                            weapons[currentWeapon].SendMessage("TriggerUpdate");
                        }
                    }
                    else if (!canShoot && hand.GetStandardInteractionButtonUp())
                    {
                        canShoot = true;
                    }

                    // Switch weapons
                    if (!hand.GetStandardInteractionButton() &&
                        hand.controller.GetPressDown(EVRButtonId.k_EButton_Grip))
                    {
                        SwitchGun();
                    }
                    break;
                case ShipMode.Menu:
                    if (hand.GetStandardInteractionButtonDown())
                    {
                        laserSight.TriggerUpdate();
                    }
                    break;
                default:
                    Debug.LogError(GetType().Name + " - ShipMode is not Menu or Game!");
                    break;
            }

            //// Update handedness guess
            //EvaluateHandedness();
        }

        /// <summary>
        /// Switches the currently activated gun to the next in the array
        /// </summary>
        private void SwitchGun()
        {
            int previous = currentWeapon++;
            if (currentWeapon >= weapons.Length)
            {
                currentWeapon = 0;
            }

            if (currentWeapon == previous)
            {
                return;
            }

            weapons[previous].SetActive(false);
            weapons[currentWeapon].SetActive(true);

            if (hand.GetStandardInteractionButton())
            {
                canShoot = false;
            }
        }

        /// <summary>
        /// Sets the ship to switch to either Game or Menu mode.
        /// </summary>
        /// <param name="mode"> The mode to switch the ship to </param>
        public void SetShipMode(ShipMode mode)
        {
            shipMode = mode;
            laserSight.SetMode(mode);
        }

        /// <summary>
        /// Sets the damage boost on all weapons
        /// </summary>
        /// <param name="boost"> The damage boost multiplier </param>
        public void SetDamageBoost(int boost)
        {
            foreach (GameObject obj in weapons)
            {
                if (obj.GetComponent<Weapon>())
                {
                    obj.GetComponent<Weapon>().damageMultiplier = boost;
                }
            }
        }

        /// <summary>
        /// Deactivates the ship when the hand grabs another object.
        /// </summary>
        /// <param name="hand"> The attached hand </param>
        private void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Enables the ship when the ship regains focus by the attached hand.
        /// </summary>
        /// <param name="hand"> The attached hand </param>
        private void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);
            OnAttachedToHand(hand);
        }

        /// <summary>
        /// Destroys the ship when it is dropped.
        /// </summary>
        /// <param name="hand"> The detaching hand </param>
        private void OnDetachedFromHand(Hand hand)
        {
            Destroy(gameObject);
        }
    }
}