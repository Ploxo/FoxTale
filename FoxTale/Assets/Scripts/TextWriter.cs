using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshProText;    //Grabs the Text Mesh Pro text component we want.

    public string[] textArrays;

    [SerializeField]
    float timeBetweenCharacters;
    
    [SerializeField]
    float timeForNextWords;

    int sentence = 0;
    
    bool Test = false;
    // Start is called before the first frame update
    void Start()
    {
        SentenceEndCheck();
    }

    void SentenceEndCheck()
    {
        if (sentence <= textArrays.Length - 1)
        {
            //Assign the new text in the array to the text object in TextMeshPro and start typing the new sentence.
            textMeshProText.text = textArrays[sentence];
            StartCoroutine(TextVisible());
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Test = true;
        }
    }
    public IEnumerator TextVisible()
    {
        textMeshProText.ForceMeshUpdate();  //Will force a regeneration of text for the text object? (This is neccessary according to the tutorial).
        int totalVisibleCharacters = textMeshProText.textInfo.characterCount;   //Characters displaying will be the written message.
        int counter = 0;    //Helps tracking the time.

        Test = false;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);  //visibleCount gets incremented as time goes.
            textMeshProText.maxVisibleCharacters = visibleCount;

            if (Test == true)
            {
                Test = false;
                textMeshProText.maxVisibleCharacters = textMeshProText.textInfo.characterCount;
                Invoke("SentenceEndCheck", timeForNextWords);
                sentence++;
                break;
            }

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
