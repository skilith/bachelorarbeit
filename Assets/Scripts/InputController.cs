using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class InputController : MonoBehaviour
{
    // TODO teste Zeitdifferenz
    // TODO teste Translation
    // TODO teste Endnachricht 
    
    public SteamVR_Action_Boolean triggerClick;
    public Text finishedText;
    
    private const SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;
    
    List<Vector3> coordinates = new List<Vector3>();
    private Vector3 coordinate = new Vector3();

    private DateTime start;
    private DateTime end;
    private List<double> times = new List<double>();
    

    private void OnEnable()
    {
        
        finishedText.text = "";
        // initializes the coordinates
        initCoordinates();
        // iterate over coordinates
        foreach (Vector3 iterCoor in coordinates)
        {
            start = DateTime.Now;
            coordinate = iterCoor;
            triggerClick.AddOnStateDownListener(RemoveCube, hand);
        }

        // display message
        finishedText.text = "Fertig!";
        writeToFile();
        
        // TODO include keyboard input 
    }

    private void OnDisable()
    {
        triggerClick.RemoveOnStateDownListener(RemoveCube, hand);
    }

    private void RemoveCube(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (fromSource)
        {
            case SteamVR_Input_Sources.Any:
                end = DateTime.Now;
                double timeDifference = (end - start).TotalSeconds;
                times.Add(timeDifference);
                Debug.Log(timeDifference + " Seconds");
                
                // translate cube
                transform.position = Vector3.MoveTowards(transform.position, coordinate, 100);
                Debug.Log("translated cube");
                
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
    
    private void initCoordinates()
    {
        coordinates.Add(new Vector3(2.403f, 0.91f, 0.402f));
        coordinates.Add(new Vector3(0.2676f, 0.942f, 2.9623f));
        coordinates.Add(new Vector3(2.403f, 1.9333f, 2.419848f));
        coordinates.Add(new Vector3(0.495f, 1.276f, -1.309f));
        coordinates.Add(new Vector3(-0.8197f, 0.909f, 1.604f));
        coordinates.Add(new Vector3(1.6288f, 0.968f, 2.358f));
        coordinates.Add(new Vector3(2.728818f, 0.0517f, -1.199f));
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
                    
                    using (System.IO.StreamWriter file = new StreamWriter(path))
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            file.WriteLine("\n Zeit zwischen {0} und {1}: \n", j, j+1);
                            file.WriteLine(times[j]);
                        }
                        file.WriteLine("\n Gesamtzeit: {0}", times.Sum());
                    }
                }
            }

            
        }
    }
    
    // Update is called once per frame
    void Update()
    {
   
    }
}
