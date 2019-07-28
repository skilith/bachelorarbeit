using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    private float x;
    private float y;
    private float z;
    // Start is called before the first frame update
    
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.transform.position = randomPosition();
        }
    }
    
    private Vector3 randomPosition()
    {
        x = Random.Range(-5, 1.25f);
        y = Random.Range(0.05f, 2f);
        z = Random.Range(28, 37.19f);
        return new Vector3(x, y, z);
    }
    
    
}
