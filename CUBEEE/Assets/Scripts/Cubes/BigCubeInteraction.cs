using System.Collections.Generic;
using UnityEngine;

public class BigCubeInteraction : MonoBehaviour
{
    private bool wasMovingLastFrame = false;

    [SerializeField] private List<BigCubePushPullTrigger> lateralTriggers;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MovementPlayer playerMovement;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private ParticleSystem _attachedParticles;

    private bool isAttached = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isAttached)
        {
            foreach (var trigger in lateralTriggers)
            {
                if (trigger.isPlayerTouching && Input.GetKeyDown(KeyCode.E))
                {
                    _attachedParticles.Play();
                    isAttached = true;
                    playerMovement.SetMovementLocked(true);
                    Debug.Log("Jugador se pegó al cubo grande.");
                    break;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _attachedParticles.Play();
                DetachPlayer();
                return;
            }

            bool stillTouching = false;
            foreach (var trigger in lateralTriggers)
            {
                if (trigger.isPlayerTouching)
                {
                    stillTouching = true;
                    break;
                }
            }

            if (!stillTouching)
            {
                DetachPlayer();
                return;
            }

            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            bool isMoving = input.sqrMagnitude > 0.01f;

            if (isMoving)
            {
                Vector3 cameraForward = playerCamera.forward;
                cameraForward.y = 0f;
                cameraForward.Normalize();

                Vector3 cameraRight = playerCamera.right;
                cameraRight.y = 0f;
                cameraRight.Normalize();

                Vector3 moveDirection = cameraForward * input.z + cameraRight * input.x;
                moveDirection.Normalize();

                Vector3 moveVector = moveDirection * moveSpeed * Time.deltaTime;

                rb.MovePosition(rb.position + moveVector);

                if (playerTransform != null)
                {
                    playerTransform.position += moveVector;
                }
            }

            if (isMoving && !wasMovingLastFrame)
            {
                PlayerAudioManager.Instance.PlayPushPullLoop();
            }
            else if (!isMoving && wasMovingLastFrame)
            {
                PlayerAudioManager.Instance.StopPushPullLoop();
            }

            wasMovingLastFrame = isMoving;
        }
    }

    public void DetachPlayer()
    {
        isAttached = false;
        wasMovingLastFrame = false;
        playerMovement.SetMovementLocked(false);
        PlayerAudioManager.Instance.StopPushPullLoop();
        Debug.Log("Jugador se despegó del cubo grande.");
    }

    public void ForceDetach()
    {
        if (isAttached)
            DetachPlayer();
    }
}
