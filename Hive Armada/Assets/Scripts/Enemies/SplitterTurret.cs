﻿//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Enemy that shoots at the player and that spawns 4 more enemies
// when destroyed.
//
//=============================================================================

using System.Collections;
using System.Linq;
using Hive.Armada.Game;
using Hive.Armada.Menus;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Splitter turret enemy
    /// </summary>
    public class SplitterTurret : Enemy
    {
        /// <summary>
        /// Type identifier for this enemy's projectiles in objectPoolManager
        /// </summary>
        private short projectileTypeIdentifier = -2;

        private short childTypeIdentifier = -2;

        /// <summary>
        /// The point where this enemy shoots from. Arranged in a 5x5 grid as shown below:
        /// 00 - 01 - 02 - 03 - 04
        /// 05 - 06 - 07 - 08 - 09
        /// 10 - 11 - 12 - 13 - 14
        /// 15 - 16 - 17 - 18 - 19
        /// 20 - 21 - 22 - 23 - 24
        /// </summary>
        public Transform shootPoint;

        /// <summary>
        /// How many time per second this enemy can shoot.
        /// </summary>
        private float fireRate;

        /// <summary>
        /// Structure holding the current firing pattern
        /// </summary>
        public bool[] projectileArray;

        /// <summary>
        /// Value that determines what projectile the enemy will shoot
        /// as well as its parameters
        /// </summary>
        private int fireMode;

        /// <summary>
        /// Degrees that the projectile can be randomly rotated.
        /// Randomly picks in the range of [-spread, spread] for all 3 axes.
        /// </summary>
        private float spread;

        /// <summary>
        /// The enemy to spawn when this enemy dies
        /// </summary>
        public GameObject childTurret;

        /// <summary>
        /// Distance each child enemy will move when this enemy is destroyed
        /// </summary>
        public float splitDir;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;

        /// <summary>
        /// Whether or not the projectile being shot rotates.
        /// </summary>
        private bool canRotate;

        public Color projectileAlbedoColor;

        public Color projectileEmissionColor;

        /// <summary>
        /// Variables for hovering
        /// </summary>
        private float theta;
        private Vector3 posA;
        private Vector3 posB;
        public float xMax;
        public float yMax;
        public float movingSpeed;

        public GameObject[] projectilePatterns;

        private short[] patternIds;

        private short patternId = -2;

        protected override void Awake()
        {
            base.Awake();

            patternIds = new short[projectilePatterns.Length];

            for (int i = 0; i < projectilePatterns.Length; ++i)
            {
                patternIds[i] = objectPoolManager.GetTypeIdentifier(projectilePatterns[i]);
            }
        }

        /// <summary>
        /// Tries to look at the player and shoot at it when possible. Runs every frame.
        /// </summary>
        private void Update()
        {
            if (PathingComplete)
            {
                transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(theta) + 1.0f) / 2.0f);

                theta += movingSpeed * Time.deltaTime;

                if (theta > Mathf.PI)
                {
                    theta -= Mathf.PI * 2;
                }

                transform.LookAt(player.transform);

                if (canShoot)
                {
                    StartCoroutine(Shoot());
                }

                if (shaking)
                {
                    iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
                }
            }
        }

        /// <summary>
        /// This is run after the enemy has completed its path.
        /// Calls Hover function to set positions to hover between
        /// </summary>
        protected override void OnPathingComplete()
        {
            Hover();
            base.OnPathingComplete();
        }

        /// <summary>
        /// Function that creates 2 vector 3's to float up and down with a Sin()
        /// </summary>
        private void Hover()
        {
            posA = new Vector3(transform.position.x + xMax / 100,
                transform.position.y + yMax / 100,
                transform.position.z);

            posB = new Vector3(transform.position.x - xMax / 100,
                transform.position.y - yMax / 100,
                transform.position.z);
            theta = 0.0f;
        }

        /// <summary>
        /// Fires projectiles in a pattern determined by the firemode at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            //for (int x = 0; x < 25; x++)
            //{
            //    if (projectileArray[x])
            //    {
            //        GameObject projectile =
            //            objectPoolManager.Spawn(gameObject, projectileTypeIdentifier, shootPoint[x].position,
            //                                    shootPoint[x].rotation);

            //        projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
            //                                                    Random.Range(-spread, spread),
            //                                                    Random.Range(-spread, spread));
            //        projectile.GetComponent<Projectile>()
            //                  .SetColors(projectileAlbedoColor, projectileEmissionColor);
            //        Projectile projectileScript = projectile.GetComponent<Projectile>();
            //        projectileScript.Launch(0);
            //        //projectile.GetComponent<Rigidbody>().velocity =
            //        //    projectile.transform.forward * projectileSpeed;

            //        if (canRotate)
            //        {
            //            StartCoroutine(rotateProjectile(projectile));
            //        }
            //    }
            //}

            GameObject projectile =
                objectPoolManager.Spawn(gameObject, patternId, shootPoint.position,
                                        shootPoint.rotation);

            ProjectilePattern projectileScript = projectile.GetComponent<ProjectilePattern>();
            projectileScript.Launch(0);

            yield return new WaitForSeconds(fireRate);
            canShoot = true;
        }

        private IEnumerator rotateProjectile(GameObject bullet)
        {
            while (true)
            {
                bullet.GetComponent<Transform>().Rotate(0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>
        /// Function that determines the enemy's projectile, firerate,
        /// spread, and projectile speed.
        /// </summary>
        /// <param name="mode">Current Enemy Firemode</param>
        public override void SetAttackPattern(AttackPattern attackPattern)
        {
            base.SetAttackPattern(attackPattern);

            switch ((int)this.attackPattern)
            {
                //X-pattern
                case 0:
                    patternId = patternIds[0];
                    fireRate = 1.2f;
                    spread = 0;

                    //projectileArray[0] = true;
                    //projectileArray[1] = false;
                    //projectileArray[2] = false;
                    //projectileArray[3] = false;
                    //projectileArray[4] = true;

                    //projectileArray[5] = false;
                    //projectileArray[6] = true;
                    //projectileArray[7] = false;
                    //projectileArray[8] = true;
                    //projectileArray[9] = false;

                    //projectileArray[10] = false;
                    //projectileArray[11] = false;
                    //projectileArray[12] = true;
                    //projectileArray[13] = false;
                    //projectileArray[14] = false;

                    //projectileArray[15] = false;
                    //projectileArray[16] = true;
                    //projectileArray[17] = false;
                    //projectileArray[18] = true;
                    //projectileArray[19] = false;

                    //projectileArray[20] = true;
                    //projectileArray[21] = false;
                    //projectileArray[22] = false;
                    //projectileArray[23] = false;
                    //projectileArray[24] = true;
                    break;
                //inverse-X
                case 1:
                    patternId = patternIds[1];
                    fireRate = 1.2f;
                    spread = 0;

                    //projectileArray[0] = true;
                    //projectileArray[1] = true;
                    //projectileArray[2] = false;
                    //projectileArray[3] = true;
                    //projectileArray[4] = true;

                    //projectileArray[5] = true;
                    //projectileArray[6] = false;
                    //projectileArray[7] = false;
                    //projectileArray[8] = false;
                    //projectileArray[9] = true;

                    //projectileArray[10] = false;
                    //projectileArray[11] = false;
                    //projectileArray[12] = true;
                    //projectileArray[13] = false;
                    //projectileArray[14] = false;

                    //projectileArray[15] = true;
                    //projectileArray[16] = false;
                    //projectileArray[17] = false;
                    //projectileArray[18] = false;
                    //projectileArray[19] = true;

                    //projectileArray[20] = true;
                    //projectileArray[21] = true;
                    //projectileArray[22] = false;
                    //projectileArray[23] = true;
                    //projectileArray[24] = true;
                    break;
            }
        }

        /// <summary>
        /// Spawns 4 'childTurret' enemies when this enemy is destroyed
        /// </summary>
        protected override void Kill()
        {
            if (childTypeIdentifier > 0)
            {
                for (int i = 1; i <= 4; ++i)
                {
                    float xAdj = i % 2 == 0 ? -1.0f : 1.0f;
                    float yAdj = i == 1 || i == 4 ? 1.0f : -1.0f;
                    Vector3 spawn = transform.position + transform.rotation * (new Vector3(xAdj, yAdj, 0.0f) / 10.0f);

                    GameObject child =
                        objectPoolManager.Spawn(gameObject, childTypeIdentifier, spawn,
                                                transform.rotation);
                    Enemy enemy = child.GetComponent<Enemy>();
                    enemy.SetWave(wave);
                    enemy.SetPath("child");
                    enemy.SetAttackPattern(attackPattern);
                    child.GetComponent<Rigidbody>()
                         .AddRelativeForce(new Vector3(xAdj, yAdj, 0.0f) * splitDir * 4.0f);
                }
            }

            base.Kill();
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            canShoot = true;
            projectileTypeIdentifier =
                            enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            childTypeIdentifier = objectPoolManager.GetTypeIdentifier(childTurret);
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            spread = enemyAttributes.enemySpread[TypeIdentifier];
        }
    }
}