using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Random = UnityEngine.Random;

public class RandomPositionVR : MonoBehaviour
{
    public SteamVR_Action_Boolean triggerClick;
    public GameObject rightHand;

    private const SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;

    private float x;
    private float y;
    private float z;

    private void OnEnable()
    {
        triggerClick.AddOnStateDownListener(randomPosition, hand);
    }

    private void OnDisable()
    {
        triggerClick.RemoveOnStateDownListener(randomPosition, hand);
    }

    private void randomPosition(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (fromSource)
        {
            case SteamVR_Input_Sources.Any:
                float distance = Vector3.Distance(rightHand.transform.position, gameObject.transform.position);
                float minDistance = maxValue(gameObject.GetComponent<BoxCollider>().size) * 1.5f;

                if (distance <= minDistance)
                {
                    x = Random.Range(-4.7f, 3.92f);
                    y = Random.Range(0.33f, 1.7f);
                    z = Random.Range(-4.5f, 2.5f);
                    Vector3 newPosition = new Vector3(x, y, z);

                    transform.position = newPosition;
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

    float maxValue(Vector3 vector)
    {
        return Mathf.Max(Mathf.Max(vector.x, vector.y), vector.z);
    }
}