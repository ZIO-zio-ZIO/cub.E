using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSticky : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabRange = 2f;
    [SerializeField] private float dropRange = 2f;

    public bool _isHolding = false;
    public bool _isInside = false;
    private GameObject _currentBox = null;
    private float lastDropTime = -1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_isHolding && _isInside)
            {
                if (TryGrabBox())
                    Debug.Log("Intente agarrar");
            }
            else
            {
                if (TryDropBoxInSlot())
                    Debug.Log("Intente dropear");
            }
        }
    }

    private bool TryGrabBox()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("StickyCube"))
            {
                StickyConfirmation cubeScript = col.GetComponent<StickyConfirmation>();
                if (cubeScript != null && cubeScript.isPlaced) continue;

                _currentBox = col.gameObject;
                _currentBox.GetComponent<Rigidbody>().isKinematic = true;
                _currentBox.transform.SetParent(grabPoint);
                _currentBox.transform.localPosition = Vector3.zero;
                _isHolding = true;
                return true;
            }
        }

        return false; 
    }

    private bool TryDropBoxInSlot()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, dropRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("CubeSlot"))
            {
                StickyCubeHole slot = col.GetComponent<StickyCubeHole>();
                if (slot != null && !slot.IsSlotOccupied())
                {
                    _currentBox.transform.SetParent(null);
                    slot.PlaceCube(_currentBox);

                    StickyConfirmation cubeScript = _currentBox.GetComponent<StickyConfirmation>();
                    if (cubeScript != null)
                        cubeScript.isPlaced = true;

                    _currentBox = null;
                    _isHolding = false;
                    lastDropTime = Time.time;
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _isInside = true;
    }
    private void OnTriggerExit(Collider other)
    { 
        _isInside = false; 
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dropRange);
    }
}

