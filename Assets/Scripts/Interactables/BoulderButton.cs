using System;
using System.Collections;
using UnityEngine;

public class BoulderButton : MonoBehaviour, IInteractable
{
    public InteractableType InteractableType { get; set; }
    public bool IsActivated { get; set; }

    private void Awake()
    {
        InteractableType = InteractableType.Collect;
    }

    /// <summary>
    /// Activates the Boulder timer and spawns a boulder
    /// </summary>
    public void InteractOn(ref InteractableType _iType)
    {
        if (IsActivated) return;
        
        IsActivated = true;
        StartCoroutine(BoulderTimer());
        SpawnBoulder();
        _iType = InteractableType;
    }

    public void InteractOff() {}
    
    
    [SerializeField] private BoulderSpawner boulderSpawner; 
    
    /// <summary>
    /// Waits 20 seconds and resets the button
    /// </summary>
    /// <returns></returns>
    private IEnumerator BoulderTimer()
    {
        yield return new WaitForSeconds(20f);
        IsActivated = false;
    }
    
    /// <summary>
    /// Spawns a boulder
    /// </summary>
    private void SpawnBoulder()
    {
        boulderSpawner.Spawn();
    }
}
