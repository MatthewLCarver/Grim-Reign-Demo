using System;
using UnityEngine;


public class VFXController : MonoBehaviour
{
    public ParticleSystem[] loopingVFXArray;
    public ParticleSystem[] singleVFXArray;
    public Collider triggerCollider;

    public bool playOnEnter = false;
    
    /// <summary>
    /// Disables the collider, and if it's a play on enter, play all the particle systems
    /// Else stop all the particle systems
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider _other)
    {
        triggerCollider.enabled = false;
        
        if (playOnEnter)
        {
            foreach (ParticleSystem vfx in loopingVFXArray)
            {
                vfx.Play();
            }

            foreach (ParticleSystem vfx in singleVFXArray)
            {
                vfx.Play();
            }
        }
        else
        {
            foreach (ParticleSystem vfx in loopingVFXArray)
            {
                vfx.Stop();
            }
            
            foreach (ParticleSystem vfx in singleVFXArray)
            {
                vfx.Stop();
            }
        }
    }
}
