using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputController : MonoBehaviour
{
    public SteamVR_Action_Boolean m_GrabAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null; 
    public List<Interactable> m_ContactInteractable= new List<Interactable>();

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GrabAction.GetLastState(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + "Trigger was Pushed");
            Pickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            return;
        }
        m_ContactInteractable.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    public void Pickup()
    {
        
    }

    private Interactable GetNearestInteractable()
    {
        return null; 
    }
}
