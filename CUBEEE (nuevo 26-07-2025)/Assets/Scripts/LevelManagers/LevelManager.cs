using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private ICubeSlot[] allSlots;

    [SerializeField] private string defeatSceneName = "Derrota";
    private int _currentLevel;
    private bool levelCompleted = false;

    private void Start()
    {
        allSlots = FindObjectsOfType<MonoBehaviour>(true)
                   .OfType<ICubeSlot>()
                   .ToArray();

        _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        Debug.Log($"Nivel actual jugado: {_currentLevel}");
    }

    public void CheckLevelCompletion()
    {
        if (levelCompleted) return;

        foreach (ICubeSlot slot in allSlots)
        {
            if (!slot.IsSlotOccupied())
            {
                Debug.Log("No se completó el nivel");
                SceneManager.LoadScene(defeatSceneName);
                return;
            }
        }

        levelCompleted = true;
        Debug.Log("¡Nivel completado!");

        // Validación: solo desbloquear si el nivel actual es el último desbloqueado
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (_currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", _currentLevel + 1);
            PlayerPrefs.Save();
            Debug.Log("Nuevo nivel desbloqueado: " + (_currentLevel + 1));
        }

        // Iniciar la secuencia de victoria
        VictoryManager victoryManager = FindObjectOfType<VictoryManager>();
        if (victoryManager != null)
        {
            victoryManager.StartVictorySequence();
        }
        else
        {
            Debug.LogWarning("VictoryManager no encontrado en la escena.");
        }
    }
}
