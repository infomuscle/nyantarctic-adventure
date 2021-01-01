using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {
    public GameObject settingsUI;


    // For Common
    public void TouchButtonPlay() {
        Debug.Log("TouchButtonPlay!");
        SceneManager.LoadScene("Main");
    }


    // For Main
    public void TouchButtonStore() {
        Debug.Log("TouchButtonStore!");
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
    }
}