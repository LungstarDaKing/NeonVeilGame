// HUDManager.cs
// Manages the Heads-Up Display
using FlaxEngine;
using System;

public class HUDManager : Script
{
    // Singleton instance
    public static HUDManager Instance { get; private set; }
    
    // References to UI elements (would be set in editor)
    public UIControl HealthBar;
    public UIControl EnergyBar;
    public UIControl HeatBar;
    public UIControl HealthText;
    public UIControl EnergyText;
    public UIControl HeatText;
    
    public UIControl Minimap;
    public UIControl MinimapPlayerIcon;
    
    public UIControl HackingInterface;
    public UIText HackingTimeText;
    public UIControl HackingProgressBar;
    
    // References to game systems
    private CipherController _playerController;
    private StoryState _storyState;
    
    // Player stats (would ideally come from player controller or stats component)
    private float _maxHealth = 100f;
    private float _currentHealth = 100f;
    private float _maxEnergy = 100f;
    private float _currentEnergy = 100f;
    private float _maxHeat = 100f;
    private float _currentHeat = 0f;
    
    // Hacking state
    private bool _isHacking = false;
    private float _hackingTimeRemaining = 0f;
    private float _hackingTimeLimit = 15f; // Default from mini-game
    
    public override void OnStart()
    {
        // Set up singleton
        if (Instance != null && Instance != this)
        {
            Destroy(Actor);
            return;
        }
        Instance = this;
        
        // Find game systems
        FindGameSystems();
        
        // Initialize HUD
        InitializeHUD();
        
        // Start with HUD visible
        ShowHUD();
    }
    
    private void FindGameSystems()
    {
        // Find player controller
        var playerActor = Scene.GetActor("Player");
        if (playerActor != null)
        {
            _playerController = playerActor.GetComponent<CipherController>();
        }
        
        // Find story state
        var storyStateActor = Scene.GetActor("StoryState");
        if (storyStateActor != null)
        {
            _storyState = storyStateActor.GetComponent<StoryState>();
        }
        else
        {
            // Create story state if it doesn't exist
            var storyStateGO = new Actor("StoryState");
            _storyState = storyStateGO.AddComponent<StoryState>();
        }
        
        Debug.Log("HUDManager found game systems");
    }
    
    private void InitializeHUD()
    {
        // Set initial values
        UpdateHealthUI();
        UpdateEnergyUI();
        UpdateHeatUI();
        
        // Hide hacking interface by default
        if (HackingInterface != null)
        {
            HackingInterface.IsVisible = false;
        }
        
        Debug.Log("HUD initialized");
    }
    
    public override void OnUpdate()
    {
        // Update player stats from actual game systems
        UpdatePlayerStats();
        
        // Update HUD elements
        UpdateHealthUI();
        UpdateEnergyUI();
        UpdateHeatUI();
        UpdateMinimap();
        UpdateHackingInterface();
        
        // Update text displays
        UpdateStatText();
    }
    
