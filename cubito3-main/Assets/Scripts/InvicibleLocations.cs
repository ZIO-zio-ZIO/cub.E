using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvicibleLocations : MonoBehaviour
{
    [SerializeField] private LayerMask _invisibleRayMask;
    [SerializeField] private float _visionRadius = 4f;
    private Vector3 _posOffset = new(0, 1f, 0);
    private ParticleSystem _alertParticles;
    private bool _isNear = false;
    private bool _wasNear = false;

    private void Start()
    {
        _alertParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()

    {
        _isNear = IsNear();

        if (_isNear && !_wasNear)
        {
            Debug.Log("particulas si");
            _alertParticles.Play();
        }
        else if (!_isNear && _wasNear)
        {
            Debug.Log("particulas no");
            _alertParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        _wasNear = _isNear;
    }

    private bool IsNear()
    {
        Vector3 sphereCenter = transform.position + _posOffset;
        return Physics.CheckSphere(sphereCenter, _visionRadius, _invisibleRayMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isNear ? Color.green : Color.yellow;
        Vector3 sphereCenter = transform.position + _posOffset;
        Gizmos.DrawWireSphere(sphereCenter, _visionRadius);
    }
}

