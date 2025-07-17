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

}

