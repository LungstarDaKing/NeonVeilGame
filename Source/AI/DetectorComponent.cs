// DetectorComponent.cs
// Shared vision and detection component for AI NPCs
using FlaxEngine;

public class DetectorComponent : Script
{
    // Detection settings
    public float ViewAngle = 110.0f; // degrees
    public float ViewRange = 15.0f;
    public float HeightOffset = 1.0f; // eye height
    public LayerMask DetectionLayers = LayerMask.Default; // layers to detect
    
    // Cached components
    private Transform _transform;
    
    public override void OnStart()
    {
        _transform = Transform;
    }
    
    /// <summary>
    /// Checks if a target is within the detector's field of view and range.
    /// </summary>
    /// <param name="target">The target to check.</param>
    /// <returns>True if target is detected.</returns>
    public bool Detect(Actor target)
    {
        if (target == null || !target.EnabledInHierarchy) return false;
        
        // Check range
        float distance = Vector3.Distance(_transform.Position, target.Position);
        if (distance > ViewRange) return false;
        
        // Check height difference (simple)
        float heightDiff = Mathf.Abs(target.Position.Y - (_transform.Position.Y + HeightOffset));
        if (heightDiff > 2.0f) return false; // too high or low
        
        // Check field of view
        Vector3 toTarget = (target.Position - _transform.Position).Normalized;
        float angle = Vector3.Angle(_transform.Forward, toTarget);
        if (angle > ViewAngle * 0.5f) return false;
        
        // Optional: line of sight check (raycast)
        // Uncomment if you want to check for obstacles
        /*
        var ray = new Ray(_transform.Position + Vector3.Up * HeightOffset, toTarget);
        if (Physics.Raycast(ray, out var hit, ViewRange, DetectionLayers))
        {
            if (hit.Actor != target) return false; // blocked by something else
        }
        */
        
        return true;
    }
    
    /// <summary>
    /// Gets detected actors within view (for simplicity, returns all actors in range/angle).
    /// In a full implementation, use overlap queries or maintain a list.
    /// </summary>
    /// <returns>List of detected actors.</returns>
    public Actor[] GetDetectedActors()
    {
        // Placeholder: in a real game, use physics overlap or spatial partitioning
        return new Actor[0];
    }
}
