using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : DoDamage
{
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingUp = true;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * moveDistance;
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            Vector3 from = movingUp ? startPos : targetPos;
            Vector3 to = movingUp ? targetPos : startPos;

            float elapsed = 0f;
            float duration = Vector3.Distance(from, to) / moveSpeed;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = to;
            movingUp = !movingUp;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
