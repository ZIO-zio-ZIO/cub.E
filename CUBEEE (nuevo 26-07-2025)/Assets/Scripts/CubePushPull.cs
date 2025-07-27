using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePushPull : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 holdOffset = new Vector3(0, 0, 1f);
    [SerializeField] private KeyCode grabKey = KeyCode.E;

    private Rigidbody rb;
    private bool isGrabbing = false;
    private bool isPlayerNear = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(grabKey))
        {
            if (!isGrabbing) Grab();
            else Release();
        }

        if (isGrabbing)
        {
            Vector3 targetPos = player.position + player.forward * holdOffset.z + player.right * holdOffset.x + Vector3.up * holdOffset.y;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
        }
    }

    private void Grab()
    {
        isGrabbing = true;
        transform.SetParent(player);
        rb.isKinematic = true;
    }

    private void Release()
    {
        isGrabbing = false;
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            isPlayerNear = false;
            if (isGrabbing) Release();
        }
    }
}
