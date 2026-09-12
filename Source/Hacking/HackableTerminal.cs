// HackableTerminal.cs
// A terminal/computer that can be hacked for information or to disable systems
using FlaxEngine;

public class HackableTerminal : Script, IHackable
{
    // Terminal properties
    public string TerminalID = "TERM_001";
    public string Description = "Security Terminal";
    public bool ContainsCriticalData = false;
    public bool CanDisableCameras = false;
    public bool CanOpenDoors = false;
    
    // Hacking properties
    public bool IsBeingHacked { get; private set; }
    private CircuitRerouteMiniGame _hackingMiniGame;
    
    // Visuals
    private SpriteRenderer _screenRenderer; // For showing hacking progress
    private Color _normalColor = Color.Green;
    private Color _hackingColor = Color.Yellow;
    private Color _successColor = Color.Lime;
    private Color _failureColor = Color.Red;
    
    public override void OnStart()
    {
        _screenRenderer = GetComponent<SpriteRenderer>();
        if (_screenRenderer != null)
        {
            _screenRenderer.Color = _normalColor;
        }
        
        // Find or create hacking mini-game manager
        var hackingManager = Scene.GetActor("HackingManager");
        if (hackingManager != null)
        {
            _hackingMiniGame = hackingManager.GetComponent<CircuitRerouteMiniGame>();
        }
    }
    
    public override void OnUpdate()
    {
        // Update visual feedback based on hacking state
        if (_screenRenderer != null)
        {
            if (IsBeingHacked)
                _screenRenderer.Color = _hackingColor;
            else if (ContainsCriticalData && !IsLocked) // Assuming we track hacked state elsewhere
                _screenRenderer.Color = _successColor;
            else
                _screenRenderer.Color = _normalColor;
        }
    }
    
    // IHackable implementation
    public void StartHack()
    {
        if (IsBeingHacked) return;
        
        IsBeingHacked = true;
        Debug.Log($"Started hacking terminal: {TerminalID}");
        
        // Start the hacking mini-game
        if (_hackingMiniGame != null)
        {
            _hackingMiniGame.StartMinigame();
            Invoke(nameof(CheckHackResult), 0.1f);
        }
    }
    
    public void OnHackSuccess()
    {
        IsBeingHacked = false;
        Debug.Log($"Terminal {TerminalID} hacking successful!");
        
        // Apply effects based on terminal capabilities
        if (ContainsCriticalData)
        {
            // Grant player information, update story state, etc.
            Debug.Log("Critical data acquired!");
            // In a full implementation: StoryState.Instance.AcquireData(TerminalID);
        }
        
        if (CanDisableCameras)
        {
            // Disable nearby security cameras
            Debug.Log("Security cameras disabled!");
            // Find and disable camera components
        }
        
        if (CanOpenDoors)
        {
            // Unlock nearby doors
            Debug log("Nearby doors unlocked!");
            // Find and unlock door components
        }
    }
    
    public void OnHackFailure()
    {
        IsBeingHacked = false;
        Debug.Log($"Terminal {TerminalID} hacking failed!");
        // Maybe trigger alarm or increase heat
    }
    
    private void CheckHackResult()
    {
        // Simple random result for demonstration
        if (Random.Range(0, 100) < 60) // 60% success rate for terminals
            OnHackSuccess();
        else
            OnHackFailure();
        
        // Clean up the invoke
        CancelInvoke(nameof(CheckHackResult));
    }
}
