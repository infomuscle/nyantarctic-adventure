﻿using UnityEngine;

public class Cat : MonoBehaviour {
    public AudioClip deathClip;
    private const float FORCE_MAGNITUDE = 500;
    private const float MAX_JUMP_FORCE = 12000;
    private const float MIN_JUMP_FORCE = 3000;
    private float jumpForce = 0;

    private bool isDead = false;
    private bool isJumping = false;
    private bool isLanding = true;
    private bool isRepositioning = false;

    private Vector2 defaultPos;

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
        
        defaultPos = new Vector2(120, 364);
    }

    private void Update() {
        if (isDead) {
            return;
        }

        if (!isJumping) {
            if (Input.GetMouseButtonDown(0)) {
                projector.projectile.SetActive(true);
                jumpForce = MIN_JUMP_FORCE;
            }

            if (Input.GetMouseButton(0)) {
                if (jumpForce <= MAX_JUMP_FORCE) {
                    jumpForce += 10000 * Time.deltaTime;
                } else {
                    jumpForce = MAX_JUMP_FORCE;
                }

                // TODO - Why 2900?
                direction = Vector2.one * jumpForce / 2900;
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
        Vector2 vector = Vector2.zero;
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
        transform.localPosition = defaultPos;
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
        projector.projectile.SetActive(false);
    }

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