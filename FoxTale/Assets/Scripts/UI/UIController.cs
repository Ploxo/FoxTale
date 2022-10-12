using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Used for outputting typewriter text
    [SerializeField]
    private TextWriter textWriter;

    // UI elements
    [Header("UI elements")]
    [SerializeField]
    private Image uiBackground;
    [SerializeField]
    private Image sceneBackground;
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
        // Register to GameController event for state updates
        GameController.OnStateChanged += UpdateUI;
    }

    private void OnDisable()
    {
        // Deregister to avoid memory leaks
        GameController.OnStateChanged -= UpdateUI;
    }

    /// <summary>
    /// Update the UI with the new state data.
    /// </summary>
    /// <param name="currentState">The state containing the data to update with.</param>
    public void UpdateUI(State currentState)
    {
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
