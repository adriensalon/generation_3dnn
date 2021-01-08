using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public static class GeometrieProceduraleAPI
{
    //#region behaviours
    //private static BehaviourGeometrieProceduraleAPI referenceBehaviour;
    //private static BehaviourEditeur referenceEditeur;

    //public static void EnregistrerBehaviourAPI(BehaviourGeometrieProceduraleAPI behaviour)
    //{
    //    referenceBehaviour = behaviour;
    //    return;
    //}
    //public static void EnregistrerBehaviourEditeur(BehaviourEditeur behaviour)
    //{
    //    referenceEditeur = behaviour;
    //}

    //public static Material ObtenirMateriau(int indice)
    //{
    //    //Debug.Log(indice);
    //    return referenceBehaviour.materiaux[Mathf.Max(0, indice)];
    //}
    //public static List<Material> ObtenirMateriaux(List<int> indices)
    //{
    //    List<Material> l = new List<Material>(indices.Count);
    //    for (int i = 0; i < l.Count; i++)
    //    {
    //        l[i] = ObtenirMateriau(indices[i]);
    //    }
    //    return l;
    //}
    //#endregion



    //#region fonctions natives
    ///// <summary>
    ///// Initialise la DLL : à appeler avant d'utiliser les autres fonctions API
    ///// </summary>
    //public static void API_Construction(string cheminBibliotheque)
    //{
    //    IntPtr ptr = Marshal.StringToHGlobalAnsi(cheminBibliotheque);

    //    Construction(
    //        ptr,
    //        callbackLog,
    //        callbackGeometrie);
    //    return;
    ////}

    ////public static void API_GenererNouvelleRegion(Region r)
    ////{
    ////    var g = new Geometrie();
    ////    g.Generer(r);
    ////    return;
    ////}


    ///// <summary>
    ///// Récupère les tags utilisés par les fichiers trouvés lors de la Construction()
    ///// </summary>
    //public static void API_Editeur_RecupererTags()
    //{
    //    Editeur_RecupererTags(callbackTags);
    //    return;
    //}





    ///// <summary>
    ///// Importe de la geométrie depuis un fichier .off
    ///// </summary>
    //public static void API_Editeur_ImporterFichierMotif(
    //    Action<Geometrie_Old_EditerMeshParPointeur> callback, 
    //    string url)
    //{
    //    action_receptionGeometrieMainThread = callback;
    //    IntPtr ptr_nomFichier = Marshal.StringToHGlobalAnsi(url);

    //    Editeur_ImporterFichierMotif(
    //        ptr_nomFichier);

    //    return;

    //}

    ///// <summary>
    ///// Calcule le motif et le prépare pour être généré
    ///// </summary>
    //public static void API_Editeur_CalculerMotif(
    //    ParametresCalculGeometrie motif,
    //    string urlModele)
    //{
    //    IntPtr ptr_urlModele = Marshal.StringToHGlobalAnsi(urlModele);
    //    IntPtr ptr_tag = Marshal.StringToHGlobalAnsi(motif.tag);
    //    IntPtr ptr_materiau = new IntPtr(motif.materiau);

    //    GCHandle handle_indicateurA = GCHandle.Alloc(motif.indicateur1, GCHandleType.Pinned);
    //    GCHandle handle_indicateurB = GCHandle.Alloc(motif.indicateur2, GCHandleType.Pinned);
    //    GCHandle handle_indicateurC = GCHandle.Alloc(motif.indicateur3, GCHandleType.Pinned);
    //    GCHandle handle_xA = GCHandle.Alloc(motif.xA, GCHandleType.Pinned);
    //    GCHandle handle_xB = GCHandle.Alloc(motif.xB, GCHandleType.Pinned);
    //    GCHandle handle_xC = GCHandle.Alloc(motif.xC, GCHandleType.Pinned);

    //    IntPtr ptr_indicateurA = handle_indicateurA.AddrOfPinnedObject();
    //    IntPtr ptr_indicateurB = handle_indicateurB.AddrOfPinnedObject();
    //    IntPtr ptr_indicateurC = handle_indicateurC.AddrOfPinnedObject();
    //    IntPtr ptr_xA = handle_xA.AddrOfPinnedObject();
    //    IntPtr ptr_xB = handle_xB.AddrOfPinnedObject();
    //    IntPtr ptr_xC = handle_xC.AddrOfPinnedObject();

    //    Editeur_CalculerMotif(
    //        ptr_urlModele,
    //        ptr_tag,
    //        ptr_indicateurA, ptr_indicateurB, ptr_indicateurC,
    //        ptr_materiau,
    //        ptr_xA, ptr_xB, ptr_xC);

    //    handle_indicateurA.Free();
    //    handle_indicateurB.Free();
    //    handle_indicateurC.Free();
    //    handle_xA.Free();
    //    handle_xB.Free();
    //    handle_xC.Free();

    //    return;
    //}

    ///// <summary>
    ///// Génère le motif précédemment créé avec CalculerMotif() en l'ajustant à la dimension voulue
    ///// </summary>
    //public static void API_Editeur_GenererMotif(
    //    Action<Geometrie_Old_EditerMeshParPointeur> callback, 
    //    float tailleX)
    //{
    //    action_receptionGeometrieMainThread = callback;

    //    GCHandle handle_tailleX = GCHandle.Alloc(tailleX, GCHandleType.Pinned);

    //    IntPtr ptr_tailleX = handle_tailleX.AddrOfPinnedObject();

    //    Editeur_GenererMotif(
    //        ptr_tailleX);

    //    handle_tailleX.Free();
    //    return;
    //}

    ///// <summary>
    ///// Sauvegarde le motif précédemment créé avec CalculerMotif()
    ///// </summary>
    //public static void API_Editeur_SauvegarderFichierMotif(
    //    string nomFichier)
    //{
    //    IntPtr p = Marshal.StringToHGlobalAnsi(nomFichier);
    //    Editeur_SauvegarderFichierMotif(p);
    //    return;
    //}







    /////// <summary>
    /////// Calcule le schema et le prépare pour être généré
    /////// </summary>
    ////public static void API_Editeur_CalculerSchema(
    ////    Editeur_SchemaCalcul schema)
    ////{
    ////    int nombreBlocs = schema.tags.Count;

    ////    string tags = "";
    ////    float[] positionsX = new float[nombreBlocs];
    ////    float[] positionsY = new float[nombreBlocs];
    ////    float[] positionsZ = new float[nombreBlocs];
    ////    float[] rotationsX = new float[nombreBlocs];
    ////    float[] rotationsY = new float[nombreBlocs];
    ////    float[] rotationsZ = new float[nombreBlocs];
    ////    float[] taillesZ = new float[nombreBlocs];
    ////    //float[] taillesXY = new float[nombreBlocs];

    ////    for (int i = 0; i < nombreBlocs; i++)
    ////    {
    ////        tags += schema.tags[i];
    ////        positionsX[i] = schema.translations[i].x;
    ////        positionsY[i] = schema.translations[i].y;
    ////        positionsZ[i] = schema.translations[i].z;
    ////        rotationsX[i] = schema.rotations[i].x;
    ////        rotationsY[i] = schema.rotations[i].y;
    ////        rotationsZ[i] = schema.rotations[i].z;
    ////        taillesZ[i] = schema.tailles[i].x;
    ////        //taillesXY[i] = schema.tailles[i].y;
    ////    }

    ////    IntPtr ptr_categorie = new IntPtr(schema.categorie);
    ////    IntPtr ptr_tailleBlocs = new IntPtr(schema.tags.Count);
    ////    IntPtr ptr_tags = Marshal.StringToHGlobalAnsi(tags);

    ////    GCHandle handle_positionsX = GCHandle.Alloc(positionsX, GCHandleType.Pinned);
    ////    GCHandle handle_positionsY = GCHandle.Alloc(positionsY, GCHandleType.Pinned);
    ////    GCHandle handle_positionsZ = GCHandle.Alloc(positionsZ, GCHandleType.Pinned);
    ////    GCHandle handle_rotationsX = GCHandle.Alloc(rotationsX, GCHandleType.Pinned);
    ////    GCHandle handle_rotationsY = GCHandle.Alloc(rotationsY, GCHandleType.Pinned);
    ////    GCHandle handle_rotationsZ = GCHandle.Alloc(rotationsZ, GCHandleType.Pinned);
    ////    GCHandle handle_taillesX = GCHandle.Alloc(taillesZ, GCHandleType.Pinned);
    ////    //GCHandle handle_taillesY = GCHandle.Alloc(taillesXY, GCHandleType.Pinned);

    ////    IntPtr ptr_positionsX = handle_positionsX.AddrOfPinnedObject();
    ////    IntPtr ptr_positionsY = handle_positionsY.AddrOfPinnedObject();
    ////    IntPtr ptr_positionsZ = handle_positionsZ.AddrOfPinnedObject();
    ////    IntPtr ptr_rotationsX = handle_rotationsX.AddrOfPinnedObject();
    ////    IntPtr ptr_rotationsY = handle_rotationsY.AddrOfPinnedObject();
    ////    IntPtr ptr_rotationsZ = handle_rotationsZ.AddrOfPinnedObject();
    ////    IntPtr ptr_taillesX = handle_taillesX.AddrOfPinnedObject();
    ////    //IntPtr ptr_taillesY = handle_taillesY.AddrOfPinnedObject();

    ////    Editeur_CalculerSchema(
    ////        ptr_categorie,
    ////        ptr_tailleBlocs,
    ////        ptr_tags,
    ////        ptr_positionsX, ptr_positionsY, ptr_positionsZ,
    ////        ptr_rotationsX, ptr_rotationsY, ptr_rotationsZ,
    ////        ptr_taillesX);

    ////    handle_positionsX.Free();
    ////    handle_positionsY.Free();
    ////    handle_positionsZ.Free();
    ////    handle_rotationsX.Free();
    ////    handle_rotationsY.Free();
    ////    handle_rotationsZ.Free();
    ////    handle_taillesX.Free();
    ////    //handle_taillesXY.Free();

    ////    return;
    ////}

    ///// <summary>
    ///// Génère à partir du schema calculé une représentation en blocs
    ///// </summary>
    ///// <param name="callback"></param>
    //public static void API_Editeur_GenererBlocsSchema(
    //    Action<Geometrie_Old_EditerMeshParPointeur> callback)
    //{
    //    action_receptionGeometrieMainThread = callback;
    //    Editeur_GenererBlocsSchema();
    //    return;
    //}

    ///// <summary>
    ///// Génère en choisissant les motifs dans la bibliotheque
    ///// </summary>
    //public static void API_Editeur_GenererSchema(
    //    Action<Geometrie_Old_EditerMeshParPointeur> callback,
    //    float indicateurA,
    //    float indicateurB,
    //    float indicateurC)
    //{
    //    action_receptionGeometrieMainThread = callback;

    //    GCHandle handle_indicateurA = GCHandle.Alloc(indicateurA, GCHandleType.Pinned);
    //    GCHandle handle_indicateurB = GCHandle.Alloc(indicateurB, GCHandleType.Pinned);
    //    GCHandle handle_indicateurC = GCHandle.Alloc(indicateurC, GCHandleType.Pinned);

    //    IntPtr ptr_a = handle_indicateurA.AddrOfPinnedObject();
    //    IntPtr ptr_b = handle_indicateurB.AddrOfPinnedObject();
    //    IntPtr ptr_c = handle_indicateurC.AddrOfPinnedObject();

    //    Editeur_GenererSchema(
    //        ptr_a,
    //        ptr_b,
    //        ptr_c);

    //    handle_indicateurA.Free();
    //    handle_indicateurB.Free();
    //    handle_indicateurC.Free();
    //    return;
    //}

    ///// <summary>
    ///// Sauvegarde le schema précédemment créé avec CalculerSchema()
    ///// </summary>
    //public static void API_Editeur_SauvegarderFichierSchema(
    //   string nomFichier)
    //{
    //    IntPtr p = Marshal.StringToHGlobalAnsi(nomFichier);
    //    Editeur_SauvegarderFichierSchema(p);
    //    return;
    //}
    //#endregion


    //#region callbacks
    //private delegate void CallbackLog(
    //    IntPtr tailleMessage,
    //    IntPtr message);

    //private delegate void CallbackTags(
    //    IntPtr tailleChaine, IntPtr chaine,
    //    IntPtr tailleIndices, IntPtr indices);

    //private delegate void CallbackGeometrie(
    //    IntPtr taille_positionsEtNormales,
    //    IntPtr taille_triangles,
    //    IntPtr taille_submeshes,
    //    IntPtr positionsX, IntPtr positionsY, IntPtr positionsZ,
    //    IntPtr normalesX, IntPtr normalesY, IntPtr normalesZ,
    //    IntPtr trianglesA, IntPtr trianglesB, IntPtr trianglesC,
    //    IntPtr submeshesPositions,
    //    IntPtr submeshesTriangles,
    //    IntPtr submeshesMateriaux);

    //private static readonly CallbackLog callbackLog = new CallbackLog(WrapperCallback_Log);
    //private static readonly CallbackTags callbackTags = new CallbackTags(WrapperCallback_Tags);
    //private static readonly CallbackGeometrie callbackGeometrie = new CallbackGeometrie(WrapperCallback_Geometrie);

    //private static Action<Geometrie_Old_EditerMeshParPointeur> action_receptionGeometrieMainThread;

    //private static void WrapperCallback_Log(
    //   IntPtr tailleMessage,
    //   IntPtr message)
    //{
    //    int l = tailleMessage.ToInt32();
    //    Debug.Log(Marshal.PtrToStringAnsi(message, l));
    //    return;
    //}

    //private static void WrapperCallback_Tags(
    //    IntPtr tailleChaine, IntPtr chaine,
    //    IntPtr tailleIndices, IntPtr indices)
    //{
    //    int nombreCaracteresTousLesTags = tailleChaine.ToInt32();
    //    string tousLesTags = Marshal.PtrToStringAnsi(chaine, nombreCaracteresTousLesTags);

    //    int nombreIndices = tailleIndices.ToInt32();
    //    int[] indicesTags = new int[nombreIndices];
    //    Marshal.Copy(indices, indicesTags, 0, nombreIndices);

    //    List<string> tags = new List<string>();
    //    for (int i = 0; i < nombreIndices - 1; i++)
    //    {
    //        int indiceActuel = indicesTags[i];
    //        int nombreCaracteres = indicesTags[i + 1] - indiceActuel;
    //        string tagActuel = tousLesTags.Substring(indiceActuel, nombreCaracteres);
    //        tags.Add(tagActuel);
    //    }
    //    if (nombreIndices > 1)
    //    {
    //        int indiceActuelF = indicesTags[nombreIndices - 1];
    //        string tagActuelF = tousLesTags.Substring(indiceActuelF, nombreCaracteresTousLesTags);
    //        tags.Add(tagActuelF);
    //    }        

    //    referenceEditeur.vueImporter.fenetreBibliotheque.DefinirTags(tags); // go action pluto comme ca plus besoin de la ref c'est immonde
    //    //Debug.Log(tags);
    //    return;
    //}

    //private static void WrapperCallback_Geometrie(
    //    IntPtr taille_positionsEtNormales,
    //    IntPtr taille_triangles,
    //    IntPtr taille_submeshes,
    //    IntPtr positionsX, IntPtr positionsY, IntPtr positionsZ,
    //    IntPtr normalesX, IntPtr normalesY, IntPtr normalesZ,
    //    IntPtr trianglesA, IntPtr trianglesB, IntPtr trianglesC,
    //    IntPtr submeshesPositions,
    //    IntPtr submeshesTriangles,
    //    IntPtr submeshesMateriaux)
    //{
    //    int lPositionsEtNormales = taille_positionsEtNormales.ToInt32();
    //    int lTriangles = taille_triangles.ToInt32();
    //    int lSubmeshes = taille_submeshes.ToInt32();

    //    float[] vPositionsX = new float[lPositionsEtNormales];
    //    float[] vPositionsY = new float[lPositionsEtNormales];
    //    float[] vPositionsZ = new float[lPositionsEtNormales];
    //    float[] vNormalesX = new float[lPositionsEtNormales];
    //    float[] vNormalesY = new float[lPositionsEtNormales];
    //    float[] vNormalesZ = new float[lPositionsEtNormales];
    //    int[] vTrianglesA = new int[lTriangles];
    //    int[] vTrianglesB = new int[lTriangles];
    //    int[] vTrianglesC = new int[lTriangles];
    //    int[] vSubPositions = new int[lSubmeshes];
    //    int[] vSubTriangles = new int[lSubmeshes];
    //    int[] vSubMateriaux = new int[lSubmeshes];

    //    Marshal.Copy(positionsX, vPositionsX, 0, lPositionsEtNormales);
    //    Marshal.Copy(positionsY, vPositionsY, 0, lPositionsEtNormales);
    //    Marshal.Copy(positionsZ, vPositionsZ, 0, lPositionsEtNormales);
    //    Marshal.Copy(normalesX, vNormalesX, 0, lPositionsEtNormales);
    //    Marshal.Copy(normalesY, vNormalesY, 0, lPositionsEtNormales);
    //    Marshal.Copy(normalesZ, vNormalesZ, 0, lPositionsEtNormales);
    //    Marshal.Copy(trianglesA, vTrianglesA, 0, lTriangles);
    //    Marshal.Copy(trianglesB, vTrianglesB, 0, lTriangles);
    //    Marshal.Copy(trianglesC, vTrianglesC, 0, lTriangles);
    //    Marshal.Copy(submeshesPositions, vSubPositions, 0, lSubmeshes);
    //    Marshal.Copy(submeshesTriangles, vSubTriangles, 0, lSubmeshes);
    //    Marshal.Copy(submeshesMateriaux, vSubMateriaux, 0, lSubmeshes);

    //    Geometrie_Old_EditerMeshParPointeur g = new Geometrie_Old_EditerMeshParPointeur();
    //    g.positions = new List<Vector3>(lPositionsEtNormales);
    //    g.normales = new List<Vector3>(lPositionsEtNormales);
    //    for (int i = 0; i < lPositionsEtNormales; i++)
    //    {
    //       g.positions.Add(new Vector3(vPositionsX[i], vPositionsY[i], vPositionsZ[i]));
    //       g.normales.Add(new Vector3(vNormalesX[i], vNormalesY[i], vNormalesZ[i]));
    //    }
    //    g.triangles = new List<Vector3Int>(lTriangles);
    //    for (int i = 0; i < lTriangles; i++)
    //    {
    //        g.triangles.Add(new Vector3Int(vTrianglesA[i], vTrianglesB[i], vTrianglesC[i]));
    //    }
    //    g.indices_vertices = new List<int>(vSubPositions);
    //    g.indices_triangles = new List<int>(vSubTriangles);
    //    g.materiaux = new List<int>(vSubMateriaux);

    //    action_receptionGeometrieMainThread(g);
    //    return;
    //}
    //#endregion


    //#region dll
    //[DllImport("GeometrieProcedurale")]
    //private static extern void Construction(
    //    IntPtr parametres, 
    //    CallbackLog callbackLog, 
    //    CallbackGeometrie callbackGeometrie);






    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_RecupererTags(
    //    CallbackTags callback);








    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_ImporterFichierMotif(
    //    IntPtr urlFichierOFF);

    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_CalculerMotif(
    //    IntPtr urlModele,
    //    IntPtr tag,
    //    IntPtr indicateurA, IntPtr indicateurB, IntPtr indicateurC,
    //    IntPtr materiau,
    //    IntPtr a_ajustementDebutX, IntPtr a_ajustementExtensionX, IntPtr a_ajustementFinX);

    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_GenererMotif(
    //    IntPtr tailleX);

    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_SauvegarderFichierMotif(
    //    IntPtr nomFichier);









    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_CalculerSchema(
    //    IntPtr categorie,
    //    IntPtr taille_blocs,
    //    IntPtr tags,
    //    IntPtr positionsX, IntPtr positionsY, IntPtr positionsZ,
    //    IntPtr rotationsX, IntPtr rotationsY, IntPtr rotationsZ,
    //    IntPtr taillesX);

    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_GenererBlocsSchema();

    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_GenererSchema(
    //    IntPtr indicateurA,
    //    IntPtr indicateurB,
    //    IntPtr indicateurC);

    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_SauvegarderFichierSchema(
    //    IntPtr nomFichier);


    //[DllImport("GeometrieProcedurale")]
    //private static extern void Test(
    //    IntPtr p);
    //public static void Te(Texture2D t)
    //{
    ////    var dimA = t.width;
    ////    var dimB = t.height;
    ////    var dimC = 3;
    ////    var longueur = dimA * dimB * dimC;

    ////    IntPtr ptr_tailleX = new IntPtr(dimA);
    ////    IntPtr ptr_tailleY = new IntPtr(dimB);
    ////    IntPtr ptr_tailleCanaux = new IntPtr(dimC);
    ////    float[] p = new float[longueur];

    ////    for (var z = 0; z < dimC; z++)
    ////    {
    ////        for (var y = 0; y < dimB; y++)
    ////        {
    ////            for (var x = 0; x < dimA; x++)
    ////            {
    ////                var i = z * dimC + y * dimA + x;
    ////                Color c = t.GetPixel(x, y);

    ////                switch (z)
    ////                {
    ////                    case 0:
    ////                        {
    ////                            p[i] = c.r;
    ////                            break;
    ////                        }

    ////                    case 1:
    ////                        {
    ////                            p[i] = c.g;
    ////                            break;
    ////                        }
    ////                    case 2:
    ////                        {
    ////                            p[i] = c.b;
    ////                            break;
    ////                        }
    ////                }
    ////            }
    ////        }
    ////    }

    ////    var handle_data = GCHandle.Alloc(p, GCHandleType.Pinned);
    ////    IntPtr ptr_data = handle_data.AddrOfPinnedObject();

    //    int va = 4;
    //    int vb = 5;
    //    int vc = 6;
    //    int vd = 7;
    //    IntPtr a = t.GetNativeTexturePtr();
    //    IntPtr b = t.GetNativeTexturePtr();
    //    IntPtr c = t.GetNativeTexturePtr();
    //    IntPtr d = t.GetNativeTexturePtr();

    //    IntPtr[] array = new IntPtr[4] { a, b, c, d };

    //    GCHandle h = GCHandle.Alloc(array, GCHandleType.Pinned);
    //    IntPtr p = h.AddrOfPinnedObject();
    //    Test(p);


    //    return;
    //}















    //// IA
    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_EntrainerIa(
    //    IntPtr nomIa,
    //    IntPtr nombreEpochs,
    //    IntPtr aaa);

    //[DllImport("GeometrieProcedurale")]
    //private static extern void Editeur_ContinuerEntrainementIa(
    //    IntPtr nomIa,
    //    IntPtr nombreEpochs,
    //    IntPtr aaa);

    //// MOTEUR ENFIN !!!!

    //[DllImport("GeometrieProcedurale")]
    //private static extern void Moteur_PredireIa(
    //    IntPtr nomIa,
    //    IntPtr nombreEpochs,
    //    IntPtr aaa);


    ////public static void LancerEntrainementIa(string nomIa, parametres_entrainement p)
    ////{
        
    ////}

    ////public static Texture2D LancerEntrainementIa2(string nomIa, parametres_entrainement p)
    ////{
    ////    return new Texture2D(2, 2);
    ////}

    //public static void ObtenirResultatsEntrainementIa(string nomIa)
    //{

    //}

    //#endregion
























    //[DllImport("GeometrieProcedurale")]
    //private static extern void API_EditeurSpec_ChargerProjet(
    //    IntPtr urlDossier);


    //[DllImport("GeometrieProcedurale")]
    //private static extern void API_EditeurSpec_Calculer_Motif(
    //    IntPtr urlMeshOBJ,
    //    IntPtr categorie,
    //    IntPtr nomMotif,
    //    //
    //    IntPtr ptrMeshUnity);

    //[DllImport("GeometrieProcedurale")]
    //private static extern void API_EditeurSpec_Generer_Motif(
    //    IntPtr categorie,
    //    IntPtr nomMotif,
    //    IntPtr nomIa,
    //    IntPtr tailleX,
    //    IntPtr tailleY,
    //    IntPtr tailleZ,
    //    IntPtr ptrMeshUnity,
    //    IntPtr ptrTextureUnity_xMoins,
    //    IntPtr ptrTextureUnity_xPlus,
    //    IntPtr ptrTextureUnity_yMoins,
    //    IntPtr ptrTextureUnity_yPlus,
    //    IntPtr ptrTextureUnity_zMoins,
    //    IntPtr ptrTextureUnity_zPlus);

    //[DllImport("GeometrieProcedurale")]
    //private static extern void API_EditeurSpec_EntrainerIa(
    //    IntPtr categorie,
    //    IntPtr nomIa
    //    //
    //    );

    //[DllImport("GeometrieProcedurale")]
    //private static extern void API_EditeurSpec_ObtenirResultatsIa(
    //    IntPtr categorie,
    //    IntPtr nomIa);
}