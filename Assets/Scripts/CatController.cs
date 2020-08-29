using UnityEngine;

public class CatController : MonoBehaviour {
    public AudioClip deathClip;
    public float jumpForce = 0;

    private bool isDead = false;
    private bool isGrounded = false;

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

        if (Input.GetMouseButton(0)) {
            // jumpForce += 100;
            jumpForce = 10000;
            // Debug.Log("Getting Force: " + jumpForce);
        }

        if (Input.GetMouseButtonUp(0)) {
            // Debug.Log("Jump!");
            catRigidbody.AddForce(new Vector2(7000, jumpForce));
        }
        
        // animator.SetBool("isGroundes", isGrounded);
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
        isGrounded = true;
        jumpForce = 0;
        Debug.Log("isGrounded: " + isGrounded);
    }

    private void OnCollisionExit2D(Collision2D other) {
        isGrounded = false;
        Debug.Log("isGrounded: " + isGrounded);
    }
}