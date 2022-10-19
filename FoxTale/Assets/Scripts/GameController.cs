using System;
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
    private ExerciseController exerciseController;
    [SerializeField]
    private int startIndex = 0;
 
    [SerializeField]
    State[] states;

    public delegate void StateAction(State newState);
    public event StateAction OnStateChanged;

    public delegate void ExerciseAction(ExerciseInfo exercise);
    public event ExerciseAction OnExerciseStart;
    public event ExerciseAction OnExerciseEnd;


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
        if (currentState.options[option].exercise)
        {
            StartCoroutine(WaitForCompletion(currentState.options[option].exercise, option));

            //StartCoroutine(WaitForExercise(option));
        }
        else
        {
            Debug.Log("Chose option " + option);
            currentState = states[currentState.options[option].nextState];
            Debug.Log("Current state is: " + currentState.stateId);

            OnStateChanged(currentState);
        }
    }

    private IEnumerator WaitForCompletion(ExerciseInfo exercise, int option)
    {
        OnExerciseStart(exercise);

        bool success = false;
        yield return exerciseController.StartExercise(exercise, result => success = result);

        if (success)
            currentState = states[currentState.options[option].nextState];

        OnExerciseEnd(exercise);
        OnStateChanged(currentState);
    }

    //private void OnExerciseCallback(bool success)
    //{
    //    if (success)
    //        currentState = states[currentState.options[option].nextState];

    //    OnStateChanged(currentState);
    //}

    /// <summary>
    /// Progress to the next state with no options.
    /// </summary>
    public void Advance()
    {
        AdvanceWithOption(0);
    }
}
