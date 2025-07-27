using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMechanic : MonoBehaviour
{
    [SerializeField] float maxYaw = 60f;
    [SerializeField] float maxPitch = 30f;
    [SerializeField] Transform cameraTransform;

    private Renderer _objectRenderer;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        _objectRenderer = GetComponent<Renderer>();
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

        if (_objectRenderer != null)
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
        Gizmos.DrawRay(origin, yawLeft * forward * gizmoLength);
        Gizmos.DrawRay(origin, yawRight * forward * gizmoLength);
        Gizmos.DrawRay(origin, pitchUp * forward * gizmoLength);
        Gizmos.DrawRay(origin, pitchDown * forward * gizmoLength);
    }
}
