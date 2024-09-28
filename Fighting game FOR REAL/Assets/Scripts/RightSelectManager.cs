using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class RightSelectManager : MonoBehaviour
{
    // Static variable to store the selected character
    public static int selectedCharacter = 0;

    // UI Buttons for character selection
    public Button character1Button;
    public Button character2Button;
    public Button character3Button;
    public Button confirmButton;

    // Private variable to store the current selection
    private int currentSelection = 0;

    void Start()
    {
        // Add listeners to the buttons
        character1Button.onClick.AddListener(() => OnCharacterSelected(1));
        character2Button.onClick.AddListener(() => OnCharacterSelected(2));
        character3Button.onClick.AddListener(() => OnCharacterSelected(3));
        confirmButton.onClick.AddListener(OnConfirmSelection);

        // Optionally, disable the confirm button until a character is selected
        confirmButton.interactable = false;
    }

    void OnCharacterSelected(int characterIndex)
    {
        currentSelection = characterIndex;
        confirmButton.interactable = true;  // Enable the confirm button
    }

    void OnConfirmSelection()
    {
        if (currentSelection != 0)
        {
            selectedCharacter = currentSelection;
            Debug.Log("Character " + selectedCharacter + " selected!");
            DisableSelectionButtons();
            StartCoroutine(LoadNextSceneAfterDelay(0.5f)); // Start the coroutine to load the scene after 0.5 seconds
        }
    }

    void DisableSelectionButtons()
    {
        character1Button.interactable = false;
        character2Button.interactable = false;
        character3Button.interactable = false;
        confirmButton.interactable = false;
    }

    // Coroutine to wait for a delay and then load the scene
    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("SampleScene");
    }
}