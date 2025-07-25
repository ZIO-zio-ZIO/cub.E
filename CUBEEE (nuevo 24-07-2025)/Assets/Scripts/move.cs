using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] Transform[] puntos;
    [SerializeField] float velocidadMin = 1f;
    [SerializeField] float velocidadMax = 5f;
    [SerializeField] float tolerancia = 0.1f;
    [SerializeField] Buttom boton;
    [SerializeField] bool requiereBoton = false;

    int _indiceActual = 0;
    bool _enReversa = false;
    Rigidbody rb;

    Vector3 lastPosition;
    Vector3 platformDelta;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (requiereBoton && (boton == null || !boton.IsPressed)) return;
        if (puntos.Length == 0) return;

        Transform objetivo = puntos[_indiceActual];

        float totalDistance = Vector3.Distance(puntos[0].position, puntos[puntos.Length - 1].position);
        float currentDistance = Vector3.Distance(puntos[0].position, transform.position);
        if (_enReversa)
            currentDistance = Vector3.Distance(puntos[puntos.Length - 1].position, transform.position);

        float progress = Mathf.Clamp01(currentDistance / totalDistance);
        float velocidadActual = Mathf.Lerp(velocidadMin, velocidadMax, Mathf.SmoothStep(0, 1, 1 - Mathf.Abs(progress - 0.5f) * 2));

        Vector3 direccion = (objetivo.position - transform.position).normalized;
        Vector3 nuevaPosicion = transform.position + direccion * velocidadActual * Time.fixedDeltaTime;

        rb.MovePosition(nuevaPosicion);

        platformDelta = nuevaPosicion - lastPosition;
        lastPosition = nuevaPosicion;

        if (Vector3.Distance(transform.position, objetivo.position) <= tolerancia)
        {
            if (!_enReversa)
            {
                _indiceActual++;
                if (_indiceActual >= puntos.Length)
                {
                    _indiceActual = puntos.Length - 2;
                    _enReversa = true;
                }
            }
            else
            {
                _indiceActual--;
                if (_indiceActual < 0)
                {
                    _indiceActual = 1;
                    _enReversa = false;
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.rigidbody;
            if (playerRb != null)
            {
                playerRb.MovePosition(playerRb.position + platformDelta);
            }
        }
    }
}

