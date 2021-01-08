using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;






[Serializable]
public class ParametresCalculGeometrie // interface pour creer un motif a
{
    public string nomFichier = "";
    public List<Vector3> positions;
    public List<Vector3> normales;
    public List<Vector2> uvs;
    public List<Vector3Int> indices;
    public float tailleX;
    public float tailleY;
    public float tailleZ;
    public float xA = 0.25f;
    public float xB = 0.5f;
    public float xC = 0.75f;
}

[Serializable]
public class Mouvement3d
{
    public GameObject cible;

    private Vector2 memoireSouris;
    private bool etaitClicDroitDerniereFrame = false;

    public void EchelleMoletteSouris()
    {
        float e = 0.01f * Input.mouseScrollDelta.y;
        cible.transform.localScale += (Vector3)new Vector3(e, e, e);
    }
    public void RotationClicDroit()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!etaitClicDroitDerniereFrame)
            {
                etaitClicDroitDerniereFrame = true;
                memoireSouris = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                return;
            }
            etaitClicDroitDerniereFrame = true;
            Vector2 pSouris = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 difference = 0.1f * (memoireSouris - pSouris);
            cible.transform.rotation = Quaternion.Euler(
                                                cible.transform.rotation.eulerAngles.x - difference.y,
                                                cible.transform.rotation.eulerAngles.y + difference.x,
                                                cible.transform.rotation.eulerAngles.z);
            memoireSouris = pSouris;
        }
    }
}

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













[Serializable]
public class VueMenu
{
    public GameObject menu;
    public GameObject start;
    public InputField urlChargerProjet;
    public InputField urlCreerProjet;

    public void ActiverStart()
    {
        menu.SetActive(false);
        start.SetActive(true);
    }
    public void DesactiverStart()
    {
        menu.SetActive(true);
        start.SetActive(false);
    }
}
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
[Serializable]
public class VueApprendre
{
    public GameObject gui;
    public GameObject rendu;
    public GameObject fenetreResultats;
    public GameObject fenetreGenerer;

    public Dropdown categories;
    public Dropdown ias;

    public Dropdown geometriePrevisualiser;

    public Toggle estNouvelleIa;
    public InputField nomNouvelleIa;

    public Toggle utiliserGpu;
    public Toggle continuer;


    public Text nombreEpochs;
    public Text boutonCommencerInterrompre;

}
[Serializable]
public class VueScripter
{
    public GameObject gui;
    public GameObject rendu;

    public Mouvement3d mouvement3d;
    public FenetreEditeurRegionFichier fenetreFichier;
    public FenetreEditeurRegionScript fenetreScript;

    public void Activer()
    {
        gui.SetActive(true);
        rendu.SetActive(true);
        fenetreScript.LancerAnalyseurSyntaxe();
        return;
    }
    public void Desactiver()
    {
        gui.SetActive(false);
        rendu.SetActive(false);
        fenetreScript.ArreterAnalyseurSyntaxe();
        return;
    }
}










public class BehaviourEditeur : MonoBehaviour
{
    public AppareilPhoto appareilPhoto;

    public VueMenu vueMenu;
    public VueImporter vueImporter;
    public VueApprendre vueApprendre;
    //public VueScripter vueScripter;

    private ParametresCalculGeometrie m_calculGeometrie = new ParametresCalculGeometrie();
    private string urlProjet;
    private Mesh meshImporte;
    private bool entrainementEnCours = false;

    



    private bool fichiersImportes = false;
    private bool geometrieCalculee = false;

    private void _Callback_NomsCategories(List<string> noms)
    {
        List<Dropdown.OptionData> liste = new List<Dropdown.OptionData>();
        foreach (string s in noms)
        {
            liste.Add(new Dropdown.OptionData(s));
        }
        vueImporter.categorie.ClearOptions();
        vueImporter.categorie.AddOptions(liste);
        vueApprendre.categories.ClearOptions();
        vueApprendre.categories.AddOptions(liste);
        return;
    }
    private void _Callback_NomsGeometries(List<string> noms)
    {
        List<Dropdown.OptionData> liste = new List<Dropdown.OptionData>();
        foreach (string s in noms)
        {
            liste.Add(new Dropdown.OptionData(s));
        }
        vueApprendre.geometriePrevisualiser.ClearOptions();
        vueApprendre.geometriePrevisualiser.AddOptions(liste);
        return;
    }
    private void _Callback_NomsIas(List<string> noms)
    {
        List<Dropdown.OptionData> liste = new List<Dropdown.OptionData>();
        foreach (string s in noms)
        {
            liste.Add(new Dropdown.OptionData(s));
        }
        vueApprendre.ias.ClearOptions();
        vueApprendre.ias.AddOptions(liste);
        return;
    }
    private void _Callback_Resultats(int nombreEpochs, float gLoss, float dLoss, float lossL1, float ganLoss)
    {
        vueApprendre.nombreEpochs.text = nombreEpochs.ToString();
        // DD ici !!!!!
        // DD ici !!!!!
        // DD ici !!!!!
        // DD ici !!!!!
        

        if (!entrainementEnCours) // dernier callback, on a arrete l'entrainement
        {
            vueApprendre.fenetreResultats.SetActive(false);
            if (nombreEpochs > 10)
            {
                vueApprendre.fenetreGenerer.SetActive(true);

                geometrie_procedurale.ObtenirNomsGeometries(
                    vueApprendre.categories.itemText.text, 
                    _Callback_NomsGeometries);
            }
        }
        return;
    }
    private void _Callback_Geometrie_Importer(Mesh m)
    {
        vueImporter.modeleMesh.sharedMesh = m;

        m_calculGeometrie.tailleX = m.bounds.size.x; // local axis aligned https://docs.unity3d.com/ScriptReference/Mesh-bounds.html
        m_calculGeometrie.tailleY = m.bounds.size.y;
        m_calculGeometrie.tailleZ = m.bounds.size.z;

        vueImporter.cube.transform.localScale = new Vector3(m.bounds.size.x, m.bounds.size.y, m.bounds.size.z);
        vueImporter.modeleMesh.transform.localPosition = -m.bounds.center;

        return;
    }




