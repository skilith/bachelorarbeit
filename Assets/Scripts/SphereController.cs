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
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(cubeTransform.position, gameObject.transform.position);
        gameObject.transform.position = head.position;
        gameObject.transform.localScale = new Vector3(distance / 2, distance / 2, distance / 2);
        rotateToCube();
    }
    
    void rotateToCube()
    {
        Vector3 relativePos = cubeTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.left);
        transform.rotation = rotation *= Quaternion.Euler(90, 0, 0);
    }
}
