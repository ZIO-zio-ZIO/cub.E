using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DoDamage : MonoBehaviour
{
    [SerializeField] float _dmg = 10f;
    [SerializeField] float intensity = 0f;
    PostProcessVolume _volume;
    Vignette _vignette;

    private void Start()
    {
        _volume = GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings<Vignette>(out _vignette);

        if (!_vignette)
        {
            print("error, vignite empty");
        }
        else
        {
            _vignette.enabled.Override(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().GetDamage(_dmg);
            StartCoroutine(TakeDamageEffect());
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        intensity = 0.4f;
        _vignette.enabled.Override(true);
        _vignette.intensity.Override(0.4f);

        yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.1f;

            if (intensity < 0) intensity = 0;
            _vignette.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);
        }

        _vignette.enabled.Override(false);
        yield break;
    }
}
