using UnityEngine;

public class GroundFootstepDetector : MonoBehaviour
{
    private string currentSurfaceTag = "Default";
    private bool wasGrounded = false;
    private bool isGrounded = false;
    private bool isMoving = false;
    private float stepTimer = 0f;
    public float stepInterval = 0.4f; // Tiempo entre pasos

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        isMoving = rb != null && new Vector2(rb.velocity.x, rb.velocity.z).magnitude > 0.2f;


        if (isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayerAudioManager.Instance.PlayFootstepSound(currentSurfaceTag);
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Grass") || other.CompareTag("Floor"))
        {
            currentSurfaceTag = other.tag;
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(currentSurfaceTag))
        {
            isGrounded = false;
        }
    }
}
