using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAction : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _uiSlider;

    private void Start()
    {
        _masterSlider.value = SoundManager.Instance.InitMasterVol;
        _musicSlider.value = SoundManager.Instance.InitMusicVol;
        _sfxSlider.value = SoundManager.Instance.InitSfxVol;
        _uiSlider.value = SoundManager.Instance.InitUiVol;
    }

    public void SetMasterVolume(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }
    public void SetMusicVolume(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }
    public void SetSfxVolume(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }
    public void SetUiVolume(float value)
    {
        SoundManager.Instance.SetUiVolume(value);
    }
}
