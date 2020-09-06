using UnityEngine;

public class Platform : MonoBehaviour {
    private const float DEFAULT_POS_Y = -224f;
    private const float SPEED = 1000f;
    public bool isMove = false;

    private float width;
    private bool isRepositioning = false;

    private float targetPosX;
    private float leftEndPosX;
    private float leftOutPosX;
    private float rightEndPosX;
    private float rightOutPosX;

    private void Awake() {
        BoxCollider2D platformCollider = GetComponent<BoxCollider2D>();
        width = platformCollider.size.x;
    }

    private void Start() {
        leftEndPosX = -180f + (width * transform.localScale.x / 2);
        leftOutPosX = -180f - (width * transform.localScale.x / 2);
        rightEndPosX = 180f - (width * transform.localScale.x / 2);
        rightOutPosX = 180f + (width * transform.localScale.x / 2);
    }

    private void Update() {
        if (isMove) {
            Move();
        }

        switch (this.tag) {
            case "StandPlatform":
                if (!isRepositioning && CheckPositionOfScreen("LeftOut")) {
                    Repositon();
                }

                if (isRepositioning && CheckPositionOfScreen("Target")) {
                    Stop(targetPosX);
                    isRepositioning = false;
                }

                break;
            case "TargetPlatform":
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
        isRepositioning = true;
        transform.position = new Vector2(rightOutPosX, DEFAULT_POS_Y);
        targetPosX = Random.Range(-36f, rightEndPosX);

        ChangeWidth();
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

    public void ChangeWidth() {
        float newWidth = Random.Range(0.08f, 0.2f);
        transform.localScale = new Vector2(newWidth, 0.3f);
    }

    public void ChangeTag() {
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