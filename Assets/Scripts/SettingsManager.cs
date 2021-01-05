using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start() {
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVol", 0.7f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.7f);
    }

    public void SaveChanges() {
        Debug.Log("SaveChanges");
        PlayerPrefs.SetFloat("bgmVol", bgmSlider.value);
        PlayerPrefs.SetFloat("sfxVol", sfxSlider.value);
    }

    public void SetDefault() {
        PlayerPrefs.SetFloat("bgmVol", 0.7f);
        PlayerPrefs.SetFloat("sfxVol", 0.7f);
    }
}