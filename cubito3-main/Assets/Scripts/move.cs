using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] Transform[] puntos;
    [SerializeField] float velocidad = 2f;
    [SerializeField] float tolerancia = 0.1f;
    [SerializeField] Buttom boton;
    [SerializeField] bool requiereBoton = false;

    int _indiceActual = 0;
    bool _enReversa = false;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (requiereBoton && (boton == null || !boton.IsPressed)) return;
        if (puntos.Length == 0) return;

        Transform objetivo = puntos[_indiceActual];
        Vector3 direccion = (objetivo.position - transform.position).normalized;
        Vector3 nuevaPosicion = transform.position + direccion * velocidad * Time.fixedDeltaTime;

        rb.MovePosition(nuevaPosicion);

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
}

