using System;
using UnityEngine;

public class Cat : MonoBehaviour {
    public AudioClip deathClip;
    private const float MAX_JUMP_FORCE = 10000;
    private float jumpForce = 0;
    private const float FORCE_MAGNITUDE = 500;

    private bool isDead = false;
    private bool isJumping = false;
    private bool isLanding = true;
    private bool isRepositioning = false;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource catAudio;

    private Projector projector;
    private Vector2 direction;

    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        catAudio = GetComponent<AudioSource>();
        projector = GetComponent<Projector>();
    }

    private void Update() {
        if (isDead) {
            return;
        }

        if (!isJumping) {
            if (Input.GetMouseButtonDown(0)) {
                projector.projectile.SetActive(true);
            }

            if (Input.GetMouseButton(0)) {
                if (jumpForce <= MAX_JUMP_FORCE) {
                    jumpForce += 10000 * Time.deltaTime;
                } else {
                    jumpForce = MAX_JUMP_FORCE;
                }

                direction = new Vector2(1, 1) * jumpForce / 3000;
                projector.Project(direction * FORCE_MAGNITUDE);
            }

            if (Input.GetMouseButtonUp(0)) {
                isJumping = true;
                rigidbody.isKinematic = false;

                rigidbody.AddForce(new Vector2(jumpForce, jumpForce));
            }
        }

        if (isRepositioning) {
            Reposition("Right");
            if (CheckEndOnPlatform()) {
                Stop();
                GameManager.instance.NextStep();
            }
        }

        // animator.SetBool("isJumping", isJumping);
    }

    private void Reposition(string forward) {
        Vector2 vector = new Vector2();
        switch (forward) {
            case "Right":
                vector = Vector2.right;
                break;
            case "Left":
                vector = Vector2.left;
                break;
        }

        transform.Translate(vector * (33f * Time.deltaTime));
    }

    private void Stop() {
        isRepositioning = false;
        isJumping = false;
        transform.localPosition = new Vector2(120, 364);
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
        isLanding = true;
        // projectile.SetActive(false);
        Debug.Log("Position: " + transform.position);
    }

    private void OnCollisionExit2D(Collision2D other) { }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.collider.tag == "TargetPlatform" && rigidbody.velocity == Vector2.zero && isLanding) {
            ChangeParent();
            isRepositioning = true;
            isLanding = false;
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