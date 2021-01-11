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

    public void Use() {
    }

    IEnumerator Jump(float delay) {
        Debug.Log(Time.time);
        rigidbody.isKinematic = false;
        rigidbody.AddForce(new Vector2(0, JUMP_FORCE));
        yield return new WaitForSeconds(delay);
        StartCoroutine(Jump(3));
    }
}