﻿//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This is the wave manager for the main game mode.
//
//=============================================================================

using System;
using System.Collections;
using Hive.Armada.Player;
using UnityEngine;
using SubjectNerd.Utilities;

namespace Hive.Armada.Game
{
    /// <summary>
    /// All spawn zones in the scene.
    /// </summary>
    public enum SpawnZone
    {
        /// <summary>
        /// The main spawn region in front of the player.
        /// </summary>
        Center,

        /// <summary>
        /// The spawn region that is in the front left.
        /// </summary>
        FrontLeft,

        /// <summary>
        /// The spawn region that is in the front right.
        /// </summary>
        FrontRight,

        /// <summary>
        /// The spawn region that is up in the back left.
        /// </summary>
        BackLeft,

        /// <summary>
        /// The spawn region that is up in the back right.
        /// </summary>
        BackRight
    }

    public enum Powerups
    {
        Shield = 0,
        DamageBoost = 1,
        AreaBomb = 2,
        Clear = 3,
        Ally = 4
    }

    /// <summary>
    /// All enemy types that are in normal mode.
    /// </summary>
    public enum EnemyType
    {
        Standard,
        Buckshot,
        Moving,
        Splitter,
        Kamikaze,
        SplitterChild
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct EnemyTypeSetup
    {
        [Tooltip("The prefab for the Standard enemy.")]
        public GameObject standardEnemyPrefab;

        [Tooltip("The prefab for the Buckshot enemy.")]
        public GameObject buckshotEnemyPrefab;

        [Tooltip("The prefab for the Moving enemy.")]
        public GameObject movingEnemyPrefab;

        [Tooltip("The prefab for the Splitter enemy.")]
        public GameObject splitterEnemyPrefab;

        [Tooltip("The prefab for the Kamikaze enemy.")]
        public GameObject kamikazeEnemyPrefab;

        [Tooltip("The prefab for the Splitter Child enemy.")]
        public GameObject splitterChildEnemyPrefab;
    }

    /// <summary>
    /// Structure with 2 game objects that define the lower and upper bounds of a spawn zone.
    /// </summary>
    [Serializable]
    public struct SpawnZoneBounds
    {
        /// <summary>
        /// Game object representing the lower bound of the spawn zone.
        /// </summary>
        public GameObject lowerBound;

        /// <summary>
        /// Game object representing the upper bound of the spawn zone.
        /// </summary>
        public GameObject upperBound;
    }

    /// <summary>
    /// The manager for waves and spawning.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        public BossManager bossManager;

        public WaveLoader waveLoader;

        public EnemyTypeSetup enemyTypeSetup;

        public int[] EnemyIDs { get; private set; }

        public string[] PathNames { get; private set; }

        /// <summary>
        /// Array of all available spawn zones in the scene.
        /// </summary>
        [Header("Bounds")]
        [Reorderable("Spawn Zone", false)]
        public SpawnZoneBounds[] spawnZonesBounds;

        [Reorderable("Spawn Points", false)]
        public Transform[] enemySpawnPoints;

        /// <summary>
        /// The lower and upper bounds of the powerup spawn zone.
        /// </summary>
        public SpawnZoneBounds powerupSpawnZone;

        public Transform[] powerupSpawnPoints;

        /// <summary>
        /// Array of power-up prefabs for the waves to use.
        /// </summary>
        [Header("Power-ups")]
        [Reorderable("Powerup", false)]
        public GameObject[] powerupPrefabs;

        /// <summary>
        /// Which wave to start at?
        /// </summary>
        [Header("Waves")]
        public int startingWave;

        /// <summary>
        /// Array of all waves that will be run.
        /// </summary>
        [Reorderable("Wave", false)]
        public Wave[] waves;

        /// <summary>
        /// The index for the wave that is currently running.
        /// </summary>
        private int currentWave;

        /// <summary>
        /// The source to play the wave count from.
        /// </summary>
        [Header("Audio")]
        public AudioSource waveCountSource;

        /// <summary>
        /// The clips for "wave" and wave numbers.
        /// </summary>
        [Tooltip("Sounds for the wave counts. 0 is \"Wave\", 1-15 is wave number.")]
        [Reorderable("Sound")]
        public AudioClip[] waveCountSounds;

        public AudioSource firstEnemySource;

        public AudioClip[] firstEnemyClips;

        public AudioSource bossAudioSource;

        public AudioClip[] bossAudioClips;

        /// <summary>
        /// If there are currently waves running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If all waves have been run and completed.
        /// </summary>
        public bool IsComplete { get; private set; }

        public bool IsInfinite { get; private set; }

        [HideInInspector]
        public bool spawnedPowerupOnce;

