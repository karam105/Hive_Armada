﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLaserProjectile : MonoBehaviour
{

    GameObject enemy;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //other.GetComponent<CalcHealth>().isHit = true;
            //other.GetComponent<CalcHealth>().healthVal -= 10;
            other.GetComponent<ShootyVR.Enemies.EnemyBasic>().Hit(10);
            Destroy(gameObject);
        }
        else if (other.tag == "Room")
        {
            Destroy(gameObject);
        }
    }
}
