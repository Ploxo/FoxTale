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
    [HideInInspector]
    public State currentState;

    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private int startIndex = 0;
    [SerializeField]
    State[] states;

    public delegate void StateAction(State newState);
    public static event StateAction OnStateChanged;


    private void Start()
    {
        currentState = states[startIndex];
        Debug.Log("Current state is: " + currentState.stateId);

        // Handle state change
        OnStateChanged(currentState);
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
        OnStateChanged(currentState);
    }

    public void RunExercise(string type, int reps)
    {

    }

    /// <summary>
    /// Progress to the next state with no options.
    /// </summary>
    public void Advance()
    {
        AdvanceWithOption(0);
    }
}
