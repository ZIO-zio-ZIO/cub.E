using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMechanic : MonoBehaviour
{
    [Header("Ángulo de Visión (en grados)")]
    [SerializeField] float maxYaw = 60f;
    [SerializeField] float maxPitch = 30f;

    [Header("Referencia de la cámara")]
    [SerializeField] Transform cameraTransform;

    private Renderer _objectRenderer;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        _objectRenderer = GetComponent<Renderer>();

        if (_objectRenderer == null)
        {
            Debug.LogError("AngleBasedVisibility: No Renderer found on this object.");
        }
    }

    void Update()
    {
        Vector3 toObject = transform.position - cameraTransform.position;
        Vector3 forward = cameraTransform.forward;

        Vector3 toObjectFlat = Vector3.ProjectOnPlane(toObject, Vector3.up).normalized;
        Vector3 forwardFlat = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
        float yawAngle = Vector3.Angle(forwardFlat, toObjectFlat);

        Vector3 toObjectVert = Vector3.ProjectOnPlane(toObject, cameraTransform.right).normalized;
        Vector3 forwardVert = Vector3.ProjectOnPlane(forward, cameraTransform.right).normalized;
        float pitchAngle = Vector3.Angle(forwardVert, toObjectVert);

        bool inYawRange = yawAngle <= maxYaw / 2f;
        bool inPitchRange = pitchAngle <= maxPitch / 2f;

        _objectRenderer.enabled = inYawRange && inPitchRange;
    }

    void OnDrawGizmosSelected()
    {
        if (cameraTransform == null) return;

        Gizmos.color = Color.yellow;

        Vector3 origin = cameraTransform.position;
        Vector3 forward = cameraTransform.forward;

        Quaternion yawLeft = Quaternion.AngleAxis(-maxYaw / 2f, Vector3.up);
        Quaternion yawRight = Quaternion.AngleAxis(maxYaw / 2f, Vector3.up);

        Quaternion pitchUp = Quaternion.AngleAxis(-maxPitch / 2f, cameraTransform.right);
        Quaternion pitchDown = Quaternion.AngleAxis(maxPitch / 2f, cameraTransform.right);

        float gizmoLength = 5f;

        Vector3 dirLeft = yawLeft * forward;
        Vector3 dirRight = yawRight * forward;

        Vector3 dirUp = pitchUp * forward;
        Vector3 dirDown = pitchDown * forward;

        Gizmos.DrawRay(origin, dirLeft * gizmoLength);
        Gizmos.DrawRay(origin, dirRight * gizmoLength);

        Gizmos.DrawRay(origin, dirUp * gizmoLength);
        Gizmos.DrawRay(origin, dirDown * gizmoLength);
    }
}
