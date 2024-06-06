using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Toggle fullScreenToggle;

    public void Awake()
    {
        masterSlider.onValueChanged.AddListener(OnMasterChange);
        musicSlider.onValueChanged.AddListener(OnMusicChange);
        sfxSlider.onValueChanged.AddListener(OnSFXChange);
        
        fullScreenToggle.onValueChanged.AddListener(OnToggleChange);

        float value;
        value = PlayerPrefs.GetFloat("MasterMixer");
        mixer.SetFloat("Master", value);
        masterSlider.value = value;
        
        value = PlayerPrefs.GetFloat("MusicMixer");
        mixer.SetFloat("Music", value);
        musicSlider.value = value;

        value = PlayerPrefs.GetFloat("SFXMixer");
        mixer.SetFloat("SFX", value);
        sfxSlider.value = value;

        bool bValue;
        bValue = PlayerPrefs.GetInt("FullScreenToggle") == 1;
        Screen.fullScreen = bValue;
    }

    public void OnMasterChange(float value)
    {
        mixer.SetFloat("Master", value);
    }

    public void OnMusicChange(float value)
    {
        mixer.SetFloat("Music", value);
    }

    public void OnSFXChange(float value)
    {
        mixer.SetFloat("SFX", value);
    }

    public void OnToggleChange(bool value)
    {
        Screen.fullScreen = value;
    }

    public void Save()
    {
        float value = 0.0f;
        mixer.GetFloat("Master", out value);
        PlayerPrefs.SetFloat("MasterMixer", value);
        
        mixer.GetFloat("Music", out value);
        PlayerPrefs.SetFloat("MusicMixer", value);
        
        mixer.GetFloat("SFX", out value);
        PlayerPrefs.SetFloat("SFXMixer", value);
        
        PlayerPrefs.SetInt("FullScreenToggle", fullScreenToggle.isOn ? 1 : 0);
    }
}
