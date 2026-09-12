// IdleState.cs
// NPC is idle, waiting or performing ambient animations
using FlaxEngine;

public class IdleState : NPCState
{
    public float IdleTime = 2.0f;
    private float _timer;
    
    public override void Enter(NPCController controller)
    {
        base.Enter(controller);
        _timer = 0.0f;
        // Play idle animation
        Debug.Log("NPC entering Idle state");
    }
    
    public override void Update()
    {
        base.Update();
        _timer += Time.DeltaTime;
        
        // Transition to patrol after idle time
        if (_timer >= IdleTime)
        {
            _controller.ChangeState(new PatrolState());
        }
    }
}
