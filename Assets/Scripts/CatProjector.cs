using UnityEngine;

public class CatProjector : MonoBehaviour {
    private Vector3 OriginalPos;
    private int childCount;
    private Transform[] projectiles;

    public float FixedForceMagnitude = 500;

    private Rigidbody2D rigidbody;

    void Start() {
        OriginalPos = transform.position;

        Transform projectileSet = GameObject.Find("Projectile").transform;
        childCount = projectileSet.childCount;
        projectiles = new Transform[childCount];
        for (int i = 0; i < childCount; i++) {
            projectiles[i] = projectileSet.GetChild(i);
        }

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = false;
    }

    private Vector3 direction;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
        rigidbody.isKinematic = true;
        transform.position = OriginalPos;
        rigidbody.velocity = Vector3.zero;
        }
        
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log("Ray: " + ray);
                if (hit.collider.gameObject.name == "touchPoint") {
                    direction = new Vector3(OriginalPos.x - transform.position.x, OriginalPos.y - transform.position.y,
                        0);
                    transform.position = new Vector3(hit.point.x, hit.point.y, 0);
                    // transform.position = new Vector3(0, 0, 0);
                    SetFlightPredict(direction * FixedForceMagnitude);
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0)) {
            rigidbody.isKinematic = false;
        }
    }


    private void SetFlightPredict(Vector3 velocity) {
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

    float GetHeight(float t_Start, float t_End, float t_Current, float vel_Init_y) {
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

    float GetHeightGraph(float t_Start, float t_End, float t_Current, float vel_Init_y) {
        return -((vel_Init_y * 2) / (t_End - t_Start)) * t_Current + vel_Init_y;
    }
}