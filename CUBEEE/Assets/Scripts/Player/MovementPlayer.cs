using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    private bool stuckCooldown = false;
    [SerializeField] private float stuckCooldownDelay = 1.5f;
    [SerializeField] private LayerMask _groundRayMask;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] public float _moveSpeed = 3.5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _grounRayDistance = 0.25f;
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private bool _isGrounded = true;
    [SerializeField] private float _extraForce = 0f;

    private Animator _animator;
    private bool isMovementLocked = false;
    public Rigidbody _rb;
    private Vector3 _dir = new(), _posOffset = new();
    private Ray _groundRay;
    public bool IsFrozen { get; set; } = false;

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
        if (isMovementLocked) return;
        if (IsFrozen) return;

        _dir.x = Input.GetAxis("Horizontal");
        _dir.z = Input.GetAxis("Vertical");

        float speed = new Vector2(_dir.x, _dir.z).magnitude;
        //_animator.SetFloat("Speed", Mathf.Clamp01(speed));

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

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
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

        Vector3 moveDir = camForward * _dir.z + camRight * _dir.x;
        moveDir.Normalize();

        if (moveDir.sqrMagnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir) * Quaternion.Euler(0, 180, 0);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed));
        }

        Vector3 velocity = moveDir * _moveSpeed;
        velocity.y = _rb.velocity.y;
        _rb.velocity = velocity;
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
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        PlayerAudioManager.Instance.PlayJumpSound();
        Invoke("Falling", 0.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_groundRay);
    }

    private IEnumerator StuckCooldown()
    {
        stuckCooldown = true;
        yield return new WaitForSeconds(stuckCooldownDelay);
        stuckCooldown = false;
    }

    private void Falling()
    {
        _rb.AddForce(_extraForce * Physics.gravity);
    }
}

