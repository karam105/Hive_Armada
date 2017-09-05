﻿//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC 340-01 & CPSC-344-01
// Group Project
// 
// This class controls the laser pointer for the player ship. It uses a
// Line Renderer to draw a laser from the front of the player ship to the wall.
// It originally used a cube stretched between the ship and a RayCastHit point,
// but it is easier for the player to see where they are aiming when the laser
// continues through enemies. If not, the laser could potential end just in
// front of the ship and could go unnoticed by the player.
// 

using UnityEngine;
using UnityEngine.Rendering;

namespace ShootyVR
{
    public class LaserSight : MonoBehaviour
    {
        private LineRenderer laser;
        [Tooltip("View makes line face camera. Local makes the line face the direction of the transform component")]
        public LineAlignment alignment;
        public Color color;
        public float thickness = 0.002f;
        public float alpha = 1.0f;
        public ShadowCastingMode castShadows;
        public bool receiveShadows = false;
        private Vector3 endPoint;

        // Use this for initialization
        void Start()
        {
            laser = GetComponent<LineRenderer>();
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f), },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f), });
            laser.shadowCastingMode = castShadows;
            laser.receiveShadows = receiveShadows;
            laser.alignment = alignment;
            laser.colorGradient = gradient;
            laser.startWidth = thickness;
            laser.endWidth = thickness;
        }

        // Update is called once per frame
        void Update()
        {
            // Makes the Raycast ignore all objects without the "Room" tag
            int layerMask = LayerMask.GetMask("Room");
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
            {
                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, hit.point);
            }
        }
    }
}