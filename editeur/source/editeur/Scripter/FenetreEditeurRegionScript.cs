using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FenetreEditeurRegionScript
{
    public TMPro.TMP_InputField script;
    public List<string> fonctions;
    public string[] syntaxeFonctions;
    public Color couleurFonctions;

    private URoutineSimple r;

    public void LancerAnalyseurSyntaxe()
    {
        r = new URoutineSimple(EPrioriteRoutine.ChaqueFrame, Syntaxer);
        SRoutines.LancerRoutine(r);
        return;
    }

    public void ArreterAnalyseurSyntaxe()
    {
        SRoutines.ArreterRoutine(r);
        return;
    }

    private void Desyntaxer()
    {
        string s = script.text;
        string c = "#" + ColorUtility.ToHtmlStringRGBA(couleurFonctions);
        s = s.Replace("<color=" + c + ">", "");
        s = s.Replace("</color>", "");

        for (var i = 0; i < syntaxeFonctions.Length; i++)
        {
            s = s.Replace("<" + syntaxeFonctions[i] + ">", "");
            s = s.Replace("</" + syntaxeFonctions[i] + ">", "");
        }
        script.text = s;
        return;
    }

    public void Syntaxer()
    {
        Desyntaxer();
        string s = script.text;
        string c = "#" + ColorUtility.ToHtmlStringRGBA(couleurFonctions);
        for (var iFonction = 0; iFonction < fonctions.Count; iFonction++)
        {
            string avant = "<color=" + c + ">";
            string apres = "</color>";
            for (var iSyntaxeFonctions = 0; iSyntaxeFonctions < syntaxeFonctions.Length; iSyntaxeFonctions++)
            {
                avant = avant + "<" + syntaxeFonctions[iSyntaxeFonctions] + ">";
                apres = "</" + syntaxeFonctions[iSyntaxeFonctions] + ">" + apres;
            }

            s = s.Replace(fonctions[iFonction], avant + fonctions[iFonction] + apres);
        }
        script.text = s;
        return;
    }
}