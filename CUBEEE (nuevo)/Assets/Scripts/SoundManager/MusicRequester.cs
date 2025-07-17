using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _clipToPlay;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(_clipToPlay);
    }
}
