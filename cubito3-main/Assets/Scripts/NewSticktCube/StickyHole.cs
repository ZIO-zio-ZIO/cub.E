using UnityEngine;

public class StickyHole : MonoBehaviour
{
    private PlayerStickyGrab playerGrab;
    private MovementPlayer playerMovement;
    private StickyCube cubeInHands;

    private bool playerInside = false;

    private void Start()
    {
        playerGrab = FindObjectOfType<PlayerStickyGrab>();
        playerMovement = FindObjectOfType<MovementPlayer>();
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (playerGrab != null && playerGrab.IsHoldingStickyCube())
            {
                cubeInHands = playerGrab.GetHeldCube();
                if (cubeInHands != null)
                {
                    
                    cubeInHands.PlaceInHole(transform.position);
                    playerGrab.DropStickyCube();
                    playerMovement.IsFrozen = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}


