using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 1f;
    private float yawRotationX = 0f;
    private float yawRotationY = 0f;

    [SerializeField] private float defaultAcceleration = 1f;
    [SerializeField] private float sprintAcceleration = 2f;

    public Vector3 CalculateLocalVelocity(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        return direction * moveSpeed * acceleration;
    }

    public Quaternion UpdateYawRotationX(float delta)
    {
        yawRotationX += delta;
        return Quaternion.Euler(yawRotationY, yawRotationX, 0f);
    }
    public Quaternion UpdateYawRotationY(float delta)
    {
        yawRotationY += delta;
        yawRotationY = Mathf.Clamp(yawRotationY, -15f, 30f);
        return Quaternion.Euler(yawRotationY, yawRotationX, 0f);
    }

    public void ToggleSprint()
    {
        acceleration = acceleration == defaultAcceleration ? sprintAcceleration : defaultAcceleration;
    }
}
