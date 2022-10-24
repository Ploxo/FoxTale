using System.Collections;
using UnityEngine;
using ReadSpeaker;


public class DialogueReader : MonoBehaviour
{
    public TTSSpeaker speaker;
    public float delay; //Delay in seconds between reading different UI-elements.
    public Sentence[] textArrays;


    void Start()
    {
        //speaker = TTS.GetDefaultSpeaker();
        //TTS.Init();     //Initializes the TTS system.
    }

    public void StartReader()
    {
        StopAllCoroutines();
        TTS.InterruptAll();

        StartCoroutine(ReadTextCoroutine());
    }

    public void StopReader()
    {
        StopAllCoroutines();
        TTS.InterruptAll();
    }

    private IEnumerator ReadTextCoroutine()
    {
        Debug.Log("Ran dialoguereader coroutine");
        if (speaker == null)
            Debug.Log($"Speaker is null");
        int sentence = 0;
        while (sentence < textArrays.Length)
        {
            TTS.Say(textArrays[sentence].text, speaker);   //Reads the text.

            yield return new WaitUntil(() => !speaker.audioSource.isPlaying);   //Coroutine waits until the audio source has stopped playing.
            yield return new WaitForSeconds(delay); //Wait for the defined delay.

            sentence++;
            yield return null;
        }
    }
}