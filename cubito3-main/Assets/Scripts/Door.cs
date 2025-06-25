using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Door : MonoBehaviour
{
    [Header("Huecos necesarios")]
    [SerializeField] private List<MonoBehaviour> cubeHoles = new List<MonoBehaviour>();

    [Header("Cantidad mínima de cubos colocados para abrir")]
    [SerializeField] private int requiredOccupiedCount = 1;

    [Header("Puerta")]
    [SerializeField] private Transform door;
    [SerializeField] private Vector3 openOffset;
    [SerializeField] private float openSpeed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool opening = false;

    void Start()
    {
        closedPos = door.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        if (!opening && CountOccupiedHoles() >= requiredOccupiedCount)
        {
            opening = true;
        }

        if (opening)
        {
            door.position = Vector3.MoveTowards(door.position, openPos, openSpeed * Time.deltaTime);
        }
    }

    private int CountOccupiedHoles()
    {
        int count = 0;

        foreach (MonoBehaviour mb in cubeHoles)
        {
            if (mb == null) continue;

            MethodInfo method = mb.GetType().GetMethod("IsSlotOccupied");
            if (method != null)
            {
                object result = method.Invoke(mb, null);
                if (result is bool && (bool)result)
                {
                    count++;
                }
            }
            else
            {
                Debug.LogWarning($"{mb.name} no tiene un método IsSlotOccupied()");
            }
        }

        return count;
    }
}

