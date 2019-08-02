using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveTest : MonoBehaviour
{
    public Transform prefab;
    public Transform playerTransform;
    public GameObject target;

    private Transform targetTransform;
    private Vector3 playerPosition;
    private Vector3 cubePosition;
    private Vector3 oldCubePosition;
    private Vector3 middlePoint;
    private BezierCurve curve;
    private float increment;
    private float distance;
    
    // Start is called before the first frame update
    void Start()
    {
        targetTransform = target.transform;
        curve = gameObject.GetComponent<BezierCurve>();
        createCurve();  
    }

    // Update is called once per frame
    void Update()
    {
        removeGates();
        //cubePosition != null
        if (target.active)
        {
            createCurve();    
        }
    }

    private void OnDisable()
    {
        removeGates();
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
        cubePosition = targetTransform.position;
        oldCubePosition = cubePosition;
        middlePoint = playerPosition + playerTransform.forward;
        middlePoint.y = (playerPosition.y + cubePosition.y) / 2;
        
        curve.points = new Vector3[]
        {
            playerPosition,
            middlePoint,
            cubePosition
        };

        distance = Vector3.Distance(playerPosition, cubePosition);
        increment = 1 / Mathf.Round(10 * distance / 6);
        
        for (float i = 0.1f; i < 1f; i += increment)
        {
            Vector3 currentPoint = curve.GetPoint(i);
            Vector3 nextPoint = curve.GetPoint(i += 0.1f);
            Vector3 direction = nextPoint - currentPoint;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            rotation *= Quaternion.Euler(-90, 0, 0);
            
            Instantiate(prefab, currentPoint, rotation);
        }
    }
}
