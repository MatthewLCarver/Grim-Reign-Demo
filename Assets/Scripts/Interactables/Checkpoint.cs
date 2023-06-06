using System;
using UnityEngine;
using TMPro;


public class Checkpoint : MonoBehaviour, ITrap
{
    public Vector3 checkPointPosition;
    public Vector3 checkPointRotation;
    
    private bool isActivated = false;
    private GameObject player;
    private Respawner respawner;

    /// <summary>
    /// Sets the Checkpoint position of the GameObject
    /// </summary>
    private void Awake()
    {
        respawner = FindObjectOfType<Respawner>();
        //respawner = player.GetComponent<Respawner>();
        
        // get the sphere collider local position
        Vector3 colliderPosition = GetComponent<SphereCollider>().center;
        if (checkPointPosition == new Vector3(0,0,0))
        {
            checkPointPosition = new Vector3(
                transform.position.x + Mathf.Abs(colliderPosition.x),
                transform.position.y + Mathf.Abs(colliderPosition.y),
                transform.position.z + Mathf.Abs(colliderPosition.z));
        }
        else
        {
            checkPointPosition = new Vector3(
                transform.position.x + colliderPosition.x + checkPointPosition.x,
                transform.position.y + colliderPosition.y + checkPointPosition.y,
                transform.position.z + colliderPosition.z + checkPointPosition.z);
        }
    }

    /// <summary>
    /// When the player Activates the checkpoint trigger, the code accesses the Respawner component and sets the new
    /// respawn point
    /// </summary>
    public void ActivateTrapTrigger()
    {
        if (isActivated)
            return;
        
        isActivated = true;
        
        Quaternion quaternionRotation = Quaternion.Euler(checkPointRotation);
        respawner.SetCheckPoint(checkPointPosition, quaternionRotation);
    }
}
