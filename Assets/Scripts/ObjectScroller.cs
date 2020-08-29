using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroller : MonoBehaviour {
    public float speed;

    void Update() {
        if (!GameManager.instance.isGameOver) {
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
        }
    }
}