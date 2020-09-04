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
        lineRenderer.SetVertexCount(3);

        lineRenderer.SetPosition(0, catPos);
        lineRenderer.SetPosition(1, catPos + new Vector3(targetPos.x, targetPos.y, 0));
        lineRenderer.SetPosition(2, catPos + new Vector3(targetPos.x * 2, 0, 0));

        // lineRenderer.SetPosition(1, catPos + new Vector3(100, 100, 0));
        // lineRenderer.SetPosition(2, catPos + new Vector3(150, 0, 0));
    }
}