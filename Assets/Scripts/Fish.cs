using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour, IItem {
    private const float DEFAULT_POS_Y = -350f;
    private const float JUMP_FORCE = 50000f;

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    private float posX;

    public void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();


        StartCoroutine(Jump(3));
    }

    public void Update() {
        if (rigidbody.position.y < DEFAULT_POS_Y) {
            rigidbody.isKinematic = true;
            rigidbody.position = new Vector3(-10, DEFAULT_POS_Y);
        }
    }

    public void Use() {
    }

    IEnumerator Jump(float delay) {
        Debug.Log(Time.time);
        rigidbody.isKinematic = false;
        rigidbody.AddForce(new Vector2(0, JUMP_FORCE));
        yield return new WaitForSeconds(delay);
        StartCoroutine(Jump(3));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("FISH!");
            GameManager.instance.AddFish(1);
        }
    }
}