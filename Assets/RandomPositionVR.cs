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

    //private float x;
    //private float y;
    //private float z;
    //private List<Vector3> randPos;
    //private int index = 0;

    private void OnEnable()
    {
        //randPos = randomPositions();
        triggerClick.AddOnStateDownListener(randomPosition, hand);
        Debug.Log("test1");

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
                Debug.Log("test2");

                float distance = Vector3.Distance(rightHand.transform.position, gameObject.transform.position);
                Debug.Log(rightHand.transform.position);
                Debug.Log(transform.position);
                Debug.Log(distance);
                
                Debug.Log(gameObject.GetComponent<BoxCollider>().bounds.Contains(rightHand.transform.position));
                
                float minDistance = maxValue(gameObject.GetComponent<BoxCollider>().size);

                // distance <= minDistance
                if (distance <= 0.2)
                {
                    Debug.Log("test3");

                    //transform.position = randPos[index];
                    float x = Random.Range(-4.7f, 3.7f);
                    float y = Random.Range(0.6f, 2f);
                    float z = Random.Range(-4, 2);

                    Vector3 newPos = new Vector3(x, y, z);
                    transform.localPosition = newPos;
                    
                    Debug.Log("___");
                    Debug.Log(transform.localPosition);
                    Debug.Log(transform.position);
                    Debug.Log(newPos);
                    //if (transform.position != randPos[index]) transform.position.Set(randPos[index].x, randPos[index].y, randPos[index].z);
                    //index++;
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

    List<Vector3> randomPositions()
    {
        List<Vector3> temp = new List<Vector3>();
        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(-4.7f, 3.9f);
            float y = Random.Range(0.3f, 1.7f);
            float z = Random.Range(-4.5f, 2.5f);
            Vector3 vec = new Vector3(x, y, z);
            temp.Add(vec);
        }

        return temp;
    }
}