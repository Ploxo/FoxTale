using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles core game logic, such as progressing states.
/// </summary>
public class GameController : MonoBehaviour
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

    [SerializeField]
    private int startIndex = 0;
    [SerializeField]
    private UIController uiController;

    [SerializeField]
    State[] states;

    [HideInInspector]
    public State currentState;


    //public delegate void StateAction(DialogueData data);
    //public static event StateAction OnStateChanged;


    private void Start()
    {
        currentState = states[startIndex];
        Debug.Log("Current state is: " + currentState.stateId);

        // Handle state change
        //OnStateChanged(currentState.data);
        UpdateUI();
    }

    /// <summary>
    /// Progress a state depending on input (called by Unity events in the editor)
    /// </summary>
    /// <param name="option">The option chosen by the user.</param>
    public void AdvanceWithOption(int option)
    {
        Debug.Log($"Advanced to new state: {states[currentState.stateId]}");
        currentState = states[currentState.options[option].nextState];
        Debug.Log("Current state is: " + currentState.stateId);

        // Handle state change
        //OnStateChanged(currentState.data);
        UpdateUI();
    }

    private void UpdateUI() 
    {
        sceneBackground.sprite = currentState.graphics;
        sceneTextArea.text = currentState.text;

        bool showOptions = currentState.options.Length > 1;
        optionAButton.gameObject.SetActive(showOptions);
        optionBButton.gameObject.SetActive(showOptions);
        advanceButton.gameObject.SetActive(!showOptions);
    }

    /// <summary>
    /// Progress to the next state with no options.
    /// </summary>
    public void Advance()
    {
        AdvanceWithOption(0);
    }
}
