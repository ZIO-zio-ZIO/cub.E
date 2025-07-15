using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraAnim : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Idle", 0, 0f);
    }

    public void GoingToGuide()
    {
        anim.SetTrigger("GoToGuide");
    }

    public void GoingToLevelSelector()
    {
        anim.SetTrigger("GoToLevels");
    }

    public void GoingToMenu()
    {
        anim.SetTrigger("GoToMenu");
    }
}
