using UnityEngine;

public class MetalCage : MonoBehaviour, ITrap
{
    private bool isOpen = false;
    
    
    /// <summary>
    /// Sets the MetalCage GameObject to a dynamic opject
    /// </summary>
    public void OpenCage()
    {
        if(isOpen) return;
        
        isOpen = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    
    
    // ITrap implementation
    public void ActivateTrapTrigger()
    {
        OpenCage();
    }
}
