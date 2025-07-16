using UnityEngine;

public class FollowCamera : MonoBehaviour {
    public Transform target;
    public Transform cameraTransform;
    public float distance = 5f;
    public float height = 2f;
    public float rotationSpeed = 0.2f;
    public Vector2 pitchClamp = new Vector2(-20, 60);

    private float yaw = 0f;
    private float pitch = 10f;

    private void LateUpdate() {
        HandleTouch();
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = target.position + Vector3.up * height + offset;

        cameraTransform.position = desiredPosition;
        cameraTransform.LookAt(target.position + Vector3.up * height);
    }

    void HandleTouch() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved) {
                Vector2 delta = touch.deltaPosition;
                yaw += delta.x * rotationSpeed;
                pitch -= delta.y * rotationSpeed;
                pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);
            }
        }
    }
}
