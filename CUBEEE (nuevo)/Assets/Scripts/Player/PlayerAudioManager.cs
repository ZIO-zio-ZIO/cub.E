using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource interactionSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip grabClip;
    [SerializeField] private AudioClip dropClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip stickyGrabClip;
    [SerializeField] private AudioClip pushPullClip;
    [SerializeField] private AudioSource loopSource;
    [SerializeField] private AudioClip stuckClip;
    private bool hasPlayedStuckSound = false;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioClip grassFootstepClip;
    [SerializeField] private AudioClip floorFootstepClip;


    [SerializeField] private float stepInterval = 0.5f; // tiempo entre pasos
    private float stepTimer = 0f;

    [SerializeField] private Transform groundCheck; // punto desde donde lanzar el raycast
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask groundMask;
    private bool hasJustStartedWalking = false;

    [Header("Landing Sounds")]

    [SerializeField] private AudioClip landingFloorClip;
    [SerializeField] private AudioClip landingGrassClip;

    public static PlayerAudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void PlayJumpSound()
    {
        if (interactionSource && jumpClip)
            interactionSource.PlayOneShot(jumpClip);
    }

    public void PlayGrabSound()
    {
        if (interactionSource && grabClip)
            interactionSource.PlayOneShot(grabClip);
    }

    public void PlayDropSound()
    {
        if (interactionSource && dropClip)
            interactionSource.PlayOneShot(dropClip);
    }

    public void PlayCoinSound()
    {
        if (interactionSource && coinClip)
            interactionSource.PlayOneShot(coinClip);
    }

    public void PlayStickyGrabSound()
    {
        if (interactionSource && stickyGrabClip)
            interactionSource.PlayOneShot(stickyGrabClip);
    }

    public void PlayStuckSound()
    {
        if (interactionSource && stuckClip && !hasPlayedStuckSound)
        {
            interactionSource.PlayOneShot(stuckClip);
            hasPlayedStuckSound = true;
        }
    }

    public void ResetStuckSound()
    {
        hasPlayedStuckSound = false;
    }

    public void PlayPushPullLoop()
    {
        if (loopSource && pushPullClip && !loopSource.isPlaying)
        {
            loopSource.clip = pushPullClip;
            loopSource.loop = true;
            loopSource.Play();
        }
    }

    public void StopPushPullLoop()
    {
        if (loopSource && loopSource.isPlaying)
        {
            loopSource.Stop();
        }
    }
    /*/
    public void HandleFootstepSound(bool isMoving, bool isGrounded)
    {
        // Detectar presionado de tecla de movimiento (WASD o flechas)
        bool movementKeyPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                                   Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
                                   Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                                   Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow);

        if (movementKeyPressed && isGrounded && !hasJustStartedWalking)
        {
            PlayFootstepSound();
            stepTimer = stepInterval;
            hasJustStartedWalking = true;
        }

        if (isMoving && isGrounded)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = stepInterval;
            }
        }

        // Reset si el jugador deja de moverse
        if (!isMoving)
        {
            hasJustStartedWalking = false;
        }
    }
    /*/

    public void PlayFootstepSound(string surfaceTag)
    {
        AudioClip clipToPlay = GetFootstepClip(surfaceTag);
        if (interactionSource && clipToPlay)
            interactionSource.PlayOneShot(clipToPlay);
    }
    private AudioClip GetFootstepClip(string surfaceTag)
    {
        switch (surfaceTag)
        {
            case "Grass": return grassFootstepClip;
            case "Floor": return floorFootstepClip;
            default: return floorFootstepClip;
        }
    }
    public void PlayLandingSound(string surfaceTag)
    {
        AudioClip clipToPlay = null;

        switch (surfaceTag)
        {
            case "Floor":
                clipToPlay = landingFloorClip;
                break;
            case "Grass":
                clipToPlay = landingGrassClip;
                break;
        }

        if (clipToPlay != null && interactionSource)
        {
            interactionSource.PlayOneShot(clipToPlay);
        }
    }

    /*/
    private void PlayFootstepSound()
    {
        RaycastHit hit;

        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red, 0.1f);

        if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundCheckDistance, groundMask))
        {
            string surfaceTag = hit.collider.tag;

            if (surfaceTag == "Grass")
            {
                AudioClip clip = GetRandomClip(grassStepClips);
                if (clip != null) interactionSource.PlayOneShot(clip);
            }
            else if (surfaceTag == "Floor")
            {
                AudioClip clip = GetRandomClip(floorStepClips);
                if (clip != null) interactionSource.PlayOneShot(clip);
            }
            else
            {
                // fallback: usar floor si no se reconoce el tag
                AudioClip clip = GetRandomClip(floorStepClips);
                if (clip != null) interactionSource.PlayOneShot(clip);
            }
        }
    }
    /*/

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }
}
