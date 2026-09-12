// MissionManager.cs
// Manages active missions and mission flow
using FlaxEngine;
using System.Collections.Generic;

public class MissionManager : Script
{
    // Singleton pattern
    public static MissionManager Instance { get; private set; }
    
    private List<Mission> _availableMissions = new List<Mission>();
    private Mission _activeMission;
    private List<Mission> _completedMissions = new List<Mission>();
    
    public override void OnStart()
    {
        // Set up singleton
        if (Instance != null && Instance != this)
        {
            Destroy(Actor);
            return;
        }
        Instance = this;
        
        // Load available missions
        LoadAvailableMissions();
    }
    
    private void LoadAvailableMissions()
    {
        // Find all mission scripts in the scene
        var missions = Scene.GetAllActors()
                        .SelectMany(a => a.GetComponents<Mission>())
                        .ToList();
        
        _availableMissions.AddRange(missions);
        Debug.Log($"Loaded {_availableMissions.Count} available missions");
    }
    
    public void StartMission(string missionID)
    {
        var mission = _availableMissions.Find(m => m.MissionID == missionID);
        if (mission == null)
        {
            Debug.LogError($"Mission not found: {missionID}");
            return;
        }
        
        if (_activeMission != null)
        {
            // Optionally fail or pause current mission
            Debug.LogWarning("Starting new mission while another is active");
        }
        
        _activeMission = mission;
        _activeMission.StartMission();
        
        // Subscribe to mission events
        _activeMission.OnMissionCompleted += OnMissionCompleted;
        _activeMission.OnMissionFailed += OnMissionFailed;
        _activeMission.OnObjectiveCompleted += OnObjectiveCompleted;
    }
    
    private void OnMissionCompleted()
    {
        if (_activeMission != null)
        {
            _completedMissions.Add(_activeMission);
            _availableMissions.Remove(_activeMission);
            
            // Unsubscribe from events
            _activeMission.OnMissionCompleted -= OnMissionCompleted;
            _activeMission.OnMissionFailed -= OnMissionFailed;
            _activeMission.OnObjectiveCompleted -= OnObjectiveCompleted;
            
            _activeMission = null;
            
            Debug.Log("Mission completed - checking for next mission");
            // Auto-start next mission if applicable
            CheckForNextMission();
        }
    }
    
    private void OnMissionFailed()
    {
        if (_activeMission != null)
        {
            // Unsubscribe from events
            _activeMission.OnMissionCompleted -= OnMissionCompleted;
            _activeMission.OnMissionFailed -= OnMissionFailed;
            _activeMission.OnObjectiveCompleted -= OnObjectiveCompleted;
            
            _activeMission = null;
            
            Debug.Log("Mission failed");
        }
    }
    
    private void OnObjectiveCompleted(MissionObjective objective)
    {
        Debug.Log($"Objective completed: {objective.Description}");
        // Update mission tracker UI, etc.
    }
    
    private void CheckForNextMission()
    {
        // In a full implementation, this would check story flags
        // and automatically start the next appropriate mission
        // For now, we'll just log
        Debug.Log("Checking for next mission...");
    }
    
    public Mission GetActiveMission => _activeMission;
    public IReadOnlyList<Mission> GetAvailableMissions => _availableMissions.AsReadOnly();
    public IReadOnlyList<Mission> GetCompletedMissions => _completedMissions.AsReadOnly();
}
