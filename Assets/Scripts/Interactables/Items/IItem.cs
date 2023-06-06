using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface for an Item GameObject
/// </summary>
public interface IItem
{
    public string itemName { get; set; }
    public bool isCollected { get; set; }
    public GameObject usableObject { get; set; }
    
    public void CollectItem();
    public void UseItem();
}
