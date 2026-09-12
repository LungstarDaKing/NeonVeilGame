// HackableDoor.cs
// A door that can be hacked to open/unlock
using FlaxEngine;

public class HackableDoor : Script, IHackable
{
    // Door properties
    public float OpenAngle = 90.0f; // degrees to open
    public float OpenSpeed = 50.0f; // degrees per second
    public float CloseSpeed = 30.0f;
    public bool IsLocked = true;
    public bool IsOpen = false;
    
    // Hacking properties
    public bool IsBeingHacked { get; private set; }
    private CircuitRerouteMiniGame _hackingMiniGame;
    
    // Visuals
    private Transform _doorTransform;
    private float _currentAngle; // 0 = closed, OpenAngle = open
    
    public override void OnStart()
    {
        _doorTransform = Transform;
        _currentAngle = IsOpen ? OpenAngle : 0;
        UpdateDoorTransform();
        
        // Find or create hacking mini-game manager
        var hackingManager = Scene.GetActor("HackingManager");
        if (hackingManager != null)
        {
            _hackingMiniGame = hackingManager.GetComponent<CircuitRerouteMiniGame>();
        }
    }
    
    public override void OnUpdate()
    {
        // Update door animation
        float targetAngle = IsOpen ? OpenAngle : (IsLocked ? 0 : 0); // If not open and not locked, still closed
        if (IsLocked && !IsOpen) targetAngle = 0;
        if (!IsLocked && !IsOpen) targetAngle = 0; // Unlocked but not opened yet
        
        // Actually, let's simplify: if unlocked, player can open it manually or it opens automatically
        // For now, we'll just handle the locked state
        
        float speed = IsOpen ? OpenSpeed : CloseSpeed;
        if (Mathf.Abs(_currentAngle - targetAngle) > 0.1f)
        {
            float delta = speed * Time.DeltaTime;
            if (Mathf.Abs(targetAngle - _currentAngle) < delta)
                _currentAngle = targetAngle;
            else
                _currentAngle += Mathf.Sign(targetAngle - _currentAngle) * delta;
            
            UpdateDoorTransform();
        }
    }
    
    private void UpdateDoorTransform()
    {
        // Rotate door around Y-axis (hinge)
        _doorTransform.SetLocalRotation(Quaternion.RotationY(Mathf.DegreesToRadians(_currentAngle)));
    }
    
    // IHackable implementation
    public void StartHack()
    {
        if (!IsLocked || IsBeingHacked) return;
        
        IsBeingHacked = true;
        Debug.Log("Started hacking door...");
        
        // Start the hacking mini-game
        if (_hackingMiniGame != null)
        {
            _hackingMiniGame.StartMinigame();
            // In a full implementation, we'd pass callbacks for success/failure
            // For now, we'll use a simple approach
            Invoke(nameof(CheckHackResult), 0.1f); // Check after a short delay
        }
    }
    
    public void OnHackSuccess()
    {
        IsBeingHacked = false;
        IsLocked = false;
        Debug.Log("Door hacking successful! Door unlocked.");
        // Optionally open the door automatically
        // IsOpen = true;
    }
    
    public void OnHackFailure()
    {
        IsBeingHacked = false;
        Debug.Log("Door hacking failed!");
        // Maybe trigger alarm or increase heat
    }
    
    private void CheckHackResult()
    {
        // Simple random result for demonstration
        // In reality, this would be handled by the mini-game callbacks
        if (_hackingMiniGame != null && _hackingMiniGame.GetType().GetField("_isSolved", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic) != null)
        {
            // This is hacky - better to use events/callbacks
            var isSolvedField = _hackingMiniGame.GetType().GetField("_isSolved", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (isSolvedField != null)
            {
                bool isSolved = (bool)isSolvedField.GetValue(_hackingMiniGame);
                if (isSolved)
                    OnHackSuccess();
                else
                    OnHackFailure();
            }
        }
        else
        {
            // Fallback: random result
            if (Random.Range(0, 100) < 70) // 70% success rate
                OnHackSuccess();
            else
                OnHackFailure();
        }
        
        // Clean up the invoke
        CancelInvoke(nameof(CheckHackResult));
    }
}
