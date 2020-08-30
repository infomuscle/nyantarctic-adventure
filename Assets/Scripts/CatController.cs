using System;
using UnityEngine;

public class CatController : MonoBehaviour {
    public AudioClip deathClip;
    public float jumpForce = 0;

    public float speed = 1000f;

    private bool isDead = false;
    private bool isJumping = false;
    private bool isLanding = true;
    private bool isRepositioning = false;

    private Rigidbody2D catRigidbody;
    private Animator animator;
    private AudioSource catAudio;


    private void Start() {
        catRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        catAudio = GetComponent<AudioSource>();
    }

    private void Update() {
        if (isDead) {
            return;
        }

        if (!isJumping) {
            if (Input.GetMouseButton(0)) {
                // jumpForce += 100;
                jumpForce = 7600;
            }

            if (Input.GetMouseButtonUp(0)) {
                isJumping = true;
                catRigidbody.AddForce(new Vector2(7000, jumpForce));
            }
        }

        if (isRepositioning) {
            Reposition();
            if (CheckEndOnPlatform()) {
                Stop();
                GameManager.instance.JumpSuccess();
            }
        }
        // animator.SetBool("isJumping", isJumping);
    }

    private void Reposition() {
        transform.Translate(Vector3.right * (33f * Time.deltaTime));
        // transform.localPosition = new Vector3(110,362,0);
    }

    private void Stop() {
        isRepositioning = false;
        transform.localPosition = new Vector3(120, 364, 0);
    }

    private void Die() {
        Debug.Log("Die!");
        // catAudio.clip = deathClip;
        // catAudio.Play();

        GameManager.instance.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead) {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        jumpForce = 0;
        isJumping = false;
        isLanding = true;
    }

    private void OnCollisionExit2D(Collision2D other) { }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.collider.tag == "TargetPlatform" && catRigidbody.velocity == Vector2.zero) {
            if (isLanding) {
                ChangeParent();
                isRepositioning = true;
                isLanding = false;
            }
        }
    }

    private bool CheckEndOnPlatform() {
        float positionX = 120f;
        if (transform.localPosition.x >= positionX) {
            return true;
        }

        return false;
    }

    private void ChangeParent() {
        gameObject.transform.parent = GameObject.FindWithTag("TargetPlatform").transform;
    }
}