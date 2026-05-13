
using System;

public abstract class ObjectiveRuntime
{
    public ObjectiveState State { get; protected set; }
    public string description;

    public virtual int CurrentAmount => 0;
    public virtual int RequiredAmount => 0;

    public Action OnObjectiveCompleted;
    public Action OnObjectiveProgressed;

    protected void NotifyCompleted()
    {
        OnObjectiveCompleted?.Invoke();
    }

    protected void NotifyProgress()
    {
        OnObjectiveProgressed?.Invoke();
    }

    public abstract void Activate();

    public abstract void Deactivate();
}

public enum ObjectiveState
{
    Inactive,
    Active,
    Completed,
    Failed
}