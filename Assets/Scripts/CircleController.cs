using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CircleController : MonoBehaviour
{
    public Transform target;
    public Transform playerHead;
    public RectTransform other;
    
    private Color standardCol;
    private Color transparent = Color.clear;
    
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
        startLookAt = playerHead.forward;
        standardCol = GetComponent<UICircle>().color;
    }

    private void Update()
    {
        targetLookAt = target.position - playerHead.position;
        targetPos = CalcCirclePos(startLookAt, targetLookAt);
        GetComponent<RectTransform>().anchoredPosition = targetPos;
        //if (targetVisible()) GetComponent<UICircle>().color = transparent;
        //else GetComponent<UICircle>().color = standardCol;
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

        offsetX = (horRot / 180) * (Screen.width / 2);
        offsetY = (vertRot / 90) * (Screen.height / 2);
        
        //return new Vector2(center.x + offsetX, center.y + offsetY);
        return new Vector2(offsetX, offsetY);
        
        //return new Vector2(offsetX, offsetY);
    }

    bool targetVisible()
    {
        // todo within 60%
        Vector2 otherPos = other.GetComponent<RectTransform>().anchoredPosition;
        Vector2 thisPos = GetComponent<RectTransform>().anchoredPosition;
        
        bool b1 = thisPos.x >= otherPos.x && thisPos.y >= otherPos.y;
        bool b2 = thisPos.x <= otherPos.x + rectW && thisPos.y <= otherPos.y + rectH;
        return b1 && b2;
    }
}
