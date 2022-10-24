using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private ExerciseController exerciseController;
    // Used for outputting typewriter text
    [SerializeField]
    private TextWriter textWriter;
    [SerializeField]
    private DialogueReader dialogueReader;

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
    //[SerializeField]
    //private Slider progress;
    //[SerializeField]
    //private GameObject nextButton;
    //[SerializeField]
    //private GameObject cancelButton;


    private void OnEnable()
    {
        // Register to GameController event for state updates
        gameController.OnStateChanged += UpdateUIState;

        exerciseController.OnExerciseStart += OnExerciseStart;
        exerciseController.OnExerciseEnd += OnExerciseEnd;

        // Register to TextWriter events to hide/unhide buttons 
        textWriter.OnWriterComplete += OnWriterComplete;
    }

    private void OnDisable()
    {
        // Deregister to avoid memory leaks
        gameController.OnStateChanged -= UpdateUIState;

        exerciseController.OnExerciseStart -= OnExerciseStart;
        exerciseController.OnExerciseEnd -= OnExerciseEnd;

        textWriter.OnWriterComplete -= OnWriterComplete;
    }

    public void OnExerciseStart(ExerciseInfo exercise)
    {
        SetGameOrExercisePanel(false);
        dialogueReader.StopReader();
    }

    public void OnExerciseEnd(ExerciseInfo exercise)
    {
        SetGameOrExercisePanel(true);
    }

    public void OnWriterComplete()
    {
        EnableOptionButtons(true);
    }

    /// <summary>
    /// Set the options for the player as active or not, using Button.interactable.
    /// </summary>
    /// <param name="value">The value to set for the buttons.</param>
    private void EnableOptionButtons(bool value)
    {
        EnableButton(advanceButton, value);
        EnableButton(optionAButton, value);
        EnableButton(optionBButton, value);
    }

    private void EnableButton(Button button, bool value)
    {
        //if (button.gameObject.activeInHierarchy)
        //{
            button.interactable = value;
        //}
    }

    /// <summary>
    /// Toggle between visible game panel or exercise panel.
    /// </summary>
    /// <param name="value">True for game panel and no exercise panel.</param>
    private void SetGameOrExercisePanel(bool value)
    {
        gamePanel.SetActive(value);
        exercisePanel.SetActive(!value);
    }

    /// <summary>
    /// Update the UI with the new state data.
    /// </summary>
    /// <param name="currentState">The state containing the data to update with.</param>
    public void UpdateUIState(State currentState)
    {
        SetGameOrExercisePanel(true);
        EnableOptionButtons(false);
        
        uiBackground.color = currentState.backgroundColor;
        sceneBackground.sprite = currentState.graphics;

        textWriter.textArrays = currentState.sentences;
        textWriter.StartWriter();

        dialogueReader.textArrays = currentState.sentences;
        dialogueReader.StartReader();

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
