using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider volumeBar;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private AudioSource clickSound;

    private void Start()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
            LoadSoundVolume();
        else
            SetSoundVolume();
    }

    public void ClickPlay()
    {
        clickSound.Play();
    }
    public void SetSoundVolume()
    {
        float volume = volumeBar.value;
        myMixer.SetFloat("soundVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    private void LoadSoundVolume()
    {
        volumeBar.value = PlayerPrefs.GetFloat("soundVolume");
        SetSoundVolume();
    }
}
