using System;
using UnityEngine;
using UnityEngine.UI;


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
