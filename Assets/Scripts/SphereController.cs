using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public Transform head;
    public Transform targetTransform;

    private Vector3 waveCenter;
    private Vector3 sphereCenter;
    
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(targetTransform.position, gameObject.transform.position);
        gameObject.transform.position = head.position;
        gameObject.transform.localScale = new Vector3(distance / 2, distance / 2, distance / 2);
        rotateToCube();
    }
    
    void rotateToCube()
    {
        Vector3 relativePos = targetTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.left);
        // x = 90 for lines, y = 90, z = 180 for circles
        transform.rotation = rotation *= Quaternion.Euler(-25, 90, 180);
    }
}
