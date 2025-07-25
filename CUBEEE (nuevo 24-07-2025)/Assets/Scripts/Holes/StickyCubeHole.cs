using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCubeHole : MonoBehaviour, ICubeSlot
{
    private GameObject _currentBox = null;
    private LevelOrderManager levelOrderManager;

    private void Start()
    {
        levelOrderManager = FindObjectOfType<LevelOrderManager>();
        if (levelOrderManager == null)
            Debug.LogError("LevelOrderManager no encontrado en la escena.");
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
            cube.transform.SetParent(transform);
            cube.transform.position = transform.position;
            cube.transform.rotation = transform.rotation;
            cube.GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("Cube placed in the slot.");
            cube.tag = "Untagged";

            levelOrderManager?.RegisterCube(cube);

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
