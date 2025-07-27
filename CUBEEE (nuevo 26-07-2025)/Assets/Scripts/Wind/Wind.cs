using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float windForce = 5f;
    [SerializeField] private Vector3 areaSize = new Vector3(5, 5, 5);
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
        boxCollider.size = areaSize;
        boxCollider.center = Vector3.zero;
    }

    void FixedUpdate()
    {
        foreach (var rb in rigidbodiesInZone)
        {
            if (rb == null) continue;

            Vector3 direction = isVertical ? transform.up : transform.forward;
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
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
#endif
}
