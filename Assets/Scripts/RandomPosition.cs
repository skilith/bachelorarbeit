using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    private float x;
    private float y;
    private float z;
    
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
          Debug.Log("click");
            gameObject.transform.localPosition = randomPosition();
        }
    }
    
    private Vector3 randomPosition()
    {
        x = Random.Range(-5.5f, 3);
        y = Random.Range(-1f, 0.8f);
        z = Random.Range(-0.6f, -7.7f);
        return new Vector3(x, y, z);
    }
}
