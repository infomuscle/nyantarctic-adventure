using UnityEngine;

public class Cat : MonoBehaviour {
    private const float FORCE_MAGNITUDE = 500;
    private const float MAX_JUMP_FORCE = 12000;
    private const float MIN_JUMP_FORCE = 3600;
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


    private float[] stopPosXs;
    private float stopPosX;

    private float localStandPosY;
    private Vector2 standOffset;
    private Vector2 standSize;

    private float localReadyPosY;
    private Vector2 readyOffset;
    private Vector2 readySize;

    private float localJumpPosY;
    private Vector2 jumpOffset;
    private Vector2 jumpSize;

    private Vector2 slideOffset;
    private Vector2 slideSize;

    private float localWalkPosY;
    private Vector2 walkOffset;
    private Vector2 walkSize;

    private Iceberg target;


    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        projector = GetComponent<Projector>();

        animator = GetComponent<Animator>();
        animator.enabled = false;

        catAudio = GetComponent<AudioSource>();
        catAudio.volume = PlayerPrefs.GetInt("sfxOn", 1);

        localStandPosY = 415f;
        standOffset = new Vector2(0f, 12f);
        standSize = new Vector2(169f, 108f);

        localReadyPosY = 430f;
        readyOffset = new Vector2(0f, 4f);
        readySize = new Vector2(188f, 128f);

        // localJumpPosY = 445f;
        localJumpPosY = 437.5f;
        jumpOffset = new Vector2(0f, 0f);
        jumpSize = new Vector2(256f, 144f);

        slideOffset = new Vector2(16f, 0f);
        slideSize = new Vector2(256f, 112f);

        localWalkPosY = 445f;
        walkOffset = new Vector2(0f, 0f);
        walkSize = new Vector2(196f, 150f);

        stopPosXs = new float[] {-32, 32, 92, 152, 212, 272};
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
                // Frisk_10
                // Frisk_11
                // Frisk_01
                // TODO 길이에 따른 구간 설정(Short-Long) + 각 구간 내 베이스 및 랜덤 리스트 
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

            string way = (transform.localPosition.x <= stopPosX) ? "Right" : "Left";
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


            target = other.gameObject.GetComponent<Iceberg>();

            stopPosX = stopPosXs[target.centers.Length];
            isRepositioning = true;
            isLanding = false;
        }
    }

    private bool CheckEndOnIceberg(string forward) {
        if (forward == "Right") {
            if (transform.localPosition.x >= stopPosX) {
                return true;
            }
        }
        else if (forward == "Left") {
            if (transform.localPosition.x <= stopPosX) {
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
                transform.localPosition = new Vector2(stopPosX, localStandPosY);
                spriteRenderer.sprite = standSprite;
                boxCollider.offset = standOffset;
                boxCollider.size = standSize;
                break;
            case "Ready":
                transform.localPosition = new Vector2(stopPosX, localReadyPosY);
                spriteRenderer.sprite = readySprite;
                boxCollider.offset = readyOffset;
                boxCollider.size = readySize;
                break;
            case "Jump":
                transform.localPosition = new Vector2(stopPosX, localJumpPosY);
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
                transform.localPosition = new Vector3(transform.localPosition.x, localWalkPosY, 0);
                spriteRenderer.sprite = walkSprite;
                boxCollider.offset = walkOffset;
                boxCollider.size = walkSize;
                break;
        }
    }
}