using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainMenuCamera;
    [SerializeField] private Vector3 mainMenuCameraPosition;
    private bool isGoForthPressed = false;

    private void Update()
    {
        if (isGoForthPressed)
        {
            
            
            if (mainMenuCamera.transform.position != mainMenuCameraPosition)
            {
                mainMenuCamera.LookAt.position = 
                     Vector3.Lerp(mainMenuCamera.LookAt.position, new Vector3(0, 7.5f, -4), 
                         Time.deltaTime);
                mainMenuCamera.transform.position = 
                    Vector3.Lerp(mainMenuCamera.transform.position, mainMenuCameraPosition, 
                        Time.deltaTime);
            }
        }
    }
    
    public void OnGoForthButtonClicked()
    {
        isGoForthPressed = true;
    }
}
