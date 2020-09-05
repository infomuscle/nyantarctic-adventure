using UnityEngine;

public class Cat : MonoBehaviour {
    public AudioClip deathClip;
    private const float MAX_JUMP_FORCE = 10000;
    private float jumpForce = 0;

    private bool isDead = false;
    private bool isJumping = false;
    private bool isLanding = true;
    private bool isRepositioning = false;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource catAudio;

    private Projector projector;
    public float FixedForceMagnitude;
    private Vector3 direction;

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
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
            }

            if (Input.GetMouseButton(0)) {
                if (jumpForce <= MAX_JUMP_FORCE) {
                    jumpForce += 10000 * Time.deltaTime;
                } else {
                    jumpForce = MAX_JUMP_FORCE;
                }

                direction = new Vector3(1, 1, 0) * jumpForce / 3000;
                projector.SetFlightPredict(direction * FixedForceMagnitude);
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
        Vector3 vector3 = new Vector3();
        switch (forward) {
            case "Right":
                vector3 = Vector3.right;
                break;
            case "Left":
                vector3 = Vector3.left;
                break;
        }

        transform.Translate(vector3 * (33f * Time.deltaTime));
    }

    private void Stop() {
        isRepositioning = false;
        isJumping = false;
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