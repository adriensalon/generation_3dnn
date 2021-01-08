using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class ElementRendu
{
    public Vector3 position;
    public Mesh mesh;
    public Material material;
}


public static class MoteurRendu
{
    public static Shader shaderTriplanaire;
    public static string nomTexture_xMoins;
    public static string nomTexture_xPlus;
    public static string nomTexture_yMoins;
    public static string nomTexture_yPlus;
    public static string nomTexture_zMoins;
    public static string nomTexture_zPlus;

    public static List<ElementRendu> elements;

    public static IntPtr[] CallbackNouvelElement() // A TESTER
    {
        ElementRendu e = new ElementRendu();
        e.mesh = new Mesh();
        e.material = new Material(shaderTriplanaire);

        IntPtr[] p = new IntPtr[8];
        p[0] = e.mesh.GetNativeVertexBufferPtr(0);
        p[1] = e.mesh.GetNativeIndexBufferPtr();
        p[2] = e.material.GetTexture(nomTexture_xMoins).GetNativeTexturePtr();
        p[3] = e.material.GetTexture(nomTexture_xPlus).GetNativeTexturePtr();
        p[4] = e.material.GetTexture(nomTexture_yMoins).GetNativeTexturePtr();
        p[5] = e.material.GetTexture(nomTexture_yPlus).GetNativeTexturePtr();
        p[6] = e.material.GetTexture(nomTexture_zMoins).GetNativeTexturePtr();
        p[7] = e.material.GetTexture(nomTexture_zPlus).GetNativeTexturePtr();

        elements.Add(e);
        return p;
    }
    public static void Rendu() // A TESTER
    {
        for (int i = 0; i < elements.Count; i++)
        {
            Graphics.DrawMesh(
                    elements[i].mesh,
                    elements[i].position,
                    Quaternion.identity,
                    elements[i].material,
                    0,
                    Camera.main,
                    0);
        }
    }
};
