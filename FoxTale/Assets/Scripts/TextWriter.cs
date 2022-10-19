using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    public enum TextMode
    {
        SINGLE,
        CONTINUOUS
    }

    public TextMode defaultTextMode;

    [SerializeField]
    LayoutController layoutController;

    public Sentence[] textArrays;

    [SerializeField]
    float timeBetweenCharacters;

    [SerializeField]
    float timeForNextWords;

    Coroutine coroutine;

    int sentence = 0;
    TextMode currentTextMode;
    bool finishWriting;


    public void StartWriter()
    {
        StartWriter(defaultTextMode);
    }

    public void StartWriter(TextMode mode)
    {
        currentTextMode = mode;
        layoutController.ClearItems();
        finishWriting = false;

        if (coroutine != null)
            StopCoroutine(coroutine);

        sentence = 0;
        SentenceEndCheck();
    }

    void SentenceEndCheck()
    {
        if (currentTextMode == TextMode.SINGLE && sentence <= textArrays.Length - 1)
        {
            //Assign the new text in the array to the text object in TextMeshPro and start typing the new sentence.
            TextMeshProUGUI textObject = layoutController.GetFirstItem();
            coroutine = StartCoroutine(TextVisible(textObject));
        }
        else if (currentTextMode == TextMode.CONTINUOUS && sentence <= textArrays.Length - 1)
        {
            TextMeshProUGUI textObject = layoutController.GetItem(sentence);
            coroutine = StartCoroutine(TextVisibleContinuous(textObject));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!finishWriting)
            {
                finishWriting = true;
            }
            else if (currentTextMode == TextMode.SINGLE)
            {
                Debug.Log("entered else");
                sentence++;
                Invoke("SentenceEndCheck", 0.1f);
            }
        }
    }

    public IEnumerator TextVisible(TextMeshProUGUI textObject)
    {
        textObject.text = textArrays[sentence].text;
        textObject.ForceMeshUpdate();  //Will force a regeneration of text for the text object? (This is neccessary according to the tutorial).

        int totalVisibleCharacters = textObject.textInfo.characterCount;   //Characters displaying will be the written message.
        int counter = 0;    //Helps tracking the time.

        finishWriting = false;

        while (true)
        {
            int visibleCount;

            if (finishWriting == true)
            {
                //finishWriting = false; // this delays the clicking
                visibleCount = textObject.textInfo.characterCount;
            }
            else
            {
                visibleCount = counter % (totalVisibleCharacters + 1);
            }

            textObject.maxVisibleCharacters = visibleCount;

            //Checks if the sentence is completed, if it is, feed the next sentence to the text writer.
            if (visibleCount >= totalVisibleCharacters)
            {
                //sentence++;
                //Invoke("SentenceEndCheck", timeForNextWords);
                break;
            }

            counter++;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }

    /// <summary>
    /// Write text sentences as one continuous segment.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TextVisibleContinuous(TextMeshProUGUI textObject)
    {
        Debug.Log(sentence);
        textObject.text = textArrays[sentence].text;
        Debug.Log(textArrays[sentence].text);
        textObject.ForceMeshUpdate();

        int totalVisibleCharacters = textObject.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount;

            if (finishWriting == true)
            {
                finishWriting = false;
                visibleCount = textObject.textInfo.characterCount;
            }
            else
            {
                visibleCount = counter % (totalVisibleCharacters + 1);
            }

            textObject.maxVisibleCharacters = visibleCount;

            //Checks if the sentence is completed, if it is, feed the next sentence to the text writer.
            if (visibleCount >= totalVisibleCharacters)
            {
                sentence++;
                Invoke("SentenceEndCheck", timeForNextWords);
                break;
            }

            counter++;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }

    }

}
