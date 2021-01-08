using System.Collections.Generic;
using UnityEngine;

public class Geometrie_Old_EditerMeshParPointeur
{
    public List<Vector3> positions;
    public List<Vector3> normales;
    public List<Vector3Int> triangles = new List<Vector3Int>();
    public List<int> indices_vertices;
    public List<int> indices_triangles;
    public List<int> materiaux;

    public void Afficher(MeshFilter f, MeshRenderer r, bool recalculerNormales = true)
    {
        Mesh m = new Mesh();

        int nombreVertices = positions.Count;
        Vector3[] aPositions = new Vector3[nombreVertices];
        Vector3[] aNormales = new Vector3[nombreVertices];
        for (int i = 0; i < nombreVertices; i++)
        {
            aPositions[i] = positions[i];
            aNormales[i] = normales[i];
        }
        m.SetVertices(aPositions);
        m.SetNormals(aNormales);

        int nombreIndicesTriangles = indices_triangles.Count;
        for (int iSubmesh = 0; iSubmesh < nombreIndicesTriangles; iSubmesh++)
        {

            int indicePremierTriangle = indices_triangles[iSubmesh];
            int nombreTriangles = (iSubmesh == nombreIndicesTriangles - 1 ? triangles.Count : indices_triangles[iSubmesh + 1]) - indicePremierTriangle;
            int[] aTriangles = new int[3 * nombreTriangles];
            for (int iTriangle = 0; iTriangle < nombreTriangles; iTriangle++)
            {
                aTriangles[3 * iTriangle] = triangles[iTriangle + indicePremierTriangle].x;
                aTriangles[3 * iTriangle + 1] = triangles[iTriangle + indicePremierTriangle].y;
                aTriangles[3 * iTriangle + 2] = triangles[iTriangle + indicePremierTriangle].z;
            }
            m.SetTriangles(aTriangles, iSubmesh);
            //r.sharedMaterials[iSubmesh] = GeometrieProceduraleAPI.ObtenirMateriau(materiaux[iSubmesh]);
        }
        if (recalculerNormales)
        {
            m.RecalculateNormals();
        }
        f.sharedMesh = m;
        return;
    }
}