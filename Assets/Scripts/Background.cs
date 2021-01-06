using UnityEngine;

public class Background : MonoBehaviour {
    private float SPEED = 90f;
    // private float MOVE_DISTANCE = 30f;
    private float MOVE_DISTANCE = 300f;

    public bool isMove;

    private float orgnPosX;
    private float width;

    private void Awake() {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }

    private void Start() {
        isMove = false;
        orgnPosX = transform.position.x;
    }

    private void Update() {
        if (!GameManager.instance.isGameOver) {
            if (isMove) {
                Move();
                if (CheckDistanceMoved()) {
                    Stop();
                }
            }

            if (transform.position.x <= -width + 245) {
                Reposition();
            }
        }
    }

    private void Move() {
        transform.Translate(Vector2.left * (SPEED * Time.deltaTime));
    }

    private void Stop() {
        isMove = false;
        orgnPosX = transform.position.x;
    }

    private bool CheckDistanceMoved() {
        if (orgnPosX - transform.position.x >= MOVE_DISTANCE) {
            return true;
        }

        return false;
    }

    private void Reposition() {
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2) transform.position + offset;
        
        // Should Calculate Move Distance When Repositioned
        orgnPosX = transform.position.x + (orgnPosX - (-width + 245));
    }
}