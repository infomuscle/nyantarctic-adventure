using UnityEngine;

public class PlatformMover : MonoBehaviour {
    public bool isMove = false;

    private float width;
    private float speed = 1000f;
    private bool isReposed = false;

    private void Awake() {
        BoxCollider2D platformCollider = GetComponent<BoxCollider2D>();
        width = platformCollider.size.x;
    }

    private void Update() {
        if (isMove) {
            Move();
        }

        switch (this.tag) {
            case "StandPlatform":
                if (!isReposed && CheckPositionOfScreen("Out")) {
                    Repositon();
                }

                if (isReposed && CheckPositionOfScreen("Target")) {
                    float targetPositionX = Mathf.Round(transform.position.x);
                    Stop(targetPositionX);
                    isReposed = false;
                }

                break;
            case "TargetPlatform":
                if (CheckPositionOfScreen("End")) {
                    float endPositionX = -108f - (width * transform.localScale.x / 2);
                    Stop(endPositionX);
                }

                break;
        }
    }

    private void Move() {
        transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }

    private void Repositon() {
        isReposed = true;

        float newPositionX = 180 + (width * transform.localScale.x / 2);
        transform.position = new Vector3(newPositionX, -224, 0);

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
            case "Out":
                positionX = -180 - (width * transform.localScale.x / 2);
                break;
            case "End":
                positionX = -108f - (width * transform.localScale.x / 2);
                break;
            case "Target":
                positionX = Random.Range(0f, 180 - (width * transform.localScale.x / 2));
                // positionX = 40f;
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