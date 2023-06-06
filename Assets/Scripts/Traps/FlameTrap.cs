using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class FlameTrap : MonoBehaviour, ITrap, IInteractable
{
    public bool isTriggerable = false;
    public GameObject flameHeadContainer;
    private GameObject[] flameTrapHolesArray;
    private ParticleSystem[] flameFXArray;
    private Light[] flameHeadLightArray;
    private CapsuleCollider[] flameHeadColliderTriggerArray;
    private CapsuleCollider[] flameHeadColliderArray;
    private GameObject rotator;
    public InteractableType InteractableType { get; set; }
    public bool IsActivated { get; set; }
    private Vector3 flameAngle;
    
    /// <summary>
    /// Loads the flames and if the trap is not triggerable, it begins firing
    /// </summary>
    void Start()
    {
        LoadFlames();
        rotator = transform.GetChild(0).gameObject;

        if (!isTriggerable)
        {
            IsActivated = true;
            FlameOn();
        }
    }

    /// <summary>
    /// Loads all the flameTrapHoles, flameFXs, flameHeadLights, flameHeadColliderTriggers, and flameHeadColliders into
    /// their own array allowing for variable flame pillar styles for each object
    /// </summary>
    private void LoadFlames()
    {
        // Setup and capture the flame trap holes within the flame head container
        flameTrapHolesArray = new GameObject[flameHeadContainer.transform.childCount];
        for (int i = 0; i < flameHeadContainer.transform.childCount; i++)
        {
            flameTrapHolesArray[i] = flameHeadContainer.transform.GetChild(i).gameObject;
        }

        // Setup and capture the flame particle systems within the flame trap holes
        flameFXArray = new ParticleSystem[flameTrapHolesArray.Length];
        for (int i = 0; i < flameTrapHolesArray.Length; i++)
        {
            flameFXArray[i] = flameTrapHolesArray[i].transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            flameFXArray[i].Stop();
        }
        
        // Setup and capture the flame head lights within the flame trap holes
        flameHeadLightArray = new Light[flameTrapHolesArray.Length];
        for (int i = 0; i < flameTrapHolesArray.Length; i++)
        {
            flameHeadLightArray[i] = flameTrapHolesArray[i].transform.GetChild(1).gameObject.GetComponent<Light>();
        }
        
        // Setup and capture the flame head collider triggers within the flame trap holes
        flameHeadColliderTriggerArray = new CapsuleCollider[flameTrapHolesArray.Length];
        for (int i = 0; i < flameTrapHolesArray.Length; i++)
        {
            flameHeadColliderTriggerArray[i] = flameTrapHolesArray[i].transform.GetChild(2).gameObject.GetComponent<CapsuleCollider>();
        }
        
        // Setup and capture the flame head colliders within the flame trap holes
        flameHeadColliderArray = new CapsuleCollider[flameTrapHolesArray.Length];
        for (int i = 0; i < flameTrapHolesArray.Length; i++)
        {
            flameHeadColliderArray[i] = flameTrapHolesArray[i].transform.GetChild(3).gameObject.GetComponent<CapsuleCollider>();
        }
    }


    /// <summary>
    /// A coroutine that:
    /// Plays the particle effect, lerps the light up.
    /// Lerps the light down.
    /// Rotates the pillar and the flame FX
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitiateFlameTrap()
    {
        foreach (ParticleSystem flame in flameFXArray)
        {
            flame.Play(true);
        }

        for (int i = 0; i < flameHeadColliderArray.Length; i++)
        {
            flameHeadColliderTriggerArray[i].enabled = true;
            flameHeadColliderArray[i].enabled = true;
        }
        
        float time = 0;
        float duration = 1f;
        while (time < duration)
        {
            // Lerps the lights up
            for(int i = 0; i < flameHeadLightArray.Length; i++)
            {
                flameHeadLightArray[i].intensity = Mathf.Lerp(0, 3f, time / duration);
            }
            time += Time.deltaTime;
            yield return null;
        }
        
        for (int i = 0; i < flameHeadColliderArray.Length; i++)
        {
            flameHeadColliderTriggerArray[i].enabled = false;
            flameHeadColliderArray[i].enabled = false;
        }

        yield return new WaitForSeconds(2f);
        
        time = 0;
        duration = 0.25f;
        while (time < duration)
        {
            // Lerps the lights down
            for(int i = 0; i < flameHeadLightArray.Length; i++)
            {
                flameHeadLightArray[i].intensity = Mathf.Lerp(3f, 0f, time / duration);
            }
            time += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(.1f);
        if(!IsActivated)
            yield break;

        // smooth rotation of the pillar 90 degrees over 2 seconds
        time = 0;
        duration = 2f;
        
        flameAngle = rotator.transform.rotation.eulerAngles;
        float startRotation = flameAngle.y;
        float endRotation = startRotation + 90f;
        
        // Rotate the pillar and the flame FX
        while (time < duration)
        {
            rotator.transform.rotation = Quaternion.Euler(
                rotator.transform.rotation.eulerAngles.x, 
                Mathf.Lerp(startRotation, endRotation, time / duration), 
                rotator.transform.rotation.eulerAngles.z);

            for(int i = 0; i < flameTrapHolesArray.Length; i++)
            {
                flameAngle = flameFXArray[i].transform.rotation.eulerAngles;
                float fxStartRotation = flameAngle.y;
                flameFXArray[i].transform.rotation = Quaternion.Euler(
                    flameFXArray[i].transform.rotation.eulerAngles.x, 
                    flameFXArray[i].transform.rotation.eulerAngles.y,
                    flameFXArray[i].transform.rotation.eulerAngles.z);
            }
            
            // Turn off the lights
            for(int i = 0; i < flameHeadLightArray.Length; i++)
            {
                flameHeadLightArray[i].intensity = 0;
            }
            
            
            time += Time.deltaTime;
            yield return null;
        }
        yield return InitiateFlameTrap();
    }

    /// <summary>
    /// Will start the InitiateFlameTrap coroutine
    /// </summary>
    public void FlameOn()
    {
        StartCoroutine(InitiateFlameTrap());
    }
    
    /// <summary>
    /// When the trap is triggered, it will activate the trap. If its already active, it will turn it off
    /// </summary>
    public void ActivateTrapTrigger()
    {
        if (isTriggerable)
        {
            isTriggerable = false;
            IsActivated = true;
            FlameOn();
        }
        else
        {
            IsActivated = false;
        }
    }

    
    public void InteractOn(ref InteractableType _iType)
    {
        if (isTriggerable)
            FlameOn();
        
        isTriggerable = false;
        _iType = InteractableType.Collect;
    }

    public void InteractOff()
    {
        if(IsActivated)
            isTriggerable = true;
    }
}
