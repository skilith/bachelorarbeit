using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI.Extensions;

public class LineController : MonoBehaviour
{
    public Camera vrCamera;
    public Transform playerHead;
    public RectTransform other;
    
    private Color standardCol;
    private Color transparent = Color.clear;

    private float rectH;
    private float rectW;

    private Vector3 startLookAt;
    private Vector3 currentLookAt;
    private Vector3 targetLookAt;

    private Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
    private Vector2 rectPos;
    private Vector2 targetPos;
    private float lineThickness;
    
    private float offsetX;
    private float offsetY;

    private void Start()
    {
        startLookAt = playerHead.forward;

        lineThickness = GetComponent<UILineRenderer>().lineThickness;
        
        standardCol = GetComponent<UILineRenderer>().color;
        CalcRectSize();
    }

    private void Update()
    {
        currentLookAt = playerHead.forward;
        rectPos = CalcRectPosition(startLookAt, currentLookAt);
        SetRectPoints(rectPos);
        
        //if (targetVisible()) GetComponent<UILineRenderer>().color = transparent;
        //else GetComponent<UILineRenderer>().color = standardCol;
    }

    void SetRectPoints(Vector2 center)
    {
        Vector2 tl = new Vector2((center.x - rectW / 2) - (lineThickness / 2), center.y + rectH / 2);
        Vector2 tr = new Vector2(center.x + rectW / 2, center.y + rectH / 2);
        Vector2 br = new Vector2(center.x + rectW / 2, center.y - rectH / 2);
        Vector2 bl = new Vector2(center.x - rectW / 2, center.y - rectH / 2);
        Vector2 tlo = new Vector2(center.x - rectW / 2, (center.y + rectH / 2) + (lineThickness / 2));
       
        GetComponent<UILineRenderer>().Points = new [] {tl, tr, br, bl, tlo};
    }

    void CalcRectSize()
    {
        rectPos = center;

        // calculate height and width of box
        float vertFov = vrCamera.fieldOfView;
        float vertRatio = vertFov / 180;
        rectH = Screen.height * vertRatio;

        float horFov = Mathf.Rad2Deg *
                       (2 * Mathf.Atan(Mathf.Tan((vrCamera.fieldOfView * Mathf.Deg2Rad) / 2) * vrCamera.aspect));
        float horRatio = horFov / 360;
        rectW = Screen.width * horRatio;
    }

    Vector2 CalcRectPosition(Vector3 reference, Vector3 target)
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

        //return new Vector2(center.x + offsetX, center.y - offsetY);
        return new Vector2(offsetX, offsetY);
    }

    bool targetVisible()
    {
        // todo within 60%
        Vector2 otherPos = other.GetComponent<RectTransform>().anchoredPosition;
        Vector2 thisPos = GetComponent<RectTransform>().anchoredPosition;
        
        bool b1 = otherPos.x >= thisPos.x && otherPos.y >= thisPos.y;
        bool b2 = otherPos.x <= thisPos.x + rectW && otherPos.y <= thisPos.y + rectH;
        return b1 && b2;
    }
}