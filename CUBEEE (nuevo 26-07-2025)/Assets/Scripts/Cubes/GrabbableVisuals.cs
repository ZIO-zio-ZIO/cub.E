using UnityEngine;

public class GrabbableVisuals : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material glowingMaterial;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>(); // por si el renderer está en un hijo

        if (_renderer != null && defaultMaterial != null)
            _renderer.material = defaultMaterial;
    }

    public void SetGlowing(bool state)
    {
        if (_renderer == null)
        {
            Debug.LogWarning($"Renderer not found on {gameObject.name}");
            return;
        }

        _renderer.material = state ? glowingMaterial : defaultMaterial;
        Debug.Log($"GrabbableVisuals {gameObject.name} → {(state ? "Glowing" : "Default")}");
    }
}
