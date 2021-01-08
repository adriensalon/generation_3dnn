using System;
using System.Collections.Generic;
using UnityEngine;

public enum EPrioriteRoutine
{
    ChaqueFrame
}

public static class SRoutines
{
    public const float PRIORITE_ABSOLUE = 0f;
    public const float PRIORITE_MODIFIABLES = 0.2f;
    public const float PRIORITE_INTERFACE = 0.3f;

    private static readonly Dictionary<EPrioriteRoutine, float> m_dureesYield = new Dictionary<EPrioriteRoutine, float>()
    {
        {EPrioriteRoutine.ChaqueFrame, 2f}
    };
    private static readonly HashSet<URoutineBase> m_routines = new HashSet<URoutineBase>();
    private static List<URoutineBase> m_del = new List<URoutineBase>();
    private static List<URoutineBase> m_add = new List<URoutineBase>();
    private static float m_tempsMemoire = 0f;
    private static float m_temps = 0f;
    private static bool m_verrouillageRoutines = false;

    public static float ObtenirDureeYield(EPrioriteRoutine priorite)
    {
        return m_dureesYield[priorite];
    }

    public static void LancerRoutine(URoutineBase routine)
    {
        if (m_verrouillageRoutines)
        {
            if (m_add.Contains(routine)) Debug.Log("routine deja enregistree pendant verrouillage");
            m_add.Add(routine);
        }
        else
        {
            if (m_routines.Contains(routine)) Debug.Log("routine deja enregistree");
            m_routines.Add(routine);
        }        
        return;
    }
    public static void ArreterRoutine(URoutineBase routine)
    {
        if (m_verrouillageRoutines)
        {
            if (m_del.Contains(routine)) Debug.Log("routine pas encore enregistree pendant verrouillage");
            m_del.Add(routine);
        }
        else
        {
            if (!m_routines.Contains(routine)) Debug.Log("routine pas encore enregistree");
            m_routines.Remove(routine);
        }
        return;
    }

    public static void Boucle()
    {
        m_tempsMemoire = m_temps;
        m_temps = Time.time;
        float dt = m_temps - m_tempsMemoire;
        foreach (URoutineBase routine in m_routines)
        {
            m_verrouillageRoutines = true;
            routine.Tick(dt);
            m_verrouillageRoutines = false;
        }
        for (int i = 0; i < m_del.Count; i++)
        {
            if (!m_routines.Contains(m_del[i])) Debug.Log("routine non enregistree");
            m_routines.Remove(m_del[i]);
        }
        m_del = new List<URoutineBase>();
        for (int i = 0; i < m_add.Count; i++)
        {
            if (m_routines.Contains(m_add[i])) Debug.Log("routine deja enregistree");
            m_routines.Add(m_add[i]);
        }
        m_add = new List<URoutineBase>();
        return;
    }
}

public abstract class URoutineBase
{
    public float Chrono { get; set; } = 0f;

    protected float m_dureeYield;

    public abstract void Tick(float dt);
}

public class URoutineSimple : URoutineBase
{
    private float m_accumulation = 0f;
    private readonly Action m_action;

    public URoutineSimple(EPrioriteRoutine priorite, Action action)
    {
        m_dureeYield = SRoutines.ObtenirDureeYield(priorite);
        m_action = action;
        return;
    }

    public override void Tick(float dt)
    {
        Chrono += dt;
        m_accumulation += dt;
        if (m_accumulation > m_dureeYield)
        {
            m_accumulation -= m_dureeYield;
            m_action();
        }
        return;
    }
}