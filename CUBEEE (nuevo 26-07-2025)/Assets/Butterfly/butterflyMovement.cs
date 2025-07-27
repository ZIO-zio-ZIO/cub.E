using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterflyMovement : MonoBehaviour
{
    [SerializeField] float rotSpeed;
    [SerializeField] float zMoveSpeed;
    [SerializeField] float yMoveSpeed;

    float randomZspeed;

    void Start() 
    {
        randomZspeed = Random.Range(0.2f, 3.5f);
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        transform.Translate(0, yMoveSpeed, zMoveSpeed * randomZspeed);
    }
}
