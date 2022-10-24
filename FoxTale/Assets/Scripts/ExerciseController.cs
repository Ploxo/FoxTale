using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseController : MonoBehaviour
{
    // See Start and Update below
    private bool manualCounters = false;

    [SerializeField]
    private GameProgressBar progressBar;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private TextMeshProUGUI exerciseText;
    [SerializeField]
    private TextMeshProUGUI completionText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Image foxImage;

    [SerializeField]
    private JumpTracker jumpTracker;
    [SerializeField]
    private StepTracker stepTracker;

    [SerializeField]
    private Sprite[] foxSprites;

    private ExerciseInfo currentExercise;
    private bool cancelButtonPressed = false;

    public delegate void ExerciseAction(ExerciseInfo exercise);
    public event ExerciseAction OnExerciseStart;
    public event ExerciseAction OnExerciseEnd;


    private void Start()
    {
        // For testing without phone sensors
        #if UNITY_EDITOR
                manualCounters = true;
        #endif
    }

    int stepCount = 0;
    int jumps = 0;
    // We only use Update for manual updating of values when in editor, with the keys below.
    private void Update()
    {
        if (manualCounters)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                stepCount = 0;
                jumps = 0;
            }

            if (Input.GetKeyDown(KeyCode.S))
                stepCount++;
            if (Input.GetKeyDown(KeyCode.J))
                jumps++;
        }
    }

    public void OnCancelButtonPressed()
    {
        cancelButtonPressed = true;
    }

    public void OnRetryButtonPressed()
    {
        foxImage.sprite = foxSprites[0];
        StopAllCoroutines();
        StartCoroutine(StartExercise(currentExercise));
    }

    public void OnNextButtonPressed()
    {
        foxImage.sprite = foxSprites[0];
        SetCompletionText(false);
        OnExerciseEnd(currentExercise);
    }

    private void SetCompletionText(bool value)
    {
        completionText.gameObject.SetActive(value);
        progressBar.gameObject.SetActive(!value);
    }

    #region BUTTON_DISPLAY
    // Display only one button
    private void SetNextButtonActive()
    {
        nextButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
    }

    private void SetCancelButtonActive()
    {
        nextButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(false);
    }

    private void SetRetryButtonActive()
    {
        nextButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(true);
    }
    #endregion

    // Set the text field's text to the respective exercise.
    private void SetExerciseText()
    {
        switch (currentExercise.sensorType)
        {
            case SensorController.SensorType.STEPCOUNTER:
                exerciseText.text = $"Walk {currentExercise.repetitions} steps!";
                break;
            case SensorController.SensorType.ACCELEROMETER:
                exerciseText.text = $"Jump {currentExercise.repetitions} times!";
                break;
            default:
                break;
        }
    }

    private void SetExerciseComplete()
    {
        timerText.gameObject.SetActive(false);
        StartCoroutine(ShowWinScreenForSeconds(2f));
    }

    private void SetExerciseFailed()
    {
        timerText.gameObject.SetActive(false);
        StartCoroutine(ShowFailScreenForSeconds(2f));
    }

    private IEnumerator ShowWinScreenForSeconds(float time)
    {
        foxImage.sprite = foxSprites[1];
        exerciseText.text = "You made it!";
        SetCompletionText(true);
        SetNextButtonActive();
        yield return new WaitForSeconds(time);

        foxImage.sprite = foxSprites[0];
    }

    private IEnumerator ShowFailScreenForSeconds(float time)
    {
        foxImage.sprite = foxSprites[2];
        exerciseText.text = "Oh no! You didn't make it!";
        SetRetryButtonActive();
        yield return new WaitForSeconds(time);

        foxImage.sprite = foxSprites[0];
        exerciseText.text = "So close!\n<size=80%>Try again, you can do it!";
    }

    #region EXERCISE COROUTINES
    // Check which exercise we've been handed and check results
    public IEnumerator StartExercise(ExerciseInfo exercise)
    {
        // Notify that an exercise has started and set it as current
        OnExerciseStart(exercise);
        currentExercise = exercise;

        SetCancelButtonActive();
        SetCompletionText(false);
        timerText.gameObject.SetActive(true);
        cancelButtonPressed = false;

        SetExerciseText();
        progressBar.ResetValue();

        // Reset manual values if we're in editor
        if (manualCounters)
        {
            stepCount = 0;
            jumps = 0;
        }

        // Start and wait for Exercise coroutine completion
        if (currentExercise.sensorType == SensorController.SensorType.STEPCOUNTER)
        {
            yield return RunWalkExercise();
        }
        else if (currentExercise.sensorType == SensorController.SensorType.ACCELEROMETER)
        {
            yield return RunJumpExercise();
        }

        // Check button states after Exercise coroutine has completed
        if (cancelButtonPressed)
        {
            //Debug.Log("cancel pressed");
            SetExerciseFailed();
        }
        else
        {
            //Debug.Log("Unhandled case");
        }
    }

    public IEnumerator RunJumpExercise()
    {
        int initialCount = jumpTracker.JumpsPerformed;
        int count = 0;
        float progress = 0;
        float timeLimit = Time.time + currentExercise.time;

        while (true)
        {
            if (progress >= 1f)
            {
                SetExerciseComplete();
                break;
            }

            if (Time.time > timeLimit)
                cancelButtonPressed = true;

            if (cancelButtonPressed)
                break;

            progress = count / (float)currentExercise.repetitions;
            progressBar.SetValue(progress);
            timerText.text = "" + Mathf.Round(timeLimit - Time.time);

            if (manualCounters)
                count = jumps;
            else
                count = jumpTracker.JumpsPerformed - initialCount;

            yield return null;
        }

        yield return null;
    }

    public IEnumerator RunWalkExercise()
    {
        int initialCount = stepTracker.StepsTaken;
        int count = 0;
        float progress = 0;
        float timeLimit = Time.time + currentExercise.time;

        while (true)
        {
            if (progress >= 1f)
            {
                SetExerciseComplete();
                break;
            }

            if (Time.time > timeLimit)
                cancelButtonPressed = true;

            if (cancelButtonPressed)
                break;

            progress = count / (float)currentExercise.repetitions;
            progressBar.SetValue(progress);
            timerText.text = "" + Mathf.Round(timeLimit - Time.time);

            if (manualCounters)
                count = stepCount;
            else
                count = stepTracker.StepsTaken - initialCount;

            yield return null;
        }

        yield return null;
    }
    #endregion
}
