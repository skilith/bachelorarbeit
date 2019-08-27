using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI.Extensions;

public class RectController : MonoBehaviour
{
    public Camera vrCamera;
    public Transform playerHead;

    private Color standardCol;
    private Color transparent = Color.clear;
    
    private float rectH;
    private float rectW;
    private float rectX;
    private float rectY;

    private Vector3 startLookAt;
    private Vector3 currentLookAt;
    private Vector3 targetLookAt;
    
    private Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
    private Vector2 rectPos;
    private Vector2 targetPos;

    private float offsetX;
    private float offsetY;
    private Rect rectTransform;
    
    
    private void Start()
    {
        // todo gegenseitig referenzieren für bool
        standardCol = GetComponent<UIPolygon>().color;
        GetComponent<RectTransform>().sizeDelta = CalcRectSize();
        /*rectTransform = GetComponent<RectTransform>().rect;
        rectTransform.width = rectW;
        rectTransform.height = rectH;*/
    }

    private void Update()
    {
        currentLookAt = playerHead.forward;
        rectPos = CalcRectPosition(startLookAt, currentLookAt);
        transform.position = new Vector3(rectPos.x, rectPos.y, 0 );
        
        if (targetVisible()) GetComponent<UIPolygon>().color = transparent;
        else GetComponent<UIPolygon>().color = standardCol;
    }

    Vector2 CalcRectSize()
    {
        startLookAt = playerHead.forward;
        rectPos = center;
                
        // calculate height and width of box
        float vertFov = vrCamera.fieldOfView;
        float vertRatio = vertFov / 180;
        rectH = Screen.height * vertRatio;
        Debug.Log(vertFov + " / " + "180 = " + vertRatio + " * " + Screen.height + " = " + rectH);
        
        float horFov = Mathf.Rad2Deg * (2 * Mathf.Atan(Mathf.Tan((vrCamera.fieldOfView * Mathf.Deg2Rad) / 2) * vrCamera.aspect));
        float horRatio = horFov / 360;
        rectW = Screen.width * horRatio;
        Debug.Log(horFov + " / " + "360 = " + horRatio + " * " + Screen.width + " = " + rectW);
        
        return new Vector2(rectW, rectH);
    }

    Vector2 CalcRectPosition(Vector3 reference, Vector3 target)
    {
        // calculate box position relative to start
        currentLookAt = playerHead.forward;        
        
        // rotation between start and current view
        Quaternion rotation = Quaternion.FromToRotation(reference, target);
        float vertRot = rotation.eulerAngles.x;
        float horRot = rotation.eulerAngles.y;

        // 90, 270
        // todo entfernen?
        //if (vertRot > 100 && vertRot < 260) break;
        if (vertRot <= 90) vertRot *= -1;
        else if (vertRot <= 360 && vertRot >= 270) vertRot = -vertRot + 360;

        if (horRot >= 180) horRot = horRot - 360;
        
        offsetX = (horRot / 180) * Screen.width / 2;
        offsetY = (vertRot / 90) * Screen.height / 2;
        
        return new Vector2(center.x + offsetX, center.y + offsetY);
        //return new Vector2(offsetX, offsetY);
    }

    bool targetVisible()
    {
        // todo within 60%
        bool b1 = targetPos.x >= rectPos.x && targetPos.y >= rectPos.y;
        bool b2 = targetPos.x <= rectPos.x + rectW && targetPos.y <= rectPos.y + rectH;
        return b1 && b2;
    }
}
