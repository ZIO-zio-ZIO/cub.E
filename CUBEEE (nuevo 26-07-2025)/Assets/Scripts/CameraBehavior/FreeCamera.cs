using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public static bool cameraLocked = false;
    [Header("Target")]
    public Transform target;

    [Header("Orbit Settings")]
    public float distance = 10f;
    public float zoomedDistance = 5f;
    public float rotationSpeed = 5.0f;

    [Header("Pitch Settings")]
    public bool clampPitch = true;
    public float minPitch = -45f;
    public float maxPitch = 45f;

    [Header("Follow Smoothing")]
    public float followSpeed = 5f;

    [Header("Zoom Settings")]
    public float normalFOV = 60f;
    public float zoomedFOV = 40f;

    [Header("Collision Settings")]
    public bool enableCollision = true;
    public LayerMask collisionMask;
    public float collisionRadius = 0.3f;
    public float minDistance = 1f;

    private float yaw;
    private float pitch;
    private Vector3 currentVelocity;
    private bool isZoomedIn = false;

    private Camera cameraComponent;

    void Start()
    {
        cameraComponent = GetComponentInChildren<Camera>();
        if (cameraComponent == null)
        {
            Debug.Log("No Camera component found in children of CameraRig.");
            enabled = false;
            return;
        }

        // Siempre busca un nuevo target al iniciar
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }

        if (target == null)
        {
            Debug.Log("FreeCamera: No target assigned and Player not found.");
            enabled = false;
            return;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
        cameraComponent.fieldOfView = normalFOV;

        // Posicionar cámara instantáneamente sobre el jugador al empezar
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.back;
        Vector3 rayOrigin = target.position + Vector3.up * 1.5f;
        float adjustedDistance = distance;

        if (enableCollision)
        {
            RaycastHit hit;
            if (Physics.SphereCast(rayOrigin, collisionRadius, direction, out hit, distance, collisionMask))
            {
                adjustedDistance = Mathf.Clamp(hit.distance, minDistance, distance);
            }
        }

        Vector3 desiredPosition = target.position + direction * adjustedDistance;
        transform.position = desiredPosition;
        transform.LookAt(target.position);

        Invoke(nameof(ForceCameraSnapToTarget), 0.1f);
    }

    private void ForceCameraSnapToTarget()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.back;

        Vector3 rayOrigin = target.position + Vector3.up * 1.5f;
        float adjustedDistance = distance;

        if (enableCollision)
        {
            RaycastHit hit;
            if (Physics.SphereCast(rayOrigin, collisionRadius, direction, out hit, distance, collisionMask))
            {
                adjustedDistance = Mathf.Clamp(hit.distance, minDistance, distance);
            }
        }

        Vector3 desiredPosition = target.position + direction * adjustedDistance;
        transform.position = desiredPosition;
        transform.LookAt(target.position);
    }
    public void ResetTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            ForceCameraSnapToTarget();
        }
    }


    void LateUpdate()
    {
        if (target == null || cameraLocked) return;

        HandleRotation();

        if (Input.GetKeyDown(KeyCode.K))
        {
            isZoomedIn = !isZoomedIn;
            cameraComponent.fieldOfView = isZoomedIn ? zoomedFOV : normalFOV;
            distance = isZoomedIn ? zoomedDistance : 15f;
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.back;

        Vector3 rayOrigin = target.position + Vector3.up * 1.5f;
        float adjustedDistance = distance;

        if (enableCollision)
        {
            RaycastHit hit;
            if (Physics.SphereCast(rayOrigin, collisionRadius, direction, out hit, distance, collisionMask))
            {
                adjustedDistance = Mathf.Clamp(hit.distance, minDistance, distance);
            }
        }

        Vector3 desiredPosition = target.position + direction * adjustedDistance;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, Time.deltaTime * followSpeed);
        transform.LookAt(target.position);
    }


    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed;
            pitch -= mouseY * rotationSpeed;

            if (clampPitch)
            {
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }
        }
    }
    public void ForceAssignTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            transform.position = target.position + new Vector3(0, 2, -distance);
            transform.LookAt(target);
        }
    }

}

