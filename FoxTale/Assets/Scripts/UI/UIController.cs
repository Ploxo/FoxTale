using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // UI elements
    [SerializeField]
    private Image sceneBackground;
    [SerializeField]
    private TextMeshProUGUI sceneTextArea;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private Button advanceButton;
    [SerializeField]
    private Button optionAButton;
    [SerializeField]
    private Button optionBButton;

    private void OnEnable()
    {
        //GameController.OnStateChanged += UpdateUI;
    }

    private void OnDisable()
    {
        //GameController.OnStateChanged -= UpdateUI;
    }

    public void UpdateUI(DialogueData data)
    {
        sceneBackground.sprite = data.background;
        sceneTextArea.text = data.dialogueSequence[0];

        bool showOptions = data.options.Length > 1;
        optionAButton.gameObject.SetActive(showOptions);
        optionBButton.gameObject.SetActive(showOptions);
        advanceButton.gameObject.SetActive(!showOptions);
    }
}
