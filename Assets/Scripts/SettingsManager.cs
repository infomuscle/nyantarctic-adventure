using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
    // public Slider bgmSlider;
    // public Slider sfxSlider;
    public Toggle bgmToggle;
    public Toggle sfxToggle;
    public Toggle vibToggle;
    public Toggle pushToggle;

    void Start() {
        // bgmSlider.value = PlayerPrefs.GetFloat("bgmVol", 0.7f);
        // sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", 0.7f);
        bgmToggle.isOn = IntToBool(PlayerPrefs.GetInt("bgmOn", 1));
        sfxToggle.isOn = IntToBool(PlayerPrefs.GetInt("sfxOn", 1));
        vibToggle.isOn = IntToBool(PlayerPrefs.GetInt("vibOn", 1));
        pushToggle.isOn = IntToBool(PlayerPrefs.GetInt("pushOn", 1));
    }

    public void SaveChanges() {
        Debug.Log("SaveChanges");
        // PlayerPrefs.SetFloat("bgmVol", bgmSlider.value);
        // PlayerPrefs.SetFloat("sfxVol", sfxSlider.value);
        PlayerPrefs.SetInt("bgmOn", BoolToInt(bgmToggle.isOn));
        PlayerPrefs.SetInt("sfxOn", BoolToInt(sfxToggle.isOn));
        PlayerPrefs.SetInt("vibOn", BoolToInt(vibToggle.isOn));
        PlayerPrefs.SetInt("pushOn", BoolToInt(pushToggle.isOn));
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