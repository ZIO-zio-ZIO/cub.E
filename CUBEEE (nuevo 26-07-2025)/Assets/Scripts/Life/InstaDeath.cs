using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstaDeath : CubePosition
{
    [SerializeField] PlayerHealth playerHealth;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            playerHealth.GetDamage(100f);
            Debug.Log("Toqué la zona roja");
        }

        else if (other.gameObject.layer == 31)
        {
 
            CubePosition cubePosition = other.gameObject.GetComponent<CubePosition>();

            if (cubePosition != null)
            {
                cubePosition.ResetPosition();
                Debug.Log("Cubo reseteado");
            }
            else
            {
                Debug.LogWarning("El cubo no tiene el script CubePosition adjunto.");
            }
        }
    }

  
}
