using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionFunnelController : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cubeTransform;
    public Transform gatePrefab;
    

    private Vector3 playerPosition;
    private Vector3 cubePosition;
    private Vector3 middlePoint;
    private BezierCurve curve;
    private Vector3 oldCubePosition;

    private void Start()
    {
        // TODO adapts to direction
        // TODO certain distance between gates
        curve = gameObject.GetComponent<BezierCurve>();
        oldCubePosition = cubePosition;
    }

    private void Update()
    {
        if (oldCubePosition != cubePosition)
        {
            oldCubePosition = cubePosition;
            createCurve();
        }
    }

    private void createCurve()
    {
        playerPosition = playerTransform.position;
        cubePosition = cubeTransform.position;
        middlePoint = playerPosition;
        middlePoint.z += 2;
        
        curve.points = new Vector3[] {playerPosition, middlePoint, cubePosition};

        for (float i = 0.1f; i < 1; i += 0.1f)
        {
            Vector3 currentPoint = curve.GetPoint(i);
            Vector3 nextPoint = curve.GetPoint(i += 0.1f);
            Vector3 direction = nextPoint - currentPoint;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            
            Debug.Log(currentPoint);
            
            Instantiate(gatePrefab, curve.GetPoint(i), rotation);
        }
    }
}
