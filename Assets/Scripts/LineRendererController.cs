using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour {
    public LineRenderer lineRenderer;
    public Cat cat;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetColors(Color.white, Color.white);
        lineRenderer.SetWidth(5f, 5f);
    }

    public void DrawLine(Vector3 catPos, Vector3 targetPos) {
        lineRenderer.SetPosition(0, catPos);
        lineRenderer.SetPosition(1, catPos + targetPos);

        // lineRenderer.SetVertexCount((int) targetPos.x);
    }
}