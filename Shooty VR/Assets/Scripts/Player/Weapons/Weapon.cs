﻿//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This is the base class for all player weapons. It contains all variables and
// methods that are common between the weapons.
// 
//=============================================================================

using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// Base class for all player weapons.
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        protected ReferenceManager reference;

        /// <summary>
        /// The shipController. Used to send haptic feedback and set damage and fire rate.
        /// </summary>
        public ShipController shipController;

        /// <summary>
        /// The damage multiplier on the weapon.
        /// </summary>
        public int damageMultiplier;

        /// <summary>
        /// The radius for the aim assist SphereCast
        /// </summary>
        [Tooltip("Radius for the aim assist SphereCast")]
        public float radius = 0.3f;

        /// <summary>
        /// Damage done with each hit.
        /// </summary>
        protected int damage;

        /// <summary>
        /// Number of times this weapon can fire per second.
        /// </summary>
        protected float fireRate;

        /// <summary>
        /// If this weapon can shoot or not. Used for the firing Coroutine
        /// </summary>
        protected bool canShoot = true;

        protected virtual void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }
        }

        /// <summary>
        /// Called every frame that the controller's trigger is down.
        /// </summary>
        public virtual void TriggerUpdate()
        {
            if (canShoot)
            {
                Clicked();
            }
        }

        /// <summary>
        /// Handles the logic behind shooting.
        /// </summary>
        protected abstract void Clicked();
    }
}