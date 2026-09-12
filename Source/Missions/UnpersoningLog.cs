// UnpersoningLog.cs
// Interactive object representing the hidden audit log that starts the story
using FlaxEngine;

public class UnpersoningLog : Script, IInteractable
{
    // Reference to the mission to notify when discovered
    private Act1Awakening _mission;
    
    // Visual feedback
    public bool IsDiscovered { get; private set; } = false;
    
    // Interaction properties
    public string InteractionPrompt => "Investigate Log";
    public float InteractionRange = 2.0f;
    
    public override void OnStart()
    {
        // Find the mission in the scene
        _mission = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>()
                    ?.GetActiveMission as Act1Awakening;
        
        if (_mission == null)
        {
            Debug.LogWarning("UnpersoningLog: Could not find Act1Awakening mission");
        }
        
        // Set up visual indicator (would be done in editor)
        Debug.Log("UnpersoningLog initialized");
    }
    
    public override void OnUpdate()
    {
        // If already discovered, no need to check interaction
        if (IsDiscovered) return;
        
        // Check if player is nearby and interacting
        var player = Scene.GetActor("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(player.Position, Transform.Position);
            if (distance <= InteractionRange && Input.GetButtonDown("Interact"))
            {
                DiscoverLog();
            }
        }
    }
    
    private void DiscoverLog()
    {
        if (IsDiscovered) return;
        
        IsDiscovered = true;
        Debug.Log("���������������� Unpersoning log discovered!");
        
        // Show discovery effect (would be implemented with particles/sound)
        // For now, just log
        
        // Notify mission
        if (_mission != null)
        {
            _mission.CompleteObjective(_mission.GetObjectiveByDescription(
                "Discover the hidden unpersoning log during audit work")?.ID);
        }
        
        // Update story state
        var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
        if (storyState != null)
        {
            storyState.SetFlag("discovered_truth", true);
            storyState.AddEndingWeight(StoryState.EndingType.Liberation, 5); // leaning toward liberation
            storyState.AddReputation(StoryState.Faction.Choir, 5); // gain favor with Choir
            Debug.Log("���������������� Story updated: truth discovered, Choir reputation +5");
        }
    }
}

// Interface for interactable objects
public interface IInteractable
{
    string InteractionPrompt { get; }
    float InteractionRange { get; }
    // In a full implementation, this would be called by an interaction system
    void Interact();
}
