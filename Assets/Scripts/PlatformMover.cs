using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            isMove = false;
            float targetPositionX = -180 + (width * transform.localScale.x / 2);
            transform.position = new Vector3(targetPositionX, -224, 0);
        }
    }

    private void MovePlatform() {
        transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }

    private bool CheckOutOfScreen() {
        float targetPositionX = -180 - (width * transform.localScale.x / 2);
        if (transform.position.x <= targetPositionX) {
            return true;
        }

        return false;
    }

    private bool CheckEndOfScreen() {
        float targetPositionX = -180 + (width * transform.localScale.x / 2);
        if (transform.position.x <= targetPositionX) {
            return true;
        }

        return false;
    }

    private void Repositon() {
        isMove = false;
        switch (this.tag) {
            case "StandPlatform":
                break;
            case "TargetPlatform":
                break;
        }
        transform.position = new Vector3(0, -224, 0);
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