using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Toggle vibToggle;

    void Start() {
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVol", 0.7f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.7f);
        vibToggle.isOn = IntToBool(PlayerPrefs.GetInt("vibOn", 1));
    }

    public void SaveChanges() {
        Debug.Log("SaveChanges");
        PlayerPrefs.SetFloat("bgmVol", bgmSlider.value);
        PlayerPrefs.SetFloat("sfxVol", sfxSlider.value);
        PlayerPrefs.SetInt("vibOn", BoolToInt(vibToggle.isOn));
    }

    public void SetDefault() {
        PlayerPrefs.SetFloat("bgmVol", 0.7f);
        PlayerPrefs.SetFloat("sfxVol", 0.7f);
    }


    private int BoolToInt(bool value) {
        return value ? 1 : 0;
    }

    private bool IntToBool(int value) {
        return value > 0;
    }
}