﻿using UnityEngine;

public class Cat : MonoBehaviour {
    private const float FORCE_MAGNITUDE = 500;
    private const float MAX_JUMP_FORCE = 12000;
    private const float MIN_JUMP_FORCE = 3900;
    private float jumpForce = 0;
    private float addForce = 0;
    private bool forceUp = true;

    private int scoreOnReady;

    private bool isDead = false;
    private bool isJumping = false;
    private bool isLanding = false;
    private bool isRepositioning = false;


    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    private Animator animator;
    private AudioSource catAudio;
    public AudioClip jumpClip;
    public AudioClip deathClip;

    private Projector projector;
    private Vector2 direction;

    private SpriteRenderer spriteRenderer;
    public Sprite readySprite;
    public Sprite standSprite;
    public Sprite jumpSprite;
    public Sprite slideSprite;
    public Sprite walkSprite;

    private Vector2 localStandPos;
    private Vector3 standPos;
    private Vector2 standOffset;
    private Vector2 standSize;

    private Vector2 localReadyPos;
    private Vector3 readyPos;
    private Vector2 readyOffset;
    private Vector2 readySize;

    private Vector2 jumpOffset;
    private Vector2 jumpSize;
    private Vector2 slideOffset;
    private Vector2 slideSize;

    private float walkPosY;
    private Vector2 walkOffset;
    private Vector2 walkSize;


    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        projector = GetComponent<Projector>();

        animator = GetComponent<Animator>();
        animator.enabled = false;

        catAudio = GetComponent<AudioSource>();
        catAudio.volume = PlayerPrefs.GetInt("sfxOn", 1);

        // localStandPos = new Vector2(92, 277);
        localStandPos = new Vector2(45, 277);
        standPos = new Vector3(-125.5f, -119.5f, 0);
        standOffset = new Vector2(-1.5f, 17f);
        standSize = new Vector2(177f, 118f);

        localReadyPos = new Vector2(45, 286);
        readyPos = new Vector3(-127.5f, -114.5f, 0);
        readyOffset = new Vector2(0.1f, 0.5f);
        readySize = new Vector2(194f, 135f);

        jumpOffset = new Vector2(-0.4f, 4f);
        jumpSize = new Vector2(252f, 161f);
        slideOffset = new Vector2(-0.2f, -0.4f);
        slideSize = new Vector2(284f, 117f);

        walkPosY = -112f;
        walkOffset = new Vector2(-1.3f, -0.4f);
        walkSize = new Vector2(202f, 157f);
    }

    private void Update() {
        if (isDead) {
            return;
        }

        if (!isJumping && !isRepositioning) {
            if (Input.GetMouseButtonDown(0)) {
                ChangeSprite("Ready");
                projector.projectile.SetActive(true);
                jumpForce = MIN_JUMP_FORCE;
                scoreOnReady = GameManager.instance.getScore();
            }

            if (Input.GetMouseButton(0)) {
                if (forceUp && jumpForce > MAX_JUMP_FORCE) {
                    forceUp = false;
                }
                else if (!forceUp && jumpForce < MIN_JUMP_FORCE) {
                    forceUp = true;
                }

                addForce = 10000 * Time.deltaTime;
                jumpForce += (forceUp) ? addForce : -1 * addForce;

                // TODO - Why 2900?
                direction = Vector2.one * jumpForce / 2900;
                projector.Project(direction * FORCE_MAGNITUDE);
            }

            if (Input.GetMouseButtonUp(0)) {
                ChangeSprite("Jump");
                catAudio.clip = jumpClip;
                catAudio.Play();

                isJumping = true;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(new Vector2(jumpForce, jumpForce));
            }
        }

        if (isRepositioning) {
            ChangeSprite("Walk");
            animator.enabled = true;

            if (GameManager.instance.getScore() == scoreOnReady) {
                GameManager.instance.AddScore(1);
            }

            string way = (transform.localPosition.x <= 92f) ? "Right" : "Left";
            if (way == "Left") {
                spriteRenderer.flipX = true;
            }

            Reposition(way);
            if (CheckEndOnIceberg(way)) {
                if (way == "Left") {
                    spriteRenderer.flipX = false;
                }

                Stop();
                GameManager.instance.NextStep();
            }
        }
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
        ChangeSprite("Stand");
        animator.enabled = false;

        isRepositioning = false;
        isJumping = false;
    }

    private void Die() {
        Debug.Log("Die!");
        catAudio.clip = deathClip;
        catAudio.Play();
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
        if (isJumping && isLanding) {
            ChangeSprite("Slide");
        }

        if (other.collider.tag == "TargetIceberg" && rigidbody.velocity == Vector2.zero && isLanding) {
            ChangeParent();
            isRepositioning = true;
            isLanding = false;
        }
    }

    private bool CheckEndOnIceberg(string forward) {
        float positionX = 92f;

        if (forward == "Right") {
            if (transform.localPosition.x >= positionX) {
                return true;
            }
        }
        else if (forward == "Left") {
            if (transform.localPosition.x <= positionX) {
                return true;
            }
        }


        return false;
    }

    private void ChangeParent() {
        gameObject.transform.SetParent(GameObject.FindWithTag("TargetIceberg").transform);
    }

    private void ChangeSprite(string status) {
        switch (status) {
            case "Stand":
                transform.localPosition = localStandPos;
                spriteRenderer.sprite = standSprite;
                boxCollider.offset = standOffset;
                boxCollider.size = standSize;
                break;
            case "Ready":
                // transform.position = readyPos;
                transform.localPosition = localReadyPos;
                spriteRenderer.sprite = readySprite;
                boxCollider.offset = readyOffset;
                boxCollider.size = readySize;
                break;
            case "Jump":
                spriteRenderer.sprite = jumpSprite;
                boxCollider.offset = jumpOffset;
                boxCollider.size = jumpSize;
                break;
            case "Slide":
                spriteRenderer.sprite = slideSprite;
                boxCollider.offset = slideOffset;
                boxCollider.size = slideSize;
                break;
            case "Walk":
                transform.position = new Vector3(transform.position.x, walkPosY, 0);
                spriteRenderer.sprite = walkSprite;
                boxCollider.offset = walkOffset;
                boxCollider.size = walkSize;
                break;
        }
    }
}