using UnityEngine;

public static class FichierJpg
{
    public static void Exporter(Texture2D texture, string url)
    {
        var bytes = texture.EncodeToJPG();
        System.IO.File.WriteAllBytes(url, bytes);
        return;
    }

    public static Texture2D Importer(string url)
    {
        var bytes = System.IO.File.ReadAllBytes(url);
        var texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        return texture;
    }
}