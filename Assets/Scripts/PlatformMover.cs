using UnityEngine;

public class PlatformMover : MonoBehaviour {
    public float width;

    public bool isMove = false;
    private bool isReposed = false;
    private float speed = 1000f;
    private GameObject hiddenPlatform;
    private PlatformMover hiddenPlatformMover;

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
                if (!isReposed && CheckOutOfScreen()) {
                    Repositon();
                }

                if (isReposed && CheckTargetOfScreen()) {
                    float targetPositionX = Mathf.Round(transform.position.x);
                    Stop(targetPositionX);
                    isReposed = false;
                }

                break;
            case "TargetPlatform":
                if (CheckEndOfScreen()) {
                    float endPositionX = -180 + (width * transform.localScale.x / 2);
                    Stop(endPositionX);
                }

                break;
        }
    }

    private void Move() {
        ChangeMovingStatus(true);
        transform.Translate(Vector3.left * (speed * Time.deltaTime));
    }

    private void Repositon() {
        isReposed = true;

        float newPositionX = 180 + (width * transform.localScale.x / 2);
        transform.position = new Vector3(newPositionX, -224, 0);

        ChangeMovingStatus(false);
    }

    private void Stop(float positionX) {
        isMove = false;

        transform.position = new Vector3(positionX, -224, 0);

        ChangeMovingStatus(false);
        ChangeTag();
    }

    private bool CheckOutOfScreen() {
        float outPositionX = -180 - (width * transform.localScale.x / 2);
        if (transform.position.x <= outPositionX) {
            return true;
        }

        return false;
    }

    private bool CheckEndOfScreen() {
        float endPositionX = -180 + (width * transform.localScale.x / 2);
        if (transform.position.x <= endPositionX) {
            return true;
        }

        return false;
    }

    private bool CheckTargetOfScreen() {
        float targetPositonX = Random.Range(0f, 180 - (width * transform.localScale.x / 2));
        if (transform.position.x <= targetPositonX) {
            return true;
        }

        return false;
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

    public void ChangeMovingStatus(bool status) {
        switch (this.tag) {
            case "StandPlatform":
                GameManager.instance.isMoving[0] = status;
                break;
            case "TargetPlatform":
                GameManager.instance.isMoving[1] = status;
                break;
        }
    }
}