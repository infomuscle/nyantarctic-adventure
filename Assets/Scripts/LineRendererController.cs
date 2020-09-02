using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour {
    public LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetColors(Color.white, Color.white);
        lineRenderer.SetWidth(10f, 10f);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + new Vector3(0, 100, 0));
    }


    void Update() { }
}