using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IceCube : MonoBehaviour
{
    public float slideSpeed = 5f;
    public LayerMask obstacleLayer;
    public float maxSlideDistance = 20f;

    private Vector3 targetPosition;
    private bool isSliding = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        if (isSliding)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, slideSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);

            if (Vector3.Distance(rb.position, targetPosition) < 0.01f)
            {
                isSliding = false;
                rb.MovePosition(targetPosition);
                //Ezio acuerdate que aca pogo efectos de sonido >:(
            }
        }
    }

    public void Slide(Vector3 direction)
    {
        if (isSliding) return;

        Vector3 origin = transform.position + Vector3.up * 0.1f; 
        RaycastHit hit;

        Vector3 halfExtents = transform.localScale / 2f;

        
        Debug.DrawRay(origin, direction * maxSlideDistance, Color.cyan, 1f);

        if (Physics.BoxCast(origin, halfExtents, direction, out hit, Quaternion.identity, maxSlideDistance, obstacleLayer))
        {
           
            targetPosition = hit.point - direction.normalized * halfExtents.magnitude;
        }
        else
        {
            targetPosition = transform.position + direction.normalized * maxSlideDistance;
        }

        isSliding = true;
    }
}


