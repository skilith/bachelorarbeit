using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputController : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    
    // Update is called once per frame
    void Update()
    {
        if (grabPinchAction.GetStateDown(handType))
        {
            print("pressed");
        }
    }
}
