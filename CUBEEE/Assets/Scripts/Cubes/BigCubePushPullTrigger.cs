using UnityEngine;

public class BigCubePushPullTrigger : MonoBehaviour
{
    public bool isPlayerTouching = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerTouching = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerTouching = false;
    }
}
