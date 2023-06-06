using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe_Z : MonoBehaviour
{
    public float force = 500f;
    public float swingPauseTime = 1f;
    public bool canSwingAxe = true;
    public bool swingTrigger = true;
    public float startSwingArch = 45f;

    public Rigidbody anchorRb;
    public Rigidbody chainRb;
    private Transform chainTransform;
    
    private const float swingArch = 45f;
    
    private void Awake()
    {
        chainTransform = chainRb.GetComponent<Transform>();
    }

    /// <summary>
    /// Set the swing arch and start the coroutine to Swing the Axe
    /// </summary>
    void Start()
    {
        chainRb.isKinematic = false;
        chainTransform.rotation = Quaternion.Euler(startSwingArch, 0, 0);
        StartCoroutine(SwingAxe());
    }

    /// <summary>
    /// If the axe can swing and the trigger is set, start the SwingAxe coroutine and check its position
    /// </summary>
    void FixedUpdate()
    {
        if(canSwingAxe && swingTrigger)
            StartCoroutine(SwingAxe());
        
        CheckAxePosition();
    }

    /// <summary>
    /// Check the Axe position on the z axis and if it exceeds 45 degrees or less than -45 degrees, set the swing bool
    /// </summary>
    private void CheckAxePosition()
    {
        if (canSwingAxe && !swingTrigger)
            return;
        
        if(chainTransform.rotation.eulerAngles.x > swingArch && 
           chainTransform.rotation.eulerAngles.x < 180f)
        {
            chainTransform.transform.rotation = Quaternion.Euler(swingArch - 1, 0, 0);
            canSwingAxe = true;
        }
        else if(chainTransform.rotation.eulerAngles.x < (360 - swingArch) &&
                chainTransform.rotation.eulerAngles.x > 180f)
        {
            chainTransform.transform.rotation = Quaternion.Euler((360 - swingArch) + 1, 0, 0);
            canSwingAxe = true;
        }
    }

    /// <summary>
    /// Turn off the swing trigger, set the axe to kinematic to kill its momentum, set it back to dynamic,
    /// apply downward force, and reset booleans 
    /// </summary>
    /// <returns></returns>
    IEnumerator SwingAxe()
    {
        swingTrigger = false;
        chainRb.isKinematic = true;
        yield return new WaitForSeconds(swingPauseTime);
        chainRb.isKinematic = false;

        AddDownwardForce();
        canSwingAxe = false;
        swingTrigger = true;
        yield break;
    }
    
    /// <summary>
    /// Applies downward force to the chain to perpetuate the swing
    /// </summary>
    private void AddDownwardForce()
    {
        chainRb.AddForce(new Vector3(0, -1, 0) * force);
    }
}
