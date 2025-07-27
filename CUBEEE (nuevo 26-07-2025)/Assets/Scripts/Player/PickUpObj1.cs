using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : MonoBehaviour
{
    [SerializeField] Transform grabPoint;
    [SerializeField] float grabRange = 2f;
    GameObject _currentBox;
    bool _isHolding = false;
    Collider _playerCollider;
    private Animator _currentAnimator;


    private void Start()
    {
        _playerCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isHolding)
            {
                DropBox();
            }
            else
            {
                TryGrabBox();
            }
        }
    }

    private void TryGrabBox()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Grabbable"))
            {
                _currentBox = col.gameObject;
                _currentBox.GetComponent<Rigidbody>().isKinematic = true;
                _currentBox.transform.SetParent(grabPoint);
                _currentBox.transform.localPosition = Vector3.zero;
                _isHolding = true;

                GrabbableVisuals visuals = _currentBox.GetComponent<GrabbableVisuals>();
                if (visuals == null)
                    visuals = _currentBox.GetComponentInChildren<GrabbableVisuals>();

                if (visuals != null)
                    visuals.SetGlowing(true); // Aquí activamos el brillo

                PlayerAudioManager.Instance.PlayGrabSound();
            }
        }
    }

    private void DropBox()
    {
        if (_currentBox != null)
        {
            GrabbableVisuals visuals = _currentBox.GetComponent<GrabbableVisuals>();
            if (visuals == null)
                visuals = _currentBox.GetComponentInChildren<GrabbableVisuals>();

            if (visuals != null)
                visuals.SetGlowing(false); // Aquí lo apagamos

            _currentBox.transform.SetParent(null);
            Rigidbody rb = _currentBox.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            _currentBox = null;
            _isHolding = false;

            PlayerAudioManager.Instance.PlayDropSound();
        }
    }

}
