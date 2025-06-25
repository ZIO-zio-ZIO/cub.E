using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelOrderManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cubesInOrder = new List<GameObject>();
    private int currentIndex = 0;

    [SerializeField] private string defeatSceneName = "Defeat Screen";

    public void RegisterCube(GameObject placedCube)
    {
        if (currentIndex >= cubesInOrder.Count)
            return;

        GameObject expectedCube = cubesInOrder[currentIndex];

        if (placedCube == expectedCube)
        {
            Debug.Log($"Cubo correcto colocado en orden: {placedCube.name}");
            currentIndex++;

            
            SoundManager.Instance?.PlayCorrectSound();
        }
        else
        {
            if (placedCube.CompareTag("StickyCube"))
            {
                Debug.LogWarning("❌ StickyCube colocado antes de tiempo. GAME OVER");

                
                SoundManager.Instance?.PlayIncorrectSound();

                SceneManager.LoadScene(defeatSceneName);
                return;
            }

            CubePosition resetScript = placedCube.GetComponent<CubePosition>();
            if (resetScript != null)
            {
                Debug.LogWarning("❌ Cubo colocado fuera de orden. Se resetea.");

                
                SoundManager.Instance?.PlayIncorrectSound();

                resetScript.ResetPosition();
            }

            
            ICubeSlot slot = FindSlotForCube(placedCube);
            if (slot != null)
            {
                if (slot is NormalCubeHole normalHole)
                {
                    normalHole.ResetHole();
                }
                else if (slot is BigCubeHole bigHole)
                {
                    bigHole.ResetHole();
                }
               
            }
        }
    }


    private ICubeSlot FindSlotForCube(GameObject cube)
    {
        ICubeSlot[] allSlots = FindObjectsOfType<MonoBehaviour>(true).OfType<ICubeSlot>().ToArray();

        foreach (var slot in allSlots)
        {
            if (slot is NormalCubeHole normalHole && cube.CompareTag("Grabbable"))
            {
                if (normalHole.IsSlotOccupied())
                    return normalHole;
            }
            else if (slot is BigCubeHole bigHole && cube.CompareTag("BigCube"))
            {
                if (bigHole.IsSlotOccupied())
                    return bigHole;
            }
        }
        return null;
    }
}
