using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class SearchObjects : MonoBehaviour
{
    public List<Transform> transforms;
    public List<GameObject> visualizations;
    public int visIndex = -1;   
    
    public Text timesText;
    public Text instructionText;
    public Text countdownText;
    public Transform player;

    private GameObject visualization;
    private MeshRenderer vizMeshRenderer;

    private List<GameObject> siblings;
    private List<GameObject> copies;
    private int currentTransformIndex = 0;
    private Vector3 currentTransformPosition;
    private BoxCollider currentTransformCollider;
    private BoxCollider mainCollider;

    private List<DateTime> times = new List<DateTime>();
    private List<double> timeDifferences = new List<double>();
    private DateTime timer;
    private double difference = 0;

    private float countdown = 3;
    private bool countdownComplete = false;
    private bool rectVis;

    private string uniFilePath = @"E:\BA_Protokolle\UserLog.txt";
    private string homeFilePath = @"F:\BA_Protokolle\UserLog.txt";

    void OnMouseOver()
    {
        //Debug.Log("mouse over!");
        if (Input.GetMouseButtonDown(0) && countdown <= 0)
        {
            searchObjects();
        }
    }

    private void OnEnable()
    {
        initVariables();
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

    private void searchObjects()
    {
        float distance = Vector3.Distance(player.position, gameObject.GetComponent<BoxCollider>().center);
        float minDistance = maxValue(gameObject.GetComponent<BoxCollider>().size);

        // 1.5
        if (distance <= 10)
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
                timesText.text += "Du hast insgesamt " + Sum(timeDifferences).ToString("0.0") + " Sekunden gebraucht. ";
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
    }

    private void initVariables()
    {
        if(visIndex == -1) visIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, 3));
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