using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

/*
 * This script handle the behaviour of the mainMenu.
 */

public class MainMenu : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;

    // Go to levelSelection with previous progression
    public void PlayGame()
    {

        // There is no data saved
        if (PlayerPrefs.GetInt("progressionStarted", 0) == 0)
        {
            bool destroy = EditorUtility.DisplayDialog("Info", "As there is no data saved, you will start a new game !", "Start", "Cancel");

            if (destroy)
            {
                FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        
        else
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }

    // Go to levelSelection while deleting previous progression
    public void NewGame()
    {
           
        // Check if the player already started the game, if yes display a message to informe you him that this will erase his last progression
        if (PlayerPrefs.GetInt("progressionStarted", 0) == 1)
        {
            bool destroy = EditorUtility.DisplayDialog("Info", "This will delete any previous progression.", "Continue", "Cancel");
                // If the player confirm his choice, delete the stored data
            if (destroy)
            {

                PlayerPrefs.DeleteKey("level1Completed");
                PlayerPrefs.DeleteKey("level2Completed");

                FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }

    //Quit the game
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    //Go Back one scene
    public void Back()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // Start the Fade_Out animation and sets the next level to load
    public void FadeToLevel(int index)
    {
        animator.SetTrigger("FadeOut");
        levelToLoad = index;
    }

    // Load the given level when the animation is over
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
