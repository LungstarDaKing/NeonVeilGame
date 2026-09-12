// PatrolState.cs
// NPC follows a patrol route
using FlaxEngine;
using System.Collections.Generic;

public class PatrolState : NPCState
{
    public List<Vector3> PatrolPoints = new List<Vector3>();
    public float PatrolSpeed = 2.0f;
    public float WaypointRadius = 1.0f;
    private int _currentPointIndex = 0;
    
    public override void Enter(NPCController controller)
    {
        base.Enter(controller);
        Debug.Log("NPC entering Patrol state");
        
        // Initialize patrol points if empty (for testing)
        if (PatrolPoints.Count == 0)
        {
            // Create a simple square patrol around the NPC
            var pos = controller.transform.Position;
            PatrolPoints.Add(pos);
            PatrolPoints.Add(pos + new Vector3(5, 0, 0));
            PatrolPoints.Add(pos + new Vector3(5, 0, 5));
            PatrolPoints.Add(pos + new Vector3(0, 0, 5));
        }
    }
    
    public override void Update()
    {
        base.Update();
        
        if (PatrolPoints.Count == 0) return;
        
        // Move towards current waypoint
        var targetPoint = PatrolPoints[_currentPointIndex];
        var direction = (targetPoint - _controller.transform.Position).Normalized;
        direction.Y = 0; // Keep movement on XZ plane
        
        _controller.transform.Position += direction * _controller.PatrolSpeed * Time.DeltaTime;
        
        // Check if we reached the waypoint
        if (Vector3.Distance(_controller.transform.Position, targetPoint) < WaypointRadius)
        {
            _currentPointIndex = (_currentPointIndex + 1) % PatrolPoints.Count;
        }
        
        // Check for player detection
        DetectPlayer();
    }
    
    private void DetectPlayer()
    {
        var detector = _controller.GetComponent<DetectorComponent>();
        if (detector != null)
        {
            var player = Scene.GetActor("Player");
            if (player != null && detector.Detect(player))
            {
                _controller.ChangeState(new SuspiciousState { LastKnownPlayerPosition = player.Position });
            }
        }
    }
}
