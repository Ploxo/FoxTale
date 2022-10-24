using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    [SerializeField]
    private Button muteButton;
    [SerializeField]
    private Sprite mutedSprite;
    [SerializeField]
    private Sprite unmutedSprite;

    private bool muted = false;
    private float currentVolume;


    private void Start()
    {
        currentVolume = volumeSlider.value;
        SetVolume(currentVolume);
    }

    public void SetVolume(float volume)
    {
        //Debug.Log($"Changing volume to: {volume}");
        currentVolume = volume;

        if (!muted)
            audioMixer.SetFloat("Volume", volume);
    }

    public void ToggleMute()
    {
        muted = !muted;
        if (muted)
        {
            audioMixer.SetFloat("Volume", volumeSlider.minValue);
            muteButton.GetComponent<Image>().sprite = mutedSprite;
        }
        else
        {
            audioMixer.SetFloat("Volume", currentVolume);
            muteButton.GetComponent<Image>().sprite = unmutedSprite;
        }
    }

    public void SaveSettings()
    {

    }
}
