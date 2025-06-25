using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] MenuCameraAnim camerAnim;
    
    public void StartGame()
    {
        camerAnim.GetComponent<MenuCameraAnim>().GoingToLevelSelector();

    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToGuide()
    {
        camerAnim.GetComponent<MenuCameraAnim>().GoingToGuide();
    }
    public void GoToMenu()
    {
        camerAnim.GetComponent<MenuCameraAnim>().GoingToMenu();
    }

    public void GoBackMenu()
    {
        SceneManager.LoadScene(0);
    } 
    
    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level_" + PlayerPrefs.GetInt("CurrentLevel"));
    }
}
