using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType
{
    Collect,
    CollectLeft,
    Push
}

/// <summary>
/// An interface for objects that can be interacted with using the 'E' key
/// </summary>
public interface IInteractable
{
    InteractableType InteractableType { get; set; }
    bool IsActivated { get; set; }
    void InteractOn(ref InteractableType _iType);
    void InteractOff();
}
