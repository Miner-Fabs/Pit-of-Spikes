using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class OptionsScript : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;

    public Slider musicVol, sfxVol;
    public AudioMixer mainAudioMixer;

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void ChangeMusicVolume()
    {
        if (musicVol.value <= -60f)
        {
            mainAudioMixer.SetFloat("MusicVolume", -80f);
        }
        else
        {
            mainAudioMixer.SetFloat("MusicVolume", musicVol.value);
        }
    }

    public void ChangeSFXVolume()
    {
        if (sfxVol.value <= -60f)
        {
            mainAudioMixer.SetFloat("SFXVolume", -80f);
        }
        else
        {
            mainAudioMixer.SetFloat("SFXVolume", sfxVol.value);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
