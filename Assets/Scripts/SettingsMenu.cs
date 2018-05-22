using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    // Sets the volume of an AudioMixer
	public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
