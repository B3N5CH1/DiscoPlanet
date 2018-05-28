﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelBonus : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;

    // Update is called once per frame
    void Update () {

        if (Input.anyKey)
        {
            PlayerPrefs.DeleteKey("level1Completed");
            PlayerPrefs.DeleteKey("level2Completed");
            PlayerPrefs.DeleteKey("progressionStarted");

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