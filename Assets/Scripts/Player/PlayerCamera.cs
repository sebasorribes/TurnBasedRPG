using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Vector3 offset;
    private Vector3 standarOffset;
    [SerializeField] private Transform player;
    private PlayerInput inputs;
    [Range(0, 1)] public float lerpValue;
    public float sensitivity;
    private float xCurrentYaw = 0f;
    private float yCurrentYaw = 0f;

    private void Awake()
    {
        inputs = player.gameObject.GetComponent<PlayerInput>();
        standarOffset = offset;
    }

    private void Start()
    {
        inputs.OnMouseMoveX += ManageXRotation;
        inputs.OnMouseMoveY += ManageYRotation;
    }

    public void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(yCurrentYaw, xCurrentYaw, 0f);
        Vector3 rotatedOffset = rotation * offset;

        Vector3 targetPosition = player.position + rotatedOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpValue);

        transform.LookAt(player);
    }

    private void ManageXRotation(float delta)
    {
        xCurrentYaw += delta;
    }
    private void ManageYRotation(float delta)
    {
        yCurrentYaw += delta;
        yCurrentYaw = Mathf.Clamp(yCurrentYaw, -30f, 30f);
    }


    //TO DO: fix this, it is not working properly
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent.gameObject.name == "Walls")
        {
            offset = new Vector3(2.5f, standarOffset.y, 2.5f);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.parent.gameObject.name == "Walls")
        {
            offset = standarOffset;
        }
    }
}
