using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable, IItem
{
    public Vector3 moveDistance = new Vector3(0, 0.4f, 0);
    private bool hasHoveredUp = true;

    private void Awake()
    {
        InteractableType = InteractableType.Collect; 
        IsActivated = false;
        isCollected = false;
        itemName = "Key";
        StartCoroutine(RaiseKey());
        StartCoroutine(RotateKey());
    }

    /// <summary>
    /// On Instantiation, raise the key up from the treasure chest
    /// </summary>
    /// <returns></returns>
    private IEnumerator RaiseKey()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + moveDistance;
        while (elapsedTime < 0.5f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return HoverKey();
    }
    
    
    /// <summary>
    /// Hovers the key up and down
    /// </summary>
    /// <returns></returns>
    private IEnumerator HoverKey()
    {
        float elapsedTime = 0;
        Vector3 startPos = (hasHoveredUp) ? 
            transform.position + new Vector3 (0, 0.1f, 0) :
            transform.position - new Vector3 (0, 0.1f, 0);
        
        while(elapsedTime < 2f)
        {
            // smoothly lerp the position of the key
            transform.position = Vector3.Lerp(transform.position, startPos, (elapsedTime / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        hasHoveredUp = !hasHoveredUp;
        yield return HoverKey();
    }

    
    /// <summary>
    /// Rotates the key on the y axis 360 degrees over 5 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotateKey()
    {
        float elapsedTime = 0;
        while (elapsedTime < 5f)
        {
            transform.Rotate(0, 360 * Time.deltaTime, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(RotateKey());
    }
    
    // IInteractable implementation
    public InteractableType InteractableType { get; set; }
    public bool IsActivated { get; set; }
    public void InteractOn(ref InteractableType _iType)
    {
        _iType = InteractableType;
        CollectItem();
        //Destroy(gameObject);
    }
    public void InteractOff() {}

    
    // IItem implementation
    public string itemName { get; set; }
    public bool isCollected { get; set; }
    [Tooltip("The object that the item is used for e.g. A Key will open a 'Door' object")]
    public GameObject usableObject { get; set; }
    
    /// <summary>
    /// Adds the IItem to the Player's inventory and makes the key disappear and disables the collider 
    /// </summary>
    public void CollectItem()
    {
        GameManager.GetPlayer().AddItemToInventory(this);
        GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary>
    /// Removes the reference to the key in the Player's inventory and destroys the original object
    /// </summary>
    public void UseItem()
    {
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.05f);
        GameManager.GetPlayer().GetPlayerInventory().GetInventory().Remove(this);
        Destroy(gameObject);
    }
}
