using UnityEngine;

public class StickyHole : MonoBehaviour, ICubeSlot
{
    private PlayerStickyGrab playerGrab;
    private MovementPlayer playerMovement;
    private StickyCube cubeInHands;
    [SerializeField] private GameObject visualLight;
    [SerializeField] private ParticleSystem particles;

    public bool hasCube = false;
    private bool playerInside = false;

    public bool IsSlotOccupied() => hasCube;

    private void Start()
    {
        playerGrab = FindObjectOfType<PlayerStickyGrab>();
        playerMovement = FindObjectOfType<MovementPlayer>();
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (playerGrab != null && playerGrab.IsHoldingStickyCube() && !hasCube)
            {
                cubeInHands = playerGrab.GetHeldCube();
                if (cubeInHands != null)
                {
                    // Apagar luz visual
                    if (visualLight != null)
                        visualLight.SetActive(false);

                    // Apagar partículas
                    if (particles != null)
                        particles.Stop();

                    // Colocar cubo en el hueco
                    cubeInHands.PlaceInHole(transform.position);
                    playerGrab.DropStickyCube();

                    // Congelar al jugador
                    playerMovement.IsFrozen = true;

                    hasCube = true;

                    // Registrar el cubo colocado en orden
                    LevelOrderManager lom = FindObjectOfType<LevelOrderManager>();
                    if (lom != null)
                    {
                        lom.RegisterCube(cubeInHands.gameObject);
                    }

                    Debug.Log("Sticky cube colocado en su hueco.");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    // Método para resetear estado si se necesita
    public void ResetHole()
    {
        hasCube = false;
        playerMovement.IsFrozen = false;  // Descongelar jugador si aplica
        Debug.Log("StickyHole reseteado.");
    }
}
