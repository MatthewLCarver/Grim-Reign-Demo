using System;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public BoxCollider collider;

    private void Awake()
    {
        collider = GetComponents<BoxCollider>()[0];
    }

    /// <summary>
    /// Turns off the collider from an animation event within the Spike Trap Animation
    /// </summary>
    public void FlipCollider()
    {
        collider.enabled = !collider.enabled;
    }
}
