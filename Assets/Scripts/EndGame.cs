/**
*   Filename: EndGame.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script handles what happens after the spaceship animation ends.
*   
**/
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

    public  AudioSource _audioR;
    public AudioSource _audioE;

  
    // Play the audio attached to this gameobject
    void PlaySoundRocket()
    {
        _audioR.Play();
    }

    void PlaySoundExplosion()
    {
        _audioE.Play();
    }

    // Go to the end scene
    public void GoToEndScene()
    {
        SceneManager.LoadScene(6);
    }
}
