using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour {
    public float speed = 1000f;

    private void Start() {
        // Debug.Log("Tag: " + this.tag);
    }

    private void Update() {
        // MovePlatform();
    }

    private void MovePlatform() {
        if (this.tag == "StandPlatform") {
            // Move and Disappear
            // Debug.Log("Move TargetPlatform");
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
            
        } else if (this.tag == "TargetPlatform") {
            // Move to the Left
            // Debug.Log("Move TargetPlatform");
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
        }
    }
}