using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {
    public GameObject settingsUI;
    public GameObject storeUI;

    private AudioSource btnAudio;
    private AudioClip btnClip;

    public void Start() {
        btnAudio = GetComponent<AudioSource>();
        btnAudio.clip = (AudioClip) Resources.Load("Click_Electronic_01_btn", typeof(AudioClip));
        // btnClip = (AudioClip) Resources.Load("Click_Electronic_01_btn", typeof(AudioClip));
    }

    // For Common
    public void TouchButtonPlay() {
        Debug.Log("TouchButtonPlay!");
        SceneManager.LoadScene("Main");
    }


    // For Main
    public void TouchButtonStore() {
        Debug.Log("TouchButtonStore!");
        storeUI.SetActive(true);
    }

    public void TouchButtonSettings() {
        Debug.Log("TouchButtonSettings!");
        settingsUI.SetActive(true);
        // SceneManager.LoadScene("Settings");
    }

    public void TouchButtonNoads() {
        Debug.Log("TouchButtonNoads!");
    }


    // For Gameover
    public void TouchButtonHome() {
        Debug.Log("TouchButtonHome!");
        SceneManager.LoadScene("Home");
    }

    public void TouchButtonAds() {
        Debug.Log("TouchButtonAds!");
        AdmobManager.instance.RequestReward();
    }

    public void TouchButtonRank() {
        Debug.Log("TouchButtonRank!");
        btnAudio.Play();
    }


    // For Settings
    public void TouchButtonLanguage() {
        Debug.Log("TouchButtonLanguage!");
    }

    public void TouchButtonAsk() {
        Debug.Log("TouchButtonAsk!");
    }

    public void TouchButtonRestore() {
        Debug.Log("TouchButtonRestore!");
    }
}