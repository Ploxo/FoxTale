using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ExerciseController : MonoBehaviour
{
    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private TextMeshProUGUI exerciseText;

    private ExerciseInfo currentExercise;
    private bool cancelButtonPressed = false;
    private bool nextButtonPressed = false;

    //public delegate void ExerciseAction();
    //public event ExerciseAction OnExerciseBegin;
    //public event ExerciseAction OnExerciseEnd;


    public void OnCancelButtonPressed()
    {
        cancelButtonPressed = true;
    }

    public void OnNextButtonPressed()
    {
        nextButtonPressed = true;
    }

    private void SetNextButtonActive(bool value)
    {
        nextButton.gameObject.SetActive(value);
        cancelButton.gameObject.SetActive(!value);
    }

    private void SetExerciseText()
    {
        switch (currentExercise.sensorType)
        {
            case SensorController.SensorType.STEPCOUNTER:
                exerciseText.text = $"Jump {currentExercise.repetitions} times to proceed!";
                break;
            default:
                break;
        }
    }

    public IEnumerator StartExercise(ExerciseInfo exercise, System.Action<bool> callback)
    {
        currentExercise = exercise;

        SetNextButtonActive(false);
        cancelButtonPressed = false;
        nextButtonPressed = false;

        SetExerciseText();

        yield return RunExercise(callback);

        if (cancelButtonPressed)
            callback(false);
        else if (nextButtonPressed)
            callback(true);
        else
            Debug.LogError("Unhandled case in Exercise");
    }

    public IEnumerator RunExercise(System.Action<bool> callback)
    {
        int initialCount = SensorController.Instance.CurrentStepsTaken();
        int count = 0;
        float progress = 0;

        while (true)
        {
            if (progress >= 1f)
            {
                SetNextButtonActive(true);
                exerciseText.text = "Challenge complete. \nWell done!";
            }

            if (cancelButtonPressed || nextButtonPressed)
                break;

            progress = count / (float)currentExercise.repetitions;
            progressBar.value = progress;

            switch (currentExercise.sensorType)
            {
                case SensorController.SensorType.STEPCOUNTER: 
                    count = SensorController.Instance.CurrentStepsTaken() - initialCount;
                    break;
                default:
                    break;
            }

            yield return null;
        }

        yield return null;
    }
}
