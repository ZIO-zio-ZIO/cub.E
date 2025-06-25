using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] LifeBar lifeBar;
    [SerializeField] public float _playerLife;
    [SerializeField] float _maxPlayerLife;
    [SerializeField] GameObject happyFace;
    [SerializeField] GameObject neutralFace;
    [SerializeField] GameObject deadFace;

    [SerializeField] private GameObject _antenaRenderer;
    [SerializeField] private Renderer _bodyRenderer;
    [SerializeField] private ParticleSystem _deathParticles;
    private ParticleSystem _alertParticles;

    private void Start()
    {
        _playerLife = _maxPlayerLife;
        happyFace.SetActive(true);
        _alertParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GetDamage(4);
        }
    }
    public void GetDamage(float damage)
    {
        _playerLife -= damage;
        lifeBar.UpdateLifeBar(_maxPlayerLife, _playerLife);

        FaceUpdate();

        if (_playerLife <= 0)
        {
            GetComponent<MovementPlayer>()._moveSpeed = 0f;
            _alertParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _antenaRenderer.SetActive(false);
            _bodyRenderer.enabled = false;
            _deathParticles.Play();
            Invoke("ResetScene", 0.7f);
        }
    }

    private void FaceUpdate()
    {
        if (_playerLife >= 60)
        {
            happyFace.SetActive(true);
            neutralFace.SetActive(false);
            deadFace.SetActive(false);
        }
        else if (_playerLife < 60 && _playerLife > 30)
        {
            happyFace.SetActive(false);
            neutralFace.SetActive(true);
            deadFace.SetActive(false);
        }
        else
        {
            happyFace.SetActive(false);
            neutralFace.SetActive(false);
            deadFace.SetActive(true);
        }
    }

    private void ResetScene()
    {
        Destroy(gameObject, 0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
