using System.Collections;
using UnityEngine;
using System;

[Serializable]
public class Projecteur
{
    public Shader shader6planar;
    public string nomTextureMoinsX;
    public string nomTexturePlusX;
    public string nomTextureMoinsY;
    public string nomTexturePlusY;
    public string nomTextureMoinsZ;
    public string nomTexturePlusZ;

    public Material ProjeterPack(string url)
    {
        Material mat = new Material(shader6planar);
        Texture2D tex = FichierJpg.Importer(url);
        // decouper en 6
        //mat.SetTexture(nomTextureMoinsX, )
        return mat;
    }

}