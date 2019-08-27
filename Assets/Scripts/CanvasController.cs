using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private float distanceToCamera = 10f;
    public Camera vrCamera;

    private void Start()
    {
        distanceToCamera = Vector3.Distance(vrCamera.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // move canvas to position in front of main camera
        //transform.position = vrCamera.transform.position + (vrCamera.transform.forward * distanceToCamera);
        // get the camera height at the frustum range- if it's orthographic, it's constant, so that's easy
        float camHeight;
        if (vrCamera.orthographic)
        {
            camHeight = vrCamera.orthographicSize * 2;
        }
        else
        {
            camHeight = 2.0f * distanceToCamera * Mathf.Tan(Mathf.Deg2Rad * (vrCamera.fieldOfView * .5f));
        }
        // now set the canvas to scale based on the difference
        // this assumes the canvas is set to the same width/height
        // as the screen resolution, so adjust that accordingly
        transform.localScale = new Vector3(camHeight / Screen.height, camHeight / Screen.height, 1);
        
        //GetComponent<RectTransform>().sizeDelta = new Vector2(vrCamera.pixelWidth, vrCamera.pixelHeight);
        //transform.forward = transform.parent.forward;
        //transform.position = vrCamera.transform.position + (vrCamera.transform.forward * 1.75f);
    }
}
