using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    private bool wasGrounded = true;
    private string currentSurfaceTag = "Untagged";

    [Header("Step Settings")]
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepCheckDistance = 0.5f;
    [SerializeField] private float stepSmooth = 0.1f;
    [SerializeField] private LayerMask stairsLayer;

    private bool stuckCooldown = false;
    [SerializeField] private float stuckCooldownDelay = 1.5f;
    [SerializeField] private LayerMask _groundRayMask;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] public float _moveSpeed = 3.5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _grounRayDistance = 0.25f;
    [SerializeField] private float _jumpForce = 3f;

    [Header("Air Control Settings")]
    [SerializeField] private float airControlMultiplier = 0.5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 12f;

    [SerializeField] private bool _isGrounded = true;
    [SerializeField] private float _extraForce = 0f;

    [Header("Jump Responsiveness")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool justJumped = false;

    private Animator _animator;
    private bool isMovementLocked = false;
    public Rigidbody _rb;
    private Vector3 _dir = new(), _posOffset = new();
    private Ray _groundRay;
    public bool IsFrozen { get; set; } = false;

    // Variables para suavizado manual del input
    private float _xSmooth = 0.5f;
    private float _zSmooth = 0.5f;

    public void SetMovementLocked(bool state)
    {
        isMovementLocked = state;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        if (isMovementLocked || IsFrozen) return;

        float xRaw = Input.GetAxisRaw("Horizontal");
        float zRaw = Input.GetAxisRaw("Vertical");

        float accelerationSpeed = 20f;
        float decelerationSpeed = 15f;

        // Suavizado para X
        if (Mathf.Abs(xRaw) > Mathf.Abs(_xSmooth))
            _xSmooth = Mathf.MoveTowards(_xSmooth, xRaw, accelerationSpeed * Time.deltaTime);
        else
            _xSmooth = Mathf.MoveTowards(_xSmooth, xRaw, decelerationSpeed * Time.deltaTime);

        // Suavizado para Z
        if (Mathf.Abs(zRaw) > Mathf.Abs(_zSmooth))
            _zSmooth = Mathf.MoveTowards(_zSmooth, zRaw, accelerationSpeed * Time.deltaTime);
        else
            _zSmooth = Mathf.MoveTowards(_zSmooth, zRaw, decelerationSpeed * Time.deltaTime);

        _dir = new Vector3(_xSmooth, 0f, _zSmooth);

        float speed = new Vector2(_dir.x, _dir.z).magnitude;
        //PlayerAudioManager.Instance?.HandleFootstepSound(speed > 0f, _isGrounded);

        if (_dir.sqrMagnitude > 0)
        {
            if (isMovementLocked)
                PlayerAudioManager.Instance.PlayStuckSound();
            else
                PlayerAudioManager.Instance.ResetStuckSound();
        }

        if (IsFrozen && (_dir.sqrMagnitude > 0 || Input.GetKey(KeyCode.Space)))
        {
            if (!stuckCooldown)
            {
                PlayerAudioManager.Instance?.PlayStuckSound();
                StartCoroutine(StuckCooldown());
            }
        }

        _isGrounded = IsGrounded();

        if (_isGrounded && !justJumped)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            Jump();
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        HandleBetterJump();

        RaycastHit groundHit;
        bool groundDetected = Physics.Raycast(transform.position, Vector3.down, out groundHit, _grounRayDistance + 0.2f, _groundRayMask);

        if (groundDetected)
        {
            currentSurfaceTag = groundHit.collider.tag;

            if (!wasGrounded)
            {
                PlayerAudioManager.Instance?.PlayLandingSound(currentSurfaceTag);
            }
        }

        wasGrounded = groundDetected;

    }

    private void FixedUpdate()
    {
        if (IsFrozen || isMovementLocked) return;

        Vector3 camForward = _cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = _cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 inputDir = camForward * _dir.z + camRight * _dir.x;

        if (inputDir.sqrMagnitude > 0.01f)
            inputDir.Normalize();

        Vector3 currentVelocity = _rb.velocity;
        Vector3 targetVelocity = inputDir * _moveSpeed;
        targetVelocity.y = currentVelocity.y;

        if (_isGrounded)
        {
            if (inputDir.sqrMagnitude > 0.01f)
            {
                Vector3 smoothVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
                _rb.velocity = smoothVelocity;

                Quaternion targetRotation = Quaternion.LookRotation(inputDir) * Quaternion.Euler(0, 180, 0);
                _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
            }
            else
            {
                Vector3 horizontalVel = new Vector3(currentVelocity.x, 0, currentVelocity.z);
                horizontalVel = Vector3.MoveTowards(horizontalVel, Vector3.zero, deceleration * Time.fixedDeltaTime);
                _rb.velocity = new Vector3(horizontalVel.x, currentVelocity.y, horizontalVel.z);
            }
        }
        else
        {
            Vector3 airTarget = inputDir * _moveSpeed * airControlMultiplier;
            airTarget.y = currentVelocity.y;

            Vector3 smoothAirVelocity = Vector3.Lerp(currentVelocity, airTarget, (acceleration * 0.5f) * Time.fixedDeltaTime);
            _rb.velocity = smoothAirVelocity;
        }

        TryClimbStep();
    }

    private bool IsGrounded()
    {
        _posOffset = transform.position + Vector3.up * 0.1f;
        _groundRay = new Ray(_posOffset, Vector3.down);
        bool grounded = Physics.Raycast(_groundRay, _grounRayDistance, _groundRayMask);
        Debug.DrawRay(_posOffset, Vector3.down * _grounRayDistance, grounded ? Color.green : Color.red);
        return grounded;
    }

    private void Jump()
    {
        Vector3 velocity = _rb.velocity;
        velocity.y = 0;
        _rb.velocity = velocity;

        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isGrounded = false;

        justJumped = true;
        StartCoroutine(ResetJustJumped());

        PlayerAudioManager.Instance.PlayJumpSound();
    }

    private IEnumerator ResetJustJumped()
    {
        yield return new WaitForSeconds(0.05f);
        justJumped = false;
    }

    private void HandleBetterJump()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * 2f * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * 1.5f * Time.deltaTime;
        }
    }

    private IEnumerator StuckCooldown()
    {
        stuckCooldown = true;
        yield return new WaitForSeconds(stuckCooldownDelay);
        stuckCooldown = false;
    }

    private void TryClimbStep()
    {
        Vector3[] directions = new Vector3[]
        {
            transform.forward,
            (transform.forward + transform.right).normalized,
            (transform.forward - transform.right).normalized
        };

        foreach (var dir in directions)
        {
            Vector3 lowerRayStart = transform.position + Vector3.up * 0.05f;
            Vector3 upperRayStart = transform.position + Vector3.up * stepHeight;

            if (Physics.Raycast(lowerRayStart, dir, stepCheckDistance, stairsLayer))
            {
                if (!Physics.Raycast(upperRayStart, dir, stepCheckDistance, stairsLayer))
                {
                    _rb.position += Vector3.up * stepSmooth;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_groundRay);
    }
}
