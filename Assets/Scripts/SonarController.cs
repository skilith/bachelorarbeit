using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarController : MonoBehaviour
{
    public Transform mainObject;
    public Transform player;    

    // Update is called once per frame
    void Update()
    {
        transform.position = mainObject.position;
        transform.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
    }
}
