using UnityEngine;

public class Cat : MonoBehaviour {
    public AudioClip deathClip;
    public float jumpForce = 0;

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
    }

    private void OnCollisionExit2D(Collision2D other) { }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.collider.tag == "TargetPlatform" && catRigidbody.velocity == Vector2.zero && isLanding) {
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