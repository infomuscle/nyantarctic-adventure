using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Text scoreText;
    public Text gameScoreText;
    public Text bestScoreText;
    public GameObject GameOverUI;
    public bool isGameOver = false;

    private int score = 0;
    private Platform[] platforms;

    BackgroundController[] backgrounds;
    
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
        
        backgrounds = GameObject.Find("Backgrounds").GetComponentsInChildren<BackgroundController>();
    }

    public void NextStep() {
        AddScore(1);
        MovePlatform();
        MoveBacgkrounds();
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
        saveBestScore();
        setGameScoretext();
        GameOverUI.SetActive(true);
    }

    public void MovePlatform() {
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i].isMove = true;
        }
    }

    public void MoveBacgkrounds() {
        for (int i = 0; i < backgrounds.Length; i++) {
            backgrounds[i].isMove = true;
        }
    }

    private void saveBestScore() {
        if (!PlayerPrefs.HasKey("Best")) {
            PlayerPrefs.SetInt("Best", 0);
        }

        if (score > PlayerPrefs.GetInt("Best")) {
            PlayerPrefs.SetInt("Best", score);
        }
    }

    private void setGameScoretext() {
        gameScoreText.text = "Score: " + score;
        bestScoreText.text = "Best: " + PlayerPrefs.GetInt("Best");
    }
}