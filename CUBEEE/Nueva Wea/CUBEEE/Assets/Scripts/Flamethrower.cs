using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [Header("Prefab del fuego")]
    [SerializeField] private GameObject firePrefab;

    [Header("Punto de aparición del fuego")]
    [SerializeField] private Transform fireSpawnPoint;

    [Header("Configuración")]
    [SerializeField] private float fireInterval = 3f; // Tiempo entre fuegos
    [SerializeField] private float fireDuration = 1.5f; // Cuánto dura cada fuego

    private void Start()
    {
        InvokeRepeating(nameof(SpawnFire), 0f, fireInterval);
    }

    private void SpawnFire()
    {
        GameObject fire = Instantiate(firePrefab, fireSpawnPoint.position, fireSpawnPoint.rotation);
        Destroy(fire, fireDuration);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.GetDamage(10f); // o el valor que quieras
            }
        }
    }

}
