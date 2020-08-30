using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public bool isGameOver = false;
    public GameObject GameOverUI;

    public int score = 0;
    public Text scoreText;

    public Platform[] platforms;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("Multple GameMangers on Scene");
            Destroy(gameObject);
        }
    }

    void Start() {
        platforms = new Platform[2];
        platforms[0] = GameObject.FindWithTag("StandPlatform").GetComponent<Platform>();
        platforms[1] = GameObject.FindWithTag("TargetPlatform").GetComponent<Platform>();
    }

    public void NextStep() {
        AddScore(1);
        MovePlatform();
    }

    public void AddScore(int newScore) {
        if (!isGameOver) {
            score += newScore;
            scoreText.text = score.ToString();
        }
    }

    public void OnPlayerDead() {
        Debug.Log("OnPlayerDead!");
        isGameOver = true;

        GameOverUI.SetActive(true);
    }

    public void MovePlatform() {
        for (int i = 0; i < 2; i++) {
            platforms[i].isMove = true;
        }
    }
}