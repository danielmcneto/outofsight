using UnityEngine;

[RequireComponent(typeof(Camera))]
public class UIMap : MonoBehaviour
{
    public Shader nightVisionShader;

    private Material material;

    void Start()
    {
        material = new Material(nightVisionShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    void OnDestroy()
    {
        if (material != null)
            Destroy(material);
    }
}