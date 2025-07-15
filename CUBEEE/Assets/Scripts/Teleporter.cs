using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Teleporter linkedTeleporter;
    [SerializeField] private float cooldownTime = 0.5f;

    private float cooldownTimer = 0f;

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!linkedTeleporter || cooldownTimer > 0f)
            return;

        Rigidbody rb = other.attachedRigidbody;
        if (!rb)
            return;

        linkedTeleporter.Teleport(other.gameObject);
        cooldownTimer = cooldownTime;
    }

    public void Teleport(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (!rb)
            return;

        obj.transform.position = transform.position + Vector3.up;
        rb.velocity = Vector3.zero;
        cooldownTimer = cooldownTime;
    }
}
