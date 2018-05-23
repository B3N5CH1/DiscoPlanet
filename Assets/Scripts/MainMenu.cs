using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This script handle the behaviour of the mainMenu.
 */

public class MainMenu : MonoBehaviour {

    //Start the game with the next scene
	public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Quit the game
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    //Quit the game
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
