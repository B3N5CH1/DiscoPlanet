using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/*
 * This script handle the behaviour of sliders in the main menu scene.
 */

public class SetSliders : MonoBehaviour {

    public Slider sliderBG;
    public Slider sliderSFX;
    public AudioMixer audioMixer;

    // When the Gameobject is activated calls this function,
    // which sets the value of the sliders in accordance to the value of the player preferences or the default one.
    public void OnEnable()
    {
        float valBG;
        float valSFX;

        valBG = PlayerPrefs.GetFloat("volumeBG", 0);
        valSFX = PlayerPrefs.GetFloat("volumeSFX", 0);

        sliderBG.value = valBG;
        sliderSFX.value = valSFX;
    }
}
