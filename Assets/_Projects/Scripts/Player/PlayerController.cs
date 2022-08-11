using EasyCharacterMovement;
using EasyCharacterMovement.CharacterMovementDemo;
using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float rotationRate = 540.0f;
    public float maxSpeed = 5;
    public float acceleration = 20.0f;
    public float deceleration = 20.0f;
    public float groundFriction = 8.0f;
    public float airFriction = 0.5f;
    public float jumpImpulse = 6.5f;
    [Range(0.0f, 1.0f)]
    public float airControl = 0.3f;
    public Vector3 gravity = Vector3.down * 9.81f;
    
    private PlayerInput input;
    
    public CharacterMovement CharacterMovement { get; private set; }

    [Networked]
    public Vector3 MoveDirection { get; set; }

    [Networked]
    public NetworkBool Jump { get; set; }

    private void Awake()
    {
        CacheComponents();
    }
    
    private void CacheComponents()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
        input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        input.onInput.AddListener(OnGetInput);
        input.onPressed.AddListener(OnButtonPressed);
        input.onReleased.AddListener(OnButtonReleased);
    }

    private void OnDisable()
    {
        input.onInput.RemoveListener(OnGetInput);
        input.onPressed.RemoveListener(OnButtonPressed);
        input.onReleased.RemoveListener(OnButtonReleased);
    }

    #region INPUT
    private void OnGetInput(InputData inputData)
    {
        var moveDir = inputData.moveDirection.normalized;
        MoveDirection = new Vector3(moveDir.x, 0f, moveDir.y);
    }

    private void OnButtonPressed(NetworkButtons pressed)
    {
        
    }

    private void OnButtonReleased(NetworkButtons pressed)
    {

    }
    #endregion

    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
            return;
        
        GameObject camera = Camera.main.gameObject;
        if (camera != null)
        {
            if (camera.TryGetComponent<SimpleCameraController>(out var cameraController))
            {
                cameraController.target = transform;
                cameraController.enabled = cameraController.target != null;
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        UpdateMovement();
    }

    #region ROTATION METHOD

    private void UpdateMovement()
    {        
        Vector3 desiredVelocity = MoveDirection * maxSpeed;
        float actualAcceleration = CharacterMovement.isGrounded ? acceleration : acceleration * airControl;
        float actualDeceleration = CharacterMovement.isGrounded ? deceleration : 0.0f;
        float actualFriction = CharacterMovement.isGrounded ? groundFriction : airFriction;
        float deltaTime = Runner.Simulation.DeltaTime;

        CharacterMovement.SimpleMove(desiredVelocity, maxSpeed, actualAcceleration, actualDeceleration,
            actualFriction, actualFriction, gravity, true, deltaTime);
    }
    #endregion
}
