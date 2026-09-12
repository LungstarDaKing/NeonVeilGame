// StoryState.cs
// Singleton tracking game progress, faction standing, and ending weights
using FlaxEngine;
using System.Collections.Generic;

public class StoryState : Script
{
    // Singleton instance
    public static StoryState Instance { get; private set; }
    
    // Faction reputation (-100 to 100)
    public int ChoirReputation = 0;        // Underground hacker network
    public int CanopyCorpReputation = 0;   // Corporate elite
    public int UnderveilGangsReputation = 0; // Street-level gangs
    
    // Story flags - key events that have happened
    private HashSet<string> _storyFlags = new HashSet<string>();
    
    // Ending weight tracking (affects which ending is triggered)
    // Each ending has a weight threshold
    public int LiberationWeight = 0;    // Burn down the Mark system
    public int ReformWeight = 0;        // Reform the Mark system
    public int AscensionWeight = 0;     // Take control of the Mark system
    
    // Player progress
    public int ActProgress = 0;         // 0 = Act I, 1 = Act II, 2 = Act III
    public bool HasDiscoveredTruth = false;
    public bool HasContactedChoir = false;
    public bool HasEscapedCanopy = false;
    
    // Current mission/story state
    public string CurrentMissionID = "";
    public string LastLocationVisited = "";
    
    public override void OnStart()
    {
        // Set up singleton
        if (Instance != null && Instance != this)
        {
            Destroy(Actor);
            return;
        }
        Instance = this;
        
        // Don't destroy when loading new scenes
        DontDestroyOnLoad(Actor);
        
        Debug.Log("StoryState initialized");
    }
    
    /// <summary>
    /// Sets a story flag to true
    /// </summary>
    public void SetFlag(string flagID, bool value = true)
    {
        if (value)
        {
            _storyFlags.Add(flagID);
        }
        else
        {
            _storyFlags.Remove(flagID);
        }
        
        Debug.Log($"Story flag set: {flagID} = {value}");
    }
    
    /// <summary>
    /// Checks if a story flag is set
    /// </summary>
    public bool GetFlag(string flagID)
    {
        return _storyFlags.Contains(flagID);
    }
    
    /// <summary>
    /// Adds reputation with a faction
    /// </summary>
    public void AddReputation(Faction faction, int amount)
    {
        switch (faction)
        {
            case Faction.Choir:
                ChoirReputation = Mathf.Clamp(ChoirReputation + amount, -100, 100);
                break;
            case Faction.CanopyCorp:
                CanopyCorpReputation = Mathf.Clamp(CanopyCorpReputation + amount, -100, 100);
                break;
            case Faction.UnderveilGangs:
                UnderveilGangsReputation = Mathf.Clamp(UnderveilGangsReputation + amount, -100, 100);
                break;
        }
        
        Debug.Log($"Reputation change: {faction} += {amount} (now {GetReputation(faction)})");
    }
    
    /// <summary>
    /// Gets current reputation with a faction
    /// </summary>
    public int GetReputation(Faction faction)
    {
        switch (faction)
        {
            case Faction.Choir: return ChoirReputation;
            case Faction.CanopyCorp: return CanopyCorpReputation;
            case Faction.UnderveilGangs: return UnderveilGangsReputation;
            default: return 0;
        }
    }
    
    /// <summary>
    /// Adds weight toward a specific ending
    /// </summary>
    public void AddEndingWeight(EndingType ending, int amount)
    {
        switch (ending)
        {
            case EndingType.Liberation:
                LiberationWeight = Mathf.Max(0, LiberationWeight + amount);
                break;
            case EndingType.Reform:
                ReformWeight = Mathf.Max(0, ReformWeight + amount);
                break;
            case EndingType.Ascension:
                AscensionWeight = Mathf.Max(0, AscensionWeight + amount);
                break;
        }
        
        Debug.Log($"Ending weight: {ending} += {amount} (now {GetEndingWeight(ending)})");
    }
    
    /// <summary>
    /// Gets current weight for an ending
    /// </summary>
    public int GetEndingWeight(EndingType ending)
    {
        switch (ending)
        {
            case EndingType.Liberation: return LiberationWeight;
            case EndingType.Reform: return ReformWeight;
            case EndingType.Ascension: return AscensionWeight;
            default: return 0;
        }
    }
    
    /// <summary>
    /// Determines which ending will be triggered based on weights
    /// </summary>
    public EndingType GetLikelyEnding()
    {
        // In the actual game, this would be checked at the end of Act III
        // For now, just return the highest weight
        if (LiberationWeight >= ReformWeight && LiberationWeight >= AscensionWeight)
            return EndingType.Liberation;
        else if (ReformWeight >= LiberationWeight && ReformWeight >= AscensionWeight)
            return EndingType.Reform;
        else
            return EndingType.Ascension;
    }
    
    /// <summary>
    /// Resets story state (for new game)
    /// </summary>
    public void Reset()
    {
        ChoirReputation = 0;
        CanopyCorpReputation = 0;
        UnderveilGangsReputation = 0;
        _storyFlags.Clear();
        LiberationWeight = 0;
        ReformWeight = 0;
        AscensionWeight = 0;
        ActProgress = 0;
        HasDiscoveredTruth = false;
        HasContactedChoir = false;
        HasEscapedCanopy = false;
        CurrentMissionID = "";
        LastLocationVisited = "";
        
        Debug.Log("StoryState reset");
    }
}

// Factions in the game
public enum Faction
{
    Choir,
    CanopyCorp,
    UnderveilGangs
}

// Possible endings
public enum EndingType
{
    Liberation,    // Burn down the Mark system
    Reform,        // Reform the Mark system
    Ascension      // Take control of the Mark system
}
