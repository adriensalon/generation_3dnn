using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public class AppareilPhoto
{
    public Camera cameraMoinsX;
    public Camera cameraPlusX;
    public Camera cameraMoinsY;
    public Camera cameraPlusY;
    public Camera cameraMoinsZ;
    public Camera cameraPlusZ;

    public Texture2D texturePhoto;
    //public float tailleCameras = 1f;

    public void PrendreLes6PhotosPack(string url)
    {
        texturePhoto = new Texture2D(Screen.width, 6 * Screen.height);

        PrendreUnePhoto(cameraMoinsX, 0, texturePhoto);
        PrendreUnePhoto(cameraPlusX, 1, texturePhoto);
        PrendreUnePhoto(cameraMoinsY, 2, texturePhoto);
        PrendreUnePhoto(cameraPlusY, 3, texturePhoto);
        PrendreUnePhoto(cameraMoinsZ, 4, texturePhoto);
        PrendreUnePhoto(cameraPlusX, 5, texturePhoto);

        FichierJpg.Exporter(texturePhoto, url);

        return;
    }

    private void PrendreUnePhoto(Camera c, int i, Texture2D pack)
    {
        c.gameObject.SetActive(true);
        c.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        c.Render();
        RenderTexture rTex = c.activeTexture;
        c.targetTexture = null;
        RenderTexture.active = rTex;
        pack.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, i * Screen.height);
        pack.Apply();
        c.gameObject.SetActive(false);
        return;
    }
}