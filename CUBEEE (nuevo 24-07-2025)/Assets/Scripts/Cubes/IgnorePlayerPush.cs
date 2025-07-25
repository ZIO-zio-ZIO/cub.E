using UnityEngine;

public class IgnorePlayerPush : MonoBehaviour
{
    private void Start()
    {
        Collider[] myColliders = GetComponentsInChildren<Collider>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Collider[] playerColliders = player.GetComponentsInChildren<Collider>();

            foreach (var myCol in myColliders)
            {
                foreach (var playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(myCol, playerCol, true);
                }
            }
        }
    }
}
