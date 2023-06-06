using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpikeTrap : MonoBehaviour
{
    private GameObject spikeTrapController;
    private Animator stAnimator;
    private Rigidbody stRigidbody;
    private BoxCollider boxCollider;
    private static readonly int spikeTrigger = Animator.StringToHash("spikeTrigger");

    private void Awake()
    {
        spikeTrapController = transform.GetChild(1).gameObject;
        stAnimator = spikeTrapController.GetComponent<Animator>();
        stRigidbody = spikeTrapController.GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// When the player enters the trigger, activate the spikeTrigger in the spike trap animator and change the
    /// collision mode
    /// </summary>
    /// <param name="_collision"></param>
    void OnTriggerEnter(Collider _collision)
    {
        CharacterController cc = _collision.GetComponent<CharacterController>();
        
        if (cc != null)
        {
            boxCollider.enabled = false;
            stAnimator.SetTrigger(spikeTrigger);
            StartCoroutine(ChangeCollisionDetectionMode());
        }
    }

    /// <summary>
    /// Change the detection mode to Continuous Dynamic, wait 4 seconds then set it back to discrete for efficiency
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeCollisionDetectionMode()
    {
        stRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        yield return new WaitForSeconds(4f);
        stRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        boxCollider.enabled = true;
    }
}
