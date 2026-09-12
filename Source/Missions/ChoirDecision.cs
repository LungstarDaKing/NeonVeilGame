// ChoirDecision.cs
// Interactive object representing a decision point with the Choir faction
using FlaxEngine;
using System.Collections.Generic;

public class ChoirDecision : Script, IInteractable
{
    // Reference to the mission
    private Act1Awakening _mission;
    
    // Decision state
    private bool _decisionMade = false;
    
    // Interaction properties
    public string InteractionPrompt => "Speak with Choir Representative";
    public float InteractionRange = 2.5f;
    
    // Decision options and their effects
    private struct DecisionOption
    {
        public string Description;
        public int ChoirReputationChange;
        public int LiberationWeightChange;
        public int ReformWeightChange;
        public int AscensionWeightChange;
        public string StoryFlag;
    }
    
    private List<DecisionOption> _options = new List<DecisionOption>
    {
        // Option 1: Support the Choir's liberation goals
        new DecisionOption
        {
            Description = "Support the Choir's plan to expose the Mark system publicly",
            ChoirReputationChange = 15,
            LiberationWeightChange = 10,
            ReformWeightChange = -5,
            AscensionWeightChange = -5,
            StoryFlag = "choir_support_liberation"
        },
        // Option 2: Seek a middle path with reform
        new DecisionOption
        {
            Description = "Propose reforming the Mark system through oversight and transparency",
            ChoirReputationChange = 5,
            LiberationWeightChange = -5,
            ReformWeightChange = 15,
            AscensionWeightChange = -5,
            StoryFlag = "choir_support_reform"
        },
        // Option 3: Refuse to get involved (neutral)
        new DecisionOption
        {
            Description = "Decline to assist the Choir and maintain neutrality",
            ChoirReputationChange = -5,
            LiberationWeightChange = 0,
            ReformWeightChange = 0,
            AscensionWeightChange = 5,
            StoryFlag = "choir_neutral"
        }
    };
    
    public override void OnStart()
    {
        _mission = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>()
                    ?.GetActiveMission as Act1Awakening;
        
        Debug.Log("ChoirDecision initialized - ready for player interaction");
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
        Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        Debug.Log("���������������������������������������������������������� CHOIR DECISION POINT ������������������������������ ����������������������������");
        Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        Debug.Log("You have made contact with the Choir. They seek your assistance.");
        Debug.Log("");
        Debug.Log("Choose your response:");
        
        for (int i = 0; i < _options.Count; i++)
        {
            Debug.Log($"{i+1}) {_options[i].Description}");
        }
        
        Debug.Log("");
        Debug.Log("Press 1, 2, or 3 to select an option (check console for feedback)");
        
        // Store the fact that we're waiting for decision input
        // We'll handle input in Update
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
        Debug.Log($"��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        Debug.Log($"�������������������������������������������������������������� YOU CHOSE: {option.Description}");
        Debug.Log($"��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
        
        // Apply reputation changes
        var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
        if (storyState != null)
        {
            storyState.AddReputation(StoryState.Faction.Choir, option.ChoirReputationChange);
            storyState.AddEndingWeight(StoryState.EndingType.Liberation, option.LiberationWeightChange);
            storyState.AddEndingWeight(StoryState.EndingType.Reform, option.ReformWeightChange);
            storyState.AddEndingWeight(StoryState.EndingType.Ascension, option.AscensionWeightChange);
            storyState.SetFlag(option.StoryFlag, true);
            
            Debug.Log($"���������������������������������������������������������������� Effects applied:");
            Debug.Log($"���������������������������������������������������������������� Choir Reputation: {option.ChoirReputationChange:+0;-0}");
            Debug.Log($"���������������������������������������������������������������� Liberation Weight: {option.LiberationWeightChange:+0;-0}");
            Debug.Log($"���������������������������������������������������������������� Reform Weight: {option.ReformWeightChange:+0;-0}");
            Debug.Log($"���������������������������������������������������������������� Ascension Weight: {option.AscensionWeightChange:+0;-0}");
            Debug.Log($"���������������������������������������������������������������� Story Flag Set: {option.StoryFlag}");
        }
        
        // Complete mission objective
        if (_mission != null)
        {
            var objective = _mission.GetObjectiveByDescription(
                "Make your first faction-reputation decision with the Choir");
            if (objective != null && !objective.IsComplete)
            {
                _mission.CompleteObjective(objective.ID);
                Debug.Log("�������������������������������������������������������������������������������������������������������������������������������� Objective completed: Make your first faction-reputation decision with the Choir");
            }
        }
        
        // Clean up invoke
        CancelInvoke(nameof(WaitForDecisionInput));
    }
}
