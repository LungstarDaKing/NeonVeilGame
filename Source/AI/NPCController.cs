// NPCController.cs
// Main controller for NPC AI that manages state transitions
using FlaxEngine;

public class NPCController : Script
{
    // References to components
    private DetectorComponent _detector;
    private CharacterController _characterController;
    
    // State management
    private NPCState _currentState;
    
    // Movement speed (can be overridden by states)
    public float PatrolSpeed = 2.0f;
    public float InvestigationSpeed = 1.5f;
    public float ChaseSpeed = 4.0f;
    public float SearchSpeed = 2.0f;
    
    public override void OnStart()
    {
        _detector = GetComponent<DetectorComponent>();
        _characterController = GetComponent<CharacterController>();
        
        // Start in idle state
        ChangeState(new IdleState());
    }
    
    public override void OnUpdate()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }
    
    /// <summary>
    /// Changes the current AI state
    /// </summary>
    /// <param name="newState">The new state to transition to</param>
    public void ChangeState(NPCState newState)
    {
        // Exit current state
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        
        // Enter new state
        _currentState = newState;
        if (_currentState != null)
        {
            _currentState.Enter(this);
        }
    }
    
    // Helper method for movement that respects CharacterController
    public void Move(Vector3 velocity)
    {
        if (_characterController != null)
        {
            _characterController.Move(velocity * Time.DeltaTime);
        }
    }
    
    // Properties for easy access
    public DetectorComponent Detector => _detector;
    public CharacterController CharacterController => _characterController;
}
