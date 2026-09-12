// Act3Confrontation.cs
// Third mission: Act III - Confrontation
// Final confrontation with multiple branching paths based on player choices
using FlaxEngine;
using System.Collections.Generic;
using System.Linq;

public class Act3Confrontation : Mission
{
    // Objective descriptions for easy lookup
    private const string OBJ_INFILTRATE_CORE = "Infiltrate the Mark System Core Facility";
    private const string OBJ_NAVIGATE_SECURITY = "Navigate through escalating security defenses";
    private const string OBJ_REACH_CORE_CHAMBER = "Reach the central core chamber";
    private const string OBJ_FINAL_CHOICE = "Make your final choice regarding the Mark system";
    private const string OBJ_ESCAPE_FACILITY = "Escape the facility (if possible)";
    private const string OBJ_SURVIVE_AFTERMATH = "Survive the aftermath of your decision";

    public override void OnStart()
    {
        base.OnStart();

        MissionID = "ACT3_CONFRONTATION";
        MissionName = "Confrontation";
        Description = "Face the ultimate choice: destroy, reform, or seize control of the Mark system.";

        // Objectives will be initialized in InitializeObjectives
    }

    protected override void InitializeObjectives()
    {
        // Objective 1: Infiltrate the Mark System Core Facility
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_INFILTRATE_CORE,
            IsOptional = false,
            TargetValue = 1
        });

        // Objective 2: Navigate through escalating security defenses
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_NAVIGATE_SECURITY,
            IsOptional = false,
            TargetValue = 1
        });

        // Objective 3: Reach the central core chamber
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_REACH_CORE_CHAMBER,
            IsOptional = false,
            TargetValue = 1
        });

        // Objective 4: Make your final choice regarding the Mark system
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_FINAL_CHOICE,
            IsOptional = false,
            TargetValue = 1
        });

        // Objective 5: Escape the facility (if possible)
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_ESCAPE_FACILITY,
            IsOptional = false,
            TargetValue = 1
        });

        // Objective 6: Survive the aftermath of your decision
        Objectives.Add(new MissionObjective
        {
            Description = OBJ_SURVIVE_AFTERMATH,
            IsOptional = false,
            TargetValue = 1
        });
    }

    protected override void UpdateMission()
    {
        base.UpdateMission();

        // Check for objective completions through game events and story flags
        CheckCoreInfiltration();
        CheckSecurityNavigation();
        CheckCoreChamberReached();
        CheckFinalChoiceMade();
        CheckFacilityEscape();
        CheckAftermathSurvival();
    }

    private void CheckCoreInfiltration()
    {
        var objective = GetObjectiveByDescription(OBJ_INFILTRATE_CORE);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has infiltrated core facility via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("core_facility_infiltrated"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Infiltrate the Mark System Core Facility");
            }
        }
    }

    private void CheckSecurityNavigation()
    {
        var objective = GetObjectiveByDescription(OBJ_NAVIGATE_SECURITY);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has navigated security defenses via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("security_defenses_navigated"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Navigate through escalating security defenses");
            }
        }
    }

    private void CheckCoreChamberReached()
    {
        var objective = GetObjectiveByDescription(OBJ_REACH_CORE_CHAMBER);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has reached core chamber via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("core_chamber_reached"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Reach the central core chamber");
            }
        }
    }

    private void CheckFinalChoiceMade()
    {
        var objective = GetObjectiveByDescription(OBJ_FINAL_CHOICE);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has made final choice via story flags
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null &&
                (storyState.GetFlag("choice_destroy_system") ||
                 storyState.GetFlag("choice_reform_system") ||
                 storyState.GetFlag("choice_seize_control")))
            {
                CompleteObjective(objective.ID);
                Debug.Log("����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Objective completed: Make your final choice regarding the Mark system");
            }
        }
    }

    private void CheckFacilityEscape()
    {
        var objective = GetObjectiveByDescription(OBJ_ESCAPE_FACILITY);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has escaped facility via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("facility_escaped"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������██Objectives completed: Escape the facility (if possible)");
            }
        }
    }

    private void CheckAftermathSurvival()
    {
        var objective = GetObjectiveByDescription(OBJ_SURVIVE_AFTERMATH);
        if (objective != null && !objective.IsComplete)
        {
            // Check if player has survived aftermath via story flag
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            if (storyState != null && storyState.GetFlag("aftermath_survived"))
            {
                CompleteObjective(objective.ID);
                Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������██Objectives completed: Survive the aftermath of your decision");
            }
        }
    }

    // Helper method to find objective by description
    private MissionObjective GetObjectiveByDescription(string description)
    {
        return Objectives.FirstOrDefault(obj => obj.Description == description);
    }

    public override void OnMissionCompleted()
    {
        base.OnMissionCompleted();
        Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������??");
        Debug.Log("����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ACT III COMPLETED: CONFRONTATION  ���������������������������������� ���������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ��������������������������������� ����������� �������������������������������� �������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ��������� �������������������������������� �������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ������������������������������� ��������� ������������������������������ ������������������������������ ����������������������������� ����������������������������� ����������������������������� ����������������������������� ����������������������������� ����������������������������� ����������������������������� ����������������������������� ����������������������������� ����������������������������� �������██����������������");
        Debug.Log("����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������██");

        // Log the final ending based on StoryState
        var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
        if (storyState != null)
        {
            Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������ FINAL STORY STATE:");
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Choir Reputation: {storyState.GetReputation(StoryState.Faction.Choir)}");
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Liberation Weight: {storyState.GetEndingWeight(StoryState.EndingType.Liberation)}");
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Reform Weight: {storyState.GetEndingWeight(StoryState.EndingType.Reform)}");
            Debug.Log($"������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������ Ascension Weight: {storyState.GetEndingWeight(StoryState.EndingType.Ascension)}");

            // Determine and log the ending
            var liberation = storyState.GetEndingWeight(StoryState.EndingType.Liberation);
            var reform = storyState.GetEndingWeight(StoryState.EndingType.Reform);
            var ascension = storyState.GetEndingWeight(StoryState.EndingType.Ascension);

            string ending = "Unknown";
            if (liberation >= reform && liberation >= ascension)
                ending = "Liberation - The Mark system is destroyed";
            else if (reform >= liberation && reform >= ascension)
                ending = "Reform - The Mark system is reformed with oversight";
            else if (ascension >= liberation && ascension >= reform)
                ending = "Ascension - You now control the Mark system";

            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� FINAL ENDING: {ending}");
        }
    }
}