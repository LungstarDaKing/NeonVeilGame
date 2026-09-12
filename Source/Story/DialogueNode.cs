// DialogueNode.cs
// Single node in a dialogue tree
using FlaxEngine;
using System.Collections.Generic;

[Serializable]
public class DialogueNode
{
    public string ID;
    public string Speaker; // Name or ID of the speaker
    public string Text;    // The dialogue text
    public string Portrait; // Optional: portrait/sprite for speaker
    
    // Available choices/responses from this node
    public List<DialogueChoice> Choices = new List<DialogueChoice>();
    
    // Actions to execute when this node is entered
    public List<DialogueAction> Actions = new List<DialogueAction>();
    
    public bool IsEntryPoint = false; // Is this the starting node of a conversation?
    public bool EndsConversation = false; // Does this node end the conversation?
    
    public DialogueNode()
    {
        ID = System.Guid.NewGuid().ToString();
    }
}

[Serializable]
public class DialogueChoice
{
    public string Text; // What the player can say/choose
    public string TargetNodeID; // ID of the node to go to if chosen
    
    // Conditions for this choice to be available
    public List<string> RequiredFlags = new List<string>(); // Story flags that must be set
    public List<string> RequiredNotFlags = new List<string>(); // Story flags that must NOT be set
    public Dictionary<string, int> RequiredReputation = new Dictionary<string, int>(); // Faction reputation requirements
    
    // Actions to execute when this choice is selected
    public List<DialogueAction> Actions = new List<DialogueAction>();
    
    public bool IsAvailable(StoryState storyState)
    {
        // Check required flags
        foreach (var flag in RequiredFlags)
        {
            if (!storyState.GetFlag(flag))
                return false;
        }
        
        // Check required NOT flags
        foreach (var flag in RequiredNotFlags)
        {
            if (storyState.GetFlag(flag))
                return false;
        }
        
        // Check reputation requirements
        foreach (var kvp in RequiredReputation)
        {
            Faction faction;
            if (System.Enum.TryParse(kvp.Key, true, out faction))
            {
                if (storyState.GetReputation(faction) < kvp.Value)
                    return false;
            }
            else
            {
                // Try to parse as integer index
                if (int.TryParse(kvp.Key, out int factionIndex) && factionIndex >= 0 && factionIndex < 3)
                {
                    Faction fac = (Faction)factionIndex;
                    if (storyState.GetReputation(fac) < kvp.Value)
                        return false;
                }
            }
        }
        
        return true;
    }
}

[Serializable]
public class DialogueAction
{
    public enum ActionType
    {
        SetFlag,
        AddReputation,
        AddEndingWeight,
        UpdateMissionObjective,
        CustomScript
    }
    
    public ActionType Type;
    public string Parameter1; // For SetFlag: flag ID, for AddReputation: faction name, etc.
    public int Parameter2;    // For AddReputation: amount, for AddEndingWeight: amount, etc.
    public string Parameter3; // Additional parameter if needed
    
    public void Execute(StoryState storyState, MissionManager missionManager = null)
    {
        switch (Type)
        {
            case ActionType.SetFlag:
                bool setValue = Parameter2 != 0; // Non-zero means true
                storyState.SetFlag(Parameter1, setValue);
                break;
                
            case ActionType.AddReputation:
                if (System.Enum.TryParse(Parameter1, true, out Faction faction))
                {
                    storyState.AddReputation(faction, Parameter2);
                }
                break;
                
            case ActionType.AddEndingWeight:
                if (System.Enum.TryParse(Parameter1, true, out EndingType ending))
                {
                    storyState.AddEndingWeight(ending, Parameter2);
                }
                break;
                
            case ActionType.UpdateMissionObjective:
                if (missionManager != null && !string.IsNullOrEmpty(Parameter1))
                {
                    missionManager.GetActiveMission?.CompleteObjective(Parameter1);
                }
                break;
                
            case ActionType.CustomScript:
                // Would execute a custom script - for now just log
                Debug.Log($"Executing custom dialogue action: {Parameter1}");
                break;
        }
    }
}
