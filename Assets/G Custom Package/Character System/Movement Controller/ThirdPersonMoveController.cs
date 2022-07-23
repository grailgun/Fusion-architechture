using UnityEngine;

public class ThirdPersonMoveController : MonoBehaviour
{
    [Header("Move Control")]
    [SerializeField]
    private Vector2 move;
    public bool IsMoving => move != Vector2.zero;
    [SerializeField]
    private bool sprint;
    [SerializeField]
    private bool rotateOnMove = true;
    public bool analogMovement = false;
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public float SpeedChangeRate = 10.0f;

    private bool isMovementDisabled = false;
    private float _speed;
    private float _animationBlend;
    private float _animationHorizontalBlend;
    private float _animationVerticalBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    [Header("Jump Control")]
    [SerializeField]
    private bool jump;
    [Space(10)]
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;
    public float JumpTimeout = 0.50f;
    public float FallTimeout = 0.15f;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    [Header("Ground Control")]
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [SerializeField]
    private Vector2 look;
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;
    
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private Animator _animator;
    private CharacterController _controller;
    private Transform _mainCamera;

    private const float _threshold = 0.01f;
    private bool _hasAnimator;

    protected void Awake()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main.transform;

        _controller = GetComponent<CharacterController>();
        _hasAnimator = TryGetComponent(out _animator);
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        //AssignAnimationIDs();

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    public void SetCamera(Transform camera)
    {
        _mainCamera = camera;
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    #region CAMERA CONTROL METHOD
    private void CameraRotation()
    {
        if (look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = 1f;

            _cinemachineTargetYaw += look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += look.y * deltaTimeMultiplier;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    #endregion

    #region GROUND METHOD
    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, 
            transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, 
            GroundLayers, QueryTriggerInteraction.Ignore);

        if (_hasAnimator)
        {
            //_animator.SetBool("Grounded", Grounded);
        }
    }
    #endregion

    #region MOVE METHOD
    private void Move()
    {
        if (isMovementDisabled) return;

        float targetSpeed = sprint ? SprintSpeed : MoveSpeed;

        if (move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = analogMovement ? move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        _animationHorizontalBlend = Mathf.Lerp(_animationHorizontalBlend, move.x, Time.deltaTime * SpeedChangeRate);
        _animationVerticalBlend = Mathf.Lerp(_animationVerticalBlend, move.y, Time.deltaTime * SpeedChangeRate);

        if (_animationBlend < 0.01f) _animationBlend = 0f;

        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        if (move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            if(rotateOnMove)
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetFloat("Speed", _animationBlend);
            _animator.SetFloat("MotionSpeed", inputMagnitude);

            /*_animator.SetFloat("Horizontal", _animationHorizontalBlend);
            _animator.SetFloat("Vertical", _animationVerticalBlend);*/
        }
    }
    #endregion

    #region JUMP AND GRAVITY METHOD
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;

            if (_hasAnimator)
            {
                //_animator.SetBool("Jump", false);
                //_animator.SetBool("FreeFall", false);
            }

            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (jump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                if (_hasAnimator)
                {
                    //_animator.SetBool("Jump", true);
                }
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = JumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_hasAnimator)
                {
                    //_animator.SetBool("FreeFall", true);
                }
            }

            jump = false;
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    #endregion

    #region PUBLIC METHOD
    public void SetMoveInput(Vector2 input)
    {
        move = input;
    }

    public void SetLookInput(Vector2 input)
    {
        look = input;
    }

    public void SetJumpInput(bool input)
    {
        jump = input;
    }

    public void SetSprintInput(bool input)
    {
        sprint = input;
    }

    public void SetRotateOnMove(bool condition)
    {
        rotateOnMove = condition;
    }

    public void DisableMovement(bool condition)
    {
        isMovementDisabled = condition;
    }

    public void GetMoveParameter(out float hValue, out float vValue)
    {
        hValue = _animationHorizontalBlend;
        vValue = _animationVerticalBlend;
    }
    #endregion
}
