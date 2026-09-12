// Act1Awakening.cs
// First mission: Act I - Awakening
// Covers the opening sequence: discovery, escape, contact with Choir, first vehicle mission
using FlaxEngine;
using System.Collections.Generic;
using System.Linq;

public class Act1Awakening : Mission
{
    // Objective descriptions for easy lookup
    private const string OBJ_DISCOVER_LOG = "Discover the hidden unpersoning log during audit work";
    private const string OBJ_ESCAPE_HQ = "Escape Canopy HQ after being noticed";
    private const string OBJ_REACH_UNDERVEIL = "Reach the Underveil and make contact with the Choir";
    private const string OBJ_FACTION_DECISION = "Make your first faction-reputation decision with the Choir";
    private const string OBJ_FIRST_VEHICLE = "Hijack a vehicle to escape Midlayer patrol drones";
    private const string OBJ_INFILTRATE_RELAY = "Infiltrate the Midlayer relay station and plant hacking evidence";
    private const string OBJ_HANDLE_GUARD = "Decide the fate of the captured guard at the relay station";
    
    public override void OnStart()
    {
        base.OnStart();
        
        MissionID = "ACT1_AWAKENING";
        MissionName = "Awakening";
        Description = "Discover the truth about the Mark system and begin your resistance.";
        
        // Objectives will be initialized in InitializeObjectives
    }
    
    protected override void InitializeObjectives()
    {
        // Objective 1: Discover the unpersoning log (tutorial)
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_DISCOVER_LOG,
            IsOptional = false,
            TargetValue = 1
        });
        
        // Objective 2: Escape Canopy HQ (tutorial for stealth and first hack)
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_ESCAPE_HQ,
            IsOptional = false,
            TargetValue = 1
        });
        
        // Objective 3: Reach the Undeveil and contact the Choir
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_REACH_UNDERVEIL,
            IsOptional = false,
            TargetValue = 1
        });
        
        // Objective 4: First faction-reputation decision (tutorial for dialogue/choice system)
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_FACTION_DECISION,
            IsOptional = false,
            TargetValue = 1
        });
        
        // Objective 5: First vehicle mission - hijack a car to escape Midlayer patrol drones
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_FIRST_VEHICLE,
            IsOptional = false,
            TargetValue = 1
        });
        
        // Objective 6: Act I Climax - Infiltrate relay station and plant hacking evidence
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_INFILTRATE_RELAY,
            IsOptional = false,
            TargetValue = 1
        });
        
        // Objective 7: Handle the captured guard (kill/knock out/let go) - seeds ending weight
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_HANDLE_GUARD,
            IsOptional = false,
            TargetValue = 1
        });
    }
    
    protected override void UpdateMission()
    {
        base.UpdateMission();
        
        // Check for objective completions through game events and story flags
        CheckLogDiscovery();
        CheckEscapeHQ();
        CheckUnderveilContact();
        CheckChoirDecision();
        CheckRelayStationHack();
        CheckGuardDecision();
    }
    
    private void CheckLogDiscovery()
    {
        var objective = GetObjectiveByDescription(OBJ_DISCOVER_LOG);
        if (objective != null && !objective.IsComplete)
        {
            // Check if the log has been discovered via story state
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("discovered_truth"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Objective completed: Discover the unpersoning log");
            }
        }
    }
    
    private void CheckEscapeHQ()
    {
        var objective = GetObjectiveByDescription(OBJ_ESCAPE_HQ);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has escaped Canopy HQ via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("escaped_canopy_hq"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Escape Canopy HQ");
            }
        }
    }
    
    private void CheckUnderveilContact()
    {
        var objective = GetObjectiveByDescription(OBJ_REACH_UNDERVEIL);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has reached Underveil and made contact with Choir
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("contacted_choir"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Reach Underveil and contact Choir");
            }
        }
    }
    
    private void CheckChoirDecision()
    {
        var objective = GetObjectiveByDescription(OBJ_FACTION_DECISION);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has made the Choir faction decision via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && 
                (storyState.GetFlag("choir_support_liberation") ||
                 storyState.GetFlag("choir_support_reform") ||
                 storyState.GetFlag("choir_neutral")))
            {
                CompleteObjective(objective.ID);
                Debug.Log("����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Make your first faction-reputation decision with the Choir");
            }
        }
    }
    
    private void CheckRelayStationHack()
    {
        var objective = GetObjectiveByDescription(OBJ_INFILTRATE_RELAY);
        if (objective != null && !objective.IsComplete)
        {
            // Check if the relay station has been hacked via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("relay_station_hacked"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Infiltrate the Midlayer relay station and plant hacking evidence");
            }
        }
    }
    
    private void CheckGuardDecision()
    {
        var objective = GetObjectiveByDescription(OBJ_HANDLE_GUARD);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has made the captured guard decision via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && 
                (storyState.GetFlag("guard_executed") ||
                 storyState.GetFlag("guard_subdued") ||
                 storyState.GetFlag("guard_released")))
            {
                CompleteObjective(objective.ID);
                Debug.Log("��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Decide the fate of the captured guard at the relay station");
            }
        }
    }
    
    // Helper method to find objective by description
    private MissionObjective GetObjectiveByDescription(string description)
    {
        return Objectives.FirstOrDefault(obj => obj.Description == description);
    }
    
    // Override to provide better debug info
    public override void OnMissionCompleted()
    {
        base.OnMissionCompleted();
        Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        Debug.Log("����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ACT I COMPLETED: AWAKENING  �������������������������������� ������������������������������ ������������������������������ ������������������������������ ������������������������������ ������������������������������ ������������������������������ ������������������������������ ������������������������������ ���������������������������� ���������������������������� ���������������������������� ���������������������������� ���������������������������� ���������������������������� ����������������������������");
        Debug.Log("��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        
        // Log final story state for debugging
        var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
        if (storyState != null)
        {
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Final Story State:");
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Choir Reputation: {storyState.GetReputation(StoryState.Faction.Choir)}");
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Liberation Weight: {storyState.GetEndingWeight(StoryState.EndingType.Liberation)}");
            Debug.Log($"������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������ Reform Weight: {storyState.GetEndingWeight(StoryState.EndingType.Reform)}");
            Debug.Log($"������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������ Ascension Weight: {storyState.GetEndingWeight(StoryState.EndingType.Ascension)}");
        }
    }
}
