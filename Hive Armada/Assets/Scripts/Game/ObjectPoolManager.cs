﻿//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class contains the object pool manager. It allows us to define a list
// of prefabs to generate pools for and how large each pool should be. Then
// instead of using Instantiate(), we use Spawn() (with almost the same list of
// overloads) to "spawn" an object from the pool. Pooled objects are referred
// to by an integer that they are assigned when the pools are generated that is
// based on their index in the list of objects to pool. This allows for easy
// access to pools without having to always convert a prefab into an index.
// 
// The idea behind object pooling is to instantiate all objects that will be
// used all at once to limit spawning overhead. Then, all objects are returned
// to the pool when they are finished instead of calling Destroy() to reduce
// Unity's garbage collection as much as possible. This helps us to boost
// performance.
// 
// The pools are made up of 2 arrays of lists: one for active or objects, the
// other for inactive or objects that are ready to be spawned. When an object
// it is removed from its type's inactive pool and added to the corresponding
// active pool. When it is despawned it does the reverse.
// 
// The arrays of lists use LinkedLists. Looking at the performance and features
// of different data structures in C#, I determined that using LinkedLists
// would best satisfy the needs for the object pools.
// First, we needed easy access to any object in the pool. That eliminated
// Queues and Stacks.
// Second, we needed to be able to expand the pool in the event that we try
// to spawn an object and there isn't an inactive one available. This
// eliminated Arrays as they are set in length.
// Third, and most importantly, we needed performance. I researched the
// difference between Lists and LinkedLists. What I found was that inserting
// and removing from the end of a list was quick, it was about 100x slower to
// insert or remove from the front. LinkedLists had approximately the same
// performance for inserting and removing from both the front and back.
// For those 3 reasons, I decided that LinkedLists were the best option. It
// allows us to easily and efficiently add or remove items to the front or back
// of the pools. The only "costly" operation is if we try to despawn an object
// that is in the middle of the pool. Then we have to iterate over all nodes
// until we find the correct one with a Big-O of n.
// 
//=============================================================================