        /// <summary>
        /// Loads the waves from a file.
        /// </summary>
        public void Initialize(ReferenceManager referenceManager)
        {
            reference = referenceManager;

            ObjectPoolManager objectPool = reference.objectPoolManager;
            IsInfinite = reference.gameSettings.selectedGameMode == GameSettings.GameMode.SoloNormal;

            EnemyIDs = new[]
            {
                (int)objectPool.GetTypeIdentifier(enemyTypeSetup.standardEnemyPrefab),
                objectPool.GetTypeIdentifier(enemyTypeSetup.buckshotEnemyPrefab),
                objectPool.GetTypeIdentifier(enemyTypeSetup.movingEnemyPrefab),
                objectPool.GetTypeIdentifier(enemyTypeSetup.splitterEnemyPrefab),
                objectPool.GetTypeIdentifier(enemyTypeSetup.kamikazeEnemyPrefab),
                objectPool.GetTypeIdentifier(enemyTypeSetup.splitterChildEnemyPrefab)
            };

            PathNames = new[]
            {
                "CenterPath",
                "LeftPath",
                "RightPath",
                "BackLeftPath",
                "BackRightPath"
            };

            //waves = waveLoader.LoadWaves();
        }

        /// <summary>
        /// Runs the wave spawning for the game.
        /// </summary>
        public void Run()
        {
            if (!IsRunning)
            {
                IsRunning = true;

                if (reference == null)
                {
                    reference = FindObjectOfType<ReferenceManager>();
                }

                IsInfinite = reference.gameSettings.selectedGameMode ==
                             GameSettings.GameMode.SoloInfinite;

                if (!IsInfinite)
                {
                    --startingWave;

                    if (startingWave <= 0)
                    {
                        currentWave = 0;
                    }
                    else if (startingWave >= waves.Length)
                    {
                        currentWave = 0;
                    }
                    else
                    {
                        currentWave = startingWave;
                    }

                    reference.gameMusicSource.Play();
                    reference.statistics.IsAlive();
                    reference.iridiumSpawner.gameObject.SetActive(true);
                    RunWave(currentWave);
                }
                else
                {
                    reference.gameMusicSource.Play();
                    reference.statistics.IsAlive();
                    reference.infinite.Run();
                }

                reference.tooltips.SpawnShootEnemies();
            }
        }

        /// <summary>
        /// Begins running the waves from the waves array. Remove any stored powerups
        /// from previous wave.
        /// </summary>
        /// <param name="wave"> The index of the wave to run </param>
        private void RunWave(int wave)
        {
            StartCoroutine(WaveNumberDisplay(wave));
            reference.powerUpStatus.RemoveStoredPowerups();
        }

        /// <summary>
        /// Shows the wave number before starting the wave.
        /// </summary>
        /// <param name="wave"> The index of the wave being run </param>
        private IEnumerator WaveNumberDisplay(int wave)
        {
            //StartCoroutine(PlayWaveCount(wave + 1));
            //reference.menuWaveNumberDisplay.gameObject.SetActive(true);
            //reference.menuWaveNumberDisplay.text = "Wave: " + (wave + 1);

            //yield return new WaitForSeconds(2.0f);

            //reference.menuWaveNumberDisplay.gameObject.SetActive(false);

            yield return new WaitForSeconds(1.0f);

            waves[wave].Run(wave);

            StartCoroutine(PlayFirstEnemyAudio(wave));
        }

        private void RunBossWave(int wave)
        {
            bossManager.EnterBoss(wave);
        }

        /// <summary>
        /// Informs the wave manager that a given wave has completed.
        /// </summary>
        /// <param name="wave"> The index of the wave that completed </param>
        public void WaveComplete(int wave)
        {
            if (!waves[currentWave].IsComplete || waves[currentWave].IsRunning)
            {
                Debug.LogError(GetType().Name + " - wave" + (currentWave + 1) +
                               " says it is complete, but it isn't!");
            }

            if (waves.Length >= currentWave)
            {
                StartCoroutine(PlayBossAudio(wave));
                RunBossWave(currentWave);
            }
            else
            {
                IsRunning = false;
                IsComplete = true;

                reference.sceneTransitionManager.TransitionOut("Menu Room");
            }
        }
        
        public void BossWaveComplete(int wave)
        {
            if (waves.Length > ++currentWave)
            {
                try
                {
                    FindObjectOfType<PlayerHealth>().HealFull();
                }
                catch (Exception)
                {
                    //
                }
                RunWave(currentWave);
            }
            else
            {
                IsRunning = false;
                IsComplete = true;

                reference.statistics.won = true;
                reference.sceneTransitionManager.TransitionOut("Menu Room");
            }

            reference.statistics.WaveComplete();
        }

        public void EnemyDead(int wave)
        {
            waves[wave].EnemyDead();
        }

        public void EnemyDead(string path)
        {
            reference.infinite.EnemyDead(path);
        }

        /// <summary>
        /// Plays "wave" and then the wave number.
        /// </summary>
        /// <param name="wave"> The wave number to play, not index of the wave </param>
        private IEnumerator PlayWaveCount(int wave)
        {
            waveCountSource.PlayOneShot(waveCountSounds[0]);

            yield return new WaitForSeconds(0.9f);

            waveCountSource.PlayOneShot(waveCountSounds[wave]);
        }

        private IEnumerator PlayFirstEnemyAudio(int wave)
        {
            yield return new WaitForSeconds(1.0f);

            firstEnemySource.PlayOneShot(firstEnemyClips[wave]);

            yield return new WaitForSeconds(firstEnemyClips[wave].length);
        }

        private IEnumerator PlayBossAudio(int wave)
        {
            bossAudioSource.PlayOneShot(bossAudioClips[wave]);

            yield return new WaitForSeconds(bossAudioClips[wave].length);
        }
    }
}