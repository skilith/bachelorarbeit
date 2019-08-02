using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Valve.VR;

public class InputController : MonoBehaviour
{
    public SteamVR_Action_Boolean triggerClick;
    public GameObject rightHand;
    public GameObject leftHand;

    public GameObject visualization;
    
    //public Text upperText;
    public Text lowerText;
    public Text upperCountdownText;
    public Text lowerCountdownText;

    public Transform position1;
    public Transform position2;
    public Transform position3;
    public Transform position4;
    public Transform position5;
    public Transform position6;
    public Transform position7;
    public Transform position8;
    public Transform position9;
    public Transform position10;
    public Transform position11;
    public Transform position12;

    private List<Transform> positions = new List<Transform>();
    private int currentPositionIndex = 0;

    private const SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;
    private Collider collider;

    private MeshRenderer vizMeshRenderer;
    private MeshRenderer cubeMeshRenderer;
    
    private List<DateTime> times = new List<DateTime>();
    private List<double> timeDifferences = new List<double>();
    private DateTime timer;
    private double difference = 0;

    private float countdown = 10;

    private void OnEnable()
    {
        //swave.SetActive(true);
        //attentionFunnel.SetActive(true);
        initVariables();
        triggerClick.AddOnStateDownListener(RemoveCube, hand);
    }

    private void OnDisable()
    {
        triggerClick.RemoveOnStateDownListener(RemoveCube, hand);
    }

    private void Update()
    {
        tutorial();
        //bool buttonPressed = SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any);
        //if (buttonPressed && countdown <= 0) RemoveCube();
    }

    void tutorial()
    {
        upperCountdownText.text = "Klicke auf die Würfel";
        //lowerCountdownText.text = "Beginne in " + countdown.ToString("0") + " Sekunden";
        lowerCountdownText.text = countdown.ToString("0");
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {
            lowerCountdownText.text = "";
            upperCountdownText.text = "";
            
            if (!visualization.active)
            {
                cubeMeshRenderer.enabled = true;
                vizMeshRenderer.enabled = true;
                visualization.SetActive(true);
            }
            
            if (times.Count == 0)
            {
                timer = DateTime.Now;
                times.Add(timer);
            }
        }
    }

    /*private void RemoveCube()
    {
        if (collider.bounds.Contains(rightHand.transform.position) ||
            collider.bounds.Contains(leftHand.transform.position))
        {
            timer = DateTime.Now;
            difference = (timer - times[times.Count - 1]).TotalSeconds;
            timeDifferences.Add(difference);
            times.Add(timer);

            currentPositionIndex++;

            if (currentPositionIndex == positions.Count)
            {
                gameObject.SetActive(false);
                upperText.text = "Fertig!";
                lowerText.text = "Du hast insgesamt " + timeDifferences.Sum().ToString("0.0") +
                                 " Sekunden gebraucht. ";
                
                //writeToFile();
            }
            else
            {
                gameObject.transform.position = positions[currentPositionIndex].position;
                lowerText.text = difference.ToString("0.00") + " Sekunden";
            }
        }
        else
        {
            upperText.text = "Geh näher an den Würfel!";
        }
    }*/

    private void RemoveCube(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (fromSource)
        {
            case SteamVR_Input_Sources.Any:
                float distance = Vector3.Distance(rightHand.transform.position, gameObject.transform.position);
                
                if (distance <= 0.2)
                {
                    timer = DateTime.Now;
                    difference = (timer - times[times.Count - 1]).TotalSeconds;
                    timeDifferences.Add(difference);
                    times.Add(timer);

                    currentPositionIndex++;

                    if (currentPositionIndex == positions.Count)
                    {
                        gameObject.SetActive(false);
                        //upperText.text = "Fertig!";
                        lowerText.text += "\n Du hast insgesamt " + timeDifferences.Sum().ToString("0.0") +
                                         " Sekunden gebraucht. ";
                        //writeToFile();
                        visualization.SetActive(false);
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        gameObject.transform.position = positions[currentPositionIndex].position;
                        lowerText.text += "\n" + (currentPositionIndex) + ". " + difference.ToString("0.00") + " Sekunden";
                    }
                }
                else
                {
                    //upperText.text = "Geh näher an den Würfel!";
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

    private void initVariables()
    {
        positions.Add(position1);
        positions.Add(position2);
        positions.Add(position3);
        positions.Add(position4);
        positions.Add(position5);
        positions.Add(position6);
        positions.Add(position7);
        positions.Add(position8);
        positions.Add(position9);
        positions.Add(position10);
        positions.Add(position11);
        positions.Add(position12);

        //upperText.text = "";
        lowerText.text = "";
        
        vizMeshRenderer = visualization.GetComponent<MeshRenderer>();
        cubeMeshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    void writeToFile()
    {
        if (times.Count == positions.Count)
        {
            string path = @"C:\Users\Anita\Documents\BA_Prog_Logs\UserLog.txt";
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string fileExt = Path.GetExtension(path);

            for (int i = 1;; i++)
            {
                if (File.Exists(path))
                {
                    path = Path.Combine(dir, filename + "_" + i + fileExt);

                    using (StreamWriter file = new StreamWriter(path))
                    {
                        for (int j = 0; j < positions.Count; j++)
                        {
                            file.WriteLine("\n Zeit zwischen {0} und {1}: \n", j, j + 1);
                            file.WriteLine(timeDifferences[j]);
                        }

                        file.WriteLine("\n Gesamtzeit: {0}", timeDifferences.Sum());
                    }
                }
            }
        }
    }
}