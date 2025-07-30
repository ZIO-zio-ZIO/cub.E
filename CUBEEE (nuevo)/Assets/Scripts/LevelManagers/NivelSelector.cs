using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NivelSelector : MonoBehaviour
{
    [SerializeField] GameObject nivelButtomPrefab;
    [SerializeField] Transform buttonContainer;
    [SerializeField] int totalLevels = 5;

    private void Start()
    {
        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < totalLevels; i++)
        {
            GameObject buttonObj = Instantiate(nivelButtomPrefab, buttonContainer);
            int levelIndex = i + 1;

            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI levelText = buttonObj.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
            GameObject lockIcon = buttonObj.transform.Find("LockIcon").gameObject;
            GameObject warningText = buttonObj.transform.Find("WarningText").gameObject;

            levelText.text = "Level " + levelIndex;

            bool isUnlocked = levelIndex <= unlockedLevel;

            lockIcon.SetActive(!isUnlocked);
            warningText.SetActive(false);

            button.onClick.AddListener(() =>
            {
                if (isUnlocked)
                {
                    PlayerPrefs.SetInt("CurrentLevel" , levelIndex);
                    Debug.Log(levelIndex);
                    SceneManager.LoadScene("Level_" + levelIndex);
                }
                else
                {
                    StartCoroutine(ShowWarning(warningText));
                }
            });
        }
    }

    IEnumerator ShowWarning(GameObject warningText)
    {
        warningText.SetActive(true);
        yield return new WaitForSeconds(2.5f); 
        warningText.SetActive(false);
    }
}
