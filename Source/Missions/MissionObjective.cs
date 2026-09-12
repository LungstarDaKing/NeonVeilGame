// MissionObjective.cs
// Represents a single objective within a mission
using FlaxEngine;
using System;

[Serializable]
public class MissionObjective
{
    public string ID;
    public string Description;
    public bool IsCompleted;
    public bool IsOptional;
    
    // Completion conditions
    public enum CompletionType
    {
        ReachLocation,
        InteractWithObject,
        DefeatEnemies,
        CollectItem,
        TalkToNPC,
        CustomScript
    }
    
    public CompletionType Type;
    public string TargetID; // Reference to object, NPC, location, etc.
    public float TargetValue; // For value-based objectives (e.g., collect 5 items)
    public float CurrentValue;
    
    public MissionObjective()
    {
        ID = Guid.NewGuid().ToString();
        IsCompleted = false;
        IsOptional = false;
        Type = CompletionType.ReachLocation;
        CurrentValue = 0f;
    }
    
    public void UpdateProgress(float amount = 1f)
    {
        if (IsCompleted) return;
        
        CurrentValue += amount;
        if (CurrentValue >= TargetValue)
        {
            Complete();
        }
    }
    
    public void Complete()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            Debug.Log($"Objective completed: {Description}");
            // Notify mission of completion
        }
    }
    
    public bool IsComplete => IsCompleted;
}
