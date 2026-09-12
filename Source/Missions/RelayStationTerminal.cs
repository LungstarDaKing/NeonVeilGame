// RelayStationTerminal.cs
// Specialized hackable terminal for the Midlayer relay station mission objective
using FlaxEngine;

public class RelayStationTerminal : HackableTerminal
{
    // Reference to the mission
    private Act1Awakening _mission;
    
    public override void OnStart()
    {
        base.OnStart();
        
        // Customize terminal for relay station
        TerminalID = "RELAY_STATION_001";
        Description = "Midlayer Relay Station - Primary Communications Hub";
        ContainsCriticalData = true;
        CanDisableCameras = true;
        CanOpenDoors = false;
        
        // Find the mission
        _mission = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>()
                    ?.GetActiveMission as Act1Awakening;
        
        Debug.Log("RelayStationTerminal initialized - ready for hacking");
    }
    
    public override void OnHackSuccess()
    {
        base.OnHackSuccess(); // This will call the base effects
        
        Debug.Log("���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Relay Station Terminal Hacked Successfully!");
        Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������ Planting hacking evidence...");
        
        // Apply additional effects for planting evidence
        var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
        if (storyState != null)
        {
            storyState.SetFlag("relay_station_hacked", true);
            storyState.AddEndingWeight(StoryState.EndingType.Liberation, 15); // Strong push toward liberation
            storyState.AddEndingWeight(StoryState.EndingType.Reform, -5);
            storyState.AddEndingWeight(StoryState.EndingType.Ascension, -5);
            storyState.AddReputation(StoryState.Faction.Choir, 20); // Major reputation gain with Choir
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������� Evidence planted - Story flags updated:");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������� Liberation Weight: +15");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������� Reform Weight: -5");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������� Ascension Weight: -5");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������� Choir Reputation: +20");
        }
        
        // Complete mission objective
        if (_mission != null)
        {
            var objective = _mission.GetObjectiveByDescription(
                "Infiltrate the Midlayer relay station and plant hacking evidence");
            if (objective != null && !objective.IsComplete)
            {
                _mission.CompleteObjective(objective.ID);
                Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������ Objective completed: Infiltrate the Midlayer relay station and plant hacking evidence");
            }
        }
    }
    
    public override void OnHackFailure()
    {
        base.OnHackFailure();
        
        Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������ Relay Station Terminal Hack Failed!");
        Debug.Log("���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Alarm triggered - increased heat and suspicion");
        
        // Apply failure consequences
        var hudManager = Scene.GetActor("HUDManager")?.GetComponent<HUDManager>();
        if (hudManager != null)
        {
            hudManager.IncreaseHeat(30); // Significant heat increase
            Debug.Log("���������������������������������������������������������������������������������������������������������������������������������������� Heat increased by 30 due to failed hack");
        }
        
        var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
        if (storyState != null)
        {
            storyState.AddEndingWeight(StoryState.EndingType.Ascension, 5); // Slight push toward ascension (reckless action)
            storyState.AddReputation(StoryState.Faction.Choir, -10); // Loss of trust
            Debug.Log("�������������������������������������������������������������������������������������������������������������������������������� Story state updated for failed hack");
        }
    }
}
