using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public bool isPlayingTrack;

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private SoundDatabase database;

    private AudioSource[] audioSources;
    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

    public void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < database.audioDataArray.Length; i++)
        {
            audioDict.Add(database.audioDataArray[i].name, database.audioDataArray[i].clip);
        }

        audioSources = GetComponents<AudioSource>();
    }

    public void PlaySound(string name)
    {

    }

    public void PlayTrack(string name)
    {
        //audioSources[1].PlayOneShot(audioDict[name]);
    }
}
