using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
    
    public void CharacterSelect(string CharacterSelectScreen)
    {
        SceneManager.LoadScene(CharacterSelectScreen);
    }

    public void QuitTheApp()
    {
        Application.Quit();
        Debug.Log("BYE");
    }
    
}
