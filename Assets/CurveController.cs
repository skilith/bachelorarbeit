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
    
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = playerTransform.position;
        playerForward = playerTransform.forward;
        cubePosition = cubeTransform.position;

        Vector3 middlePoint = (playerPosition + cubePosition) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
