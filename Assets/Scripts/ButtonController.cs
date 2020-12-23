using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {
    public void touchButtonPlay() {
        Debug.Log("touchButtonPlay!");
        SceneManager.LoadScene("Main");
    }

    public void touchButtonStore() {
        Debug.Log("touchButtonStore!");
    }
    
    public void touchButtonSettings() {
        Debug.Log("touchButtonSettings!");
    }

    public void touchButtonNoads() {
        Debug.Log("touchButtonNoads!");
    }
    
    public void touchButtonHome() {
        Debug.Log("touchButtonHome!");
        SceneManager.LoadScene("Home");
    }

    public void touchButtonAds() {
        Debug.Log("touchButtonAds!");
    }


    public void touchButtonRanking() {
        Debug.Log("touchButtonRanking!");
    }
}