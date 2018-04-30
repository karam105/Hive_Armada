﻿//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// This class handles the rocket pod weapon for the player ship. Each time the
// rocket pod is fired, it launches 'burstAmount' of rockets from a set of
// barrels. These rockets home in on the target and blow it up.
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hive.Armada.Data;
using Hive.Armada.Enemies;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// The rocket pod controller for the player ship.
    /// </summary>
    public class RocketPod : Weapon
    {
        public RocketPodData rocketPodData;

        /// <summary>
        /// The type id for the rockets.
        /// </summary>
        private short rocketTypeId;

        /// <summary>
        /// Array of transforms for the barrels of the rocket pod.
        /// </summary>
        public Transform[] barrels;

        /// <summary>
        /// Index of the barrel to launch the rocket from.
        /// </summary>
        private int barrelIndex;

        /// <summary>
        /// Number of rockets launched when the rocket pod fires.
        /// </summary>
        private int burstAmount;

        private WaitForSeconds waitBurst;

        /// <summary>
        /// Emitter for the launching of the rockets.
        /// </summary>
        [Header("Emitters")]
        public GameObject rocketLaunchEmitter;

        /// <summary>
        /// The audio source for the rocket pod sounds
        /// </summary>
        [Header("Audio")]
        public AudioSource source;

        /// <summary>
        /// The sound the rocket pod makes when it fires.
        /// </summary>
        private AudioClip rocketPodLaunchSound;
        
        protected override void Awake()
        {
            base.Awake();
            SetupWeapon();
        }

        /// <summary>
        /// Initializes the rockets and active/inactive pools.
        /// </summary>
        private void SetupWeapon()
        {
            SetupWeapon(rocketPodData);
            
            rocketPodLaunchSound = rocketPodData.shootSound;
            rocketTypeId =
                reference.objectPoolManager.GetTypeIdentifier(rocketPodData.rocketPrefab);
            burstAmount = rocketPodData.burstAmount;
            
            if (reference.cheats.doubleDamage)
                damage *= 2;
            
            waitFire = new WaitForSeconds(1.0f / fireRate);
            waitBurst = new WaitForSeconds(1.0f / fireRate / burstAmount);
        }

        /// <summary>
        /// Gets the enemy, interactable, or position on the wall that player is aimed at.
        /// Shoots the enemy or interactable if there is one.
        /// </summary>
        protected override void Clicked()
        {
            RaycastHit hit;

            if (AimAssistActive)
            {
                if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f,
                                    Utility.shootableMask))
                {
                    StartCoroutine(Shoot(hit.collider.gameObject, hit.point, transform.forward));
                }
                else if (Physics.SphereCast(transform.position, radius, transform.forward, out hit,
                                            200.0f,
                                            Utility.enemyMask))
                {
                    StartCoroutine(Shoot(hit.collider.gameObject, hit.point, transform.forward));
                }
                else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                         Utility.roomMask))
                {
                    StartCoroutine(Shoot(null, hit.point, transform.forward));
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                    Utility.shootableMask))
                {
                    StartCoroutine(Shoot(hit.collider.gameObject, hit.point, transform.forward));
                }
                else if (Physics.Raycast(transform.position, transform.forward, out hit,
                                            200.0f,
                                            Utility.enemyMask))
                {
                    StartCoroutine(Shoot(hit.collider.gameObject, hit.point, transform.forward));
                }
                else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                         Utility.roomMask))
                {
                    StartCoroutine(Shoot(null, hit.point, transform.forward));
                }
            }
        }

        /// <summary>
        /// Shoots rockets. Cycles through the barrels.
        /// </summary>
        /// <param name="target"> The gameobject to launch the rocket at </param>
        /// <param name="position"> The position to launch the rocket at </param>
        /// <param name="forward"> The forward vector at the time of shooting </param>
        private IEnumerator Shoot(GameObject target, Vector3 position, Vector3 forward)
        {
            canShoot = false;

            reference.statistics.IsFiring();
            reference.statistics.WeaponFired("Rocket Pods", 1);
            reference.playerIdleTimer.SetIsIdle(false);

            barrelIndex = 0;

            for (int i = 0; i < burstAmount; ++i)
            {
                if (barrelIndex >= barrels.Length)
                {
                    barrelIndex = 0;
                }

                Transform barrel = barrels[barrelIndex++];

                yield return waitBurst;

                GameObject rocket =
                    reference.objectPoolManager.Spawn(gameObject, rocketTypeId, barrel.position,
                                                      barrel.rotation);
                Rocket rocketScript = rocket.GetComponent<Rocket>();

                bool launched = false;
                if (target != null)
                {
                    if (target.GetComponent<Poolable>() != null)
                    {
                        if (target.GetComponent<Poolable>().IsActive)
                        {
                            rocketScript.Launch(target, position, damageMultiplier);
                            launched = true;
                        }
                    }

                    if (!launched)
                    {
                        if (target.GetComponent<Enemy>() != null)
                        {
                            if (target.GetComponent<Enemy>().Health > 0)
                            {
                                rocketScript.Launch(target, position, damageMultiplier);
                                launched = true;
                            }
                        }
                    }

                    if (!launched)
                    {
                        if (target.activeInHierarchy && target.activeSelf)
                        {
                            rocketScript.Launch(target, position, damageMultiplier);
                            launched = true;
                        }
                    }
                }
                else
                {
                    rocketScript.Launch(null, position, damageMultiplier);
                    launched = true;
                }

                source.PlayOneShot(rocketPodLaunchSound);

                if (!launched)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(position, forward, out hit, 200.0f,
                                        Utility.roomMask))
                    {
                        rocketScript.Launch(null, hit.point, damageMultiplier);
                    }
                    else
                    {
                        Debug.LogWarning(GetType().Name +
                                         " - Unable to redirect from invalid target to room.");
                        reference.objectPoolManager.Despawn(rocket);
                    }
                }
            }

            yield return waitFire;

            canShoot = true;

            reference.statistics.IsNotFiring();
            reference.playerIdleTimer.SetIsIdle(true);
        }
    }
}