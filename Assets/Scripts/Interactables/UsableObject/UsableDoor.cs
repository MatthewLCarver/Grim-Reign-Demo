using System;
using System.Collections;
using UnityEngine;

public class UsableDoor : MonoBehaviour, IUsableObject, IInteractable
{
    private IItem item;
    private HingeJoint hinge;
    public Vector2 jointLimits;
    public GameObject itemObject;
    private void Awake()
    {
        InteractableType = InteractableType.Collect;
        item = itemObject.GetComponent<IItem>();
        jointLimits = new Vector2(-90, 90);
        IsActivated = false;
    }

    /// <summary>
    /// Listens to the Player's OnItemCollected function
    /// </summary>
    private void Start()
    {
        // listen to event from player
        GameManager.GetPlayer().OnItemCollected += OnItemCollected;
        hinge = GetComponent<HingeJoint>();
    }

    /// <summary>
    /// Called when the Player collects an IItem (Key)
    /// Could convert this to an enum
    /// </summary>
    /// <param name="_item"></param>
    private void OnItemCollected(IItem _item)
    {
        if(_item.itemName == "Key")
            item.itemName = "Key";
    }

    /// <summary>
    /// Make the door dynamic, deactivate the collider, activate the motor, set new hinge limits and Delay the door
    /// </summary>
    private void ActivateDoor()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = false;
        
        // Open the door
        hinge.useMotor = true;

        // adjust limits
        hinge.limits = new JointLimits {min = -90, max = 90};
        
        StartCoroutine(DoorDelay());
    }

    /// <summary>
    /// Delays the door for 3 seconds and make the door kinematic again
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoorDelay()
    {
        yield return new WaitForSeconds(3f);

        GetComponent<Collider>().enabled = true;
        // Set the doors to kinematic
        GetComponent<Rigidbody>().isKinematic = true;
    }

    
    
    // IUsableObject implementation
    /// <summary>
    /// Compare the "Key" string against the IItem, Use the IItem and activate the door
    /// </summary>
    public void OnItemUsed()
    {
        Inventory playerInventory = GameManager.GetPlayer().GetPlayerInventory();
        if(playerInventory.GetInventory().Count == 0 || playerInventory.GetInventory() == null)
            return;
        foreach (IItem playerItem in playerInventory.GetInventory())
        {
            if (playerItem.itemName == item.itemName)
            {
                playerItem.UseItem();
                ActivateDoor();
                IsActivated = true;
            }
        }
    }

    public InteractableType InteractableType { get; set; }
    public bool IsActivated { get; set; }
    
    /// <summary>
    /// When interacted with that hasn't previously been used, use the IItem if it is in the player's inventory
    /// </summary>
    public void InteractOn(ref InteractableType _iType)
    {
        _iType = InteractableType;
        if (IsActivated)
            return;
        OnItemUsed();
        
    }

    public void InteractOff() {}
}