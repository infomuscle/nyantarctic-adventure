using UnityEngine;

public class BackgroundLoop : MonoBehaviour {
    private float width;
    public float speed;

    private void Awake() {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x;
    }

    void Update() {
        if (!GameManager.instance.isGameOver) {
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
        }

        if (transform.position.x <= -width) {
            Reposition();
        }
    }

    private void Reposition() {
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2) transform.position + offset;
    }
}