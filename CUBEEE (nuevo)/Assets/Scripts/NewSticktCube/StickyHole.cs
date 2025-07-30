using UnityEngine;

public class StickyHole : MonoBehaviour, ICubeSlot
{
    private PlayerStickyGrab playerGrab;
    private MovementPlayer playerMovement;
    private StickyCube cubeInHands;

    [SerializeField] private GameObject visualLight;
    [SerializeField] private ParticleSystem particles;

    [SerializeField] private Transform stickySnapPosition; // NUEVO: punto al que se mueve el jugador

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

                    Rigidbody playerRb = playerMovement.GetComponent<Rigidbody>();
                    if (playerRb != null)
                    {
                        playerRb.velocity = Vector3.zero;
                        playerRb.angularVelocity = Vector3.zero;
                        playerRb.isKinematic = true; // ¡esto lo congela físicamente!
                    }

                    // Mover al jugador a la posición de snap (si está asignada)
                    if (stickySnapPosition != null)
                    {
                        playerMovement.transform.position = stickySnapPosition.position;
                        playerMovement.transform.rotation = stickySnapPosition.rotation;
                    }

                    hasCube = true;

                    // Registrar en LevelOrderManager
                    LevelOrderManager lom = FindObjectOfType<LevelOrderManager>();
                    if (lom != null)
                        lom.RegisterCube(cubeInHands.gameObject);

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

    public void ResetHole()
    {
        hasCube = false;
        playerMovement.IsFrozen = false;

        Rigidbody playerRb = playerMovement.GetComponent<Rigidbody>();
        if (playerRb != null)
            playerRb.isKinematic = false;

        Debug.Log("StickyHole reseteado.");
    }

}
