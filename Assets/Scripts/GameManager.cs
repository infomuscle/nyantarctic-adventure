using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public bool isGameOver = false;
    public Text scoreText;
    public GameObject GameOverUI;

    public GameObject cat;
    

    public int score = 0;

    public PlatformMover[] platformMovers;
    public bool[] isMoving;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("Multple GameMangers on Scene");
            Destroy(gameObject);
        }
    }

    void Start() {
        platformMovers = new PlatformMover[2];
        platformMovers[0] = GameObject.FindWithTag("StandPlatform").GetComponent<PlatformMover>();
        platformMovers[1] = GameObject.FindWithTag("TargetPlatform").GetComponent<PlatformMover>();
        isMoving = new bool[] {false, false};
        
        cat = GameObject.Find("Cat");
        
        cat.transform.parent = GameObject.FindWithTag("TargetPlatform").transform;
    }

    void Update() {
        // if (Input.GetMouseButtonDown(0) && CheckMovePossible()) {
            // MovePlatform();
        // }
    }

    public void JumpSuccess() {
        AddScore(1);
        MovePlatform();
        MoveCat();
        cat.transform.parent = GameObject.FindWithTag("TargetPlatform").transform;
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
        // GameOverUI.SetActive(true);
    }

    public void MoveCat() {
        
    }

    public void MovePlatform() {
        for (int i = 0; i < 2; i++) {
            platformMovers[i].isMove = true;
        }
    }

    public bool CheckMovePossible() {
        if (!isMoving[0] && !isMoving[1]) {
            return true;
        }

        return false;
    }
}