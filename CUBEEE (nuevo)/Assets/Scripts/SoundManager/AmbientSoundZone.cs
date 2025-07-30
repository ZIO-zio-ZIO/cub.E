using UnityEngine;

public class AmbientSoundZone : MonoBehaviour
{
    [SerializeField] private AudioClip ambientClip;
    [SerializeField] private float volume = 0.5f;

    private AudioSource ambientSource;

    private void Awake()
    {
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.playOnAwake = false;
        ambientSource.loop = true;
        ambientSource.volume = volume;
        ambientSource.spatialBlend = 0f; // sonido 2D
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ambientSource.clip = ambientClip;
            ambientSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ambientSource.Stop();
        }
    }
}
