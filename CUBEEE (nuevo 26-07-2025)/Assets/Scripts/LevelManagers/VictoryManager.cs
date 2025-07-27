using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class VictoryManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup victoryTextGroup;
    [SerializeField] private Image whiteTransitionPanel;
    [SerializeField] private float whiteScreenHoldDuration = 3f;
    [Header("Victory Screen UI")]
    [SerializeField] private GameObject victoryUIPanel;
    [SerializeField] private float delayBeforeShowingVictoryUI = 2f;
    [Header("Player UI")]
    [SerializeField] private GameObject playerUIRoot;
    [SerializeField] private GameObject coinCounterUI;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject pauseButtonUI;

    [Header("Music")]
    [SerializeField] private AudioClip victoryMusicClip;
    [SerializeField] private float musicFadeOutDuration = 2f;

    [Header("Environment")]
    [SerializeField] private Material nightSkybox;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Color nightLightColor = new Color(0.3f, 0.4f, 0.6f);
    [SerializeField] private float nightLightIntensity = 0.3f;

    [Header("Effects")]
    [SerializeField] private GameObject firefliesPrefab;
    [SerializeField] private GameObject fireworksPrefab;

    [Header("Camera")]
    [SerializeField] private Transform finalCameraPosition;
    [SerializeField] private float cameraMoveDuration = 2f;

    [Header("Player")]
    [SerializeField] private GameObject player;

    private bool victoryStarted = false;
    public static bool VictoryInProgress { get; private set; } = false;

    public void StartVictorySequence()
    {
        MovementPlayer movement = player.GetComponent<MovementPlayer>();
        if (movement != null)
        {
            movement.SetMovementLocked(true);
        }

        if (victoryStarted) return;
        victoryStarted = true;
        VictoryInProgress = true;
        StartCoroutine(VictoryFlow());
    }

    private IEnumerator VictoryFlow()
    {
        if (playerUIRoot != null) playerUIRoot.SetActive(false);
        if (coinCounterUI != null) coinCounterUI.SetActive(false);
        if (healthBarUI != null) healthBarUI.SetActive(false);
        if (pauseButtonUI != null) pauseButtonUI.SetActive(false);

        Debug.Log("Iniciando VictoryFlow()");
        // Paso 1: Mostrar mensaje de victoria
        victoryTextGroup.alpha = 1;
        yield return new WaitForSeconds(2f);
        victoryTextGroup.alpha = 0;

        // Paso 2: Pantalla blanca aparece
        yield return StartCoroutine(WhiteTransition(true));

        // Detener música actual suavemente
        yield return StartCoroutine(FadeOutMusic(musicFadeOutDuration));

        // Nueva pausa mientras la pantalla está completamente blanca
        yield return new WaitForSeconds(whiteScreenHoldDuration);

        // Paso 3: Cambiar skybox e iluminación
        RenderSettings.skybox = nightSkybox;
        directionalLight.color = nightLightColor;
        directionalLight.intensity = nightLightIntensity;
        DynamicGI.UpdateEnvironment(); // Actualiza GI si está habilitado

        // Paso 4: Apagar jugador
        if (player != null) player.SetActive(false);

        // Paso 5: Mover cámara
        if (finalCameraPosition != null)
        {
            yield return StartCoroutine(MoveCamera(finalCameraPosition.position, finalCameraPosition.rotation));
        }

        // Paso 6: Activar efectos
        if (firefliesPrefab) Instantiate(firefliesPrefab, Vector3.zero, Quaternion.identity);
        if (fireworksPrefab) Instantiate(fireworksPrefab, Vector3.zero, Quaternion.identity);

        // Paso 7: Pantalla blanca se va
        yield return StartCoroutine(WhiteTransition(false));

        // Reproducir música de victoria
        PlayVictoryMusic();

        // Esperar antes de mostrar UI final
        yield return new WaitForSeconds(delayBeforeShowingVictoryUI);

        // Activar UI de victoria
        if (victoryUIPanel != null)
        {
            victoryUIPanel.SetActive(true);
        }

        VictoryInProgress = false;
    }

    private IEnumerator WhiteTransition(bool appear)
    {
        float duration = 1.2f;
        float elapsed = 0f;

        RectTransform rect = whiteTransitionPanel.rectTransform;

        Vector2 startPos, endPos;

        if (appear)
        {
            startPos = new Vector2(1011, 0);     // Fuera a la derecha
            endPos = new Vector2(-1017, 0);      // Cubre pantalla
        }
        else
        {
            startPos = new Vector2(-1017, 0);    // Ya tapando pantalla
            endPos = new Vector2(-3046, 0);      // Fuera a la izquierda
        }

        // Asegurar que empieza en la posición correcta
        rect.anchoredPosition = startPos;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsed / duration); // más suave
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = endPos;
    }


    private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        FreeCamera.cameraLocked = true;

        Transform cam = Camera.main.transform;
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        float elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            float t = elapsed / cameraMoveDuration;
            cam.position = Vector3.Lerp(startPos, targetPosition, t);
            cam.rotation = Quaternion.Slerp(startRot, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.position = targetPosition;
        cam.rotation = targetRotation;
    }
    private void PlayVictoryMusic()
    {
        if (SoundManager.Instance == null || victoryMusicClip == null) return;

        AudioSource musicSource = SoundManager.Instance.GetAudioSource();
        musicSource.clip = victoryMusicClip;
        musicSource.volume = 1f;
        musicSource.loop = true;
        musicSource.Play();
    }


    private IEnumerator FadeOutMusic(float duration)
    {
        AudioSource musicSource = SoundManager.Instance.GetAudioSource();
        if (musicSource == null) yield break;

        float startVolume = musicSource.volume;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
    }


}
