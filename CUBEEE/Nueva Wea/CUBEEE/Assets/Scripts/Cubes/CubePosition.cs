using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubePosition : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    private string originalTag;
    private Rigidbody rb;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        originalTag = gameObject.tag;
        rb = GetComponent<Rigidbody>();
    }

    public void ResetPosition()
    {
        if (rb) rb.isKinematic = true;

        transform.position = startPos;
        transform.rotation = startRot;
        gameObject.tag = originalTag;

        if (rb) rb.isKinematic = false;
    }
}
