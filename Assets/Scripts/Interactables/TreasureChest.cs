using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    public InteractableType InteractableType { get; set; }
    public bool IsActivated { get; set; }
    private Animator animator;
    private static readonly int openChest = Animator.StringToHash("OpenChest");

    public GameObject treasurePrefab;

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        InteractableType = InteractableType.Push;
    }

    /// <summary>
    /// Creates a key and enables the keys collider
    /// </summary>
    private void PresentTreasure()
    {
        GameObject key = Instantiate(treasurePrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
        key.GetComponent<Collider>().enabled = true;
    }

    /// <summary>
    /// Disables the TreasureChest collider, sets the animation trigger 'OpenChest' and creates a Key
    /// </summary>
    public void InteractOn(ref InteractableType _iType)
    {
        if(!IsActivated)
        {
            IsActivated = true;
            GetComponent<BoxCollider>().enabled = false;
            animator.SetTrigger(openChest);
            PresentTreasure();
            _iType = InteractableType;
        }
    }

    
    public void InteractOff() {}
}
