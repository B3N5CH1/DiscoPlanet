using UnityEngine;
using UnityEngine.Audio;

/*
 * This script handle the behaviour of the settings menu in the maine menu scene.
 */

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    // Sets the volume of the BackGround
	public void setVolumeBG(float volume)
    {
        audioMixer.SetFloat("volumeBG", volume);
        PlayerPrefs.SetFloat("volumeBG", volume);
    }

    // Sets the volume of the SFX effects
    public void setVolumeSFX(float volume)
    {
        audioMixer.SetFloat("volumeSFX", volume);
        PlayerPrefs.SetFloat("volumeSFX", volume);
    }
}
