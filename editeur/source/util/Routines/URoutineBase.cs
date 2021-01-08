public enum EPrioriteRoutine
{
    ChaqueFrame
}

public abstract class URoutineBase
{
    public float Chrono { get; set; } = 0f;

    protected float m_dureeYield;

    public abstract void Tick(float dt);
}