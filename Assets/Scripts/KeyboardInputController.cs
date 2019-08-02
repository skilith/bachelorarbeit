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

public class KeyboardInputController : MonoBehaviour
{
    public Text lowerText;
    public Text upperCountdownText;
    public Text lowerCountdownText;
    public Transform player;

    public GameObject visualization;
    
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
    private MeshRenderer vizMeshRenderer;
    private MeshRenderer cubeMeshRenderer;
    
    private List<DateTime> times = new List<DateTime>();
    private List<double> timeDifferences = new List<double>();
    private DateTime timer;
    private double difference = 0;

    private float countdown = 10;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && countdown <= 0)
        {
            RemoveCube();
        }
    }

    private void OnEnable()
    {
        //visualization.SetActive(true);
        initVariables();
    }

    private void Update()
    {
        tutorial();
    }

    void tutorial()
    {
        upperCountdownText.text = "Klicke auf die Würfel";
        //lowerCountdownText.text = "Beginne in " + countdown.ToString("0") + " Sekunden";
        lowerCountdownText.text = countdown.ToString("0");
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {
            if (!visualization.active)
            {
                cubeMeshRenderer.enabled = true;
                vizMeshRenderer.enabled = true;
                visualization.SetActive(true);
            }
            
            lowerCountdownText.text = "";
            upperCountdownText.text = "";

            if (times.Count == 0)
            {
                timer = DateTime.Now;
                times.Add(timer);
            }
        }
    }

    private void OnDisable()
    {
    }

    private void RemoveCube()
    {
        float distance = Vector3.Distance(player.position, gameObject.transform.position);

        if (distance <= 1.5)
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
                lowerText.text += "\n Du hast insgesamt " + timeDifferences.Sum().ToString("0.0") + " Sekunden gebraucht. ";
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
        else if (distance > 1.5)
        {
            //upperText.text = "Geh näher an den Würfel!";
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