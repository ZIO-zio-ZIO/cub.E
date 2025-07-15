using UnityEngine;

public class BigCubePushPullController : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject player;
    public Rigidbody bigCubeRb;
    public Collider interactionTrigger;

    [Header("Configuración")]
    public float pushSpeed = 0.11f;
    public float pullSpeed = 0.9f;


    private bool isPlayerNearby = false;
    private bool isPushingOrPulling = false;

    private Transform playerTransform;
    private Rigidbody playerRb;

    
    public static bool isPushingCube = false;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        playerTransform = player.transform;
        playerRb = player.GetComponent<Rigidbody>();

        bigCubeRb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isPushingOrPulling)
            {
                StartPushPull();
            }
            else
            {
                StopPushPull();
            }
        }

        if (isPushingOrPulling)
        {
            HandlePushPullMovement();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNearby = false;
            if (isPushingOrPulling)
            {
                StopPushPull();
            }
        }
    }

    private void StartPushPull()
    {
        isPushingOrPulling = true;
        isPushingCube = true;

        transform.SetParent(playerTransform);

        playerRb.constraints = RigidbodyConstraints.FreezeRotation;
        bigCubeRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    private void StopPushPull()
    {
        isPushingOrPulling = false;
        isPushingCube = false;

        transform.SetParent(null);

        playerRb.constraints = RigidbodyConstraints.None;
        bigCubeRb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void HandlePushPullMovement()
    {
        float inputVertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(inputVertical) < 0.1f)
            return;

        float speed = inputVertical > 0 ? pushSpeed : pullSpeed;

        Vector3 forwardDir = playerTransform.forward;
        forwardDir.y = 0;
        forwardDir.Normalize();

        Vector3 move = forwardDir * inputVertical * speed * Time.deltaTime;
        playerTransform.position += move;
    }
}
