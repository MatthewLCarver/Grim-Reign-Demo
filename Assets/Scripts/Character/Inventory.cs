using Interactables.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    private List<IItem> inventory;
    private List<Equipment> equipment;

    /// <summary>
    /// Instantiate a new list of IItems
    /// </summary>
    public Inventory()
    {
        inventory = new List<IItem>();
    }

    /// <summary>
    ///  Returns the requested IItem
    /// </summary>
    /// <param name="_item"></param>
    /// <returns></returns>
    public IItem GetItem(IItem _item)
    {
        foreach(IItem item in inventory)
        {
            if(item == _item)
                return item;
        }

        // UI prompt that the item is not collected
        return null;
    }

    /// <summary>
    /// Adds the IItem passed to the function into the inventory list
    /// </summary>
    /// <param name="_item"></param>
    public void AddItem(IItem _item)
    {
        inventory.Add(_item);
    }
    
    /// <summary>
    /// Removes the IItem passed to the function from the inventory list
    /// </summary>
    /// <param name="_item"></param>
    public void RemoveItem(IItem _item)
    {
        inventory.Remove(_item);
    }

    /// <summary>
    /// Returns the inventory
    /// </summary>
    /// <returns></returns>
    public List<IItem> GetInventory()
    {
        return inventory;
    }
}
