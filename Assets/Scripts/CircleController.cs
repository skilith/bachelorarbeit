using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CircleController : MonoBehaviour
{
    public float screenOffset = 200;
    
    public Transform target;
    public Transform playerHead;
    public RectTransform other;
    
    private float screenWidth;
    private float screenHeight;
    
    private Color standardCol;
    
    private float rectH;
    private float rectW;

    private Vector3 startLookAt;
    private Vector3 targetLookAt;
    
    private Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
    private Vector2 rectPos;
    private Vector2 targetPos;

    private float offsetX;
    private float offsetY;
    
    private void Start()
    {
        screenWidth = Screen.width - screenOffset;
        screenHeight = Screen.height - screenOffset;
        
        //startLookAt = playerHead.forward;
        startLookAt = new Vector3(0, 0, 1);
        
        standardCol = GetComponent<UICircle>().color;
    }

    private void Update()
    {
        targetLookAt = target.position - playerHead.position;
        targetPos = CalcCirclePos(startLookAt, targetLookAt);
        GetComponent<RectTransform>().anchoredPosition = targetPos;

        float transparency = other.GetComponent<UILineRenderer>().color.a;
        GetComponent<UICircle>().color = new Color(standardCol.r, standardCol.g, standardCol.b, transparency);
        
        /*if (rectInvisible())
        {
            GetComponent<UICircle>().color = moreTransparent;
            moreTransparent.a -= transMod;
        }
        else
        {
            GetComponent<UICircle>().color = standardCol;
            moreTransparent = standardCol;
            moreTransparent.a -= transMod;
        }*/
    }


    Vector2 CalcCirclePos(Vector3 reference, Vector3 target)
    {        
        // rotation between start and current view
        Quaternion rotation = Quaternion.FromToRotation(reference, target);
        float vertRot = rotation.eulerAngles.x;
        float horRot = rotation.eulerAngles.y;

        // 90, 270
        // todo entfernen?
        //if (vertRot > 100 && vertRot < 260) borderColor = badRotationColor;
        if (vertRot <= 90) vertRot *= -1;
        else if (vertRot <= 360 && vertRot >= 270) vertRot = -vertRot + 360;

        if (horRot >= 180) horRot = horRot - 360;

        offsetX = (horRot / 180) * (screenWidth / 2);
        offsetY = (vertRot / 90) * (screenHeight / 2);
        
        //return new Vector2(center.x + offsetX, center.y + offsetY);
        return new Vector2(offsetX, offsetY);
        
        //return new Vector2(offsetX, offsetY);
    }

    bool rectInvisible()
    {
        //return other.GetComponent<UILineRenderer>().color == moreTransparent;
        return false;
    }
}
