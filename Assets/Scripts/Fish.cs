using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour, IItem {
    private const float DEFAULT_POS_Y = -350f;
    private const float JUMP_FORCE = 50000f;

    private Rigidbody2D rigidbody;

    private float posX;

    private IEnumerator jumpEnumerator;

    public void Awake() {
        jumpEnumerator = Jump(3);
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        StopCoroutine(jumpEnumerator);
        StartCoroutine(jumpEnumerator);
    }

    public void Start() {
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

    IEnumerator Jump(float delay) {
        rigidbody.isKinematic = false;
        rigidbody.AddForce(new Vector2(0, JUMP_FORCE));
        yield return new WaitForSeconds(delay);
        StartCoroutine(Jump(3));
    }

    public void Reposition(float posXleft, float posXRight) {
        gameObject.SetActive(true);
        float newPosX = Random.Range(posXleft, posXRight);
        transform.position = new Vector2(newPosX, DEFAULT_POS_Y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("FISH!");
            GameManager.instance.AddFish(1);
            gameObject.SetActive(false);
        }
    }
}