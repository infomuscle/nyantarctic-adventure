using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour, IItem {
    private const float DEFAULT_POS_Y = -350f;
    private const int JUMP_FORCE = 50000;

    public static Fish instance;
    public bool isMove = false;

    private Rigidbody2D rigidbody;


    private int jumpForce;
    private bool appear;
    private float posX;
    private float delay;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogWarning("Multple Fish on Scene");
            Destroy(gameObject);
        }
    }

    public void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;

        // jumpForce = Random.Range(0, 3) == 0 ? JUMP_FORCE : 0;
        jumpForce = 50000;

        // delay = Random.Range(2f, 5f);
        delay = 2f;
        StartCoroutine(Jump(delay));
    }

    public void Update() {
        if (rigidbody.position.y < DEFAULT_POS_Y) {
            rigidbody.isKinematic = true;
            rigidbody.position = new Vector3(transform.position.x, DEFAULT_POS_Y);
            transform.position = new Vector3(transform.position.x, DEFAULT_POS_Y);
        }

        // if (isMove) {
            // transform.Translate(Vector2.left * (1000f * Time.deltaTime));
            // transform.Translate(Vector2.left * (SPEED * Time.deltaTime));
            // if (CheckOutOfScreen()) {
                // GameManager.instance.ResetFish();
                // Destroy(this);
                // isMove = false;
            // }
        // }
    }

    public void Use() {
    }

    IEnumerator Jump(float time) {
        yield return new WaitForSeconds(time);
        rigidbody.isKinematic = false;
        rigidbody.AddForce(new Vector2(0, jumpForce));
        StartCoroutine(Jump(time));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            GameManager.instance.GetFish();
            Destroy(gameObject);
        }
    }

    private bool CheckOutOfScreen() {
        if (transform.position.x <= -188f) {
            return true;
        }

        return false;
    }
}