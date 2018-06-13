/**
*   Filename: EndScene.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script handles the behaviour of the end scene.
*   
**/
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DeletePlayerPrefs))]

public class EndScene : MonoBehaviour {

    public Animator animator;

    private DeletePlayerPrefs del;
    private int levelToLoad;

    private void Start()
    {
        del = GetComponent<DeletePlayerPrefs>();
    }
    // Update is called once per frame
    void Update()
    {

        // Listen to any input pressed, if yes calls FadeToLevel() and deletes player preferences
        if (Input.anyKey)
        {
            del.DeleteAll();
            FadeToLevel(0);
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
