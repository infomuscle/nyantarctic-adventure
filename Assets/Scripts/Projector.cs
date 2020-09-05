using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projector : MonoBehaviour {
    private Rigidbody2D rigidbody;
    private int childCount;
    public GameObject projectile;
    private Transform[] projectiles;

    void Start() {
        projectile = GameObject.Find("Projectile");
        Transform projectileSet = GameObject.Find("Projectile").transform;
        childCount = projectileSet.childCount;
        projectiles = new Transform[childCount];
        for (int i = 0; i < childCount; i++) {
            projectiles[i] = projectileSet.GetChild(i);
        }

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = false;
    }

    public void SetFlightPredict(Vector3 velocity) {
        rigidbody.isKinematic = true;

        Vector3 initVel = velocity / 50;
        float p_flightTime = (initVel.y * 2.0f) / Mathf.Abs(Physics.gravity.y);
        float makeInterval = p_flightTime / childCount;

        Vector3 ori_Pos = transform.position;
        float _tmp_flightTime = makeInterval;

        for (int i = 0; i < childCount; i++) {
            Vector3 projectionPos =
                new Vector3(ori_Pos.x + makeInterval * initVel.x * (i + 1),
                    ori_Pos.y + GetHeight(0, p_flightTime, _tmp_flightTime, initVel.y), 0);
            _tmp_flightTime += makeInterval;

            projectiles[i].position = projectionPos;
        }
    }

    private float GetHeight(float t_Start, float t_End, float t_Current, float vel_Init_y) {
        float t_Center = (t_End - t_Start) / 2.0f;
        if (t_Current == t_Center) {
            return vel_Init_y * t_Center / 2.0f;
        } else if (t_Current < t_Center) {
            return (vel_Init_y + GetHeightGraph(t_Start, t_End, t_Current, vel_Init_y)) * t_Current / 2.0f;
        } else {
            return (vel_Init_y + GetHeightGraph(t_Start, t_End, (t_End - t_Current), vel_Init_y)) *
                (t_End - t_Current) / 2.0f;
        }
    }

    private float GetHeightGraph(float t_Start, float t_End, float t_Current, float vel_Init_y) {
        return -((vel_Init_y * 2) / (t_End - t_Start)) * t_Current + vel_Init_y;
    }
}