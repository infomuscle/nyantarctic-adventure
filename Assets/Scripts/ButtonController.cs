using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {
    public GameObject settingsUI;
    public GameObject storeUI;

    private AudioClip btnClip;

    // For Common
    public void TouchButtonPlay() {
        Debug.Log("TouchButtonPlay!");
        SoundManager.instance.PlayButtonSound();
        SceneManager.LoadScene("Main");
    }


    // For Main
    public void TouchButtonStore() {
        Debug.Log("TouchButtonStore!");
        SoundManager.instance.PlayButtonSound();
        storeUI.SetActive(true);
    }

    public void TouchButtonSettings() {
        Debug.Log("TouchButtonSettings!");
        SoundManager.instance.PlayButtonSound();
        settingsUI.SetActive(true);
    }

    public void TouchButtonNoads() {
        Debug.Log("TouchButtonNoads!");
        SoundManager.instance.PlayButtonSound();
    }


    // For Gameover
    public void TouchButtonHome() {
        Debug.Log("TouchButtonHome!");
        SoundManager.instance.PlayButtonSound();
        SceneManager.LoadScene("Home");
    }

    public void TouchButtonAds() {
        Debug.Log("TouchButtonAds!");
        SoundManager.instance.PlayButtonSound();
        AdmobManager.instance.RequestReward();
    }

    public void TouchButtonRank() {
        Debug.Log("TouchButtonRank!");
        SoundManager.instance.PlayButtonSound();
    }


    // For Settings
    public void TouchButtonLanguage() {
        Debug.Log("TouchButtonLanguage!");
        SoundManager.instance.PlayButtonSound();
    }

    public void TouchButtonAsk() {
        Debug.Log("TouchButtonAsk!");
        SoundManager.instance.PlayButtonSound();
    }

    public void TouchButtonRestore() {
        Debug.Log("TouchButtonRestore!");
        SoundManager.instance.PlayButtonSound();
    }
}