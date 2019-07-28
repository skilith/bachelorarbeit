using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class BuzzerController : MonoBehaviour
{
    public GameObject player;
    public GameObject cube;
    public Transform playerTarget;
    
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cube.SetActive(false);
            player.transform.position = playerTarget.position;
            player.transform.rotation = playerTarget.rotation;
        }
    }
}
