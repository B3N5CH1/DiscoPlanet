using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour {


    public Animator animator;

    public Button[] levelButtons;

    private int levelToLoad;

    private void Start()
    {
        PlayerPrefs.SetInt("progressionStarted", 1);

        int level1Completed = PlayerPrefs.GetInt("level1Completed", 0);
        int level2Completed = PlayerPrefs.GetInt("level2Completed", 0);

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
