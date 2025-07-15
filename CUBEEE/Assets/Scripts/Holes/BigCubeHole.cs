using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCubeHole : MonoBehaviour, ICubeSlot
{
    [SerializeField] private Transform cubeDestination;
    [SerializeField] private bool _isLastCube = false;
    private bool isOccupied = false;

    public bool hasCube = false;

    public bool IsSlotOccupied() => hasCube;

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied || !other.CompareTag("BigCube")) return;

        BigCubeInteraction interaction = other.GetComponent<BigCubeInteraction>();
        if (interaction != null)
            interaction.ForceDetach();


        if (isOccupied || !other.CompareTag("BigCube")) return;

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
        Debug.Log("Big cube colocado!");

       if (_isLastCube)
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.CheckLevelCompletion();
            }

        }
    }

    public void ResetHole()
    {
        isOccupied = false;
        hasCube = false;
        Debug.Log("BigCubeHole reseteado.");
    }
}
