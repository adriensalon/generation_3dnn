using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FenetreEditeurSchemaMotifs
{
    public Color couleurBlocNonSelectionne;
    public Color couleurBlocSelectionne;
    public CanvasScaler ecran;
    public Scrollbar scrollbar;
    public GameObject prefab;
    public float espacementY;
    public Vector2 position;

    private readonly List<GameObject> emplacements = new List<GameObject>();
    private int emplacementSelectionne = -1;
    private float dernierePositionScrollbar;
    public float amplitudeScrollbar = 1f;

    public void DeplacerScrollbar()
    {
        for (int i = 0; i < emplacements.Count; i++)
        {
            emplacements[i].GetComponentInChildren<Image>().GetComponent<RectTransform>().localPosition += new Vector3(0f, amplitudeScrollbar * emplacements.Count * (scrollbar.value - dernierePositionScrollbar), 0f);
        }
        dernierePositionScrollbar = scrollbar.value;
        return;
    }
    //}
    //public void AjouterEmplacementMotif(Editeur_SchemaCalcul s)
    //{
    //    s.tags.Add("");
    //    s.translations.Add(new Vector3());
    //    s.rotations.Add(new Vector3());
    //    s.tailles.Add(new Vector3());
    //    GameObject go = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
    //    go.GetComponentInChildren<Image>().GetComponent<RectTransform>().localPosition += (Vector3)new Vector3(position.x, position.y, 0f) - (Vector3)(emplacements.Count * new Vector3(0f, espacementY, 0f));
    //    emplacements.Add(go);
    //    return;
    //}
    public void RetirerEmplacementMotif()
    {
        UnityEngine.Object.Destroy(emplacements[emplacementSelectionne]);
        emplacements.RemoveAt(emplacementSelectionne);
        for (int i = emplacementSelectionne; i < emplacements.Count; i++)
        {
            emplacements[i].GetComponentInChildren<Image>().GetComponent<RectTransform>().localPosition += (Vector3)new Vector3(0f, espacementY, 0f);
        }
        return;
    }










    public void ClicBoutonGaucheSouris(FenetreEditeurSchemaMotifSelectionne m)
    {
        if (Input.GetMouseButtonDown(0))            
        {
            for (int iBloc = 0; iBloc < emplacements.Count; iBloc++)
            {
                RectTransform r = emplacements[iBloc].GetComponentInChildren<Image>().GetComponent<RectTransform>();
                float ratio = Screen.width / ecran.referenceResolution.x;
                Vector2 positionZone = new Vector2(r.localPosition.x, r.localPosition.y);
                float xMin = Screen.width / 2f + ratio * positionZone.x - ratio * r.rect.width / 2f;
                float yMin = Screen.height / 2f + ratio * positionZone.y - ratio * r.rect.height / 2f;
                float xMax = Screen.width / 2f + ratio * positionZone.x + ratio * r.rect.width / 2f;
                float yMax = Screen.height / 2f + ratio * positionZone.y + ratio * r.rect.height / 2f;
                Vector2 positionSourisPixels = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                bool estDedansMin = positionSourisPixels.x > xMin && positionSourisPixels.y > yMin;
                bool estDedansMax = positionSourisPixels.x < xMax && positionSourisPixels.y < yMax;

                //Debug.Log(positionSourisPixels);

                if (estDedansMin && estDedansMax)
                {
                    r.GetComponent<Image>().color = couleurBlocSelectionne;

                    m.racine.SetActive(true);
                    m.ChangerIndiceMotifSelectionne(iBloc);

                    emplacementSelectionne = iBloc;
                    Debug.Log(iBloc);
                }
                else
                {
                    r.GetComponent<Image>().color = couleurBlocNonSelectionne;
                    //m.racine.SetActive(false);
                    //m.ChangerIndiceMotifSelectionne(-1);

                    //emplacementSelectionne = -1;
                }
            }
        }
    }
}
