using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveController : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cubeTransform;

    private Vector3 playerPosition;
    private Vector3 playerForward;
    private Vector3 cubePosition;
    private float positionDistance;
    private float forwardDistance;
    private Vector3 point1;
    private Vector3 point2;
    
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = playerTransform.position;
        playerForward = playerTransform.forward;
        cubePosition = cubeTransform.position;

        point1 = playerForward;
        point1.z += 2;
        point2 = new Vector3(0, 0, 0);
        
        float difference = Vector3.Distance(point1, cubePosition) - Vector3.Distance(playerPosition, cubePosition);
        if (difference > 0)
        {
            // if the cube is directly behind the player
            if (difference == 2)
            {
                point2 = playerTransform.right;
                point2.x += 2;
            }
            
            // increase curvature if cube is behind player
            point1.z = 4;
        }
    }

    Vector3 quadraticBezier(float t)
    {
        // t between 0 and 1
        return (1 - t) * ((1 - t) * playerPosition + t * point1) + t * ((1 - t) * point1 + t * cubePosition);
    }
    
    Vector3 cubicBezier(float t)
    {
        // t between 0 and 1
        float pow3 = (1 - t) * (1 - t) * (1 - t);
        float pow2 = (1 - t) * (1 - t);
        return pow3 * playerPosition + 3 * pow2 * t * point1 + 3 * pow2 * t * point2 + pow3 * cubePosition;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
