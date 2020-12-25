using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Text scoreText;
    public Text gameScoreText;
    public Text bestScoreText;
    public GameObject GameOverUI;
    public bool isGameOver = false;

    private int score = 0;
    private Iceberg[] icebergs;
    private Background[] backgrounds;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("Multple GameMangers on Scene");
            Destroy(gameObject);
        }
    }

    void Start() {
        icebergs = new Iceberg[2];
        icebergs[0] = GameObject.FindWithTag("StandIceberg").GetComponent<Iceberg>();
        icebergs[1] = GameObject.FindWithTag("TargetIceberg").GetComponent<Iceberg>();

        backgrounds = GameObject.Find("Backgrounds").GetComponentsInChildren<Background>();
    }

    public void AddScore(int newScore) {
        if (!isGameOver) {
            score += newScore;
            scoreText.text = score.ToString();
        }
    }
    
    public void NextStep() {
        // AddScore(1);
        MoveBacgkrounds();
        MoveIceberg();
    }

    public void OnPlayerDead() {
        Debug.Log("OnPlayerDead!");
        isGameOver = true;
        SaveBestScore();
        SetGameScoretext();
        GameOverUI.SetActive(true);
    }

    private void MoveIceberg() {
        for (int i = 0; i < icebergs.Length; i++) {
            icebergs[i].isMove = true;
        }
    }

    private void MoveBacgkrounds() {
        for (int i = 0; i < backgrounds.Length; i++) {
            backgrounds[i].isMove = true;
        }
    }

    private void SaveBestScore() {
        if (!PlayerPrefs.HasKey("Best")) {
            PlayerPrefs.SetInt("Best", 0);
        }

        if (score > PlayerPrefs.GetInt("Best")) {
            PlayerPrefs.SetInt("Best", score);
        }
    }

    private void SetGameScoretext() {
        gameScoreText.text = "Score: " + score;
        bestScoreText.text = "Best: " + PlayerPrefs.GetInt("Best");
    }
}