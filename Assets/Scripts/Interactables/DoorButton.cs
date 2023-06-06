using System;
using System.Collections;
using UnityEngine;


public class DoorButton : MonoBehaviour, IInteractable
{
    public GameObject doorLeft;
    public GameObject doorRight;
    public HingeJoint hingeLeft;
    public HingeJoint hingeRight;
    public BoxCollider boxCollider;


    private void Awake()
    {
        hingeLeft = doorLeft.GetComponent<HingeJoint>();
        hingeRight = doorRight.GetComponent<HingeJoint>();
        InteractableType = InteractableType.Collect;
    }

    public InteractableType InteractableType { get; set; }
    public bool IsActivated { get; set; }

    /// <summary>
    /// When interacted with, activate the motors, set new limits, turn off the collider and delay the door
    /// </summary>
    public void InteractOn(ref InteractableType _iType)
    {
        // Open the door
        hingeLeft.useMotor = true;
        hingeRight.useMotor = true;
        
        // use limits
        hingeLeft.useLimits = true;
        hingeRight.useLimits = true;
        hingeLeft.limits = new JointLimits {min = -90, max = 90};
        hingeRight.limits = new JointLimits {min = -90, max = 90};
        
        boxCollider.enabled = false;
        
        StartCoroutine(DoorDelay(IsActivated));
        _iType = InteractableType;
        _iType = InteractableType;
    }

    /// <summary>
    /// Perform the inverse of the InteractOn function
    /// </summary>
    public void InteractOff()
    {
        // Close the door
        hingeLeft.useMotor = false;
        hingeRight.useMotor = false;
        
        // use limits
        hingeLeft.useLimits = true;
        hingeRight.useLimits = true;
        hingeLeft.limits = new JointLimits {min = 0, max = 0};
        hingeRight.limits = new JointLimits {min = 0, max = 0};
        
        boxCollider.enabled = true;
    }

    /// <summary>
    /// Delays the doors until they've been fully opened and set them to kinematic again
    /// </summary>
    /// <param name="isActivated"></param>
    /// <returns></returns>
    private IEnumerator DoorDelay(bool isActivated)
    {
        if (isActivated)
        {
            yield return new WaitForSeconds(3f);

            // Set the doors to kinematic
            doorLeft.GetComponent<Rigidbody>().isKinematic = true;
            doorRight.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            // Set the doors to kinematic
            doorLeft.GetComponent<Rigidbody>().isKinematic = false;
            doorRight.GetComponent<Rigidbody>().isKinematic = false;
        }
    }


}
