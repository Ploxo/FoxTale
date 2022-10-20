using System.Collections;
using TMPro;
using UnityEngine;
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

    [SerializeField]
    JumpTracker jumpTracker;
    [SerializeField]
    StepTracker stepTracker;

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

    //int stepCount = 0;
    //int jumps = 0;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        stepCount = 0;
    //        jumps = 0;
    //    }

    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        stepCount++;
    //    }
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        jumps++;
    //    }
    //}

    private void SetExerciseText()
    {
        switch (currentExercise.sensorType)
        {
            case SensorController.SensorType.STEPCOUNTER:
                exerciseText.text = $"Walk {currentExercise.repetitions} more steps to proceed!";
                break;
            case SensorController.SensorType.ACCELEROMETER:
                exerciseText.text = $"Jump {currentExercise.repetitions} more times to proceed!";
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

        switch (currentExercise.sensorType)
        {
            case SensorController.SensorType.STEPCOUNTER:
                yield return RunWalkExercise();
                break;
            case SensorController.SensorType.ACCELEROMETER:
                yield return RunJumpExercise();
                break;
            default:
                break;
        }

        if (cancelButtonPressed)
            callback(false);
        else if (nextButtonPressed)
            callback(true);
        else
            Debug.LogError("Unhandled case in Exercise");
    }

    private void SetExerciseComplete()
    {
        SetNextButtonActive(true);
        exerciseText.text = "Challenge complete. \nWell done!";
    }

    public IEnumerator RunJumpExercise()
    {
        int initialCount = jumpTracker.JumpsPerformed;
        int count = 0;
        float progress = 0;

        while (true)
        {
            if (progress >= 1f)
                SetExerciseComplete();

            if (cancelButtonPressed || nextButtonPressed)
                break;

            progress = count / (float)currentExercise.repetitions;
            progressBar.value = progress;

            count = jumpTracker.JumpsPerformed - initialCount;
            //count = jumps;

            yield return null;
        }

        yield return null;
    }

    public IEnumerator RunWalkExercise()
    {
        int initialCount = stepTracker.StepsTaken;
        int count = 0;
        float progress = 0;

        while (true)
        {
            if (progress >= 1f)
                SetExerciseComplete();

            if (cancelButtonPressed || nextButtonPressed)
                break;

            progress = count / (float)currentExercise.repetitions;
            progressBar.value = progress;

            count = stepTracker.StepsTaken - initialCount;
            //count = stepCount;

            yield return null;
        }

        yield return null;
    }
}
