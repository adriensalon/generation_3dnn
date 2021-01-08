using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[Serializable]
public class FenetreEditeurSchemaMotifSelectionne
{
    public MeshFilter filter;
    public MeshRenderer renderer;
    public GameObject racine;
    public Dropdown dropdownTag;
    public InputField positionX, positionY, positionZ;
    public InputField rotationX, rotationY, rotationZ;
    public InputField tailleX, tailleY, tailleZ;

    private bool estMotifSelectionne = false;
    private int indiceMotifSelectionne = -1;

    public void ChangerIndiceMotifSelectionne(int i = -1)
    {
        estMotifSelectionne = i == -1 ? false : true;
        indiceMotifSelectionne = i;
        return;
    }
    //}
    //public void ChangerTag(Editeur_SchemaCalcul s)
    //{
    //    s.tags[indiceMotifSelectionne] = dropdownTag.itemText.text;
    //    return;
    //}
    //public void ChangerPositionX(Editeur_SchemaCalcul s)
    //{
    //    s.translations[indiceMotifSelectionne] = new Vector3(
    //        (float)Convert.ToDouble(positionX.text), 
    //        s.translations[indiceMotifSelectionne].y, 
    //        s.translations[indiceMotifSelectionne].z);
    //    Calculer(s);
    //    return;
    //}
    //public void ChangerPositionY(Editeur_SchemaCalcul s)
    //{
    //    s.translations[indiceMotifSelectionne] = new Vector3(
    //        s.translations[indiceMotifSelectionne].x,
    //        (float)Convert.ToDouble(positionY.text),
    //        s.translations[indiceMotifSelectionne].z);
    //    Calculer(s);
    //    return;
    //}
    //public void ChangerPositionZ(Editeur_SchemaCalcul s)
    //{
    //    s.translations[indiceMotifSelectionne] = new Vector3(
    //        s.translations[indiceMotifSelectionne].x,
    //        s.translations[indiceMotifSelectionne].y,
    //        (float)Convert.ToDouble(positionZ.text));
    //    Calculer(s);
    //    return;
    //}
    //public void ChangerRotationX(Editeur_SchemaCalcul s)
    //{
    //    s.rotations[indiceMotifSelectionne] = new Vector3(
    //        (float)Convert.ToDouble(rotationX.text),
    //        s.rotations[indiceMotifSelectionne].y,
    //        s.rotations[indiceMotifSelectionne].z);
    //    Calculer(s);
    //    return;
    //}
    //public void ChangerRotationY(Editeur_SchemaCalcul s)
    //{
    //    s.rotations[indiceMotifSelectionne] = new Vector3(
    //        s.rotations[indiceMotifSelectionne].x,
    //        (float)Convert.ToDouble(rotationY.text),
    //        s.rotations[indiceMotifSelectionne].z);
    //    Calculer(s);
    //    return;
    //}
    //public void ChangerRotationZ(Editeur_SchemaCalcul s)
    //{
    //    s.rotations[indiceMotifSelectionne] = new Vector3(
    //       s.rotations[indiceMotifSelectionne].x,
    //       s.rotations[indiceMotifSelectionne].y,
    //    (float)Convert.ToDouble(rotationZ.text));
    //    Calculer(s);
    //    return;
    //}
    //public void ChangerTailleX(Editeur_SchemaCalcul s)
    //{
    //    s.tailles[indiceMotifSelectionne] = new Vector3(
    //        (float)Convert.ToDouble(tailleX.text), 0f, 0f);
    //    Calculer(s);
    //    return;
    //}

    //public void Calculer(Editeur_SchemaCalcul s)
    //{
    //    GeometrieProceduraleAPI.API_Editeur_CalculerSchema(s);
    //    GeometrieProceduraleAPI.API_Editeur_GenererBlocsSchema(CallbackRendu);
    //    return;
    //}

    private void CallbackRendu(Geometrie_Old_EditerMeshParPointeur g)
    {
        g.Afficher(filter, renderer);
        return;
    }
}
