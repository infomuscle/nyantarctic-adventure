using UnityEngine;

public class Cat : MonoBehaviour {
    public AudioClip deathClip;
    private const float MAX_JUMP_FORCE = 10000;
    private float jumpForce = 0;

    private bool isDead = false;
    private bool isJumping = false;
    private bool isLanding = true;
    private bool isRepositioning = false;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource catAudio;

    // public LineRenderer lineRenderer;
    // private LineRendererController lineRendererController;


    private Vector3 OriginalPos;
    private int childCount;
    private Transform[] projectiles;
    private GameObject projectile;

    public float FixedForceMagnitude = 50000;
    private Vector3 direction;

    // private Rigidbody2D rigidbody;


    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        catAudio = GetComponent<AudioSource>();

        // lineRendererController = lineRenderer.GetComponent<LineRendererController>();


        OriginalPos = transform.position;

        Transform projectileSet = GameObject.Find("Projectile").transform;
        childCount = projectileSet.childCount;
        projectiles = new Transform[childCount];
        projectile = GameObject.Find("Projectile");
        for (int i = 0; i < childCount; i++) {
            projectiles[i] = projectileSet.GetChild(i);
        }

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = false;
    }

    private void Update() {
        if (isDead) {
            return;
        }

        if (!isJumping) {
            if (Input.GetMouseButtonDown(0)) {
                rigidbody.isKinematic = true;
                transform.position = OriginalPos;
                rigidbody.velocity = Vector3.zero;
            }


            if (Input.GetMouseButton(0)) {
                if (jumpForce <= MAX_JUMP_FORCE) {
                    jumpForce += 10000 * Time.deltaTime;
                } else {
                    jumpForce = MAX_JUMP_FORCE;
                }

                // lineRenderer.enabled = true;
                // lineRendererController.DrawLine(transform.position, new Vector3(jumpForce / 100, jumpForce / 100, 0));

                // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // RaycastHit hit;

                direction = new Vector3(1, 1, 1) * jumpForce / 3000;
                SetFlightPredict(direction * FixedForceMagnitude);
            }

            if (Input.GetMouseButtonUp(0)) {
                isJumping = true;
                // lineRenderer.enabled = false;
                rigidbody.isKinematic = false;

                rigidbody.AddForce(new Vector2(jumpForce, jumpForce));
            }
        }

        if (isRepositioning) {
            Reposition("Right");
            if (CheckEndOnPlatform()) {
                Stop();
                GameManager.instance.NextStep();
            }
        }

        // animator.SetBool("isJumping", isJumping);
    }

    private void Reposition(string forward) {
        Vector3 vector3 = new Vector3();
        switch (forward) {
            case "Right":
                vector3 = Vector3.right;
                break;
            case "Left":
                vector3 = Vector3.left;
                break;
        }

        transform.Translate(vector3 * (33f * Time.deltaTime));
    }

    private void Stop() {
        isRepositioning = false;
        isJumping = false;
        transform.localPosition = new Vector3(120, 364, 0);
    }

    private void Die() {
        Debug.Log("Die!");
        // catAudio.clip = deathClip;
        // catAudio.Play();

        GameManager.instance.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead) {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        jumpForce = 0;
        isLanding = true;
        Debug.Log("Position: " + transform.position);
    }

    private void OnCollisionExit2D(Collision2D other) { }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.collider.tag == "TargetPlatform" && rigidbody.velocity == Vector2.zero && isLanding) {
            ChangeParent();
            isRepositioning = true;
            isLanding = false;
        }
    }

    private bool CheckEndOnPlatform() {
        float positionX = 120f;
        if (transform.localPosition.x >= positionX) {
            return true;
        }

        return false;
    }

    private void ChangeParent() {
        gameObject.transform.parent = GameObject.FindWithTag("TargetPlatform").transform;
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