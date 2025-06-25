using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCubeHole : MonoBehaviour, ICubeSlot
{
    [SerializeField] private Transform cubeDestination;
    private bool isOccupied = false;

    public bool hasCube = false;

    public bool IsSlotOccupied() => hasCube;

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied || !other.CompareTag("Grabbable")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        other.transform.SetParent(null);
        rb.isKinematic = true;
        other.transform.position = cubeDestination.position;
        other.transform.rotation = cubeDestination.rotation;
        other.tag = "Untagged";

        isOccupied = true;
        hasCube = true;

        FindObjectOfType<LevelOrderManager>()?.RegisterCube(other.gameObject);
        Debug.Log("Normal cube colocado!");
    }

    public void ResetHole()
    {
        isOccupied = false;
        hasCube = false;
        Debug.Log("NormalCubeHole reseteado.");
    }
}
