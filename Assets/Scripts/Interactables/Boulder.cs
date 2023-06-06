using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour, IInteractable
{
    private Player player;
    private Rigidbody rb;
    public float playerPushForce = 20f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        IsActivated = false;
        StartCoroutine(SetLayer());
        InteractableType = InteractableType.Push;
    }

    /// <summary>
    /// Delays the setting of the boulder object to avoid collision with the ceiling
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetLayer()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.layer = 0;
    }

    private void Start()
    {
        player = GameManager.GetPlayer();
    }

    // IInteractable implementation
    public InteractableType InteractableType { get; set; }
    public bool IsActivated { get; set; }
    
    /// <summary>
    /// When interacted with, fire a ray from the player to the boulder and apply force to the boulder and the hit point
    /// </summary>
    public void InteractOn(ref InteractableType _iType)
    {
        Vector3 direction = transform.position - player.transform.position;
        if(Physics.Raycast(player.transform.position, direction, out RaycastHit hit, 10f))
        {
            if(hit.collider.gameObject == gameObject)
            {
                rb.AddForceAtPosition(direction * playerPushForce, hit.point, ForceMode.Impulse);
                _iType = InteractableType;
            }
        }
    }

    public void InteractOff() {}
}
