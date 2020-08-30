using UnityEngine;

public class PlatformMover : MonoBehaviour {
    public bool isMove = false;

    private float width;
    private float speed = 1000f;
    private bool isRepositioning = false;

    private float newTargetPositionX;

    private float leftEndPositionX;
    private float leftOutPositionX;
    private float rightOutPositionX;

    private void Awake() {
        BoxCollider2D platformCollider = GetComponent<BoxCollider2D>();
        width = platformCollider.size.x;
    }

    private void Start() {
        leftEndPositionX = -108f - (width * transform.localScale.x / 2);
        leftOutPositionX = -180 - (width * transform.localScale.x / 2);
        rightOutPositionX = 180 + (width * transform.localScale.x / 2);
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
                    Stop(newTargetPositionX);
                    isRepositioning = false;
                }

                break;
            case "TargetPlatform":
                if (CheckPositionOfScreen("LeftEnd")) {
                    Stop(leftEndPositionX);
                }

                break;
        }
    }

    private void Move() {
        transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }

    private void Repositon() {
        isRepositioning = true;
        transform.position = new Vector3(rightOutPositionX, -224, 0);
        newTargetPositionX = Random.Range(-36f, 180 - (width * transform.localScale.x / 2));

        ChangeWidth();
    }

    private void Stop(float positionX) {
        isMove = false;
        transform.position = new Vector3(positionX, -224, 0);

        ChangeTag();
    }

    private bool CheckPositionOfScreen(string position) {
        float positionX = 0f;
        switch (position) {
            case "LeftOut":
                positionX = leftOutPositionX;
                break;
            case "LeftEnd":
                positionX = leftEndPositionX;
                break;
            case "Target":
                positionX = newTargetPositionX;
                break;
        }

        if (transform.position.x <= positionX) {
            return true;
        }

        return false;
    }

    public void ChangeWidth() {
        float newWidth = Random.Range(0.08f, 0.2f);
        transform.localScale = new Vector3(newWidth, 0.3f, 0.5f);
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