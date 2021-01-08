using System;
using UnityEngine;
using UnityEngine.UI;




[Serializable]
public class VueImporter
{
    public GameObject gui;
    public GameObject guiFichiers;
    public GameObject guiSauvegarder;
    public GameObject guiGenerer;
    public GameObject guiBoutonArreterPrevisualisation;
    public GameObject cube;

    public GameObject xDebut;
    public GameObject xExtension;
    public GameObject xFin;


    public MeshFilter modeleMesh;
    public MeshRenderer modeleRendu;

    public InputField urlMesh;
    public InputField urlTextureLabel;
    public InputField urlTextureTarget;

    public Dropdown categorie;
    public InputField nomNouvelleCategorie;
    public Toggle estNouvelleCategorie;

    public Slider geometrieDebutX;
    public Slider geometrieExtensionX;
    public Slider geometrieFinX;

    public Slider previsualisationTailleX;
    public InputField nomFichier;
}