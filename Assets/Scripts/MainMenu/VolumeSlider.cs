using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volume", 0.5f);
        PlayerPrefs.SetFloat("volume", slider.value);

        AudioListener.volume = slider.value;
        slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }
}