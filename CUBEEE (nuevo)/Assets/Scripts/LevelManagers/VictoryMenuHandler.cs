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

        // Tambi�n podr�as limpiar otras cosas persistentes si las hubiera

        // Volver al men�
        SceneManager.LoadScene(menuSceneName);
    }
}
