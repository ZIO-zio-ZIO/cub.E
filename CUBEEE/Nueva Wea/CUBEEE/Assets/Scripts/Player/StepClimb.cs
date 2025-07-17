using UnityEngine;

public class StepClimb : MonoBehaviour
{
    [Header("Step Settings")]
    public float stepHeight = 0.3f;
    public float stepCheckDistance = 0.5f;
    public float stepSmooth = 0.05f;
    public LayerMask stairsLayer;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        TryClimbStep(Vector3.forward);
        TryClimbStep(Vector3.forward + Vector3.right * 0.25f);
        TryClimbStep(Vector3.forward + Vector3.left * 0.25f);
    }

    private void TryClimbStep(Vector3 direction)
    {
        Vector3 lowerRayStart = transform.position + Vector3.up * 0.05f;
        Vector3 upperRayStart = transform.position + Vector3.up * stepHeight;

        Vector3 rayDirection = transform.TransformDirection(direction).normalized;

        if (Physics.Raycast(lowerRayStart, rayDirection, stepCheckDistance, stairsLayer))
        {
            if (!Physics.Raycast(upperRayStart, rayDirection, stepCheckDistance, stairsLayer))
            {
                rb.position += Vector3.up * stepSmooth;
            }
        }
    }
}
