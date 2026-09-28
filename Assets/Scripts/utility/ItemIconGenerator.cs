using System.IO;
using UnityEngine;

public class ItemIconGenerator : MonoBehaviour
{
    public Camera iconCamera;
    public int iconWidth = 256;
    public int iconHeight = 256;

    [ContextMenu("Take Item Screenshot")]
    public void CaptureIcon()
    {
        RenderTexture rt = new RenderTexture(iconWidth, iconHeight, 24);
        iconCamera.targetTexture = rt;

        Texture2D screenShot = new Texture2D(iconWidth, iconHeight, TextureFormat.RGBA32, false);
        
        iconCamera.Render();
        RenderTexture.active = rt;

        screenShot.ReadPixels(new Rect(0, 0, iconWidth, iconHeight), 0, 0);
        screenShot.Apply();

        // Cleans up memory
        iconCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        // Saves image to Assets folder
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Application.dataPath + "/ItemIcon.png";
        File.WriteAllBytes(filename, bytes);

        Debug.Log($"Icon saved to: {filename}");
    }
}