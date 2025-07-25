using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [Header("Playlist")]
    [SerializeField] private List<AudioClip> playlist = new List<AudioClip>();

    [Header("Settings")]
    [SerializeField] private float delayBetweenSongs = 2f;

    private AudioClip lastClip;

    private void Start()
    {
        if (playlist.Count == 0)
        {
            Debug.LogWarning("No hay canciones asignadas en la playlist.");
            return;
        }

        StartCoroutine(PlayMusicLoop());
    }

    private IEnumerator PlayMusicLoop()
    {
        while (true)
        {
            AudioClip nextClip = GetRandomClip();
            lastClip = nextClip;

            SoundManager.Instance.PlayMusic(nextClip);

            yield return new WaitForSeconds(nextClip.length + delayBetweenSongs);
        }
    }

    private AudioClip GetRandomClip()
    {
        if (playlist.Count == 1) return playlist[0];

        AudioClip clip;
        do
        {
            clip = playlist[Random.Range(0, playlist.Count)];
        } while (clip == lastClip); // evitar repetir la misma canción

        return clip;
    }
}