using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {
    public void touchButtonPlay() {
        Debug.Log("touchButtonPlay!");
        SceneManager.LoadScene("Main");
    }

    public void touchButtonOptions() {
        Debug.Log("touchButtonOptions!");
    }

    public void touchButtonHome() {
        Debug.Log("touchButtonHome!");
        SceneManager.LoadScene("Home");
    }

    public void touchButtonAds() {
        Debug.Log("touchbuttonAds!");
    }
}