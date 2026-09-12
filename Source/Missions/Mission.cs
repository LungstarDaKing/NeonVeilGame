// Mission.cs
// Base class for all missions in the game
using FlaxEngine;
using System.Collections.Generic;

public abstract class Mission : Script
{
    public string MissionID;
    public string MissionName;
    public string Description;
    
    public List<MissionObjective> Objectives = new List<MissionObjective>();
    
    // Mission state
    public enum MissionState
    {
        NotStarted,
        Active,
        Completed,
        Failed
    }
    
    public MissionState CurrentState = MissionState.NotStarted;
    
    // Events
    public event Action OnMissionStarted;
    public event Action OnMissionCompleted;
    public event Action OnMissionFailed;
    public event Action<MissionObjective> OnObjectiveCompleted;
    
    public override void OnStart()
    {
        InitializeObjectives();
    }
    
    public override void OnUpdate()
    {
        if (CurrentState == MissionState.Active)
        {
            UpdateMission();
            CheckMissionCompletion();
        }
    }
    
    /// <summary>
    /// Initializes the mission objectives - to be implemented by derived classes
    /// </summary>
    protected abstract void InitializeObjectives();
    
    /// <summary>
    /// Updates mission logic - to be implemented by derived classes
    /// </summary>
    protected virtual void UpdateMission() { }
    
    /// <summary>
    /// Starts the mission
    /// </summary>
    public void StartMission()
    {
        if (CurrentState != MissionState.NotStarted) return;
        
        CurrentState = MissionState.Active;
        Debug.Log($"Mission started: {MissionName}");
        
        OnMissionStarted?.Invoke();
        
        // Show mission UI, update tracker, etc.
    }
    
    /// <summary>
    /// Checks if all objectives are completed
    /// </summary>
    protected void CheckMissionCompletion()
    {
        if (CurrentState != MissionState.Active) return;
        
        bool allComplete = true;
        foreach (var objective in Objectives)
        {
            if (!objective.IsComplete)
            {
                allComplete = false;
                break;
            }
        }
        
        if (allComplete)
        {
            CompleteMission();
        }
    }
    
    /// <summary>
    /// Completes the mission successfully
    /// </summary>
    protected void CompleteMission()
    {
        if (CurrentState != MissionState.Active) return;
        
        CurrentState = MissionState.Completed;
        Debug.Log($"Mission completed: {MissionName}");
        
        OnMissionCompleted?.Invoke();
        
        // Grant rewards, update story state, etc.
    }
    
    /// <summary>
    /// Fails the mission
    /// </summary>
    public void FailMission()
    {
        if (CurrentState != MissionState.Active) return;
        
        CurrentState = MissionState.Failed;
        Debug.Log($"Mission failed: {MissionName}");
        
        OnMissionFailed?.Invoke();
        
        // Handle failure consequences
    }
    
    /// <summary>
    /// Gets an objective by its ID
    /// </summary>
    public MissionObjective GetObjective(string objectiveID)
    {
        return Objectives.Find(obj => obj.ID == objectiveID);
    }
    
    /// <summary>
    /// Completes an objective by ID
    /// </summary>
    public void CompleteObjective(string objectiveID)
    {
        var objective = GetObjective(objectiveID);
        if (objective != null)
        {
            objective.Complete();
            OnObjectiveCompleted?.Invoke(objective);
        }
    }
}
