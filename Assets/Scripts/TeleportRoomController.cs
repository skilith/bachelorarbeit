﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Random = UnityEngine.Random;

public class TeleportRoomController : MonoBehaviour
{
    public GameObject cube;
    public GameObject startTrigger;
    public SteamVR_Action_Boolean triggerClick;
    public GameObject rightHand;
    public GameObject player;
    public Transform playerTarget;
    
    private bool start = false;
    private float x;
    private float y;
    private float z;
    private const SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            player.transform.position = playerTarget.position;
            player.transform.rotation = playerTarget.rotation;
        }
    }

    private void OnEnable()
    {
        triggerClick.AddOnStateDownListener(translateCube, hand);
    }

    private void OnDisable()
    {
        triggerClick.RemoveOnStateDownListener(translateCube, hand);
    }

    void translateCube(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (fromSource)
        {
            case SteamVR_Input_Sources.Any:
                float cubeDistance = Vector3.Distance(rightHand.transform.position, cube.transform.position);
                float buttonDistance = Vector3.Distance(rightHand.transform.position, startTrigger.transform.position);
                
                if (cubeDistance <= 0.2)
                {
                    cube.transform.position = randomPosition();
                }

                if (buttonDistance <= 0.4)
                {
                    start = true;
                }
                
                break;
            case SteamVR_Input_Sources.LeftHand:
                break;
            case SteamVR_Input_Sources.RightHand:
                break;
            case SteamVR_Input_Sources.LeftFoot:
                break;
            case SteamVR_Input_Sources.RightFoot:
                break;
            case SteamVR_Input_Sources.LeftShoulder:
                break;
            case SteamVR_Input_Sources.RightShoulder:
                break;
            case SteamVR_Input_Sources.Waist:
                break;
            case SteamVR_Input_Sources.Chest:
                break;
            case SteamVR_Input_Sources.Head:
                break;
            case SteamVR_Input_Sources.Gamepad:
                break;
            case SteamVR_Input_Sources.Camera:
                break;
            case SteamVR_Input_Sources.Keyboard:
                break;
            default:
                throw new ArgumentOutOfRangeException("fromSource", fromSource, null);
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