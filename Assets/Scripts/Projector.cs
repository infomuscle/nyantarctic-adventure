using UnityEngine;

public class Projector : MonoBehaviour {
    public GameObject projectile;

    private Rigidbody2D rigidbody;
    private int childCount;
    private Transform[] projectiles;

    private void Start() {
        projectile = GameObject.Find("Projectile");
        Transform projectileSet = projectile.transform;
        childCount = projectileSet.childCount;
        projectiles = new Transform[childCount];
        for (int i = 0; i < childCount; i++) {
            projectiles[i] = projectileSet.GetChild(i);
        }

        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 velocity) {
        rigidbody.isKinematic = true;

        Vector2 initVelocity = velocity / 50;
        float flightTime = (initVelocity.y * 2.0f) / Mathf.Abs(Physics.gravity.y);
        float makeInterval = flightTime / childCount;

        Vector2 originalPos = transform.position;
        float tmpFlightTime = makeInterval;

        for (int i = 0; i < childCount; i++) {
            Vector2 projectionPos =
                new Vector2(originalPos.x + makeInterval * initVelocity.x * (i + 1),
                    originalPos.y + GetHeight(0, flightTime, tmpFlightTime, initVelocity.y));
            tmpFlightTime += makeInterval;

            projectiles[i].position = projectionPos;
        }
    }

    private float GetHeight(float tStart, float tEnd, float tCurrent, float initVelocityY) {
        float t_Center = (tEnd - tStart) / 2.0f;
        if (tCurrent == t_Center) {
            return initVelocityY * t_Center / 2.0f;
        } else if (tCurrent < t_Center) {
            return (initVelocityY + GetHeightGraph(tStart, tEnd, tCurrent, initVelocityY)) * tCurrent / 2.0f;
        } else {
            return (initVelocityY + GetHeightGraph(tStart, tEnd, (tEnd - tCurrent), initVelocityY)) * (tEnd - tCurrent) / 2.0f;
        }
    }

    private float GetHeightGraph(float tStart, float tEnd, float tCurrent, float initVelocityY) {
        return -((initVelocityY * 2) / (tEnd - tStart)) * tCurrent + initVelocityY;
    }
}