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
    [SerializeField] CameraShake cameraShake;

    bool isDead=false;

    [SerializeField] private Transicion _transicion;

    [SerializeField] private GameObject _antenaRenderer;
    [SerializeField] private Renderer _bodyRenderer;
    [SerializeField] private ParticleSystem _deathParticles;
    private ParticleSystem _alertParticles;
    private Animator _anim;
    private bool _canGetDmg = true;

    private void Start()
    {
        _playerLife = _maxPlayerLife;
        happyFace.SetActive(true);
        _alertParticles = GetComponentInChildren<ParticleSystem>();
        _anim = GetComponent<Animator>();
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
        if (_canGetDmg)
        {
            _canGetDmg = false;
           // _anim.SetTrigger("Dmg");
            _playerLife -= damage;
            lifeBar.UpdateLifeBar(_maxPlayerLife, _playerLife);
            StartCoroutine(cameraShake.Shake(.15f, .4f));

            FaceUpdate();

            if (_playerLife <= 0)
            {
                GetComponent<MovementPlayer>()._rb.isKinematic = true;
                GetComponent<MovementPlayer>()._moveSpeed = 0f;
                Debug.Log("Me mori");
                isDead = true;
                _transicion.Transition(isDead);
                _alertParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _antenaRenderer.SetActive(false);
                _bodyRenderer.enabled = false;
                _deathParticles.Play();
                StartCoroutine(ResetSceneCoroutine());

            }


            Invoke("Iframes", 1.5f);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void Iframes()
    {
        _anim.SetTrigger("NoDmg");
        _canGetDmg = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire") && _canGetDmg)
        {
            GetDamage(10f);
        }
    }
    private IEnumerator ResetSceneCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
