using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlatform : MonoBehaviour
{
    private Rigidbody _rb;
    private Rigidbody rbPlataformaActual;
    private bool enPlataforma = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            rbPlataformaActual = collision.rigidbody;


            if (rbPlataformaActual != null)
            {
                enPlataforma = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            enPlataforma = false;
            rbPlataformaActual = null;
        }
    }

    void FixedUpdate()
    {
        if (enPlataforma && rbPlataformaActual != null)
        {

            Vector3 velPlataforma = rbPlataformaActual.velocity;

            _rb.velocity = new Vector3(velPlataforma.x, _rb.velocity.y, velPlataforma.z);
        }
    }
}
