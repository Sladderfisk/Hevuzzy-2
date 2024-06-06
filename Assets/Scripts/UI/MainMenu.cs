using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Options options;
    [SerializeField] private Canvas mainMenuCanvas;

    private void Awake()
    {
        options.Awake();
        
        mainMenuCanvas.gameObject.SetActive(true);
        options.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenOptions()
    {
        options.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
    }

    public void ReturnFromOptions()
    {
        options.Save();
        options.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
    }
}
