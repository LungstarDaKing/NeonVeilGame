// AlertState.cs
// NPC knows player is there and is actively pursuing or searching
using FlaxEngine;

public class AlertState : NPCState
{
    public float AlertTime = 5.0f;
    private float _timer;
    public float ChaseSpeed = 4.0f;
    public float SearchRadius = 10.0f;
    
    public override void Enter(NPCController controller)
    {
        base.Enter(controller);
        _timer = 0.0f;
        Debug.Log("NPC entering Alert state");
    }
    
    public override void Update()
    {
        base.Update();
        _timer += Time.DeltaTime;
        
        var player = Scene.GetActor("Player");
        if (player != null)
        {
            var detector = _controller.GetComponent<DetectorComponent>();
            if (detector != null && detector.Detect(player))
            {
                // Chase the player
                var direction = (player.Position - _controller.transform.Position).Normalized;
                direction.Y = 0;
                _controller.transform.Position += direction * _controller.ChaseSpeed * Time.DeltaTime;
                _timer = 0.0f; // Reset timer as long as we can see player
            }
            else
            {
                // Last known position search
                var direction = (player.Position - _controller.transform.Position).Normalized;
                direction.Y = 0;
                _controller.transform.Position += direction * _controller.ChaseSpeed * 0.5f * Time.DeltaTime; // Slower when searching
            }
        }
        
        // If we lose the player for too long, go to search
        if (_timer >= AlertTime)
        {
            _controller.ChangeState(new SearchState { SearchCenter = _controller.transform.Position });
        }
    }
}
