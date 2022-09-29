using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Used to add values to arrays in inspector, then populate Dictionary with them on startup.
[System.Serializable]
public class AudioData
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    //[TextArea]
    //public string text;

    //public OptionTwo optionA;
    //public OptionTwo optionB;
}

//[System.Serializable]
//public class OptionTwo
//{
//    public int nextState;
//    public string text;
//}

/// <summary>
/// Audio database ScriptableObject. Any component can fetch data from this asset to play when required.
/// </summary>
[CreateAssetMenu(fileName = "soundDatabase", menuName = "Sound Data/Database")]
public class SoundDatabase : ScriptableObject
{
    public Dictionary<string, AudioClip> audioData = new Dictionary<string, AudioClip>();
    public AudioData[] audioDataArray;

    public void Awake()
    {
        //for (int i = 0; i < audioDataArray.Length; i++)
        //{
        //    Debug.Log($"Adding AudioClip with name {audioDataArray[i].name} to dictionary");
        //    audioData.Add(audioDataArray[i].name, audioDataArray[i].clip);
        //}
    }

    public AudioClip GetAudioClip(string name)
    {
        Debug.Log($"Fetching AudioClip with string name {name} and object name {audioData[name].name}");

        return audioData[name];
    }
}
