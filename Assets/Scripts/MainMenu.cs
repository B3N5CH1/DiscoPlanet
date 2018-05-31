using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This script handle the behaviour of the mainMenu.
 */
[RequireComponent(typeof(DeletePlayerPrefs))]

public class MainMenu : MonoBehaviour {

    public Animator animator;
    public GameObject NewGameMenu;
    public GameObject LoadMenu;
    public GameObject MainMenuCanvas;

    private DeletePlayerPrefs del;
    private int levelToLoad;

    private void Start()
    {
        del = GetComponent<DeletePlayerPrefs>();
    }

    public void PlayGame()
    {
        if (PlayerPrefs.GetInt("progressionStarted", 0) == 1)
        {
            MainMenuCanvas.SetActive(false);
            LoadMenu.SetActive(true);
        }
        else
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Go to levelSelection with previous progression
    public void ContinueGame()
    {

        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Go to levelSelection while deleting previous progression
    public void NewGame()
    {
           
        // Check if the player already started the game, if yes display a message to informe him that this will erase his last progression
        if (PlayerPrefs.GetInt("progressionStarted", 0) == 1)
        {

            LoadMenu.SetActive(false);
            NewGameMenu.SetActive(true);
        }
        else
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }

    // Delete the data stored in PlayerPrefs
    public void DestroySaves()
    {
        del.DeleteAll();

        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
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
