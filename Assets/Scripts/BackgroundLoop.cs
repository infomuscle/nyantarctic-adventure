using System;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour {
    private float SPEED = 60f;

    public bool isMove;
    private Vector2 originalPos;
    
    private float width;

    private void Awake() {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }

    private void Start() {
        isMove = false;
        originalPos = transform.position;
    }

    private void Update() {
        if (!GameManager.instance.isGameOver) {
            // transform.Translate(Vector2.left * (SPEED * Time.deltaTime));
        }

        if (isMove) {
            Move();
            if (CheckPositionOfScreen(transform.position.x)) {
                Stop(transform.position.x);
            }
        }

        if (transform.position.x <= -width) {
            Reposition();
        }
    }

    public void Move() {
        transform.Translate(Vector2.left * (SPEED * Time.deltaTime));
    }

    private void Stop(float posX) {
        isMove = false;
        transform.position = new Vector2(posX, 0);
        originalPos = transform.position;
    }
    
    private bool CheckPositionOfScreen(float posX) {
        if (originalPos.x - posX >= 30) {
            return true;
        }

        return false;
    }

    private void Reposition() {
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2) transform.position + offset;
    }
}