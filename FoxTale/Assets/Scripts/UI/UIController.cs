using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    // Used for outputting typewriter text
    [SerializeField]
    private TextWriter textWriter;

    // UI elements
    [Header("UI elements")]
    [SerializeField]
    private Image uiBackground;
    [SerializeField]
    private Image sceneBackground;

    [Header("Game UI Elements")]
    [SerializeField]
    private GameObject gamePanel;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private Button advanceButton;
    [SerializeField]
    private Button optionAButton;
    [SerializeField]
    private Button optionBButton;

    [Header("Exercise UI Elements")]
    [SerializeField]
    private GameObject exercisePanel;
    [SerializeField]
    private Slider progress;
    [SerializeField]
    private GameObject nextButton;
    [SerializeField]
    private GameObject cancelButton;


    private void OnEnable()
    {
        // Register to GameController event for state updates
        gameController.OnStateChanged += UpdateUI;
        gameController.OnExerciseStart += OnExerciseStart;
        gameController.OnExerciseEnd += OnExerciseEnd;
    }

    private void OnDisable()
    {
        // Deregister to avoid memory leaks
        gameController.OnStateChanged -= UpdateUI;
        gameController.OnExerciseStart -= OnExerciseStart;
        gameController.OnExerciseEnd -= OnExerciseEnd;
    }

    public void OnExerciseStart(ExerciseInfo exercise)
    {
        ActivateExerciseUI(true);
    }

    public void OnExerciseEnd(ExerciseInfo exercise)
    {
        ActivateExerciseUI(false);
    }

    private void ActivateExerciseUI(bool value)
    {
        exercisePanel.SetActive(value);
        gamePanel.SetActive(!value);
    }

    /// <summary>
    /// Update the UI with the new state data.
    /// </summary>
    /// <param name="currentState">The state containing the data to update with.</param>
    public void UpdateUI(State currentState)
    {
        gamePanel.SetActive(true);
        exercisePanel.SetActive(false);

        uiBackground.color = currentState.backgroundColor;
        sceneBackground.sprite = currentState.graphics;
        textWriter.textArrays = currentState.sentences;
        textWriter.StartWriter();

        // Single option enables forward button and disables option buttons and vice versa.
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
