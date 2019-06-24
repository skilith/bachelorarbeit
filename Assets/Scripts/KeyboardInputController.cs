﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardInputController : MonoBehaviour
{
    public GameObject hoverSphere;
    public Text finishedText;
    
    public Transform position1;
    public Transform position2;
    public Transform position3;
    public Transform position4;
    public Transform position5;
    public Transform position6;
    public Transform position7;
    
    private List<Transform> positions = new List<Transform>();
    private int currentPositionIndex;
    
    private Collider collider;

    private List<DateTime> times = new List<DateTime>();
    private List<double> timeDifferences = new List<double>();
    private DateTime timer;
    private double difference;
    
    private void OnEnable()
    {       
        initVariables();

        bool mouseDown = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);

        if (mouseDown)
        {
            Debug.Log("Maus geklickt");
            RemoveCube();
        }
        
        // TODO text tutorial
        finishedText.text = "Klicke auf die Würfel";
        //Thread.Sleep(10000);
        
        timer = DateTime.Now;
        times.Add(timer);        
    }
    
    private void OnDisable()
    {
        
    }

    private void RemoveCube()
    {
        if (collider.bounds.Contains(hoverSphere.transform.position))
        {
            Debug.Log("Würfel geklickt");
            timer = DateTime.Now;
            difference = (timer - times[times.Count - 1]).TotalSeconds;
            timeDifferences.Add(difference);
            times.Add(timer);
            Debug.Log(difference + " Seconds");
                    
            currentPositionIndex++;
                    
            if (currentPositionIndex == positions.Count)
            {
                gameObject.SetActive(false);
                finishedText.text = "Fertig!";
                writeToFile();
            }
            else
            {
                gameObject.transform.position = positions[currentPositionIndex].position;
                Debug.Log("translated cube");
            }
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
        
        collider = gameObject.GetComponent<BoxCollider>();
        currentPositionIndex = 0;
        finishedText.text = "";
        difference = 0.0;
    }
    
    void writeToFile()
    {
        if (times.Count == 7)
        {
            string path = @"C:\Users\Anita\Documents\BA_Prog_Logs\UserLog.txt";
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string fileExt = Path.GetExtension(path);

            for (int i = 1; ; i++)
            {
                if (File.Exists(path))
                {
                    path = Path.Combine(dir, filename + "_" + i + fileExt);
                    
                    using (StreamWriter file = new StreamWriter(path))
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            file.WriteLine("\n Zeit zwischen {0} und {1}: \n", j, j+1);
                            file.WriteLine(timeDifferences[j]);
                        }
                        file.WriteLine("\n Gesamtzeit: {0}", timeDifferences.Sum());
                    }
                }
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}