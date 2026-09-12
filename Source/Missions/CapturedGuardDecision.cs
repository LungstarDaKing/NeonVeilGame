// CapturedGuardDecision.cs
// Interactive object representing a captured guard with moral choice affecting ending weights
using FlaxEngine;
using System.Collections.Generic;

public class CapturedGuardDecision : Script, IInteractable
{
    // Reference to the mission
    private Act1Awakening _mission;
    
    // Decision state
    private bool _decisionMade = false;
    
    // Interaction properties
    public string InteractionPrompt => "Interrogate Captured Guard";
    public float InteractionRange = 2.0f;
    
    // Decision options and their effects on ending weights and reputation
    private struct DecisionOption
    {
        public string Description;
        public int ChoirReputationChange;
        public int LiberationWeightChange;
        public int ReformWeightChange;
        public int AscensionWeightChange;
        public string StoryFlag;
        public string Summary; // Brief outcome description
    }
    
    private List<DecisionOption> _options = new List<DecisionOption>
    {
        // Option 1: Execute the guard (violent, anti-authoritarian)
        new DecisionOption
        {
            Description = "Execute the guard - send a message to the Mark system enforcers",
            ChoirReputationChange = -10, // Choir may disapprove of unnecessary killing
            LiberationWeightChange = 20, // Strong push toward liberation through fear
            ReformWeightChange = -10,    // Undermines reform efforts
            AscensionWeightChange = 5,   // Shows willingness to use power
            StoryFlag = "guard_executed",
            Summary = "You executed the guard. The body sends a brutal message to the Mark system."
        },
        // Option 2: Subdue and imprison the guard (merciful but firm)
        new DecisionOption
        {
            Description = "Knock out the guard and leave them for authorities",
            ChoirReputationChange = 5,   // Choir approves of showing mercy
            LiberationWeightChange = 5,  // Moderate liberation push
            ReformWeightChange = 10,     // Supports reform through justice system
            AscensionWeightChange = -5,  // Less authoritarian
            StoryFlag = "guard_subdued",
            Summary = "You subdued the guard and left them to be found by the Mark system authorities."
        },
        // Option 3: Release the guard (compassionate, potentially naive)
        new DecisionOption
        {
            Description = "Let the guard go unharmed",
            ChoirReputationChange = 15,  // Choir highly approves of mercy
            LiberationWeightChange = -10, // Undermines liberation through mercy to enforcer
            ReformWeightChange = 15,     // Strong reform through compassion
            AscensionWeightChange = -15, // Least authoritarian
            StoryFlag = "guard_released",
            Summary = "You released the guard unharmed, hoping to show the Mark system enforcers that not all resistance is violent."
        }
    };
    
    public override void OnStart()
    {
        _mission = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>()
                    ?.GetActiveMission as Act1Awakening;
        
        Debug.Log("CapturedGuardDecision initialized - ready for moral choice interaction");
    }
    
    public override void OnUpdate()
    {
        if (_decisionMade) return;
        
        var player = Scene.GetActor("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(player.Position, Transform.Position);
            if (distance <= InteractionRange && Input.GetButtonDown("Interact"))
            {
                PresentDecision();
            }
        }
    }
    
    private void PresentDecision()
    {
        Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        Debug.Log("�������������������������������������������������������������������������������������������������������������������������� CAPTURED GUARD DECISION ���������������������������������� ������������������������������ �������������������������������� ����������������������������");
        Debug.Log("��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        Debug.Log("You have captured a Mark system guard during the relay station infiltration.");
        Debug.Log("What do you do with them?");
        Debug.Log("");
        Debug.Log("Choose your action:");
        
        for (int i = 0; i < _options.Count; i++)
        {
            Debug.Log($"{i+1}) {_options[i].Description}");
        }
        
        Debug.Log("");
        Debug.Log("Press 1, 2, or 3 to select an option (check console for feedback)");
        
        // Store the fact that we're waiting for decision input
        _decisionMade = true; // Prevent repeated prompts
        Invoke(nameof(WaitForDecisionInput), 0.1f);
    }
    
    private void WaitForDecisionInput()
    {
        // Check for input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ApplyDecision(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ApplyDecision(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ApplyDecision(2);
        }
        else
        {
            // Keep checking
            Invoke(nameof(WaitForDecisionInput), 0.1f);
        }
    }
    
    private void ApplyDecision(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= _options.Count) return;
        
        var option = _options[optionIndex];
        _decisionMade = true;
        
        Debug.Log("");
        Debug.Log($"������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������� YOU CHOSE: {option.Description}");
        Debug.Log($"����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        Debug.Log($"���������������������������������������������������������������������������������������������� ���������������������������������������������������������������������������������������������������� ���� Outcome: {option.Summary}");
        
        // Apply reputation and ending weight changes
        var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
        if (storyState != null)
        {
            storyState.AddReputation(StoryState.Faction.Choir, option.ChoirReputationChange);
            storyState.AddEndingWeight(StoryState.EndingType.Liberation, option.LiberationWeightChange);
            storyState.AddEndingWeight(StoryState.EndingType.Reform, option.ReformWeightChange);
            storyState.AddEndingWeight(StoryState.EndingType.Ascension, option.AscensionWeightChange);
            storyState.SetFlag(option.StoryFlag, true);
            
            Debug.Log("");
            Debug.Log($"��������������������������������������������������������������������������������������������������������������������������������Effects applied:");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������� Choir Reputation: {option.ChoirReputationChange:+0;-0}");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������� Liberation Weight: {option.LiberationWeightChange:+0;-0}");
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������� Reform Weight: {option.ReformWeightChange:+0;-0}");
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������� Ascension Weight: {option.AscensionWeightChange:+0;-0}");
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������� Story Flag Set: {option.StoryFlag}");
        }
        
        // Complete mission objective
        if (_mission != null)
        {
            var objective = _mission.GetObjectiveByDescription(
                "Decide the fate of the captured guard at the relay station");
            if (objective != null && !objective.IsComplete)
            {
                _mission.CompleteObjective(objective.ID);
                Debug.Log("���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Objective completed: Decide the fate of the captured guard at the relay station");
            }
        }
        
        // Log final tendencies for debugging
        var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
        if (storyState != null)
        {
            var lib = storyState.GetEndingWeight(StoryState.EndingType.Liberation);
            var refo = storyState.GetEndingWeight(StoryState.EndingType.Reform);
            var asc = storyState.GetEndingWeight(StoryState.EndingType.Ascension);
            Debug.Log("");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Current Ending Tendencies:");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������� Liberation: {lib} | Reform: {refo} | Ascension: {asc}");
            
            // Determine likely ending
            EndingType likely = EndingType.Liberation;
            if (refo > lib && refo > asc) likely = EndingType.Reform;
            else if (asc > lib && asc > refo) likely = EndingType.Ascension;
            
            string likelyName = likely == EndingType.Liberation ? "Liberation" : 
                              likely == EndingType.Reform ? "Reform" : "Ascension";
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Likely Ending Path: {likelyName}");
        }
        
        // Clean up invoke
        CancelInvoke(nameof(WaitForDecisionInput));
    }
}