    private void UpdatePlayerStats()
    {
        // In a full implementation, we would get these from a player stats component
        // For now, we'll simulate with some basic logic
        
        // Health - would be affected by damage
        // For demo, let's just keep it at max unless we simulate damage
        
        // Energy - drains when sprinting, using parkour, recharges over time
        if (_playerController != null)
        {
            bool isSprinting = Input.GetButton("Run");
            bool isUsingEnergy = isSprinting || 
                                _playerController.GetType().GetField("_isVaulting", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic) != null && 
                                (bool)_playerController.GetType().GetField("_isVaulting", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(_playerController) ||
                                _playerController.GetType().GetField("_isWallRunning", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic) != null && 
                                (bool)_playerController.GetType().GetField("_isWallRunning", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(_playerController) ||
                                _playerController.GetType().GetField("_isLedgeGrabbing", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic) != null && 
                                (bool)_playerController.GetType().GetField("_isLedgeGrabbing", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(_playerController) ||
                                _playerController.GetType().GetField("_isMantling", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic) != null && 
                                (bool)_playerController.GetType().GetField("_isMantling", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(_playerController);
            
            if (isUsingEnergy && _currentEnergy > 0)
            {
                _currentEnergy -= 20f * Time.DeltaTime; // Drain rate
            }
            else if (_currentEnergy < _maxEnergy)
            {
                _currentEnergy += 10f * Time.DeltaTime; // Recharge rate
            }
            
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0f, _maxEnergy);
        }
        
        // Heat - increases with suspicious actions, combat, firing weapons
        // Decreases over time when not doing suspicious things
        // For demo, we'll just make it fluctuate slightly
        if (_currentHeat > 0)
        {
            _currentHeat -= 5f * Time.DeltaTime; // Cool down rate
        }
        _currentHeat = Mathf.Clamp(_currentHeat, 0f, _maxHeat);
    }
    
    private void UpdateHealthUI()
    {
        if (HealthBar != null)
        {
            // Assuming HealthBar is a UIControl with fill amount or scale
            float fillAmount = _currentHealth / _maxHealth;
            // In a real UI system, we would set the fill amount or scale
            // For now, we'll just log changes
            // HealthBar.FillAmount = fillAmount;
        }
    }
    
    private void UpdateEnergyUI()
    {
        if (EnergyBar != null)
        {
            float fillAmount = _currentEnergy / _maxEnergy;
            // EnergyBar.FillAmount = fillAmount;
        }
    }
    
    private void UpdateHeatUI()
    {
        if (HeatBar != null)
        {
            float fillAmount = _currentHeat / _maxHeat;
            // HeatBar.FillAmount = fillAmount;
            
            // Change color based on heat level
            // if (fillAmount > 0.7f) HeatBar.Color = Color.Red;
            // else if (fillAmount > 0.4f) HeatBar.Color = Color.Yellow;
            // else HeatBar.Color = Color.Green;
        }
    }
    
    private void UpdateMinimap()
    {
        // Minimap implementation would show:
        // - Player position and direction
        // - Nearby enemies (if detected)
        // - Points of interest
        // - Current objective marker
        
        if (MinimapPlayerIcon != null && _playerController != null)
        {
            // Rotate icon to match player direction
            // MinimapPlayerIcon.SetLocalRotation(Quaternion.RotationY(Mathf.Atan2(_playerController.transform.Forward.X, _playerController.transform.Forward.Z)));
        }
    }
    
    private void UpdateHackingInterface()
    {
        if (_isHacking)
        {
            // Show hacking interface
            if (HackingInterface != null)
            {
                HackingInterface.IsVisible = true;
            }
            
            // Update time remaining
            if (HackingTimeText != null)
            {
                int secondsRemaining = Mathf.CeilToInt(_hackingTimeRemaining);
                HackingTimeText.Text = $"Time: {secondsRemaining}s";
            }
            
            // Update progress bar
            if (HackingProgressBar != null)
            {
                float progress = 1f - (_hackingTimeRemaining / _hackingTimeLimit);
                // HackingProgressBar.FillAmount = progress;
            }
            
            // Decrease timer
            _hackingTimeRemaining -= Time.DeltaTime;
            if (_hackingTimeRemaining <= 0f)
            {
                EndHacking(false); // Timeout = failure
            }
        }
        else
        {
            // Hide hacking interface
            if (HackingInterface != null)
            {
                HackingInterface.IsVisible = false;
            }
        }
    }
    
    private void UpdateStatText()
    {
        if (HealthText != null)
        {
            HealthText.Text = $"HP: {Mathf.CeilToInt(_currentHealth)}/{Mathf.CeilToInt(_maxHealth)}";
        }
        
        if (EnergyText != null)
        {
            EnergyText.Text = $"EN: {Mathf.CeilToInt(_currentEnergy)}/{Mathf.CeilToInt(_maxEnergy)}";
        }
        
        if (HeatText != null)
        {
            HeatText.Text = $"HT: {Mathf.CeilToInt(_currentHeat)}/{Mathf.CeilToInt(_maxHeat)}";
        }
    }
    
    /// <summary>
    /// Called when player takes damage
    /// </summary>
    public void ApplyDamage(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0f, _maxHealth);
        Debug.Log($"Player took {amount} damage. Health: {_currentHealth}");
        
        if (_currentHealth <= 0f)
        {
            // Player died
            OnPlayerDeath();
        }
    }
    
    /// <summary>
    /// Called when player heals
    /// </summary>
    public void ApplyHeal(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, _maxHealth);
        Debug.Log($"Player healed {amount} amount. Health: {_currentHealth}");
    }
    
    /// <summary>
    /// Called when player uses energy
    /// </summary>
    public void UseEnergy(float amount)
    {
        _currentEnergy = Mathf.Clamp(_currentEnergy - amount, 0f, _maxEnergy);
    }
    
    /// <summary>
    /// Called when player gains energy
    /// </summary>
    public void AddEnergy(float amount)
    {
        _currentEnergy = Mathf.Clamp(_currentEnergy + amount, 0f, _maxEnergy);
    }
    
    /// <summary>
    /// Called when player's heat increases
    /// </summary>
    public void IncreaseHeat(float amount)
    {
        _currentHeat = Mathf.Clamp(_currentHeat + amount, 0f, _maxHeat);
        Debug.Log($"Heat increased by {amount}. Current heat: {_currentHeat}");
    }
    
    /// <summary>
    /// Called when player starts hacking
    /// </summary>
    public void StartHacking(float timeLimit = 15f)
    {
        _isHacking = true;
        _hackingTimeRemaining = timeLimit;
        _hackingTimeLimit = timeLimit;
        Debug.Log($"Hacking started. Time limit: {timeLimit}s");
    }
    
    /// <summary>
    /// Called when hacking ends (success or failure)
    /// </summary>
    public void EndHacking(bool success)
    {
        _isHacking = false;
        _hackingTimeRemaining = 0f;
        
        if (success)
        {
            Debug.Log("Hacking successful!");
            // Optionally show success effect
        }
        else
        {
            Debug.Log("Hacking failed!");
            // Optionally show failure effect and increase heat
            IncreaseHeat(25f); // Failed hacking increases suspicion
        }
        
        // Hide interface after a short delay
        Invoke(nameof(DelayHideHackingInterface), 1f);
    }
    
    private void DelayHideHackingInterface()
    {
        // Already handled in UpdateHackingInterface when _isHacking is false
    }
    
    private void OnPlayerDeath()
    {
        Debug.Log("Player has died!");
        // Show death screen, offer to respawn/load checkpoint, etc.
    }
    
    public void ShowHUD()
    {
        // Show all HUD elements
        if (HealthBar != null) HealthBar.IsVisible = true;
        if (EnergyBar != null) EnergyBar.IsVisible = true;
        if (HeatBar != null) HeatBar.IsVisible = true;
        if (Minimap != null) Minimap.IsVisible = true;
        Debug.Log("HUD shown");
    }
    
    public void HideHUD()
    {
        // Hide all HUD elements
        if (HealthBar != null) HealthBar.IsVisible = false;
        if (EnergyBar != null) EnergyBar.IsVisible = false;
        if (HeatBar != null) HeatBar.IsVisible = false;
        if (Minimap != null) Minimap.IsVisible = false;
        if (HackingInterface != null) HackingInterface.IsVisible = false;
        Debug.Log("HUD hidden");
    }
}
