using System;
using UnityEngine;

public class BoulderTarget : MonoBehaviour
{
    [SerializeField] private MetalCage metalCage;
    
    /// <summary>
    /// When a boulder hits the trigger, Deactivate the metal cage
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Boulder boulder = other.GetComponent<Boulder>();
        
        if(boulder != null)
            metalCage.OpenCage();
    }
}
