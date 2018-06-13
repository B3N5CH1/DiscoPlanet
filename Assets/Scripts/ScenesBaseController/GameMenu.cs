/**
*   Filename: GameMenu.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script handles the behaviour of the game menu scene.
*   
**/
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    public Player player;

    // Continue de game
    public void Continue()
    {
        this.gameObject.SetActive(false);
        player.setInMenu(false);
    }

    // Go to Main menu scene
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Continue();
        }
    }
}
