using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomButterfly : MonoBehaviour
{
    Animator animator;
    float randomOffset;

    void Start()
    {
        animator = GetComponent<Animator>();
        randomOffset = Random.Range(0, 1);
        animator.Play("Armature|ButterflyFlight", 0, randomOffset);
    }
}