using SubjectNerd.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// This object pool manager. Contains all methods needed to pool any number of
    /// dynamically-sized  object pools to reduce lag and overhead.
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour
    {
        /// <summary>
        /// Structure of a prefab to pool and how many to create in the pool.
        /// </summary>
        [Serializable]
        public struct ObjectToPool
        {
            /// <summary>
            /// The game object to create in the pool.
            /// </summary>
            public GameObject objectPrefab;

            /// <summary>
            /// How many to create in the pool.
            /// </summary>
            public int amountToPool;
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// If a pool runs out, should it Instantiate() more objects?
        /// </summary>
        [Tooltip("If a pool runs out, should it Instantiate() more objects?")]
        public bool canExpand = true;

        /// <summary>
        /// Prefab of empty game object that is the parent of each object type in the pool.
        /// </summary>
        [Tooltip("Prefab of empty game object to use as the " +
                 "parent for each object type in the pool.")]
        public GameObject poolParentPrefab;

        /// <summary>
        /// Parents for each of the object types in the pool.
        /// </summary>
        private GameObject[] poolParents;

        /// <summary>
        /// The names of the pool parents. Used to easily update the number of pooled enemies
        /// without regenerating the name. E.g. "Enemy_Standard Parent: "
        /// </summary>
        private string[] parentNames;

        /// <summary>
        /// Array of structures for the object to pool. Includes the GameObject
        /// and how many instances to create in the pool.
        /// </summary>
        [Tooltip("Array of all objects that should have pools created for " +
                 "them and how many of each to create in the pool.")]
        [Reorderable("Pooled Object", false)]
        public ObjectToPool[] objects;

        /// <summary>
        /// Array of queues for each object to pool.
        /// These queues will hold the objects that are currently deactivated in the scene.
        /// </summary>
        private Stack<GameObject>[] inactivePools;

        /// <summary>
        /// Array of queues for each object to pool.
        /// These queues will hold the objects that are currently activated in the scene.
        /// </summary>
        private Dictionary<uint, GameObject> activePool;

        /// <summary>
        /// The last pool identifier used.
        /// </summary>
        private uint lastPoolIdentifier;

        /// <summary>
        /// If Initialize() has been run yet.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Generates all pools
        /// </summary>
        public void Initialize(ReferenceManager referenceManager)
        {
            if (isInitialized)
            {
                Debug.LogWarning(GetType().Name + " - This has already initialized!");
                return;
            }

            isInitialized = true;
            reference = referenceManager;

            if (objects.Length == 0)
            {
                Debug.LogError(GetType().Name + " - has no objects to pool!");
            }
            else
            {
                poolParents = new GameObject[objects.Length];
                parentNames = new string[objects.Length];

                inactivePools = new Stack<GameObject>[objects.Length];
                activePool = new Dictionary<uint, GameObject>();

                for (int i = 0; i < objects.Length; ++i)
                {
                    if (objects[i].objectPrefab.GetComponent<Poolable>() == null)
                    {
                        Debug.LogError(GetType().Name + " - " + objects[i].objectPrefab.name +
                                       " does not inherit Poolable!");
                        continue;
                    }

                    inactivePools[i] = new Stack<GameObject>();

                    poolParents[i] = Instantiate(poolParentPrefab, gameObject.transform);
                    parentNames[i] = " - " + objects[i].objectPrefab.name;
                    poolParents[i].name = objects[i].amountToPool + parentNames[i];

                    for (int n = 0; n < objects[i].amountToPool; ++n)
                    {
                        AddObject((short)(i));
                    }
                }
            }
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject caller, short typeIdentifier, Vector3 position)
        {
            return SpawnObject(caller, typeIdentifier, position, null, null);
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject caller, short typeIdentifier, Vector3 position,
                                Transform parent)
        {
            return SpawnObject(caller, typeIdentifier, position, null, parent);
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="rotation"> The rotation to spawn the object with </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject caller, short typeIdentifier, Vector3 position,
                                Quaternion rotation)
        {
            return SpawnObject(caller, typeIdentifier, position, rotation, null);
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="rotation"> The rotation to spawn the object with </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject caller, short typeIdentifier, Vector3 position,
                                Quaternion rotation,
                                Transform parent)
        {
            return SpawnObject(caller, typeIdentifier, position, rotation, parent);
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="rotation"> The rotation to spawn the object with </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <returns> The spawned object </returns>
        private GameObject SpawnObject(GameObject caller, short typeIdentifier, Vector3? position,
                                Quaternion? rotation,
                                Transform parent)
        {
            if (typeIdentifier == -2)
            {
                Debug.LogError(GetType().Name + " - Using uninitialized type identifier \"" +
                               typeIdentifier + "\".");
                return null;
            }

            if (typeIdentifier < 0 || typeIdentifier >= objects.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier \"" + typeIdentifier +
                               "\". Called by \"" + caller.name + "\", instance ID " +
                               caller.GetInstanceID());
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            GameObject spawned = GetObjectToSpawn(typeIdentifier);

            if (parent != null)
            {
                spawned.transform.parent = parent;
            }

            if (position != null)
            {
                spawned.transform.position = (Vector3) position;
            }

            if (rotation != null)
            {
                spawned.transform.rotation = (Quaternion) rotation;
            }

            spawned.GetComponent<Poolable>().Activate();

            return spawned;
        }

        /// <summary>
        /// Gets the first object in the corresponding pool and adds it to the active pool.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <returns> The object to spawn </returns>
        private GameObject GetObjectToSpawn(short typeIdentifier)
        {
            GameObject spawned = inactivePools[typeIdentifier].Pop();
            Poolable poolable = spawned.GetComponent<Poolable>();
            activePool.Add(poolable.PoolIdentifier, spawned);

            return spawned;
        }

        /// <summary>
        /// Resets a gameobject and puts it back in the inactive pool.
        /// </summary>
        /// <param name="objectToDespawn"> The object to despawn </param>
        public void Despawn(GameObject objectToDespawn)
        {
            objectToDespawn.transform.position = gameObject.transform.position;

            Poolable poolable = objectToDespawn.GetComponent<Poolable>();
            if (poolable != null)
            {
                short typeIdentifier = poolable.TypeIdentifier;

                poolable.Deactivate();

                objectToDespawn.transform.parent =
                    poolParents[typeIdentifier].transform;
                objectToDespawn.transform.localPosition = Vector3.zero;

                if (activePool.ContainsKey(poolable.PoolIdentifier))
                {
                    activePool.Remove(poolable.PoolIdentifier);
                    inactivePools[typeIdentifier].Push(objectToDespawn);
                }
            }
            else
            {
                Debug.LogError(GetType().Name +
                               " - Cannot despawn object because object is not poolable! \"" +
                               objectToDespawn.name + "\"");
            }
        }

        /// <summary>
        /// Finds the typeIdentifier for a given GameObject.
        /// </summary>
        /// <param name="objectType"> The object to identify </param>
        /// <returns> </returns>
        public short GetTypeIdentifier(GameObject objectType)
        {
            for (int i = 0; i < objects.Length; ++i)
            {
                if (objectType.name.Equals(objects[i].objectPrefab.name))
                {
                    return (short) i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Expands a pool that has run out of objects by 5 or 5%, whichever is larger.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier of the object whose pool we need to expand </param>
        private void ExpandPool(short typeIdentifier)
        {
            if (typeIdentifier == -2)
            {
                Debug.LogError(GetType().Name + " - Using uninitialized type identifier \"" +
                               typeIdentifier + "\".");
                return;
            }

            if (typeIdentifier < 0 || typeIdentifier >= objects.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier! \"" + typeIdentifier +
                               "\"");
                return;
            }

            int expansionAmount = Mathf.CeilToInt(objects[typeIdentifier].amountToPool * 0.05f);

            if (expansionAmount < 5)
            {
                expansionAmount = 5;
            }

            objects[typeIdentifier].amountToPool += expansionAmount;

            poolParents[typeIdentifier].name =
                objects[typeIdentifier].amountToPool + parentNames[typeIdentifier];

            for (int i = 0; i < expansionAmount; ++i)
            {
                AddObject(typeIdentifier);
            }
        }

        /// <summary>
        /// Instantiates a GameObject and adds it to the appropriate inactive pool.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object to instantiate </param>
        private void AddObject(short typeIdentifier)
        {
            try
            {
                GameObject pooled = Instantiate(objects[typeIdentifier].objectPrefab,
                                                poolParents[typeIdentifier].transform);
                pooled.GetComponent<Poolable>()
                      .Initialize(reference, typeIdentifier, lastPoolIdentifier++);
                inactivePools[typeIdentifier].Push(pooled);

                if (typeIdentifier == reference.enemyAttributes.EnemyProjectileTypeIdentifiers[0])
                {
                    reference.enemyAttributes.AddProjectile(pooled);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogWarning("Out of range - " + typeIdentifier);
                Debug.LogError(e.Message);
                Debug.LogException(e);
            }
        }
    }
}