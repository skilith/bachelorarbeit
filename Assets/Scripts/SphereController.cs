using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public Transform head;
    public Transform cubeTransform;

    private Vector3 waveCenter;
    private Vector3 sphereCenter;
    
    // Start is called before the first frame update
    void Start()
    {
        sphereCenter = gameObject.transform.position;
        waveCenter = new Vector3(sphereCenter.x, sphereCenter.y + 1, sphereCenter.z);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = head.position;
        rotateToCube();
    }
    
    void rotateToCube()
    {
        Vector3 relativePos = cubeTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.left);
        transform.rotation = rotation *= Quaternion.Euler(90, 0, 0);
    }
}
