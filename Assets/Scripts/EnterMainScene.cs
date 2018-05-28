using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class EnterMainScene : MonoBehaviour {

    public Animator animator;
    public AudioMixer audioMixer;

    private int levelToLoad;

    void Start()
    {

        // Sets the volume with the stored settings data
        audioMixer.SetFloat("volumeBG", PlayerPrefs.GetFloat("volumeBG", 0));
        audioMixer.SetFloat("volumeSFX", PlayerPrefs.GetFloat("volumeSFX", 0));
    }
    // Update is called once per frame
    void Update () {
        
        if (Input.anyKey)
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
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
