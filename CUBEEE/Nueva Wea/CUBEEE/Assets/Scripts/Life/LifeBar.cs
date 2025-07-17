using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    Image barLife;

    [SerializeField] Color Good;
    [SerializeField] Color Mid;
    [SerializeField] Color Bad;


    private void Start()
    {
        barLife = GetComponent<Image>();
        barLife.fillAmount = 1;
        barLife.color = Good;
    }

    public void UpdateLifeBar(float maxHealth, float health)
    {
        float lifePercent = health / maxHealth;

        barLife.fillAmount = lifePercent;

        ColorUpdate(health);
    }

    private void ColorUpdate(float health)
    {
        if (health >= 60)
        {
            barLife.color = Good;
        }
        else if (health < 60 && health > 30) 
        { 
            barLife.color = Mid;
        }
        else barLife.color = Bad;
    }
}
