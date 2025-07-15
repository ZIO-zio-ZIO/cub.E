using UnityEngine;

public class IceCubeTrigger : MonoBehaviour
{
    public Vector3 slideDirection; 
    private IceCube parentCube;
    private bool playerInTrigger = false;

    private void Start()
    {
        parentCube = GetComponentInParent<IceCube>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            parentCube.Slide(slideDirection);
        }
    }
}

