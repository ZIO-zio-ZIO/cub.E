using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    private bool stuckCooldown = false;
    [SerializeField] private float stuckCooldownDelay = 1.5f;


    [Header("<color=green>Physics</color>")]
    [SerializeField] private LayerMask _groundRayMask;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] public float _moveSpeed = 3.5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _grounRayDistance = 0.25f;
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private bool _isGrounded = true;

    private bool isMovementLocked = false;

    private Rigidbody _rb;
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
    }

    private void Update()
    {
        if (isMovementLocked) return;

        if (IsFrozen) return;

        _dir.x = Input.GetAxis("Horizontal");
        _dir.z = Input.GetAxis("Vertical");

        if (isMovementLocked && (_dir.x != 0f || _dir.z != 0f))
        {
            PlayerAudioManager.Instance.PlayStuckSound();
        }
        else if (!isMovementLocked)
        {
            PlayerAudioManager.Instance.ResetStuckSound();
        }

        if (IsFrozen && (_dir.x != 0 || _dir.z != 0 || Input.GetKey(KeyCode.Space)))
        {
            if (!stuckCooldown)
            {
                PlayerAudioManager.Instance?.PlayStuckSound();
                StartCoroutine(StuckCooldown());
            }
        }

        _isGrounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (IsFrozen) return; 

        if (_dir.sqrMagnitude == 0f) return;

        Vector3 cameraForward = _cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = _cameraTransform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * _dir.z + cameraRight * _dir.x;
        Vector3 targetPosition = _rb.position + moveDirection.normalized * _moveSpeed * Time.fixedDeltaTime;

        _rb.MovePosition(targetPosition);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }


    private bool IsGrounded()
    {
        _posOffset = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

        _groundRay = new Ray(_posOffset, -transform.up);

        return Physics.Raycast(_groundRay, _grounRayDistance, _groundRayMask);
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);

        PlayerAudioManager.Instance.PlayJumpSound();
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

}

