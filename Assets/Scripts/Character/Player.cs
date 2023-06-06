using Interactables.Items;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Inventory playerInventory;
    
    // Player Color Customisation
    [SerializeField] private GameObject[] playerType = new GameObject[2];
    [SerializeField] private PlayerCustomisationSettings customisationSettings;
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private Transform shieldSocket;
    private int colorID = 0;
    
    public event Action<IItem> OnItemCollected;
    
    private void Awake()
    {
        GameManager.SetPlayer(this);
        playerInventory = new Inventory();
        SetColorScheme(null);
    }

    /// <summary>
    /// Debug to check the items in your inventory
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (IItem item in playerInventory.GetInventory())
            {
                if (item != null)
                {
                    Debug.Log(item.itemName);
                }
            }
        }
    }

    /// <summary>
    /// Returns the player's inventory
    /// </summary>
    /// <returns></returns>
    public Inventory GetPlayerInventory()
    {
        return playerInventory;
    }
    
    /// <summary>
    /// Add an IItem into the player's inventory
    /// </summary>
    /// <param name="_item"></param>
    public void AddItemToInventory(IItem _item)
    {
        playerInventory.AddItem(_item);
        // Update UI on item collected
        OnItemCollected?.Invoke(_item);
    }
    
    /// <summary>
    /// Returns the requested IItem from within the player's inventory
    /// </summary>
    /// <param name="_item"></param>
    /// <returns></returns>
    public IItem GetItemFromInventory(IItem _item)
     {
         return playerInventory.GetItem(_item);
     }

    /// <summary>
    /// Removes the requested IItem from the player's inventory
    /// </summary>
    /// <param name="_item"></param>
    public void RemoveItemFromInventory(IItem _item)
    {
        playerInventory.RemoveItem(_item);
    }

    public void SetColorID(int _colorID)
    {
        colorID = _colorID;
    }
    
    public void SetColor(Color _newColor)
    {
        customisationSettings.SetColor(colorID, _newColor, true);
        SetColorScheme(customisationSettings.GetColorScheme());
    }

    public void SetColorScheme(Color[] _colorScheme)
    {
        if(_colorScheme == null)
            _colorScheme = customisationSettings.GetColorScheme();
        
        Material material = playerType[customisationSettings.GetIsMale()].GetComponent<Renderer>().material;
        
        if(customisationSettings.CheckColorDefault())
            return;
        
        material.SetColor("_MetalPlateColor",      _colorScheme[0]);
        material.SetColor("_ArmourHighlightColor", _colorScheme[1]);
        material.SetColor("_UnderArmourColor",     _colorScheme[2]);
        material.SetColor("_ClothColor",           _colorScheme[3]);
        material.SetColor("_LeatherColor",         _colorScheme[4]);
        material.SetColor("_EyeColor",             _colorScheme[5]);
    }
    
    public void ResetColorScheme()
    {
        customisationSettings.ResetColorScheme();
        SetColorScheme(null);
    }

    public Transform GetWeaponSocket()
    {
        return weaponSocket;
    }

    public Transform GetShieldSocket()
    {
        return shieldSocket;
    }
}
