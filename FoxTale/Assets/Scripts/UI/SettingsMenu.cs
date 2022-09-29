using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;


    public void SetVolume(float volume)
    {
        Debug.Log($"Changing volume to: {volume}");
        audioMixer.SetFloat("Volume", volume);
    }

    public void SaveSettings()
    {

    }
}
