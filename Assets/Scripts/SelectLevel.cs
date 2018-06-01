using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * This script handle the behaviour of the level selection scene.
 */

public class SelectLevel : MonoBehaviour {


    public Animator animator;

    public Button[] levelButtons;

    private int levelToLoad;

    // Called when the scene is loaded
    private void Start()
    {
        // Saves that the progression is not 0 anymore
        PlayerPrefs.SetInt("progressionStarted", 1);

        // Get the state of the player's progression
        int level1Completed = PlayerPrefs.GetInt("level1CheckPoint", 0);
        int level2Completed = PlayerPrefs.GetInt("level2Completed", 0);

        // Check if level 1 has been completed, thus unlocking level 2
        if (level1Completed == 1)
        {
            levelButtons[1].interactable = true;
        }
        else
        {
            levelButtons[1].interactable = false;
        }
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
