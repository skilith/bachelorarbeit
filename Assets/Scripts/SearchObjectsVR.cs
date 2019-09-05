using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SearchObjectsVR : MonoBehaviour
{
    public List<Transform> transforms;
    public List<GameObject> visualizations;
    public int visIndex = -1;
    
    public SteamVR_Action_Boolean triggerClick;
    public GameObject rightHand;
    // TODO
    public GameObject leftHand;
    
    public Text timesText;
    public Text instructionText;
    public Text countdownText;

    private GameObject visualization;
    
    private List<GameObject> siblings;
    private List<GameObject> copies;
    private int currentTransformIndex = 0;
    private Vector3 currentTransformPosition;
    private BoxCollider currentTransformCollider;
    private BoxCollider mainCollider;

    private const SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;
    private Collider collider;

    private MeshRenderer vizMeshRenderer;
    //private MeshRenderer cubeMeshRenderer;
    
    private List<DateTime> times = new List<DateTime>();
    private List<double> timeDifferences = new List<double>();
    private DateTime timer;
    private double difference = 0;

    private float countdown = 3;
    private bool countdownComplete = false;
    private bool rectVis;

    private string uniFilePath = @"E:\BA_Protokolle\UserLog.txt";
    private string homeFilePath = @"F:\BA_Protokolle\UserLog.txt";

    private void OnEnable()
    {
        initVariables();
        triggerClick.AddOnStateDownListener(searchObjects, hand);
    }

    private void OnDisable()
    {
        triggerClick.RemoveOnStateDownListener(searchObjects, hand);
    }

    private void Update()
    {
        if(!countdownComplete) Countdown();      
    }

    void Countdown()
    {
        instructionText.text = "Klicke auf die Würfel";
        countdownText.text = countdown.ToString("0");
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {         
            if (!visualization.active)
            {
                //cubeMeshRenderer.enabled = true;
                vizMeshRenderer.enabled = true;
                visualization.SetActive(true);      
                
                if (rectVis)
                {
                    siblings[0].SetActive(false);
                    copies[0].SetActive(true);
                }
            }
            
            countdownText.text = "";
            instructionText.text = "";

            if (times.Count == 0)
            {
                timer = DateTime.Now;
                times.Add(timer);
            }
            countdownComplete = true;
        }
    }

    private void searchObjects(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (fromSource)
        {
            case SteamVR_Input_Sources.Any:
                //float distance = Vector3.Distance(rightHand.transform.position, gameObject.GetComponent<BoxCollider>().center);
                float distance = Vector3.Distance(rightHand.transform.position, gameObject.transform.position);
                float minDistance = maxValue(gameObject.GetComponent<BoxCollider>().size) * 1.5f;
                
                if (distance <= minDistance)
                {
                    timer = DateTime.Now;
                    difference = (timer - times[times.Count - 1]).TotalSeconds;
                    timeDifferences.Add(difference);
                    times.Add(timer);

                    currentTransformIndex++;

                    if (currentTransformIndex == transforms.Count)
                    {
                        gameObject.SetActive(false);
                        Destroy(transforms[currentTransformIndex - 1].GetComponent<Interactable>());
                
                        if (rectVis)
                        {
                            Destroy(copies[currentTransformIndex - 1].GetComponent<Interactable>());
                    
                            siblings[currentTransformIndex - 1].SetActive(true);
                            copies[currentTransformIndex - 1].SetActive(false);
                        }
                        
                        timesText.text += (currentTransformIndex) + ". " + difference.ToString("0.00") + " Sekunden \n";
                        timesText.text += "Du hast insgesamt " + Sum(timeDifferences).ToString("0.0") +
                                         " Sekunden gebraucht. ";
                        
                        writeToFile();
                        visualization.SetActive(false);
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        currentTransformPosition = transforms[currentTransformIndex].position;
                        currentTransformCollider = transforms[currentTransformIndex].GetComponent<BoxCollider>();
                        Destroy(transforms[currentTransformIndex - 1].GetComponent<Interactable>());

                        // todo neu
                        if (rectVis)
                        {
                            Destroy(copies[currentTransformIndex - 1].GetComponent<Interactable>());
                    
                            siblings[currentTransformIndex - 1].SetActive(true);
                            copies[currentTransformIndex - 1].SetActive(false);
                    
                            siblings[currentTransformIndex].SetActive(false);
                            copies[currentTransformIndex].SetActive(true);
                        }
                
                        gameObject.transform.position = currentTransformPosition;
                        mainCollider.size = currentTransformCollider.size;
                        mainCollider.center = currentTransformCollider.center;

                        timesText.text += (currentTransformIndex) + ". " + difference.ToString("0.00") + " Sekunden \n";
                    }
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
        bool withinRange = visIndex >= 0 && visIndex <= 3;
        if(visIndex == -1 || !withinRange) visIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
        visualization = visualizations[visIndex];

        rectVis = visualization.name.Equals("RectVis");
        timesText.text = "";
        vizMeshRenderer = visualization.GetComponent<MeshRenderer>();
        //cubeMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        currentTransformPosition = transforms[currentTransformIndex].position;
        currentTransformCollider = transforms[currentTransformIndex].GetComponent<BoxCollider>();
        mainCollider = gameObject.GetComponent<BoxCollider>();

        gameObject.transform.position = currentTransformPosition;
        mainCollider.size = currentTransformCollider.size;
        mainCollider.center = currentTransformCollider.center;

        // todo new
        siblings = new List<GameObject>();
        for (int i = 2; i < transform.parent.childCount; i++)
        {
            siblings.Add(transform.parent.GetChild(i).gameObject);
        }
        
        // todo new
        copies = new List<GameObject>();
        if (rectVis)
        {
            Transform temp = transform.parent.Find("ObjCopies");
            for (int i = 0; i < temp.childCount; i++)
            {
                copies.Add(temp.GetChild(i).gameObject);
            }
        }
    }

    void writeToFile()
    {
        string path = uniFilePath;
        string dir = Path.GetDirectoryName(path);
        string filename = Path.GetFileNameWithoutExtension(path);
        string fileExt = Path.GetExtension(path);

        path = Path.Combine(dir, filename + "_" + String.Format("{0}.jpg", DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss")) + fileExt);

        using (StreamWriter file = new StreamWriter(path))
        {
            for (int j = 0; j < transforms.Count; j++)
            {
                file.WriteLine("\n Zeit zwischen {0} und {1}: \n", j, j + 1);
                file.WriteLine(timeDifferences[j].ToString("0.00") + " Sekunden");
            }

            file.WriteLine("\n Gesamtzeit: {0}", Sum(timeDifferences));
        }
    }
    
    double Sum(List<double> nums)
    {
        double result = 0;
        foreach (double num in nums)
        {
            result += num;
        }

        return result;
    }

    float maxValue(Vector3 vector)
    {
        return Mathf.Max(Mathf.Max(vector.x, vector.y), vector.z);
    }
}