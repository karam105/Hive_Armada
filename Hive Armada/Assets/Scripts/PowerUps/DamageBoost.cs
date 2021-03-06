﻿//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This class is for the Damage Boost powerup. It has public variables that
// control how long it lasts for and how much it boosts the player's damage by.
// The boost behaves as a multiplier to damage done by the player's weapons.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Damage boost powerup
    /// </summary>
    public class DamageBoost : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Length of the boost, in seconds.
        /// </summary>
        public float boostLength;

        /// <summary>
        /// Multiplier of how much to boost damage by.
        /// </summary>
        public int boost;

        /// <summary>
        /// The particle emitter to play when the boost is activated.
        /// </summary>
        public GameObject spawnEmitter;

        AudioSource source;

        AudioSource bossSource;

        public AudioClip clip;

        /// <summary>
        /// Spawns the spawn particle emitter and runs the damage boost.
        /// </summary>
        private void Start()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            source = GameObject.Find("Powerup Audio Source").GetComponent<AudioSource>();
            bossSource = GameObject.Find("Boss Audio Source").GetComponent<AudioSource>();
            StartCoroutine(PauseForBoss());

            Instantiate(spawnEmitter, reference.playerShip.transform);
            StartCoroutine(Run());
        }

        /// <summary>
        /// Applies the damage boost for boostLength seconds and then resets it back to 1.
        /// </summary>
        private IEnumerator Run()
        {
            reference.playerShip.GetComponent<ShipController>().SetDamageBoost(boost);

            yield return new WaitForSeconds(boostLength);

            reference.playerShip.GetComponent<ShipController>().SetDamageBoost(1);
            Destroy(gameObject);
        }

        private IEnumerator PauseForBoss()
        {
            if (bossSource.isPlaying)
            {
                yield return new WaitWhile(() => bossSource.isPlaying);

                if (source.isPlaying)
                {
                    yield return new WaitWhile(() => source.isPlaying);
                }

                if (!source.isPlaying)
                {
                    source.PlayOneShot(clip);
                }
            }
            else
            {
                source.PlayOneShot(clip);
            }
        }
    }
}