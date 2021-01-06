using UnityEngine;

public class Background : MonoBehaviour {
    private float SPEED = 90f;
    private float MOVE_DISTANCE = 30f;
    // private float SPEED = 500f;
    // private float MOVE_DISTANCE = 500f;

    private float offsetX = 245;

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

            if (transform.position.x <= -width + offsetX) {
                Reposition();
            }
        }
    }

    private void Move() {
        transform.Translate(Vector2.left * (SPEED * Time.deltaTime));
    }

    private void Stop() {
        isMove = false;
        transform.position = new Vector2(orgnPosX - MOVE_DISTANCE, transform.position.y);
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
        // transform.position = (Vector2) transform.position + offset;
        transform.position = new Vector2(-width + offsetX, transform.position.y) + offset;

        // Should Calculate Move Distance When Repositioned
        orgnPosX = transform.position.x + (orgnPosX - (-width + offsetX));
        // orgnPosX = transform.position.x + MOVE_DISTANCE - (orgnPosX - (-width + offsetX));
    }
}