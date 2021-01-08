using System;
using UnityEngine;

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