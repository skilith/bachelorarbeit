using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveTest : MonoBehaviour
{
    public Transform prefab;
    public Transform playerTransform;
    public Transform cubeTransform;

    private Vector3 playerPosition;
    private Vector3 cubePosition;
    private Vector3 oldCubePosition;
    private Vector3 middlePoint;
    private BezierCurve curve;
    
    // Start is called before the first frame update
    void Start()
    {
        curve = gameObject.GetComponent<BezierCurve>();
        createCurve();  
    }

    // Update is called once per frame
    void Update()
    {
        removeGates();
        if (cubePosition != null)
        {
            createCurve();    
        }
        
        /*cubePosition = cubeTransform.position;
        if (oldCubePosition != cubePosition)
        {
            removeGates();
            createCurve();
        }*/
    }

    private void removeGates()
    {
        GameObject[] gates = GameObject.FindGameObjectsWithTag("Gate");

        foreach (GameObject gate in gates)
        {
            Destroy(gate);
        }
    }


    private void createCurve()
    {
        playerPosition = playerTransform.position;
        cubePosition = cubeTransform.position;
        oldCubePosition = cubePosition;
        //middlePoint = (cubePosition - playerPosition) / 2;
        //middlePoint.x -= 4;
        middlePoint = Camera.main.transform.forward;
        middlePoint.y += (playerPosition.y + cubePosition.y) / 2;
        
        /*// playerPosition
        Vector3 v1 = new Vector3(0, 0, 0);
        //v1.z += 2;
        Vector3 v2 = middlePoint;
        v2.x -= 2;
        Vector3 v3 = middlePoint;
        v3.x += 2;
        if (Vector3.Distance(v1, v2) < Vector3.Distance(v1, v3)) middlePoint = v2;
        else middlePoint = v3;*/
        
        curve.points = new Vector3[]
        {
            playerPosition,
            middlePoint,
            cubePosition
        };
        for (float i = 0.1f; i < 1f; i += 0.1f)
        {
            //Vector3 previousPoint = i == 0.1f ? curve.GetPoint(0): curve.GetPoint(i -= 0.1f);
            Vector3 currentPoint = curve.GetPoint(i);
            Vector3 nextPoint = curve.GetPoint(i += 0.1f);
            Vector3 direction = nextPoint - currentPoint;
            //Vector3 direction = currentPoint - previousPoint;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            rotation *= Quaternion.Euler(-90, 0, 0);
            
            Instantiate(prefab, currentPoint, rotation);
        }
    }
}
