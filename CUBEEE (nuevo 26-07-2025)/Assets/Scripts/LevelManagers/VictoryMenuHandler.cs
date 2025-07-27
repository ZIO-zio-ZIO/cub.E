using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenuHandler : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MainMenu";

    public void ReturnToMainMenu()
    {
        // Limpieza opcional
        FreeCamera.cameraLocked = false;

        // Asegurate de eliminar el VictoryManager si persiste
        VictoryManager existing = FindObjectOfType<VictoryManager>();
        if (existing != null)
        {
            Destroy(existing.gameObject);
        }

        // También podrías limpiar otras cosas persistentes si las hubiera

        // Volver al menú
        SceneManager.LoadScene(menuSceneName);
    }
}
