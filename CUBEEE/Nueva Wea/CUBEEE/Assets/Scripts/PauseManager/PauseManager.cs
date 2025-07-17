using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject PauseBttm;
    bool _gamePaused = false;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_gamePaused)
            {
                Reanudar();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Reanudar()
    {
        PauseMenu.SetActive(false);
        PauseBttm.SetActive(true);
        Time.timeScale = 1;
        _gamePaused = false;
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        PauseBttm.SetActive(false);
        Time.timeScale = 0;
        _gamePaused = true;
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
