using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeMelt : MonoBehaviour
{
    [SerializeField] private float meltDelay = 2f;
    [SerializeField] private float meltDuration = 5f;
    [SerializeField] private float minVelocityToStartMelting = 0.1f;

    private Rigidbody rb;
    private bool hasStartedMelting = false;
    private float meltTimer = 0f;
    private Vector3 initialScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialScale = transform.localScale;
        rb.freezeRotation = true;
        rb.drag = 0f;
        rb.angularDrag = 0f;
    }

    void Update()
    {
        if (!hasStartedMelting && rb.velocity.magnitude >= minVelocityToStartMelting)
        {
            hasStartedMelting = true;
            meltTimer = meltDelay + meltDuration;
        }

        if (hasStartedMelting)
        {
            meltTimer -= Time.deltaTime;

            if (meltTimer <= meltDuration)
            {
                float t = Mathf.Clamp01(1f - (meltDuration - meltTimer) / meltDuration);
                transform.localScale = initialScale * t;
            }

            if (meltTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
