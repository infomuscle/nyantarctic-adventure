using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {
    
    // For Common
    public void touchButtonPlay() {
            Debug.Log("touchButtonPlay!");
            SceneManager.LoadScene("Main");
        }
    
    
    // For Main
    public void touchButtonStore() {
        Debug.Log("touchButtonStore!");
    }
    
    public void touchButtonSettings() {
        Debug.Log("touchButtonSettings!");
    }

    public void touchButtonNoads() {
        Debug.Log("touchButtonNoads!");
    }
    
    
    // For Gameover
    public void touchButtonHome() {
        Debug.Log("touchButtonHome!");
        SceneManager.LoadScene("Home");
    }

    public void touchButtonAds() {
        Debug.Log("touchButtonAds!");
        AdmobManager.instance.RequestReward();
    }

    public void touchButtonRank() {
        Debug.Log("touchButtonRank!");
    }
}