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

    public void UpdateUI(State currentState)
    {
        sceneBackground.sprite = currentState.graphics;
        sceneTextArea.text = currentState.text;

        if (currentState.options.Length > 1)
        {
            optionAButton.gameObject.SetActive(true);
            optionBButton.gameObject.SetActive(true);
            advanceButton.gameObject.SetActive(false);

            optionAButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentState.options[0].text;
            optionBButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentState.options[1].text;
        }
        else
        {
            optionAButton.gameObject.SetActive(false);
            optionBButton.gameObject.SetActive(false);
            advanceButton.gameObject.SetActive(true);
            advanceButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentState.options[0].text;
        }
    }
}
