using UnityEngine;

public class StickyCube : MonoBehaviour
{
    private Rigidbody rb;
    private bool isHeld = false;

    public bool IsHeld => isHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AttachToHand(Transform hand)
    {
        if (IsPlacedInHole) return; 

        isHeld = true;
        rb.isKinematic = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }


    public bool IsPlacedInHole { get; private set; } = false;

    public void PlaceInHole(Vector3 position)
    {
        isHeld = false;
        IsPlacedInHole = true;
        transform.SetParent(null);
        transform.position = position;
        rb.isKinematic = true;
    }


    public void Detach()
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
    }
}

