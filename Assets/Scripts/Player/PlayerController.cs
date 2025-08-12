using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Vector2 movementInput;
    private PlayerInput playerInput;

    private PlayerModel playerModel;
    private PlayerView playerView;

    [SerializeField] private Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerModel = GetComponent<PlayerModel>();
        playerView = GetComponent<PlayerView>();
        AssignEvents();
    }
    

    private void AssignEvents()
    {
        playerInput.OnMovePerformed += OnMovePerformed;
        playerInput.OnMoveCanceled += OnMoveCanceled;
        playerInput.OnSprint += OnSprint;
        playerInput.OnInteract += Interact;
        playerInput.OnMouseMoveX += ManageRotationX;
        playerInput.OnMouseMoveY += ManageRotationY;
        //playerInput.OnPauseTogglePerformed += playerView.TogglePauseMenu;
        //playerView.OnAttackStateChanged += OnAttackStateChanged;
    }

    private void OnMovePerformed(Vector2 direction)
    {
        movementInput = direction;
    }

    private void OnMoveCanceled()
    {
        movementInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        PerformMovement();
    }

    private void PerformMovement()
    {
        Vector3 localVelocity = playerModel.CalculateLocalVelocity(movementInput);

        Vector3 moveDirection = playerModel.transform.forward * localVelocity.z + playerModel.transform.right * localVelocity.x;

        moveDirection.y = 0f;

        rigidBody.MovePosition(rigidBody.position + moveDirection * Time.fixedDeltaTime);
    }

    private void ManageRotationX(float amount)
    {
        Quaternion newRotation = playerModel.UpdateYawRotationX(amount);

        transform.rotation = newRotation;
    }
    private void ManageRotationY(float amount)
    {
        Quaternion newRotation = playerModel.UpdateYawRotationY(-amount);

        mainCamera.transform.rotation = newRotation;
    }

    private void OnSprint()
    {
        playerModel.ToggleSprint();
    }

    private void Interact()
    {
    }


}
