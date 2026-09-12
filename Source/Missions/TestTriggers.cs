// TestTriggers.cs
// Simple trigger scripts for testing mission objectives
// These would be replaced with proper level design in final game
using FlaxEngine;

/// <summary>
/// Trigger that simulates escaping Canopy HQ (Objective 2)
/// Place this at the exit point of the starting area
/// </summary>
public class EscapeHQTrigger : Script
{
    public override void OnStart()
    {
        Debug.Log("EscapeHQTrigger placed - Player touching this will complete Objective 2");
    }
    
    public override void OnTriggerEnter(Collider other)
    {
        // Check if it's the player
        if (other.Actor.Name == "Player")
        {
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null)
            {
                storyState.SetFlag("escaped_canopy_hq", true);
                Debug.Log("���������������������������������������������� Player escaped Canopy HQ!");
                
                // Complete objective in mission
                var missionManager = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>();
                var activeMission = missionManager?.GetActiveMission as Source.Missions.Act1Awakening;
                if (activeMission != null)
                {
                    var objective = activeMission.GetObjectiveByDescription(
                        "Escape Canopy HQ after being noticed");
                    if (objective != null && !objective.IsComplete)
                    {
                        activeMission.CompleteObjective(objective.ID);
                    }
                }
            }
        }
    }
}

/// <summary>
/// Trigger that simulates reaching Underveil and contacting Choir (Objective 3)
/// Place this in the Underveil area
/// </summary>
public class UnderveilContactTrigger : Script
{
    public override void OnStart()
    {
        Debug.Log("UnderveilContactTrigger placed - Player touching this will complete Objective 3");
    }
    
    public override void OnTriggerEnter(Collider other)
    {
        // Check if it's the player
        if (other.Actor.Name == "Player")
        {
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null)
            {
                storyState.SetFlag("contacted_choir", true);
                // Also gain some reputation with Choir for making contact
                storyState.AddReputation(StoryState.Faction.Choir, 10);
                Debug.Log("���������������������������������������������� Player contacted the Choir in Underveil! Reputation +10");
                
                // Complete objective in mission
                var missionManager = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>();
                var activeMission = missionManager?.GetActiveMission as Source.Missions.Act1Awakening;
                if (activeMission != null)
                {
                    var objective = activeMission.GetObjectiveByDescription(
                        "Reach the Underveil and make contact with the Choir");
                    if (objective != null && !objective.IsComplete)
                    {
                        activeMission.CompleteObjective(objective.ID);
                    }
                }
            }
        }
    }
}
