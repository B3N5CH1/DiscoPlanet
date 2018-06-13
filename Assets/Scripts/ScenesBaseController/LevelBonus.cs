/**
*   Filename: LevelBonus.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script handles the behaviour of the bonus level.
*   
**/
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DeletePlayerPrefs))]

public class LevelBonus : MonoBehaviour {

    public Animator animator;

    private DeletePlayerPrefs del;
    private int levelToLoad;

    // Called when the scene is loaded, instanciate a few gameojbect
    private void Start()
    {
        del = GetComponent<DeletePlayerPrefs>();
    }

    // Update is called once per frame
    void Update () {

        // Check for any input key pressed
        if (Input.anyKey)
        {
            // Delete the current progression and fade to MainMenu
            del.DeleteAll();

            FadeToLevel(1);
        }

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