    public void _VueMenu()
    {

    }
    public void _VueImporter()
    {
        vueApprendre.gui.SetActive(false);
        if (fichiersImportes)
        {
            vueImporter.gui.SetActive(true);
            if (geometrieCalculee)
            {
                vueImporter.guiSauvegarder.SetActive(true);
                vueImporter.guiGenerer.SetActive(true);
            }
        }
        else
        {
            vueImporter.guiFichiers.SetActive(true);
        }
        return;
    }
    public void _VueApprendre()
    {
        vueImporter.gui.SetActive(false);
        vueImporter.guiFichiers.SetActive(false);
        vueImporter.guiSauvegarder.SetActive(false);
        vueImporter.guiGenerer.SetActive(false);
        vueApprendre.gui.SetActive(true);
        return;
    }
    public void _VueScripter()
    {

    }



    public void Menu_ChargerProjet()
    {
        urlProjet = vueMenu.urlChargerProjet.text;
        if (!urlProjet.EndsWith("/")) urlProjet += "/";
        geometrie_procedurale.ChargerProjet(vueMenu.urlChargerProjet.text);
        vueMenu.DesactiverStart();
        vueMenu.menu.SetActive(true);
        vueMenu.start.SetActive(false);
        geometrie_procedurale.ObtenirNomsCategories(_Callback_NomsCategories);
        _VueImporter();
        return;
    }
    public void Menu_CreerProjet()
    {
        urlProjet = vueMenu.urlCreerProjet.text;
        if (!urlProjet.EndsWith("/")) urlProjet += "/";
        geometrie_procedurale.CreerProjet(vueMenu.urlCreerProjet.text);
        vueMenu.DesactiverStart();
        vueMenu.menu.SetActive(true);
        vueMenu.start.SetActive(false);
        _VueImporter();
        return;
    }



