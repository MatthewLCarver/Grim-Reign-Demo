using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject customisationPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private Renderer maskMaterial;
    
    
    public void OnCustomisationButtonClicked()
    {
        customisationPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        titlePanel.SetActive(false);
    }
    
    public void OnGoForthButtonClicked()
    {
        customisationPanel.SetActive(false);
        StartCoroutine(FadeScreen());
        StartCoroutine(StartGame());
    }

    public void OnCreditsButtonClicked()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    
    public void OnBackButtonClicked()
    {
        creditsPanel.SetActive(false);
        customisationPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        titlePanel.SetActive(true);
    }
    
    public void OnQuitButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    
    private IEnumerator StartGame()
    {
        // create a 1 second time loop
        float time = 0;
        while (time < 2.55f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }

    private IEnumerator FadeScreen()
    {
        yield return new WaitForSeconds(1);
        float time = 0;
        while (time < 1.55f)
        {
            time += Time.deltaTime;
            maskMaterial.material.SetColor("_BaseColor", new Color(0,0,0, Mathf.Lerp(0, 1, time / 1.55f)));
            yield return null;
        }
    }
}
