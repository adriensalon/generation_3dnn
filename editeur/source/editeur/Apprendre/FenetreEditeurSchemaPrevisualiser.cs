using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable]
public class FenetreEditeurSchemaPrevisualiser
{
    public MeshFilter filterModele;
    public MeshRenderer rendererModele;
    public GameObject cube;

    public GameObject parametres;
    public GameObject bouton;
    public Text texteBouton;
    public Slider sliderIndicateurA;
    public Slider sliderIndicateurB;
    public Slider sliderIndicateurC;

    //public void ActiverBouton()
    //{
    //    bouton.SetActive(true);
    //}

    //public void ActiverPrevisualisation(Editeur_SchemaCalcul s)
    //{
    //    if (parametres.activeSelf)
    //    {
    //        parametres.SetActive(false);
    //        texteBouton.text = "activer prévisualisation";
    //        cube.SetActive(true);
    //        filterModele.sharedMesh = new Mesh();
    //        return;
    //    }
    //    parametres.SetActive(true);
    //    texteBouton.text = "désactiver prévisualisation";
    //    cube.SetActive(false);

    //    GeometrieProceduraleAPI.API_Editeur_GenererSchema(
    //        CallbackPrevisualisation,
    //        sliderIndicateurA.value,
    //        sliderIndicateurB.value,
    //        sliderIndicateurC.value);
    //    return;
    //}

    //public void Generer()
    //{
    //    GeometrieProceduraleAPI.API_Editeur_GenererSchema(
    //        CallbackPrevisualisation,
    //        sliderIndicateurA.value,
    //        sliderIndicateurB.value,
    //        sliderIndicateurC.value);
    //}


    //private void CallbackPrevisualisation(Geometrie_Old_EditerMeshParPointeur g)
    //{
    //    g.Afficher(filterModele, rendererModele, false);
    //    return;
    //}
}

