using UnityEngine;

public class CubeGlowEffect : MonoBehaviour
{
    [SerializeField] private Renderer cubeRenderer;
    [SerializeField] private Color glowColor = Color.yellow;
    [SerializeField] private float intensity = 1f;

    private MaterialPropertyBlock propBlock;

    private void Awake()
    {
        if (cubeRenderer == null)
            cubeRenderer = GetComponentInChildren<Renderer>();

        propBlock = new MaterialPropertyBlock();
    }

    public void EnableGlow()
    {
        cubeRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissionColor", glowColor * intensity);
        cubeRenderer.SetPropertyBlock(propBlock);
        DynamicGI.SetEmissive(cubeRenderer, glowColor * intensity);
    }

    public void DisableGlow()
    {
        cubeRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissionColor", Color.black);
        cubeRenderer.SetPropertyBlock(propBlock);
        DynamicGI.SetEmissive(cubeRenderer, Color.black);
    }
}
