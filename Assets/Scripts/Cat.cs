using UnityEngine;

public class Cat : MonoBehaviour {
    public AudioSource audioDeath;
    public AudioSource audioJump;
    private const float FORCE_MAGNITUDE = 500;
    private const float MAX_JUMP_FORCE = 12000;
    private const float MIN_JUMP_FORCE = 3000;
    private float jumpForce = 0;
    private float addForce = 0;
    private bool forceUp = true;


    private bool isScoreAdded = false;

    private bool isDead = false;
    private bool isJumping = false;
    private bool isLanding = false;
    private bool isRepositioning = false;

    private Vector2 defaultPos;

    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    private Animator animator;
    // private AudioSource catAudio;

    private Projector projector;
    private Vector2 direction;

    private SpriteRenderer spriteRenderer;
    public Sprite readySprite;
    public Sprite standSprite;
    public Sprite jumpSprite;
    public Sprite landingSprite;
    public Sprite walkingSprite;

    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // catAudio = GetComponent<AudioSource>();
        projector = GetComponent<Projector>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        boxCollider = GetComponent<BoxCollider2D>();
        // Stand - -2 / 17 / 176 / 118
        // Ready - 0.1 / 0.5 / 194 / 135
        // Jump - -0.4 / 4 / 252 / 161 
        // Slide - 0.2 / -0.4 / 284 / 117
        // Walk - -1.3 / -0.4 / 202 / 157

        animator.enabled = false;

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

                // Ready - 0.1 / 0.5 / 194 / 135
                spriteRenderer.sprite = readySprite;
                boxCollider.offset = new Vector2(0.1f, 0.5f);
                boxCollider.size = new Vector2(194f, 135f);
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
                audioJump.Play();
                // animator.SetTrigger("Jump");
                isJumping = true;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(new Vector2(jumpForce, jumpForce));

                // Jump - -0.4 / 4 / 252 / 161 
                spriteRenderer.sprite = jumpSprite;
                boxCollider.offset = new Vector2(-0.4f, 4f);
                boxCollider.size = new Vector2(252f, 161f);
            }
        }

        if (isRepositioning) {
            // Walk - -1.3 / -0.4 / 202 / 157
            spriteRenderer.sprite = walkingSprite;
            boxCollider.offset = new Vector2(-0.3f, -0.4f);
            boxCollider.size = new Vector2(202f, 157f);
            animator.enabled = true;
            // animator.SetTrigger("Walk");
            // animator.SetBool("Walking", true);
            Reposition("Right");

            if (!isScoreAdded) {
                GameManager.instance.AddScore(1);
                isScoreAdded = true;
            }

            if (CheckEndOnIceberg()) {
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
        // Stand - -1.5 / 17 / 117 / 118
        spriteRenderer.sprite = standSprite;
        boxCollider.offset = new Vector2(-1.5f, 17f);
        boxCollider.size = new Vector2(176f, 118f);
        animator.enabled = false;
        // animator.SetTrigger("Stand");
        isRepositioning = false;
        isJumping = false;
        isScoreAdded = false;

        transform.localPosition = defaultPos;
    }

    private void Die() {
        Debug.Log("Die!");
        // catAudio.clip = deathClip;
        // catAudio.Play();
        audioDeath.Play();
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
            // Slide - 0.2 / -0.4 / 284 / 117
            spriteRenderer.sprite = landingSprite;
            boxCollider.offset = new Vector2(-0.2f, -0.4f);
            boxCollider.size = new Vector2(284f, 117f);
            // animator.SetTrigger("Slide");
        }

        if (other.collider.tag == "TargetIceberg" && rigidbody.velocity == Vector2.zero && isLanding) {
            ChangeParent();
            isRepositioning = true;
            isLanding = false;
        }
    }

    private bool CheckEndOnIceberg() {
        float positionX = 120f;
        if (transform.localPosition.x >= positionX) {
            return true;
        }

        return false;
    }

    private void ChangeParent() {
        gameObject.transform.parent = GameObject.FindWithTag("TargetIceberg").transform;
    }
}