using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour, IItem {
    private const float DEFAULT_POS_Y = -350f;
    // private const float JUMP_FORCE = 50000f;

    private Rigidbody2D rigidbody;


    private int jumpForce;
    private bool appear;
    private float posX;
    private float delay;

    public void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;

        // appear = Random.Range(0, 3) == 0;
        // jumpForce = appear ? 50000 : 0;
        jumpForce = 50000;

        delay = Random.Range(2f, 5f);
        StartCoroutine(Jump(delay));
    }

    public void Update() {
        if (rigidbody.position.y < DEFAULT_POS_Y) {
            rigidbody.isKinematic = true;
            rigidbody.position = new Vector3(transform.position.x, DEFAULT_POS_Y);
            transform.position = new Vector3(transform.position.x, DEFAULT_POS_Y);
        }
    }

    public void Use() {
    }

    IEnumerator Jump(float time) {
        rigidbody.isKinematic = false;
        rigidbody.AddForce(new Vector2(0, jumpForce));
        yield return new WaitForSeconds(time);
        StartCoroutine(Jump(time));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            GameManager.instance.GetFish();
            Destroy(gameObject);
        }
    }
}