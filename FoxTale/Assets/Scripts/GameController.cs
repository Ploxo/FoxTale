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
    public State currentState;

    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private ExerciseController exerciseController;
    [SerializeField]
    private int startIndex = 0;
 
    [SerializeField]
    State[] states;
    [SerializeField]
    List<State> endStates;

    private int currentOption;

    public delegate void StateAction(State newState);
    public event StateAction OnStateChanged;


    private void OnEnable()
    {
        exerciseController.OnExerciseEnd += OnExerciseEnd;
    }

    private void OnDisable()
    {
        exerciseController.OnExerciseEnd -= OnExerciseEnd;
    }

    private void Start()
    {
        currentState = states[startIndex];
        //Debug.Log("Current state is: " + currentState.stateId);

        StartCoroutine(WaitForOneFrame());
    }

    private IEnumerator WaitForOneFrame()
    {
        yield return new WaitForEndOfFrame();
        OnStateChanged(currentState);
    }

    public void OnExerciseEnd(ExerciseInfo exercise)
    {
        currentState = states[currentState.options[currentOption].nextState];
        OnStateChanged(currentState);
    }

    /// <summary>
    /// Progress a state depending on input (called by Unity events in the editor)
    /// </summary>
    /// <param name="option">The option chosen by the user.</param>
    public void AdvanceWithOption(int option)
    {
        currentOption = option;

        if (currentState.options[option].exercise)
        {
            //StartCoroutine(WaitForExerciseComplete(currentState.options[option].exercise, option));
            StartCoroutine(exerciseController.StartExercise(currentState.options[option].exercise));
        }
        else
        {
            //Debug.Log("Chose option " + option);
            currentState = states[currentState.options[option].nextState];
            //Debug.Log("Current state is: " + currentState.stateId);

            OnStateChanged(currentState);
        }
    }

    /// <summary>
    /// Progress to the next state with no options.
    /// </summary>
    public void Advance()
    {
        if (endStates.Contains(currentState))
            GameManager.instance.QuitGame(GameManager.SceneName.GAMEPLAY);


        AdvanceWithOption(0);
    }

    //private IEnumerator WaitForExerciseComplete(ExerciseInfo exercise, int option)
    //{
    //    //OnExerciseStart(exercise);

    //    //yield return exerciseController.StartExercise(exercise, result => success = result);
    //    yield return exerciseController.StartExercise(exercise);

    //    //currentState = states[currentState.options[option].nextState];
    //    //OnExerciseEnd(exercise);

    //}
}
