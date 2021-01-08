using System;

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