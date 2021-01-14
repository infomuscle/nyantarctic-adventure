using UnityEngine;

public class Iceberg : MonoBehaviour {
    private const float DEFAULT_POS_Y = -230f;
    private const float SPEED = 1000f;
    private const int MAX_CENTER_CNT = 5;
    private const int MIN_CENTER_CNT = 0;
    private const int RIGHT_OUT_POS_X = 216;

    public GameObject centerPrefab;
    public GameObject leftPrefab;
    public GameObject rightPrefab;

    private GameObject left;
    public GameObject[] centers;
    private GameObject right;

    private BoxCollider2D icebergCollider;

    public bool isMove = false;

    private float width;
    private bool isRepositioning = false;

    private int targetPosX;
    private int[] rightEndPosXs;
    private int rightEndPosX;
    private float leftOutPosX;
    private float leftEndPosX;


    private Rigidbody2D rigidbody;
    private Vector2[] boxOffsets;
    private Vector2[] boxSizes;

    private void Awake() {
        icebergCollider = GetComponent<BoxCollider2D>();
        width = icebergCollider.size.x;
        boxOffsets = new[] {
            new Vector2(-35, 0),
            new Vector2(-5, 0),
            new Vector2(25, 0),
            new Vector2(55, 0),
            new Vector2(85, 0),
            new Vector2(115, 0),
        };
        boxSizes = new[] {
            new Vector2(195, 760),
            new Vector2(255, 760),
            new Vector2(315, 760),
            new Vector2(375, 760),
            new Vector2(435, 760),
            new Vector2(495, 760),
        };

        // 195-163 | 255-148 | 315-133 | 375-118 | 435-103
        rightEndPosXs = new[] {163, 148, 133, 118, 103};
    }

    private void Start() {
        leftEndPosX = -180f + (width * transform.localScale.x / 2);
        leftOutPosX = -180f - (width * transform.localScale.x / 2);
        if (tag == "TargetIceberg") {
            Resize();
        }
    }

    private void Update() {
        if (isMove) {
            Move();
        }

        switch (tag) {
            case "StandIceberg":
                if (!isRepositioning && CheckPositionOfScreen("LeftOut")) {
                    Repositon();
                }

                if (isRepositioning && CheckPositionOfScreen("Target")) {
                    Stop(targetPosX);
                    isRepositioning = false;
                }

                break;
            case "TargetIceberg":
                if (CheckPositionOfScreen("LeftEnd")) {
                    Stop(leftEndPosX);
                }

                break;
        }
    }

    private void Move() {
        transform.Translate(Vector2.left * (SPEED * Time.deltaTime));
    }

    private void Repositon() {
        Resize();
        isRepositioning = true;
        transform.position = new Vector2(RIGHT_OUT_POS_X, DEFAULT_POS_Y);
        targetPosX = Random.Range(0, rightEndPosX);
    }

    private void Stop(float posX) {
        isMove = false;
        transform.position = new Vector2(posX, DEFAULT_POS_Y);

        ChangeTag();
    }

    private bool CheckPositionOfScreen(string position) {
        float posX = 0f;
        switch (position) {
            case "LeftOut":
                posX = leftOutPosX;
                break;
            case "LeftEnd":
                posX = leftEndPosX;
                break;
            case "Target":
                posX = targetPosX;
                break;
        }

        if (transform.position.x <= posX) {
            return true;
        }

        return false;
    }

    private void Resize() {
        Destroy(gameObject.transform.Find("Images").gameObject);

        GameObject newImages = new GameObject("Images");
        newImages.transform.parent = gameObject.transform;
        newImages.transform.localPosition = Vector3.zero;
        newImages.transform.localScale = Vector3.one;

        int centerCnt = Random.Range(MIN_CENTER_CNT, MAX_CENTER_CNT);
        left = Instantiate(leftPrefab);
        left.transform.parent = newImages.transform;
        left.transform.localPosition = new Vector3(-80, 0, 0);
        left.transform.localScale = Vector3.one;

        centers = new GameObject[centerCnt];
        for (int i = 0; i < centerCnt; i++) {
            centers[i] = Instantiate(centerPrefab);
            centers[i].transform.parent = newImages.transform;
            centers[i].transform.localPosition = new Vector3(60 * i, 0, 0);
            centers[i].transform.localScale = Vector3.one;
        }

        right = Instantiate(rightPrefab);
        right.transform.parent = newImages.transform;
        right.transform.localPosition = new Vector3(60 * (centerCnt - 1) + 80, 0, 0);
        right.transform.localScale = Vector3.one;

        icebergCollider.offset = boxOffsets[centerCnt];
        icebergCollider.size = boxSizes[centerCnt];

        rightEndPosX = rightEndPosXs[centerCnt];
    }

    private void ChangeTag() {
        switch (tag) {
            case "StandIceberg":
                tag = "TargetIceberg";
                break;
            case "TargetIceberg":
                tag = "StandIceberg";
                break;
        }
    }
}