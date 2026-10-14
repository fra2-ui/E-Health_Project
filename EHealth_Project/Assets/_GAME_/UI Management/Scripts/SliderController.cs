using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    int progress = 0;
    public Slider slider;
    public Button buttonMute;
    public Button buttonVolume;

    private void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            LoadVolume();
        }
        else
        {
            slider.value = 0f;
        }

        UpdateButtonVisibility();
    }

    public void OnSliderChanged(float value)
    {
        SetVolume();
        SaveVolume();
        UpdateButtonVisibility();
    }

    public void UpdateProgress()
    {
        progress++;
        slider.value = progress;
    }

    public void SetVolume()
    {
        float volume = slider.value;
        AudioListener.volume = volume;  
    }

    public void SaveVolume()
    {
        float volume = slider.value;
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat("volume", 0f);
        slider.value = volume;
        AudioListener.volume = volume;
    }

    public void OnMuteButtonPressed()
    {
        slider.value = 0.5f;
        SetVolume();
        SaveVolume();
        UpdateButtonVisibility();
    }

    public void OnVolumeButtonPressed()
    {
        slider.value = 0f;
        SetVolume();
        SaveVolume();
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        if (slider.value <= 0f)
        {
            buttonMute.gameObject.SetActive(true);
            buttonVolume.gameObject.SetActive(false);
        }
        else
        {
            buttonMute.gameObject.SetActive(false);
            buttonVolume.gameObject.SetActive(true);
        }
    }
}
