using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformMover : MonoBehaviour {
    public float speed = 1000f;
    public float width;

    public bool isMove = false;

    private void Awake() {
        BoxCollider2D platformCollider = GetComponent<BoxCollider2D>();
        width = platformCollider.size.x;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            isMove = true;
        }

        if (isMove) {
            MovePlatform();
        }

        if (this.tag == "StandPlatform" && CheckOutOfScreen()) {
            Repositon();
        }

        if (this.tag == "TargetPlatform" && CheckEndOfScreen()) {
            Stop();
        }
    }

    private void MovePlatform() {
        transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }

    private void Repositon() {
        isMove = false;

        float random = Random.Range(0f, 50f);
        float newPositionX = random;
        transform.position = new Vector3(newPositionX, -224, 0);
        ChangeTag();
    }

    private void Stop() {
        isMove = false;
        
        float endPositionX = -180 + (width * transform.localScale.x / 2);
        transform.position = new Vector3(endPositionX, -224, 0);

        ChangeTag();
    }

    private bool CheckOutOfScreen() {
        float outPositionX = -180 - (width * transform.localScale.x / 2);
        if (transform.position.x <= outPositionX) {
            return true;
        }

        return false;
    }

    private bool CheckEndOfScreen() {
        float endPositionX = -180 + (width * transform.localScale.x / 2);
        if (transform.position.x <= endPositionX) {
            return true;
        }

        return false;
    }

    private void ChangeWidth() {
        
    }

    private void ChangeTag() {
        switch (this.tag) {
            case "StandPlatform":
                this.tag = "TargetPlatform";
                break;
            case "TargetPlatform":
                this.tag = "StandPlatform";
                break;
        }
    }
}