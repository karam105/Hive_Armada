﻿//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with controls menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour {
    private ControlsHighlighter ch;
    public AudioSource source;
    public AudioClip[] clips;

    /// <summary>
    /// Activate controller highlighting
    /// </summary>
    private void OnEnable()
    {
        ch = FindObjectOfType<Hive.Armada.Player.ShipController>().transform.parent.GetComponentInChildren<ControlsHighlighter>();
        ch.FireOn();
        ch.PowerupOn();
        ch.PauseOn();
    }

    /// <summary>
    /// Back button pressed; navigates to options menu
    /// </summary>
    public void OnBackButton()
    {
        StartCoroutine(playBackSound());
        GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject.SetActive(true);

        ch.AllOff();

        gameObject.SetActive(false);
    }

    IEnumerator playBackSound()
    {
        source.PlayOneShot(clips[0]);
        yield return new WaitForSeconds(1);
    }
}