    public void Importer_ImporterFichiers()
    {
        meshImporte = FichierObj.Importer(vueImporter.urlMesh.text);

        vueImporter.modeleMesh.sharedMesh = meshImporte;
        //
        Texture2D t_label = FichierJpg.Importer(vueImporter.urlTextureLabel.text);
        Texture2D t_target = FichierJpg.Importer(vueImporter.urlTextureTarget.text);


        vueImporter.modeleRendu.sharedMaterial.SetTexture("_MainTex", t_label);
        //
        vueImporter.guiFichiers.SetActive(false);
        vueImporter.gui.SetActive(true);

        m_calculGeometrie.tailleX = meshImporte.bounds.size.x; // local axis aligned https://docs.unity3d.com/ScriptReference/Mesh-bounds.html
        m_calculGeometrie.tailleY = meshImporte.bounds.size.y; 
        m_calculGeometrie.tailleZ = meshImporte.bounds.size.z;

        vueImporter.cube.transform.localScale = new Vector3(meshImporte.bounds.size.x, meshImporte.bounds.size.y, meshImporte.bounds.size.z);
        vueImporter.modeleMesh.transform.localPosition = -meshImporte.bounds.center;

        fichiersImportes = true;
    }
    public void Importer_DefinirSiNouvelleCategorie()
    {
        vueImporter.categorie.interactable = !vueImporter.estNouvelleCategorie.isOn;
        return;
    }
    public void Importer_ChangerParametresGeometrie()
    {
        m_calculGeometrie.xA = vueImporter.geometrieDebutX.value;
        m_calculGeometrie.xB = vueImporter.geometrieExtensionX.value;
        m_calculGeometrie.xC = vueImporter.geometrieFinX.value;
        vueImporter.xDebut.transform.localPosition = new Vector3(m_calculGeometrie.xA, 0f, 0f);
        vueImporter.xExtension.transform.localPosition = new Vector3(m_calculGeometrie.xB, 0f, 0f);
        vueImporter.xFin.transform.localPosition = new Vector3(m_calculGeometrie.xC, 0f, 0f);
        return;
    }
    public void Importer_CalculerGeometrie()
    {
        Vector3 taille = vueImporter.modeleMesh.sharedMesh.bounds.size;
        Vector3 ajustementX = new Vector3(m_calculGeometrie.xA, m_calculGeometrie.xB, m_calculGeometrie.xC);

        geometrie_procedurale.CalculerGeometrie(
            vueImporter.modeleMesh.sharedMesh,
            taille,
            ajustementX);

        geometrie_procedurale.GenererGeometrie(
            _Callback_Geometrie_Importer,
            new Vector3(1f, 0f, 0f));

        vueImporter.cube.SetActive(false);

        geometrieCalculee = true;
        return;
    }
    public void Importer_PrevisualiserGeometrie()
    {
        geometrie_procedurale.GenererGeometrie(
            _Callback_Geometrie_Importer,
            new Vector3(vueImporter.previsualisationTailleX.value, 0f, 0f)); // diviser par taille mesh

        return;
    }
    public void Importer_ArreterPrevisualiserGeometrie()
    {
        vueImporter.modeleMesh.sharedMesh = meshImporte;

        vueImporter.cube.SetActive(true);

        geometrieCalculee = false;
        return;
    }
    public void Importer_Sauvegarder()
    {

        string nomCategorie;
        if (vueImporter.estNouvelleCategorie.isOn)
        {
            nomCategorie = vueImporter.nomNouvelleCategorie.text;
        }
        else
        {
            nomCategorie = vueImporter.categorie.itemText.text;
        }
        string nomFichier = vueImporter.nomFichier.text;

        geometrie_procedurale.SauvegarderGeometrie(
            nomCategorie,
            nomFichier);


        // enregistrer 2 tex
        appareilPhoto.PrendreLes6PhotosPack(urlProjet + "/data/" + nomCategorie + "/apprentissage/textures_label/" + nomFichier + ".jpg");
        // 2eme texture
        appareilPhoto.PrendreLes6PhotosPack(urlProjet + "/data/" + nomCategorie + "/apprentissage/textures_target/" + nomFichier + ".jpg");


        return;
    }






    public void Apprendre_ChangerCategorie()
    {
        string nomCategorie = vueApprendre.categories.itemText.text;
        geometrie_procedurale.ObtenirNomsIas(nomCategorie, _Callback_NomsIas);
        return;
    }
    public void Apprendre_CommencerArreterEntrainement()
    {
        if (!entrainementEnCours)
        {
            string nomCategorie = vueApprendre.categories.itemText.text;
            string nomIa;
            if (vueApprendre.estNouvelleIa.isOn)
            {
                nomIa = vueApprendre.nomNouvelleIa.text;
            }
            else
            {
                nomIa = vueApprendre.ias.itemText.text;
            }

            vueApprendre.fenetreResultats.SetActive(true);

            geometrie_procedurale.LancerEntrainementTexture(
                nomCategorie,
                nomIa,
                vueApprendre.utiliserGpu.isOn,
                vueApprendre.continuer.isOn);

            vueApprendre.boutonCommencerInterrompre.text = "arrêter l'entraînement";
            return;
        }

        vueApprendre.boutonCommencerInterrompre.text = "commencer l'entraînement";
        entrainementEnCours = false;
        //vueApprendre.fe
        geometrie_procedurale.InterrompreEntrainementTexture();
        return;
    }
    public void Apprendre_GenererTexture()
    {
        //geometrie_procedurale.Ge
    }













    private void Start()
    {
        vueMenu.menu.SetActive(false);
        vueMenu.start.SetActive(true);
        vueImporter.guiFichiers.SetActive(false);
        vueImporter.gui.SetActive(false);
        vueApprendre.gui.SetActive(false);
        return;
    }
    private void Update()
    {
        if (vueImporter.modeleMesh.sharedMesh != null)
        {
            vueImporter.modeleMesh.sharedMesh.RecalculateBounds();
            vueImporter.cube.transform.localScale = new Vector3(vueImporter.modeleMesh.sharedMesh.bounds.size.x, vueImporter.modeleMesh.sharedMesh.bounds.size.y, vueImporter.modeleMesh.sharedMesh.bounds.size.z);
        }
        //vueImporter.mouvement3d.RotationClicDroit();
        //vueImporter.mouvement3d.EchelleMoletteSouris();

        //vueApprendre.fenetreBlocs.ClicBoutonGaucheSouris(vueApprendre.fenetreMotif);
        //vueApprendre.mouvement3d.RotationClicDroit();
        //vueApprendre.mouvement3d.EchelleMoletteSouris();

        //vueScripter.mouvement3d.RotationClicDroit();
        //vueScripter.mouvement3d.EchelleMoletteSouris();

        if (entrainementEnCours)
        {
            geometrie_procedurale.ObtenirResultatsTexture(_Callback_Resultats);
        }


        SRoutines.Boucle();
        return;
    }
}