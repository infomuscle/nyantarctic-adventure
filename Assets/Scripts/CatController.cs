using System;
using UnityEngine;

public class CatController : MonoBehaviour {
    public AudioClip deathClip;
    public float jumpForce;

    private bool isDead = false;

    private Rigidbody2D catRigidbody;
    private Animator animator;
    private AudioSource catAudio;

    private void Start() {
        catRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        catAudio = GetComponent<AudioSource>();
    }

    private void Update() {
        
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
}