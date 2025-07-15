using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IconAnim : MonoBehaviour
{
    [SerializeField] GameObject icon;
    [SerializeField] GameObject text;
    Animator _anim;
    bool _overIcon = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        text.SetActive(false);
    }

    private void OnMouseOver()
    {
        _overIcon = true;
        _anim.SetBool("overIcon", _overIcon);
        text.SetActive(true);
    }
    private void OnMouseExit()
    {
        _overIcon =false;
        _anim.SetBool("overIcon", _overIcon);
        text.SetActive(false);
    }
}
