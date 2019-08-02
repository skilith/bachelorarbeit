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
    public GameObject visualization;
    
    public Text timesText;
    public Text instructionText;
    public Text countdownText;
    public Transform player;
    
    private MeshRenderer vizMeshRenderer;
    //private MeshRenderer cubeMeshRenderer;
    
    private int currentTransformIndex = 0;
    private Vector3 currentTransformPosition;
    private BoxCollider currentTransformCollider;
    private BoxCollider mainCollider;
    
    private List<DateTime> times = new List<DateTime>();
    private List<double> timeDifferences = new List<double>();
    private DateTime timer;
    private double difference = 0;

    private float countdown = 3;

    void OnMouseOver()
    {
        Debug.Log("mouse over!");
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
        tutorial();
    }

    void tutorial()
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
            }
            
            countdownText.text = "";
            instructionText.text = "";

            if (times.Count == 0)
            {
                timer = DateTime.Now;
                times.Add(timer);
            }
        }
    }

    private void searchObjects()
    {
        float distance = Vector3.Distance(player.position, gameObject.GetComponent<BoxCollider>().center);
        float minDistance = maxValue(gameObject.GetComponent<BoxCollider>().size) / 1.5f;

        // 1.5
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
                Destroy(transforms[currentTransformIndex-1].GetComponent<Interactable>());
                timesText.text += "Du hast insgesamt " + Sum(timeDifferences).ToString("0.0") + " Sekunden gebraucht. ";
                // TODO
                //writeToFile();
                visualization.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                currentTransformPosition = transforms[currentTransformIndex].position;
                currentTransformCollider = transforms[currentTransformIndex].GetComponent<BoxCollider>();
                // todo new
                Destroy(transforms[currentTransformIndex-1].GetComponent<Interactable>());
                
                gameObject.transform.position = currentTransformPosition;
                mainCollider.size = currentTransformCollider.size;
                mainCollider.center = currentTransformCollider.center;
                
                timesText.text += (currentTransformIndex) + ". " + difference.ToString("0.00") + " Sekunden \n";
            }
        } else Debug.Log("geh näher ran! Abstand: " + distance);
    }

    private void initVariables()
    {
        timesText.text = "";
        vizMeshRenderer = visualization.GetComponent<MeshRenderer>();
        //cubeMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        currentTransformPosition = transforms[currentTransformIndex].position;
        currentTransformCollider = transforms[currentTransformIndex].GetComponent<BoxCollider>();
        mainCollider = gameObject.GetComponent<BoxCollider>();

        gameObject.transform.position = currentTransformPosition;
        mainCollider.size = currentTransformCollider.size;
        mainCollider.center = currentTransformCollider.center;
        
        Debug.Log("init collider: " + currentTransformCollider);
    }

    void writeToFile()
    {
        if (times.Count == transforms.Count)
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
                        for (int j = 0; j < transforms.Count; j++)
                        {
                            file.WriteLine("\n Zeit zwischen {0} und {1}: \n", j, j + 1);
                            file.WriteLine(timeDifferences[j]);
                        }

                        // file.WriteLine("\n Gesamtzeit: {0}", timeDifferences.Sum());
                        file.WriteLine("\n Gesamtzeit: {0}", Sum(timeDifferences));
                    }
                }
            }
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
