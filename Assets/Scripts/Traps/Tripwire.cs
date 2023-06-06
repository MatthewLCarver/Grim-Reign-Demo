using System;
using UnityEngine;
using UnityEngine.Serialization;


public class Tripwire : MonoBehaviour
{
    private bool isTriggered = false;
    public GameObject trapContainer;
    public GameObject[] trapArray;
    public ITrap[] iTrapArray;
    public GameObject[] wireArray; 
    private Rigidbody[] wireRigidbodyArray;
    private Joint[] wireJointArray;
    public Collider barrierCollider;
    
    /// <summary>
    /// Setup the tripwire and traps on awake
    /// </summary>
    private void Awake()
    {
        SetupTripwire();
        SetupTraps();
    }
    
    /// <summary>
    /// If the trap has not been triggered, check each joint in the array and check id there is a disconnected joint
    /// If so, break, the tripwire and trigger the ITraps in the array
    /// </summary>
    private void FixedUpdate()
    {
        if (isTriggered) return;
        foreach (Joint joint in wireJointArray)
        {
            try
            {
                if(joint.connectedBody == null)
                {
                    BreakTripwire();
                    TriggerITrapArray();
                    break;
                }
            }
            catch
            {
                BreakTripwire();
                TriggerITrapArray();
                break;
            }
        }
    }

    /// <summary>
    /// Look at all the objects in the wire array and get all their rigidbodies and joints
    /// </summary>
    private void SetupTripwire()
    {
        wireRigidbodyArray = new Rigidbody[wireArray.Length];
        for(int i = 0; i < wireRigidbodyArray.Length; i++)
        {
            wireRigidbodyArray[i] = wireArray[i].GetComponent<Rigidbody>();
        }

        wireJointArray = new Joint[wireArray.Length];
        for (int i = 0; i < wireJointArray.Length; i++)
        {
            wireJointArray[i] = wireArray[i].GetComponent<Joint>();
        }
    }
    
    /// <summary>
    /// Get all of the ITraps and store in an array
    /// This is inefficient and I can simplify this
    /// </summary>
    private void SetupTraps()
    {
        if (trapArray.Length == 0)
        {
            trapArray = new GameObject[trapContainer.transform.childCount];
            for (int i = 0; i < trapContainer.transform.childCount; i++)
            {
                trapArray[i] = trapContainer.transform.GetChild(i).gameObject;
            }
        }
        
        iTrapArray = new ITrap[trapArray.Length];
        for (int i = 0; i < trapArray.Length; i++)
        {
            iTrapArray[i] = trapArray[i].GetComponent<ITrap>();
        }
    }

    /// <summary>
    /// Loop through all the rigidbodies and set their gravity to true, then turn off the collider that is keeping you
    /// from passing
    /// </summary>
    private void BreakTripwire()
    {
        isTriggered = true;
        foreach (Rigidbody wire in wireRigidbodyArray)
        {
            wire.useGravity = true;
        }
        
        barrierCollider.enabled = false;
    }
    
    /// <summary>
    /// Trigger each ITrap in the array
    /// </summary>
    private void TriggerITrapArray()
    {
        foreach (ITrap trap in iTrapArray)
        {
            if(trap != null)
                trap.ActivateTrapTrigger();
        }
    }
}
