using System;
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

    private bool isExploring;

    private float maxStepsInterval = 100f;
    private int stepCounter = 0;
    private Vector3 lastPosition;

    private int stepDistanceThreshold = 1;

    public Action<GameObject[]> OnSetBattlePJ;
    public Action OnStartBattle;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerModel = GetComponent<PlayerModel>();
        playerView = GetComponent<PlayerView>();
    }


    private void AssignEvents()
    {
        playerInput.OnMovePerformed += OnMovePerformed;
        playerInput.OnMoveCanceled += OnMoveCanceled;
        playerInput.OnSprint += OnSprint;
        playerInput.OnInteract += Interact;
        playerInput.OnMouseMoveX += ManageRotationX;
        playerInput.OnMouseMoveY += ManageRotationY;
        GameManager.Instance.OnEndBattle += ReturnToExplore;
        //playerInput.OnPauseTogglePerformed += playerView.TogglePauseMenu;
        //playerView.OnAttackStateChanged += OnAttackStateChanged;
    }

    private void UnassignEvents()
    {
        playerInput.OnMovePerformed -= OnMovePerformed;
        playerInput.OnMoveCanceled -= OnMoveCanceled;
        playerInput.OnSprint -= OnSprint;
        playerInput.OnInteract -= Interact;
        playerInput.OnMouseMoveX -= ManageRotationX;
        playerInput.OnMouseMoveY -= ManageRotationY;
        GameManager.Instance.OnEndBattle -= ReturnToExplore;
        //playerInput.OnPauseTogglePerformed -= playerView.TogglePauseMenu;
        //playerView.OnAttackStateChanged -= OnAttackStateChanged;
    }

    private void Start()
    {
        OnSetBattlePJ?.Invoke(GetPjs());
        EnterInDungeon();
    }

    public void EnterInDungeon()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stepCounter = (int) UnityEngine.Random.Range(1f, maxStepsInterval);
        SetExploring(true);
        AssignEvents();
        transform.position = new Vector3(4.4f,1.23f,11f);
        lastPosition = transform.position;
    }

    public void ExitDungeon()
    {
        SetExploring(false);
        UnassignEvents();
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
        if(isExploring)
        {
            PerformMovement();
        }
        
    }

    private void PerformMovement()
    {
        Vector3 localVelocity = playerModel.CalculateLocalVelocity(movementInput);

        Vector3 moveDirection = playerModel.transform.forward * localVelocity.z + playerModel.transform.right * localVelocity.x;

        moveDirection.y = 0f;

        rigidBody.MovePosition(rigidBody.position + moveDirection * Time.fixedDeltaTime);

        float distanceTraveled = Vector3.Distance(lastPosition, rigidBody.position);

        if (distanceTraveled >= stepDistanceThreshold)
        {
            lastPosition = rigidBody.position;
            RandomEncounter();
        }
    }

    private void ManageRotationX(float amount)
    {
        if(!isExploring) return;
        Quaternion newRotation = playerModel.UpdateYawRotationX(amount);

        transform.rotation = newRotation;
    }
    private void ManageRotationY(float amount)
    {
        if (!isExploring) return;

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

    public GameObject[] GetPjs()
    {
        return playerModel.pjs;
    }

    public void SetExploring(bool exploring)
    {
        isExploring = exploring;
    }

    private void RandomEncounter()
    {
        stepCounter--;
        if (stepCounter <= 0)
        {
            stepCounter = (int)UnityEngine.Random.Range(1f, maxStepsInterval);
            OnStartBattle?.Invoke();
            mainCamera.gameObject.SetActive(false);
            SetExploring(false);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void ReturnToExplore()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera.gameObject.SetActive(true);
        SetExploring(true);
    }
}
