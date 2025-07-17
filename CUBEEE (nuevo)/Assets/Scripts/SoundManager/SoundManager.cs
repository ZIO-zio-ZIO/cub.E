using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        _source = GetComponent<AudioSource>();
    }


    private AudioSource _source;
    public AudioClip correctClip;
    public AudioClip incorrectClip;
    [SerializeField] private AudioMixer _mixer;


    [SerializeField] private string _masterFloatName = "MasterVolume";
    [Range(0.0f, 1.0f)][SerializeField] private float _initMasterVol = 0.75f;
    public float InitMasterVol { get { return _initMasterVol; } }

    [SerializeField] private string _musicFloatName = "MusicVolume";
    [Range(0.0f, 1.0f)][SerializeField] private float _initMusicVol = 0.5f;
    public float InitMusicVol { get { return _initMasterVol; } }


    [SerializeField] private string _sfxFloatName = "SfxVolume";
    [Range(0.0f, 1.0f)][SerializeField] private float _initSfxVol = 1.0f;
    public float InitSfxVol { get { return _initMasterVol; } }

    [SerializeField] private string _uiFloatName = "UiVolume";
    [Range(0.0f, 1.0f)][SerializeField] private float _initUiVol = 1.0f;
    public float InitUiVol { get { return _initMasterVol; } }

    private void Start()
    {

        SetMasterVolume(_initMasterVol);
        SetMusicVolume(_initMusicVol);
        SetSfxVolume(_initSfxVol);
        SetUiVolume(_initUiVol);
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0.0f) value = 0.0001f;
        _mixer.SetFloat(_masterFloatName, Mathf.Log10(value) * 20.0f);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0.0f) value = 0.0001f;
        _mixer.SetFloat(_musicFloatName, Mathf.Log10(value) * 20.0f);
    }

    public void SetSfxVolume(float value)
    {
        if (value <= 0.0f) value = 0.0001f;
        _mixer.SetFloat(_sfxFloatName, Mathf.Log10(value) * 20.0f);
    }

    public void SetUiVolume(float value)
    {
        if (value <= 0.0f) value = 0.0001f;
        _mixer.SetFloat(_uiFloatName, Mathf.Log10(value) * 20.0f);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_source.isPlaying) _source.Stop();

        _source.clip = clip;

        _source.Play();
    }


    public void PlayCorrectSound()
    {
        if (correctClip != null)
            _source.PlayOneShot(correctClip);
    }

    public void PlayIncorrectSound()
    {
        if (incorrectClip != null)
            _source.PlayOneShot(incorrectClip);
    }
}

