using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

[RequireComponent(typeof(Light))]
public class LightController : MonoBehaviour
{
    private Light fireLight;
    [SerializeField] private float lightTemp = 3000f;
    [SerializeField] private float lightIntensity = 10f;
    [SerializeField] private float lightRadius = 10f;
    [SerializeField] private float flickerDuration = 0.25f;

    
    [SerializeField] private float minFlickerIntensity = 10f;
    [SerializeField] private float maxFlickerIntensity = 15f;
    [SerializeField] private float minFlickerSpeed = 0.05f;
    [SerializeField] private float maxFlickerSpeed = 0.15f;
    
    private bool lightUp = true;
    
    private void Awake()
    {
        fireLight = GetComponent<Light>();
    }

    /// <summary>
    /// Set all the variables for the light controller
    /// </summary>
    void Start()
    {
        fireLight.useColorTemperature = true;
        fireLight.colorTemperature = lightTemp;
        fireLight.intensity = lightIntensity + 1f;
        fireLight.range = lightRadius;
        StartCoroutine(FlickerLight());
    }

    /// <summary>
    /// Ping Pongs the light intensity up and down to simulate flickering fire
    /// </summary>
    /// <returns></returns>
    /*private IEnumerator FlickerLight()
    {
        float newFlickerSpeed;
        
        //while (Math.Abs(fireLight.intensity - newLightIntensity) > 0.01f) Floating point error check
        while (true)
        {
            // Causes a stack overflow error
            //fireLight.intensity = Mathf.Lerp(fireLight.intensity, newLightIntensity, flickerSpeed);
        
            fireLight.intensity = (Random.Range (minFlickerIntensity, maxFlickerIntensity));
            /*if (randomizer == 0) 
                fireLight.intensity = (Random.Range (minFlickerIntensity, maxFlickerIntensity));
            else 
                fireLight.intensity = (Random.Range (minFlickerIntensity, maxFlickerIntensity));#1#
            
            newFlickerSpeed = Random.Range (minFlickerSpeed, maxFlickerSpeed);
            yield return new WaitForSeconds (newFlickerSpeed);
        }
    }*/
    
    private IEnumerator FlickerLight()
    {
        
        float newFlickerSpeed = Random.Range (minFlickerSpeed, maxFlickerSpeed);;
        float newLightIntensity = Random.Range(minFlickerIntensity, maxFlickerIntensity);
        
        // Causes a stack overflow error
        fireLight.intensity = Mathf.Lerp(fireLight.intensity, newLightIntensity, .5f);
        
        if (Math.Abs(fireLight.intensity - newLightIntensity) > 0.01f)
        {
            yield return new WaitForSeconds (newFlickerSpeed);
            
        }
            
        yield return FlickerLight();
    }
}
