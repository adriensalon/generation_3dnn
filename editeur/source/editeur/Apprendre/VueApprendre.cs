using System;
using UnityEngine;
using UnityEngine.UI;

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