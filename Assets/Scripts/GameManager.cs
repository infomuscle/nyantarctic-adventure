using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager gameManager;

    public bool isGameOver = false;
    public Text textScore;
    public GameObject uiGameOver;

    public int score = 0;

    private void Awake() {
        if (gameManager == null) {
            gameManager = this;
        } else {
            Debug.LogWarning("Multple GameMangers on Scene");
            Destroy(gameObject);
        }
    }

    void Start() { }

    void Update() { }

    public void AddScore(int newScore) {
        if (!isGameOver) {
            score += newScore;
            textScore.text = score.ToString();
        }
    }

    public void OnPlayerDead() {
        isGameOver = true;
        uiGameOver.SetActive(true);
    }
}