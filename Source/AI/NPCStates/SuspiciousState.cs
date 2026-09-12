// SuspiciousState.cs
// NPC detected something unusual, investigating
using FlaxEngine;

public class SuspiciousState : NPCState
{
    public Vector3 LastKnownPlayerPosition;
    public float SuspiciousTime = 3.0f;
    private float _timer;
    public float InvestigationSpeed = 1.5f;
    
    public override void Enter(NPCController controller)
    {
        base.Enter(controller);
        _timer = 0.0f;
        Debug.Log("NPC entering Suspicious state");
    }
    
    public override void Update()
    {
        base.Update();
        _timer += Time.DeltaTime;
        
        // Move towards last known player position
        var direction = (LastKnownPlayerPosition - _controller.transform.Position).Normalized;
        direction.Y = 0;
        _controller.transform.Position += direction * _controller.InvestigationSpeed * Time.DeltaTime;
        
        // Check if we lost the player or time expired
        if (_timer >= SuspiciousTime)
        {
            _controller.ChangeState(new AlertState());
        }
        else
        {
            // Check if we can see the player again
            var detector = _controller.GetComponent<DetectorComponent>();
            if (detector != null)
            {
                var player = Scene.GetActor("Player");
                if (player != null && detector.Detect(player))
                {
                    _controller.ChangeState(new AlertState());
                }
            }
        }
    }
}
