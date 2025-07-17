using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCubeHole : MonoBehaviour, ICubeSlot
{
    [SerializeField] private Transform cubeDestination;
    [SerializeField] private GameObject visualLight; 
    [SerializeField] private ParticleSystem particles; 

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

        // Apagar luz visual
        if (visualLight != null)
            visualLight.SetActive(false);

        // Apagar partículas
        if (particles != null)
            particles.Stop();

        // Registrar en el manager
        FindObjectOfType<LevelOrderManager>()?.RegisterCube(other.gameObject);
        Debug.Log("Normal cube colocado!");
    }

    public void ResetHole()
    {
        isOccupied = false;
        hasCube = false;

        // Reactivar efectos si querés que vuelvan a encenderse
        if (visualLight != null)
            visualLight.SetActive(true);

        if (particles != null)
            particles.Play();

        Debug.Log("NormalCubeHole reseteado.");
    }
}
