using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ReadSpeaker;

public class TextReader : MonoBehaviour
{
    public TTSSpeaker speaker;
    public Selectable initiallySelected;    //Reference for a selectable component
    public float delay; //Delay in seconds between reading different UI-elements.

    // Start is called before the first frame update
    void Start()
    {
        TTS.Init();     //Initializes the TTS system.
        ReadCanvas();   //Invokes ReadCanvas.
    }

    // Update is called once per frame
    void Update()
    {
        //Restarts the reading.
        if (Input.GetKeyDown("r"))
        {
            TTS.InterruptAll();     //Interrupts all current speakers.
            StopAllCoroutines();
            ReadCanvas();
        }
    }

    void ReadCanvas()
    {
        StartCoroutine(CanvasReaderCoroutine());
    }

    void ReadSelectable(Selectable selectable)
    {
        string text = "";
        if(selectable is Button)
        {
            text = selectable.GetComponentInChildren<TextMeshProUGUI>().text;
        }
        TTS.Say(text, speaker); //Speaker speaks this text, we invoke TTS.Say.
    }

    IEnumerator CanvasReaderCoroutine()
    {
        Selectable selectableToRead = initiallySelected;    //initiallySelected becomes selectable which is the one we should read.
        while(selectableToRead != null)
        {
            ReadSelectable(selectableToRead);   //Reads the selectable.
            selectableToRead.Select();  //Selects the selectable.
            yield return new WaitUntil(() => !speaker.audioSource.isPlaying);   //Coroutine waits until the audio source has stopped playing.
            yield return new WaitForSeconds(delay); //Wait for the defined delay.

            //If there is something to the right, the selectable to read will be the element to the right of the selectable to read.
            if (selectableToRead.navigation.selectOnRight != null)
            {
                selectableToRead = selectableToRead = selectableToRead.navigation.selectOnRight;
            }
            
            else if(selectableToRead.navigation.selectOnDown != null)
            {
                selectableToRead = selectableToRead = selectableToRead.navigation.selectOnDown;
            }

            else
            {
                selectableToRead = null;
            }
        }
    }
}
