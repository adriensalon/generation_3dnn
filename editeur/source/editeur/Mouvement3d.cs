using System;
using UnityEngine;

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