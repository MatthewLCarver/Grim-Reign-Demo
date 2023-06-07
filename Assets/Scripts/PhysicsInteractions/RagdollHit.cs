using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RagdollHit : MonoBehaviour
{
    private bool triggerHit = false;
    private bool colliderHit = false;

    public Collider collider;
    public Collision collision;
    
    /// <summary>
    /// Set the collider variable and triggerHit to true
    /// </summary>
    /// <param name="_other"></param>
    void OnTriggerEnter(Collider _other)
    {
        collider = _other;
        triggerHit = true;
    }

    /// <summary>
    /// Will test if the collider variable is a CharacterMover and create a ragdoll
    /// </summary>
    private void TriggerPlayer()
    {

        // It's hitting everything lol
        PlayerInputHandler playerInputHandler = collider.GetComponentInParent<PlayerInputHandler>();

        if(playerInputHandler != null &&
           playerInputHandler.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInputHandler.GetComponent<CharacterController>().enabled = false;
            playerInputHandler.GetComponent<Ragdoll>().ragdollOn = true;
        }
    }

    /// <summary>
    /// When a Collider makes a collision, set the Collision variable and set the colliderHit to true
    /// </summary>
    /// <param name="_collision"></param>
    private void OnCollisionEnter(Collision _collision)
    {
        collision = _collision;
        colliderHit = true;
        
    }

    /// <summary>
    /// Will test if the collision variable has a Ragdoll and will apply force to the nearest contact point on the
    /// Rigidbody
    /// </summary>
    private void CheckCollision()
    {
        Ragdoll ragdoll = collision.collider.GetComponentInParent<Ragdoll>();
        
        if(ragdoll != null && 
           !ragdoll.animator.enabled)
        {
            ragdoll.ApplyForce(-collision.relativeVelocity, collision.contacts[0].point);
        }
    }

    
    /// <summary>
    /// Will process the TriggerPlayer first in a frame
    /// </summary>
    private void FixedUpdate()
    {
        if(triggerHit)
            TriggerPlayer();
        triggerHit = false;
    }
    
    /// <summary>
    /// Will process the collision second in a frame
    /// </summary>
    private void LateUpdate()
    {
        if(colliderHit)
            CheckCollision();
        colliderHit = false;
    }
}