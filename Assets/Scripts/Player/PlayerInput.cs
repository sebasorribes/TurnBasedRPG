using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.1f;

    public event Action<Vector2> OnMovePerformed;
    public event Action<float> OnMouseMoveX;
    public event Action<float> OnMouseMoveY;
    public event Action OnMoveCanceled;
    public event Action OnPauseTogglePerformed;
    public event Action OnSprint;
    public event Action OnInteract;

    private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction interactAction;
    private InputAction pauseAction;
    private InputAction sprintAction;

    private void Awake()
    {
        AssignActions();
        AssignEvents();
    }

    private void OnEnable()
    {
        OnPlayerInputEnabled();
    }

    private void AssignActions()
    {
        var playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        inputActions = playerInput.actions;

        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        interactAction = inputActions.FindAction("Interact");
        sprintAction = inputActions.FindAction("Sprint");

    }
    private void AssignEvents()
    {
        moveAction.performed += HandleMove;
        moveAction.canceled += HandleMove;
        lookAction.performed += HandleLook;
        //pauseAction.performed += HandlePause;
        interactAction.performed += HandleInteract;
        sprintAction.performed += HandleSprint;
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();

        if (context.phase == InputActionPhase.Performed)
        {
            OnMovePerformed?.Invoke(inputValue);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnMoveCanceled?.Invoke();
        }
    }

    private void HandleLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        float mouseInputX = mouseDelta.x * mouseSensitivity;
        float mouseInputY = mouseDelta.y * mouseSensitivity;

        if (Mathf.Abs(mouseInputX) > Mathf.Epsilon)
        {
            OnMouseMoveX?.Invoke(mouseInputX);
        }

        if (Mathf.Abs(mouseInputY) > Mathf.Epsilon)
        {
            OnMouseMoveY?.Invoke(mouseInputY);
        }
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnPauseTogglePerformed?.Invoke();
        }
    }
    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnInteract?.Invoke();
        }
    }
    private void HandleSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnSprint?.Invoke();
        }
    }

    public void OnPlayerInputEnabled()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        interactAction.Enable();
    }
}
