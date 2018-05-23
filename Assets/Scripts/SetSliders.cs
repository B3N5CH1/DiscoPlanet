using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetSliders : MonoBehaviour {

    public Slider sliderBG;
    public Slider sliderSFX;
    public AudioMixer audioMixer;

    // When the Gameobject is activated calls this function,
    // which sets the value of the sliders in accordance to the current volumes values.
    public void OnEnable()
    {
        float valBG;
        float valSFX;

        audioMixer.GetFloat("volumeBG", out valBG);
        audioMixer.GetFloat("volumeSFX", out valSFX);

        sliderBG.value = valBG;
        sliderSFX.value = valSFX;
    }
}
