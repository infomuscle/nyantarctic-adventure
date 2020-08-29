using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour {
    public float speed = 1000f;
    public float width;

    private void Awake() {
        BoxCollider2D platformCollider = GetComponent<BoxCollider2D>();
        width = platformCollider.size.x;
    }

    private void Start() {
        if (this.tag == "StandPlatform") {
            // Debug.Log(transform.position.x);
            // Debug.Log(width);
            // Debug.Log(transform.localScale.x);
            MovePlatform();
        }
    }

    private void Update() {
        // Debug.Log(transform.position.x);
        // if (!CheckPosition()) {
            // MovePlatform();
        // }

        // CheckPosition();
    }

    private void MovePlatform() {
        // Debug.Log("MovePlatform!");
        switch (this.tag) {
            case "StandPlatform":
                // Move to the Left
                // transform.Translate(Vector3.left * (speed * Time.deltaTime));
                while (!CheckPosition()) {
                    transform.Translate(Vector3.left * speed);
                }
                break;
            case "TargetPlatform":
                // Move and Disappear
                // transform.Translate(Vector3.left * (speed * Time.deltaTime));
                break;
        }
    }

    private bool CheckPosition() {
        float positionX = -180 - (width * transform.localScale.x / 2);
        if (transform.position.x <= positionX) {
            // transform.position = new Vector3(0, -224, 0);
            return true;
        }

        return false;
    }
}