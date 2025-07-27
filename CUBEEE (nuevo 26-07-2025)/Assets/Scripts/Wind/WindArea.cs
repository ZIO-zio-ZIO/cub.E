using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    [SerializeField] private float windForce = 5f;
    [SerializeField] private Vector2 areaSize = new Vector2(5, 5);
    [SerializeField] private bool isVertical = false;
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private LayerMask blockingLayer;

    private BoxCollider boxCollider;
    private List<Rigidbody> rigidbodiesInZone = new List<Rigidbody>();

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        UpdateColliderSize();
    }

    void UpdateColliderSize()
    {
        if (isVertical)
            boxCollider.size = new Vector3(areaSize.x, areaSize.y, 1f);
        else
            boxCollider.size = new Vector3(areaSize.x, 1f, areaSize.y);

        boxCollider.center = Vector3.zero;
    }

    void FixedUpdate()
    {
        foreach (var rb in rigidbodiesInZone)
        {
            if (rb == null) continue;

            Vector3 direction = isVertical ? Vector3.up : Vector3.forward;
            Vector3 origin = transform.position;
            Vector3 target = rb.position;
            Vector3 dirToObject = (target - origin).normalized;
            float distance = Vector3.Distance(origin, target);

            if (!Physics.Raycast(origin, dirToObject, distance, blockingLayer))
                rb.AddForce(direction * windForce, ForceMode.Force);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & affectedLayers) != 0)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && !rigidbodiesInZone.Contains(rb))
                rigidbodiesInZone.Add(rb);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && rigidbodiesInZone.Contains(rb))
            rigidbodiesInZone.Remove(rb);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 size = isVertical ? new Vector3(areaSize.x, areaSize.y, 1f) : new Vector3(areaSize.x, 1f, areaSize.y);
        Gizmos.DrawWireCube(transform.position, size);
    }
#endif
}
