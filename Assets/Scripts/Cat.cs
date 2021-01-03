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

    private Vector2 localStandPos;

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
    public Sprite slideSprite;
    public Sprite walkSprite;

    private Vector3 standPos = new Vector3(-125.5f, -119.5f, 0);
    private Vector2 standOffset = new Vector2(-1.5f, 17f);
    private Vector2 standSize = new Vector2(177f, 118f);

    private Vector3 readyPos = new Vector3(-127.5f, -114.5f, 0);
    private Vector2 readyOffset = new Vector2(0.1f, 0.5f);
    private Vector2 readySize = new Vector2(194f, 135f);

    private Vector2 jumpOffset = new Vector2(-0.4f, 4f);
    private Vector2 jumpSize = new Vector2(252f, 161f);
    private Vector2 slideOffset = new Vector2(-0.2f, -0.4f);
    private Vector2 slideSize = new Vector2(284f, 117f);

    private float walkPosY = -112f;
    private Vector2 walkOffset = new Vector2(-1.3f, -0.4f);
    private Vector2 walkSize = new Vector2(202f, 157f);


    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // catAudio = GetComponent<AudioSource>();
        projector = GetComponent<Projector>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        boxCollider = GetComponent<BoxCollider2D>();

        animator.enabled = false;

        localStandPos = new Vector2(92, 348);

        standPos = new Vector3(-125.5f, -119.5f, 0);
        standOffset = new Vector2(-1.5f, 17f);
        standSize = new Vector2(177f, 118f);

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

        if (!isJumping) {
            if (Input.GetMouseButtonDown(0)) {
                projector.projectile.SetActive(true);
                jumpForce = MIN_JUMP_FORCE;

                transform.position = readyPos;
                spriteRenderer.sprite = readySprite;
                boxCollider.offset = readyOffset;
                boxCollider.size = readySize;
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
                isJumping = true;
                rigidbody.isKinematic = false;
                rigidbody.AddForce(new Vector2(jumpForce, jumpForce));

                spriteRenderer.sprite = jumpSprite;
                boxCollider.offset = jumpOffset;
                boxCollider.size = jumpSize;
            }
        }

        if (isRepositioning) {
            transform.position = new Vector3(transform.position.x, walkPosY, 0);
            spriteRenderer.sprite = walkSprite;
            boxCollider.offset = walkOffset;
            boxCollider.size = walkSize;

            animator.enabled = true;
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
        transform.localPosition = localStandPos;
        spriteRenderer.sprite = standSprite;
        boxCollider.offset = standOffset;
        boxCollider.size = standSize;

        animator.enabled = false;
        isRepositioning = false;
        isJumping = false;
        isScoreAdded = false;
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
            spriteRenderer.sprite = slideSprite;
            boxCollider.offset = slideOffset;
            boxCollider.size = slideSize;
        }

        if (other.collider.tag == "TargetIceberg" && rigidbody.velocity == Vector2.zero && isLanding) {
            ChangeParent();
            isRepositioning = true;
            isLanding = false;
        }
    }

    private bool CheckEndOnIceberg() {
        float positionX = 92f;
        if (transform.localPosition.x >= positionX) {
            return true;
        }

        return false;
    }

    private void ChangeParent() {
        gameObject.transform.parent = GameObject.FindWithTag("TargetIceberg").transform;
    }
}