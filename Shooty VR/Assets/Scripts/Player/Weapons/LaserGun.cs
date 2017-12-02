//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This is the player laser gun. It has two barrels and shoots alternating
// lasers using LineRenderers.
// 
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Hive.Armada.Enemies;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// Player laser gun
    /// </summary>
    public class LaserGun : Weapon
    {
        /// <summary>
        /// Alternates between firing the left and right laser
        /// </summary>
        private bool isLeftFire = true;

        /// <summary>
        /// Material for the lasers
        /// </summary>
        public Material laserMaterial;

        /// <summary>
        /// The sound the laser gun makes when it fires.
        /// </summary>
        public AudioClip laserSound;

        /// <summary>
        /// Left gun
        /// </summary>
        public GameObject left;

        /// <summary>
        /// The left laser's LineRenderer
        /// </summary>
        private LineRenderer leftLaser;

        /// <summary>
        /// Right gun
        /// </summary>
        public GameObject right;

        /// <summary>
        /// The right laser's LineRenderer
        /// </summary>
        private LineRenderer rightLaser;

        /// <summary>
        /// The audio source for the laser gun sounds
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Used to update state value of PlayerIdleTimer
        /// </summary>
        private PlayerIdleTimer playerIdleTimer;

        /// <summary>
        /// Thickness of the lasers
        /// </summary>
        public float thickness = 0.002f;

        /// <summary>
        /// Initializes weapon attributes and the lasers' LineRenderers
        /// </summary>
        private void Start()
        {
            canShoot = true;
            damage = shipController.weaponDamage[0];
            fireRate = shipController.weaponFireRate[0];
            damageMultiplier = 1;

            leftLaser = left.gameObject.AddComponent<LineRenderer>();
            leftLaser.material = laserMaterial;
            leftLaser.shadowCastingMode = ShadowCastingMode.Off;
            leftLaser.receiveShadows = false;
            leftLaser.alignment = LineAlignment.View;
            leftLaser.startWidth = thickness;
            leftLaser.endWidth = thickness;
            leftLaser.enabled = false;

            rightLaser = right.gameObject.AddComponent<LineRenderer>();
            rightLaser.material = laserMaterial;
            rightLaser.shadowCastingMode = ShadowCastingMode.Off;
            rightLaser.receiveShadows = false;
            rightLaser.alignment = LineAlignment.View;
            rightLaser.startWidth = thickness;
            rightLaser.endWidth = thickness;
            rightLaser.enabled = false;
            playerIdleTimer = FindObjectOfType<PlayerIdleTimer>();
        }

        /// <summary>
        /// Gets enemy or wall aimpoint and shoots at it. Will damage enemies.
        /// </summary>
        protected override void Clicked()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f,
                                   Utility.enemyMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject.transform.position));

                if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.collider.gameObject.GetComponent<Enemy>().Hit(damage * damageMultiplier);
                }

                shipController.hand.controller.TriggerHapticPulse(2500);
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                     Utility.shootableMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject.transform.position));

                if (hit.collider.gameObject.GetComponent<Shootable>() != null
                    && hit.collider.gameObject.GetComponent<Shootable>().isShootable)
                {
                    hit.collider.gameObject.GetComponent<Shootable>().Shot();
                }

                shipController.hand.controller.TriggerHapticPulse(2500);
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                     Utility.roomMask))
            {
                StartCoroutine(Shoot(hit.point));
            }
        }

        /// <summary>
        /// Blocks the gun from shooting based on fire rate, updates LineRenderers, starts the FlashLaser
        /// coroutine.
        /// </summary>
        /// <param name="position"> The position of the target being shot </param>
        private IEnumerator Shoot(Vector3 position)
        {
            canShoot = false;

            if (isLeftFire)
            {
                leftLaser.endWidth = thickness *
                                     Mathf.Max(
                                         Vector3.Magnitude(left.transform.position - position),
                                         1.0f);
            }
            else
            {
                rightLaser.endWidth = thickness *
                                      Mathf.Max(
                                          Vector3.Magnitude(right.transform.position - position),
                                          1.0f);
            }

            leftLaser.SetPosition(0, left.transform.position);
            leftLaser.SetPosition(1, position);
            rightLaser.SetPosition(0, right.transform.position);
            rightLaser.SetPosition(1, position);
            StartCoroutine(FlashLaser(isLeftFire));

            source.PlayOneShot(laserSound);

            reference.statistics.IsFiring();
            reference.statistics.WeaponFired("Laser Gun", 1);
			playerIdleTimer.SetIsIdle(false);

            yield return new WaitForSeconds(1.0f / fireRate);

            isLeftFire = !isLeftFire;
            canShoot = true;

            reference.statistics.IsNotFiring();
			playerIdleTimer.SetIsIdle(true);
        }

        /// <summary>
        /// Flashes the appropriate laser LineRenderer.
        /// </summary>
        /// <param name="isLeft"> If the left laser is the one to flash </param>
        private IEnumerator FlashLaser(bool isLeft)
        {
            if (isLeft)
            {
                leftLaser.enabled = true;
            }
            else
            {
                rightLaser.enabled = true;
            }

            yield return new WaitForSeconds(0.006f);

            if (isLeft)
            {
                leftLaser.enabled = false;
            }
            else
            {
                rightLaser.enabled = false;
            }
        }
    }
}
