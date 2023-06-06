using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public class PressurePlate : MonoBehaviour
{
    public GameObject trapContainer;
    [Tooltip("Use a GameObject that uses the ITrap interface.")]
    public List<GameObject> trapObjectArray;
    private ITrap[] trapArray;
    private bool isActivated = false;

    /// <summary>
    /// Adds all the traps that are a child of the trap container into an object array
    /// Then cast them as ITraps and add them to the array
    /// </summary>
    private void Start()
    {
        if(trapContainer != null)
        {
            for(int i = 0; i < trapContainer.transform.childCount; i++)
            {
                trapObjectArray.Add(trapContainer.transform.GetChild(i).gameObject);
            }
        }

        // for look through the object array and save the ITrap interface reference in an array
        trapArray = new ITrap[trapObjectArray.Count];
        for (int i = 0; i < trapObjectArray.Count; i++)
        {
            trapArray[i] = trapObjectArray[i].GetComponent<ITrap>();
        }
    }

    /// <summary>
    /// When the player enters the trigger, query each ITrap in the array and activate them
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(isActivated)
            return;
        
        isActivated = true;

        foreach (ITrap trap in trapArray)
        {
            trap?.ActivateTrapTrigger();
        }
    }
}
