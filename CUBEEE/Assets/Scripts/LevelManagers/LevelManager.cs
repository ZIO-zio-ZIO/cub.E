using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private ICubeSlot[] allSlots;

    [SerializeField] private string defeatSceneName = "Derrota";
    [SerializeField] private string victorySceneName = "Victoria";
    private int _currentLevel;


    private void Start()
    {
        allSlots = FindObjectsOfType<MonoBehaviour>(true)
                   .OfType<ICubeSlot>()
                   .ToArray();
        _currentLevel = PlayerPrefs.GetInt("UnlockedLevel");
        Debug.Log(_currentLevel);
    }

    public void CheckLevelCompletion()
    {
        foreach (ICubeSlot slot in allSlots)
        {
            if (!slot.IsSlotOccupied())
            {
                Debug.Log("No se completo el nivel");
                SceneManager.LoadScene(defeatSceneName);
                return;
            }
        }

        Debug.Log("Se completo el nivel");
        PlayerPrefs.SetInt("UnlockedLevel", _currentLevel += 1);
        Debug.Log(_currentLevel);
        SceneManager.LoadScene(victorySceneName);
    }
}
