// SearchState.cs
// NPC searches an area for the player after losing them
using FlaxEngine;
using System.Collections.Generic;

public class SearchState : NPCState
{
    public Vector3 SearchCenter;
    public float SearchRadius = 10.0f;
    public float SearchTime = 10.0f;
    private float _timer;
    private List<Vector3> _searchPoints = new List<Vector3>();
    private int _currentSearchIndex = 0;
    public float SearchSpeed = 2.0f;
    
    public override void Enter(NPCController controller)
    {
        base.Enter(controller);
        _timer = 0.0f;
        _currentSearchIndex = 0;
        Debug.Log("NPC entering Search state");
        
        // Generate search points in a circle around search center
        int pointCount = 8;
        for (int i = 0; i < pointCount; i++)
        {
            float angle = (float)i / pointCount * 3.14159f * 2.0f;
            var offset = new Vector3(Mathf.Cos(angle) * SearchRadius, 0, Mathf.Sin(angle) * SearchRadius);
            _searchPoints.Add(SearchCenter + offset);
        }
    }
    
    public override void Update()
    {
        base.Update();
        _timer += Time.DeltaTime;
        
        if (_searchPoints.Count > 0)
        {
            var targetPoint = _searchPoints[_currentSearchIndex];
            var direction = (targetPoint - _controller.transform.Position).Normalized;
            direction.Y = 0;
            _controller.transform.Position += direction * _controller.SearchSpeed * Time.DeltaTime;
            
            if (Vector3.Distance(_controller.transform.Position, targetPoint) < 1.0f)
            {
                _currentSearchIndex = (_currentSearchIndex + 1) % _searchPoints.Count;
            }
        }
        
        // Check if we found the player
        var detector = _controller.GetComponent<DetectorComponent>();
        if (detector != null)
        {
            var player = Scene.GetActor("Player");
            if (player != null && detector.Detect(player))
            {
                _controller.ChangeState(new AlertState());
            }
        }
        
        // If search time expires, return to patrol
        if (_timer >= SearchTime)
        {
            _controller.ChangeState(new PatrolState());
        }
    }
}
