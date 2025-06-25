using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioClip correctClip;
    public AudioClip incorrectClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlayCorrectSound()
    {
        if (correctClip != null)
            audioSource.PlayOneShot(correctClip);
    }

    public void PlayIncorrectSound()
    {
        if (incorrectClip != null)
            audioSource.PlayOneShot(incorrectClip);
    }
}

