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
        x = Random.Range(-4.7f, 3.92f);
        y = Random.Range(0.33f, 2f);
        z = Random.Range(-4.5f, 2.5f);
        return new Vector3(x, y, z);
    }
    
    
}
