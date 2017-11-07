﻿//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Visuals and stats for win condition

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hive.Armada
{
    public class Win : MonoBehaviour
    {
        /// <summary>
        /// Run when activated
        /// </summary>
        private void Awake()
        {
            StartCoroutine(Run());
        }

        /// <summary>
        /// Call stats printing, reloads scene
        /// </summary>
        /// <returns></returns>
        private IEnumerator Run()
        {
            yield return new WaitForSeconds(3);
            FindObjectOfType<PlayerStats>().PrintStats();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
