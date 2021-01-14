using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Text scoreText;
    public Text fishScoreText;
    public Text gameScoreText;
    public Text bestScoreText;
    public GameObject gameOverUI;
    public bool isGameOver = false;

    private int score = 0;
    private int step = 0;
    private Iceberg[] icebergs;
    private Background[] backgrounds;

    private GameObject fish;
    public GameObject fishPrefab;


    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogWarning("Multple GameMangers on Scene");
            Destroy(gameObject);
        }
    }

    void Start() {
        icebergs = new Iceberg[2];
        icebergs[0] = GameObject.FindWithTag("StandIceberg").GetComponent<Iceberg>();
        icebergs[1] = GameObject.FindWithTag("TargetIceberg").GetComponent<Iceberg>();

        backgrounds = GameObject.Find("Backgrounds").GetComponentsInChildren<Background>();
        fishScoreText.text = PlayerPrefs.GetInt("fish", 0).ToString();
    }

    public void AddScore(int newScore) {
        if (!isGameOver) {
            score += newScore;
            scoreText.text = score.ToString();
        }
    }

    public int getScore() {
        return score;
    }

    public void AddFish(int newScore) {
        if (!isGameOver) {
            score += newScore;
            PlayerPrefs.SetInt("fish", PlayerPrefs.GetInt("fish") + newScore);
            fishScoreText.text = PlayerPrefs.GetInt("fish", 0).ToString();
        }
    }

    public void NextStep() {
        if (score > step) {
            MoveBacgkrounds();
            MoveIceberg();
            MoveFish();
            step++;
        }
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

    private void MoveFish() {
        fish = GameObject.Find("Fish");
        if (fish) {
            Destroy(fish);
        }

        // Debug.Log("Fish??");
        // fish = Instantiate(fishPrefab);
    }

    public void OnPlayerDead() {
        Debug.Log("OnPlayerDead!");
        isGameOver = true;
        if (PlayerPrefs.GetInt("vibOn") == 1) {
            Handheld.Vibrate();
        }

        SaveBestScore();
        SetGameScoreText();
        gameOverUI.SetActive(true);
    }

    private void SaveBestScore() {
        if (!PlayerPrefs.HasKey("Best")) {
            PlayerPrefs.SetInt("Best", 0);
        }

        if (score > PlayerPrefs.GetInt("Best")) {
            PlayerPrefs.SetInt("Best", score);
        }
    }

    private void SetGameScoreText() {
        gameScoreText.text = "Score: " + score;
        bestScoreText.text = "Best: " + PlayerPrefs.GetInt("Best");
    }
}