using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class geometrie_procedurale
{
    public static void CreerProjet(
        string urlDossier)
    {
        IntPtr ptr_url = Marshal.StringToHGlobalAnsi(urlDossier);
        unity_creer_projet(ptr_url, cb_log);
        return;
    }
    public static void ChargerProjet(
        string urlDossier)
    {
        IntPtr ptr_url = Marshal.StringToHGlobalAnsi(urlDossier);
        unity_charger_projet(ptr_url, cb_log);
        return;
    }
    public static void ObtenirNomsCategories(
        Action<List<string>> callback)
    {
        action_liste_string = callback;

        unity_obtenir_noms_categories(cb_liste_string);

        return;
    }
    public static void ObtenirNomsScripts(
        Action<List<string>> callback)
    {
        action_liste_string = callback;

        unity_obtenir_noms_scripts(cb_liste_string);

        return;
    }
    public static void ObtenirNomsGeometries(
        string nom_categorie,
        Action<List<string>> callback)
    {
        action_liste_string = callback;
        IntPtr ptr_nom_categorie = Marshal.StringToHGlobalAnsi(nom_categorie);

        unity_obtenir_noms_geometries(
            ptr_nom_categorie,
            cb_liste_string);

        return;
    }
    public static void ObtenirNomsIas(
        string nom_categorie,
        Action<List<string>> callback)
    {
        action_liste_string = callback;
        IntPtr ptr_nom_categorie = Marshal.StringToHGlobalAnsi(nom_categorie);

        unity_obtenir_noms_ias(
            ptr_nom_categorie,
            cb_liste_string);

        return;
    }






    public static void CalculerGeometrie(
        Mesh mesh,
        Vector3 taille,
        Vector3 ajustementX)
    {
        Vector3[] positions = mesh.vertices;
        Vector3[] normales = mesh.normals;
        Vector2[] uvs = mesh.uv;
        int[] indices = mesh.triangles;

        int nombreVertices = positions.Length;
        int nombreTriangles = indices.Length;

        float[] o_position_x = new float[nombreVertices];
        float[] o_position_y = new float[nombreVertices];
        float[] o_position_z = new float[nombreVertices];
        float[] o_normale_x = new float[nombreVertices];
        float[] o_normale_y = new float[nombreVertices];
        float[] o_normale_z = new float[nombreVertices];
        float[] o_uvs_x = new float[nombreVertices];
        float[] o_uvs_y = new float[nombreVertices];
        int[] o_triangles_x = new int[nombreTriangles / 3];
        int[] o_triangles_y = new int[nombreTriangles / 3];
        int[] o_triangles_z = new int[nombreTriangles / 3];

        float[] o_ajustementX = new float[3];
        o_ajustementX[0] = (ajustementX.x - 0.5f) * mesh.bounds.size.x;
        o_ajustementX[1] = (ajustementX.y - 0.5f) * mesh.bounds.size.x;
        o_ajustementX[2] = (ajustementX.z - 0.5f) * mesh.bounds.size.x;

        Debug.Log(mesh.bounds.center);
        Debug.Log(mesh.bounds.size);
        Debug.Log(new Vector3(o_ajustementX[0], o_ajustementX[1], o_ajustementX[2]));

        float[] o_taille = new float[3];
        o_taille[0] = taille.x;
        o_taille[1] = taille.y;
        o_taille[2] = taille.z;

        for (int i = 0; i < nombreVertices; i++)
        {
            o_position_x[i] = positions[i].x - mesh.bounds.center.x;
            o_position_y[i] = positions[i].y - mesh.bounds.center.y;
            o_position_z[i] = positions[i].z - mesh.bounds.center.z;

            o_normale_x[i] = normales[i].x;
            o_normale_y[i] = normales[i].y;
            o_normale_z[i] = normales[i].z;

            o_uvs_x[i] = uvs[i].x;
            o_uvs_y[i] = uvs[i].y;
        }
        for (int i = 0; i < nombreTriangles; i++)
        {
            int reste = i % 3;
            switch (reste)
            {
                case 0:
                    {
                        o_triangles_x[(i - reste) / 3] = indices[i];
                        break;
                    }
                case 1:
                    {
                        o_triangles_y[(i - reste) / 3] = indices[i];
                        break;
                    }
                case 2:
                    {
                        o_triangles_z[(i - reste) / 3] = indices[i];
                        break;
                    }
            }
        }

        IntPtr ptr_nombreVertices = new IntPtr(nombreVertices);
        IntPtr ptr_nombreTriangles = new IntPtr(nombreTriangles / 3);

        GCHandle h_positions_x = GCHandle.Alloc(o_position_x, GCHandleType.Pinned);
        GCHandle h_positions_y = GCHandle.Alloc(o_position_y, GCHandleType.Pinned);
        GCHandle h_positions_z = GCHandle.Alloc(o_position_z, GCHandleType.Pinned);
        GCHandle h_normales_x = GCHandle.Alloc(o_normale_x, GCHandleType.Pinned);
        GCHandle h_normales_y = GCHandle.Alloc(o_normale_y, GCHandleType.Pinned);
        GCHandle h_normales_z = GCHandle.Alloc(o_normale_z, GCHandleType.Pinned);
        GCHandle h_uvs_x = GCHandle.Alloc(o_uvs_x, GCHandleType.Pinned);
        GCHandle h_uvs_y = GCHandle.Alloc(o_uvs_y, GCHandleType.Pinned);
        GCHandle h_indices_x = GCHandle.Alloc(o_triangles_x, GCHandleType.Pinned);
        GCHandle h_indices_y = GCHandle.Alloc(o_triangles_y, GCHandleType.Pinned);
        GCHandle h_indices_z = GCHandle.Alloc(o_triangles_z, GCHandleType.Pinned);
        GCHandle h_ajustementX = GCHandle.Alloc(o_ajustementX, GCHandleType.Pinned);
        GCHandle h_taille = GCHandle.Alloc(o_taille, GCHandleType.Pinned);

        IntPtr ptr_ajustementX = h_ajustementX.AddrOfPinnedObject();
        IntPtr ptr_taille = h_taille.AddrOfPinnedObject();

        IntPtr ptr_positions_x = h_positions_x.AddrOfPinnedObject();
        IntPtr ptr_positions_y = h_positions_y.AddrOfPinnedObject();
        IntPtr ptr_positions_z = h_positions_z.AddrOfPinnedObject();
        IntPtr ptr_normales_x = h_normales_x.AddrOfPinnedObject();
        IntPtr ptr_normales_y = h_normales_y.AddrOfPinnedObject();
        IntPtr ptr_normales_z = h_normales_z.AddrOfPinnedObject();
        IntPtr ptr_uvs_x = h_uvs_x.AddrOfPinnedObject();
        IntPtr ptr_uvs_y = h_uvs_y.AddrOfPinnedObject();
        IntPtr ptr_indices_x = h_indices_x.AddrOfPinnedObject();
        IntPtr ptr_indices_y = h_indices_y.AddrOfPinnedObject();
        IntPtr ptr_indices_z = h_indices_z.AddrOfPinnedObject();

        unity_calculer_geometrie(
            ptr_nombreVertices,
            ptr_positions_x, ptr_positions_y, ptr_positions_z,
            ptr_normales_x, ptr_normales_y, ptr_normales_z,
            ptr_uvs_x, ptr_uvs_y,
            ptr_nombreTriangles,
            ptr_indices_x, ptr_indices_y, ptr_indices_z,
            ptr_ajustementX,
            ptr_taille);

        h_positions_x.Free();
        h_positions_y.Free();
        h_positions_z.Free();
        h_normales_x.Free();
        h_normales_y.Free();
        h_normales_z.Free();
        h_uvs_x.Free();
        h_uvs_y.Free();
        h_indices_x.Free();
        h_indices_y.Free();
        h_indices_z.Free();
        h_ajustementX.Free();
        h_taille.Free();

        return;
    }
    public static void SauvegarderGeometrie(
        string nomCategorie,
        string nomFichier)
    {
        IntPtr ptr_categorie = Marshal.StringToHGlobalAnsi(nomCategorie);
        IntPtr ptr_fichier = Marshal.StringToHGlobalAnsi(nomFichier);

        unity_sauvegarder_geometrie(
            ptr_fichier,
            ptr_categorie);

        return;
    }
    public static void GenererGeometrie(
        Action<Mesh> mesh,
        Vector3 taille)
    {
        action_geometrie = mesh;

        float[] o_taille = new float[3];
        o_taille[0] = taille.x;
        o_taille[1] = taille.y;
        o_taille[2] = taille.z;

        GCHandle h_taille = GCHandle.Alloc(o_taille, GCHandleType.Pinned);

        IntPtr ptr_taille = h_taille.AddrOfPinnedObject();

        unity_generer_geometrie_editeur(
            cb_geometrie,
            ptr_taille);

        h_taille.Free();

        return;
    }
    public static void GenererGeometrie(
        string nomFichier,
        Action<Mesh> mesh,
        Vector3 taille)
    {
        action_geometrie = mesh;

        float[] o_taille = new float[3];
        o_taille[0] = taille.x;
        o_taille[1] = taille.y;
        o_taille[2] = taille.z;

        GCHandle h_taille = GCHandle.Alloc(o_taille, GCHandleType.Pinned);

        IntPtr ptr_fichier = Marshal.StringToHGlobalAnsi(nomFichier);
        IntPtr ptr_taille = h_taille.AddrOfPinnedObject();

        unity_generer_geometrie(
            ptr_fichier,
            cb_geometrie,
            ptr_taille);

        h_taille.Free();

        return;
    }



    public static void LancerEntrainementTexture(
        string nom_categorie,
        string nom_ia,
        bool utiliser_gpu,
        bool continuer)
    {
        IntPtr ptr_nom_categorie = Marshal.StringToHGlobalAnsi(nom_categorie);
        IntPtr ptr_nom_ia = Marshal.StringToHGlobalAnsi(nom_ia);
        IntPtr ptr_utiliser_gpu = new IntPtr(utiliser_gpu ? 1 : 0);
        IntPtr ptr_continuer = new IntPtr(continuer ? 1 : 0);
        unity_commencer_entrainement_texture(
            ptr_nom_categorie,
            ptr_nom_ia,
            ptr_utiliser_gpu,
            ptr_continuer);
        return;
    }
    public static void InterrompreEntrainementTexture()
    {

    }
    public static void ObtenirResultatsTexture(
        Action<int, float, float, float, float> callback)
    {
        action_resultats_entrainement = callback;

        unity_obtenir_resultats_entrainement_texture(
            cb_resultats);

        return;
    }
    public static void GenererTexture()
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }




















    private delegate void callback_log(
        IntPtr tailleMessage,
        IntPtr message);

    private delegate void callback_liste_string(
        IntPtr tailleChaine, IntPtr chaine,
        IntPtr tailleIndices, IntPtr indices);

    private delegate void callback_resultats_entrainement(
        IntPtr nombreEpochs,
        IntPtr gLoss,
        IntPtr dLoss,
        IntPtr lossL1,
        IntPtr ganLoss);

    private delegate void callback_geometrie(
        IntPtr nombre_vertices, 
        IntPtr nombre_triangles,
        IntPtr positions_x, IntPtr positions_y, IntPtr positions_z,
        IntPtr normales_x, IntPtr normales_y, IntPtr normales_z,
        IntPtr uvs_x, IntPtr uvs_y,
        IntPtr indices);

    private static readonly callback_log cb_log = new callback_log(wrapper_callback_log);
    private static readonly callback_liste_string cb_liste_string = new callback_liste_string(wrapper_callback_liste_string);
    private static readonly callback_resultats_entrainement cb_resultats = new callback_resultats_entrainement(wrapper_callback_resultats_entrainement);
    private static readonly callback_geometrie cb_geometrie = new callback_geometrie(wrapper_callback_geometrie);
    private static Action<List<string>> action_liste_string;
    private static Action<int, float, float, float, float> action_resultats_entrainement;
    private static Action<Mesh> action_geometrie;

    private static void wrapper_callback_log(
       IntPtr tailleMessage,
       IntPtr message)
    {
        int l = tailleMessage.ToInt32();
        Debug.Log(Marshal.PtrToStringAnsi(message, l));
        return;
    }

    private static void wrapper_callback_liste_string(
        IntPtr tailleChaine, IntPtr chaine,
        IntPtr tailleIndices, IntPtr indices)
    {
        int nombreCaracteresTousLesTags = tailleChaine.ToInt32();
        string tousLesTags = Marshal.PtrToStringAnsi(chaine, nombreCaracteresTousLesTags);
        

        int nombreIndices = tailleIndices.ToInt32();

        if (nombreIndices == 0)
        {
            action_liste_string(null);
            return;
        }
        if (nombreIndices == 1)
        {
            action_liste_string(new List<string>() { tousLesTags });
            return;
        }

        int[] indicesTags = new int[nombreIndices];
        Marshal.Copy(indices, indicesTags, 0, nombreIndices);

        List<string> tags = new List<string>();
        int curseur = 0;
        for (int i = 0; i < nombreIndices; i++)
        {
            string tagActuel = tousLesTags.Substring(curseur, indicesTags[i]);
            curseur += indicesTags[i];
            tags.Add(tagActuel);
        }

        action_liste_string(tags);

        return;
    }

    private static void wrapper_callback_resultats_entrainement(
        IntPtr nombreEpochs,
        IntPtr gLoss,
        IntPtr dLoss,
        IntPtr lossL1,
        IntPtr ganLoss)
    {

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //action_resultats_entrainement()
    }

    private static void wrapper_callback_geometrie(
        IntPtr nombre_vertices,
        IntPtr nombre_triangles,
        IntPtr positions_x, IntPtr positions_y, IntPtr positions_z,
        IntPtr normales_x, IntPtr normales_y, IntPtr normales_z,
        IntPtr uvs_x, IntPtr uvs_y,
        IntPtr indices)
    {
        int nombreVertices = nombre_vertices.ToInt32();
        int nombreTriangles = nombre_triangles.ToInt32();

        float[] positionsX = new float[nombreVertices];
        float[] positionsY = new float[nombreVertices];
        float[] positionsZ = new float[nombreVertices];
        Marshal.Copy(positions_x, positionsX, 0, nombreVertices);
        Marshal.Copy(positions_y, positionsY, 0, nombreVertices);
        Marshal.Copy(positions_z, positionsZ, 0, nombreVertices);

        float[] normalesX = new float[nombreVertices];
        float[] normalesY = new float[nombreVertices];
        float[] normalesZ = new float[nombreVertices];
        Marshal.Copy(normales_x, normalesX, 0, nombreVertices);
        Marshal.Copy(normales_y, normalesY, 0, nombreVertices);
        Marshal.Copy(normales_z, normalesZ, 0, nombreVertices);

        float[] uvsX = new float[nombreVertices];
        float[] uvsY = new float[nombreVertices];
        Marshal.Copy(uvs_x, uvsX, 0, nombreVertices);
        Marshal.Copy(uvs_y, uvsY, 0, nombreVertices);

        int[] triangles = new int[nombreTriangles];
        Marshal.Copy(indices, triangles, 0, nombreTriangles);

        Vector3[] positions = new Vector3[nombreVertices];
        Vector3[] normales = new Vector3[nombreVertices];
        Vector2[] uvs = new Vector2[nombreVertices];
        for (int i = 0; i < nombreVertices; i++)
        {
            positions[i] = new Vector3(positionsX[i], positionsY[i], positionsZ[i]);
            normales[i] = new Vector3(normalesX[i], normalesY[i], normalesZ[i]);
            uvs[i] = new Vector2(uvsX[i], uvsY[i]);
        }

        Mesh m = new Mesh();
        m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        m.SetVertices(positions);
        //m.SetNormals(normales);
        //m.SetUVs(0, uvs);
        m.SetIndices(triangles, MeshTopology.Triangles, 0);

        //m.Optimize();
        //m.RecalculateNormals();
        //m.RecalculateBounds();

        action_geometrie(m);
        return;
    }




















    [DllImport("GeometrieProcedurale")]
    private static extern void unity_creer_projet(
        IntPtr url_dossier,
        callback_log cb_log);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_charger_projet(
        IntPtr url_dossier,
        callback_log cb_log);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_obtenir_noms_scripts(
        callback_liste_string callback);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_obtenir_noms_categories(
        callback_liste_string callback);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_obtenir_noms_geometries(
        IntPtr nom_categorie,
        callback_liste_string callback);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_obtenir_noms_ias(
        IntPtr nom_categorie,
        callback_liste_string callback);




    [DllImport("GeometrieProcedurale")]
    private static extern void unity_calculer_geometrie(
        IntPtr nombre_vertices,
        IntPtr positions_x, IntPtr positions_y, IntPtr positions_z,
        IntPtr normales_x, IntPtr normales_y, IntPtr normales_z,
        IntPtr uvs_x, IntPtr uvs_y,
        IntPtr nombre_triangles,
        IntPtr indices_x, IntPtr indices_y, IntPtr indices_z,
        IntPtr ajustement_x,
        IntPtr taille);


    [DllImport("GeometrieProcedurale")]
    private static extern void unity_sauvegarder_geometrie(
        IntPtr categorie,
        IntPtr nom_fichier);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_generer_geometrie_editeur(
        callback_geometrie callback,
        IntPtr ptr_taille);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_generer_geometrie(
        IntPtr nom_sauvegarde,
        callback_geometrie callback,
        IntPtr ptr_taille);




    [DllImport("GeometrieProcedurale")]
    private static extern void unity_commencer_entrainement_texture(
        IntPtr nom_categorie,
        IntPtr nom_ia,
        IntPtr utiliser_gpu,
        IntPtr continuer);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_obtenir_resultats_entrainement_texture(
        callback_resultats_entrainement cb);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_arreter_entrainement_texture();

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_generer_texture(
        IntPtr nom_categorie,
        IntPtr nom_ia,
        IntPtr url_tex_label,
        IntPtr url_tex_target);

    [DllImport("GeometrieProcedurale")]
    private static extern void unity_executer_script(
        IntPtr nom_script,
        IntPtr cb_photo,
        IntPtr cb_geo);
}
