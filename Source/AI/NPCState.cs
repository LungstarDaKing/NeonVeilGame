// NPCState.cs
// Base class for all NPC AI states
using FlaxEngine;

public abstract class NPCState : Script
{
    protected NPCController _controller;
    
    public virtual void Enter(NPCController controller)
    {
        _controller = controller;
    }
    
    public virtual void Update() { }
    
    public virtual void Exit() { }
}
