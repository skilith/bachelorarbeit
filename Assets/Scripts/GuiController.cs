using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController : MonoBehaviour
{
    public Camera vrCamera;
    public Transform target;
    public Transform playerHead;

    private Color borderColor;
    private Color standardColor = new Color(0, 1, 1, 0.5f);
    private Color targetVisibleColor = new Color(0, 1, 1, 0.1f);
    private Color badRotationColor = Color.yellow;
    private Color targetColor = Color.red;
    
    private float rectH;
    private float rectW;
    private float rectX;
    private float rectY;
    private Rect topLine;
    private Rect botLine;
    private Rect leftLine;
    private Rect rightLine;
    private Rect targetRect;
    private float borderWidth = 10;

    private Vector3 startLookAt;
    private Vector3 currentLookAt;
    private Vector3 targetLookAt;
    
    private Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
    private Vector2 rectPos;
    private Vector2 targetPos;

    private float offsetX;
    private float offsetY;

    private Transform oldTarget;

    private void Start()
    {
        CalcRectSize();
    }

    private void Update()
    {
        // todo reset start on target change?
        currentLookAt = playerHead.forward;
        rectPos = RotToScreen(startLookAt, currentLookAt);
        //CalcRectPosition();
        
        targetLookAt = target.position - playerHead.position;
        targetPos = RotToScreen(startLookAt, targetLookAt);
        targetRect = new Rect(targetPos.x, targetPos.y, 10, 10);

        if (targetVisible()) borderColor = targetVisibleColor;
        else borderColor = standardColor;
    }

    private void OnGUI()
    {
        DrawOutline(rectPos);
        DrawRectangle(targetRect, targetColor);
    }

    void DrawOutline(Vector2 center)
    {
        rectX = center.x - (rectW / 2);
        rectY = center.y - (rectH / 2);
        
        topLine = new Rect(rectX, rectY, rectW, borderWidth);
        botLine = new Rect(rectX + borderWidth, rectY + rectH, rectW, borderWidth);
        leftLine = new Rect(rectX, rectY + borderWidth, borderWidth, rectH);
        rightLine = new Rect(rectX + rectW, rectY, borderWidth, rectH);
        
        DrawRectangle(topLine, borderColor);
        DrawRectangle(botLine, borderColor);
        DrawRectangle(leftLine, borderColor);
        DrawRectangle(rightLine, borderColor);
    }
    
    void DrawRectangle(Rect position, Color color)
    {
        Texture2D image = new Texture2D(1, 1);
        image.SetPixel(0, 0, color);
        image.Apply();
        
        GUI.skin.box.normal.background = image;
        GUI.Box(position, GUIContent.none);
    }
    
    void CalcRectSize()
    {
        startLookAt = playerHead.forward;
        rectPos = center;
                
        // calculate height and width of box
        float vertFov = vrCamera.fieldOfView;
        float vertRatio = vertFov / 180;
        rectH = Screen.height * vertRatio;
        
        float horFov = Mathf.Rad2Deg * (2 * Mathf.Atan(Mathf.Tan((vrCamera.fieldOfView * Mathf.Deg2Rad) / 2) * vrCamera.aspect));
        float horRatio = horFov / 360;
        rectW = Screen.width * horRatio ;
    }

    Vector2 RotToScreen(Vector3 reference, Vector3 target)
    {
        // calculate box position relative to start
        currentLookAt = playerHead.forward;        
        
        // rotation between start and current view
        Quaternion rotation = Quaternion.FromToRotation(reference, target);
        //Debug.Log(rotation.eulerAngles);
        float vertRot = rotation.eulerAngles.x;
        float horRot = rotation.eulerAngles.y;

        // 90, 270
        // todo entfernen?
        if (vertRot > 100 && vertRot < 260) borderColor = badRotationColor;
        else if (vertRot <= 90) vertRot *= -1;
        else if (vertRot <= 360 && vertRot >= 270) vertRot = -vertRot + 360;

        if (horRot >= 180) horRot = horRot - 360;
        
        offsetX = (horRot / 180) * Screen.width / 2;
        offsetY = (vertRot / 90) * Screen.height / 2;
        
        return new Vector2(center.x + offsetX, center.y - offsetY);
    }

    bool targetVisible()
    {
        // todo within 60%
        bool b1 = targetPos.x >= rectPos.x && targetPos.y >= rectPos.y;
        bool b2 = targetPos.x <= rectPos.x + rectW && targetPos.y <= rectPos.y + rectH;
        return b1 && b2;
    }
}