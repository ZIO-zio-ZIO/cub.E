using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCubeHole : MonoBehaviour, ICubeSlot
{
    [SerializeField] private Transform playerSnapPosition; // <- nuevo campo
    private GameObject _currentBox = null;
    private LevelOrderManager levelOrderManager;
    private MovementPlayer playerMovement;

    private void Start()
    {
        levelOrderManager = FindObjectOfType<LevelOrderManager>();
        if (levelOrderManager == null)
            Debug.LogError("LevelOrderManager no encontrado en la escena.");

        playerMovement = FindObjectOfType<MovementPlayer>();
        if (playerMovement == null)
            Debug.LogError("MovementPlayer no encontrado en la escena.");
    }

    public bool IsSlotOccupied()
    {
        return _currentBox != null;
    }

    public void PlaceCube(GameObject cube)
    {
        if (_currentBox == null)
        {
            _currentBox = cube;

            // Posicionar y congelar cubo
            cube.transform.SetParent(transform);
            cube.transform.position = transform.position;
            cube.transform.rotation = transform.rotation;
            cube.GetComponent<Rigidbody>().isKinematic = true;
            cube.tag = "Untagged";
            Debug.Log("Cube placed in the slot.");

            // Mover al jugador al frente del hueco
            if (playerSnapPosition != null && playerMovement != null)
            {
                playerMovement.transform.position = playerSnapPosition.position;
                playerMovement.IsFrozen = true;
            }

            // Registrar cubo
            levelOrderManager?.RegisterCube(cube);

            // Verificar si se completa el nivel
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.CheckLevelCompletion();
            }
        }
    }

    public void RemoveCube()
    {
        if (_currentBox != null)
        {
            _currentBox.GetComponent<Rigidbody>().isKinematic = false;
            _currentBox = null;
            Debug.Log("Cube removed from the slot.");
        }
    }
}
