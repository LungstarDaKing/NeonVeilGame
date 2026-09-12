// SaveLoadSystem.cs
// Simple save/load system for preserving game progress
using FlaxEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class GameSaveData
{
    public int ActProgress = 0;
    public string LastMissionCompleted = "";
    public Dictionary<string, bool> StoryFlags = new Dictionary<string, bool>();
    public Dictionary<string, int> FactionReputation = new Dictionary<string, int>();
    public Dictionary<string, int> EndingWeights = new Dictionary<string, int>();
    public float PlayTime = 0f;
    public DateTime SaveTime;
    
    public GameSaveData()
    {
        StoryFlags = new Dictionary<string, bool>();
        FactionReputation = new Dictionary<string, int>();
        EndingWeights = new Dictionary<string, int>();
        
        // Initialize reputation for all factions
        FactionReputation[StoryState.Faction.Choir.ToString()] = 0;
        FactionReputation[StoryState.Faction.CanopyCorp.ToString()] = 0;
        FactionReputation[StoryState.Faction.UnderveilGangs.ToString()] = 0;
        
        // Initialize ending weights
        EndingWeights[StoryState.EndingType.Liberation.ToString()] = 0;
        EndingWeights[StoryState.EndingType.Reform.ToString()] = 0;
        EndingWeights[StoryState.EndingType.Ascension.ToString()] = 0;
    }
}

public class SaveLoadSystem : Script
{
    private const string SaveDirectory = "Saves";
    private const string SaveFileName = "NeonVeilSave.dat";
    private string SavePath => Path.Combine(SaveDirectory, SaveFileName);
    
    private float _playTimeTimer;
    private bool _isSaving = false;
    private bool _isLoading = false;
    
    public override void OnStart()
    {
        // Ensure save directory exists
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
        
        Debug.Log("SaveLoadSystem initialized");
        
        // Try to load existing save
        LoadGame();
    }
    
    public override void OnUpdate()
    {
        // Track play time
        _playTimeTimer += Time.DeltaTime;
        
        // Auto-save every 5 minutes
        if (_playTimeTimer >= 300f && !_isSaving) // 5 minutes
        {
            _playTimeTimer = 0f;
            SaveGame();
        }
        
        // Manual save/load for testing (press F5 to save, F9 to load)
        if (Input.GetKeyDown(KeyCode.F5) && !_isSaving)
        {
            SaveGame();
        }
        
        if (Input.GetKeyDown(KeyCode.F9) && !_isLoading)
        {
            LoadGame();
        }
    }
    
    public void SaveGame()
    {
        if (_isSaving) return;
        _isSaving = true;
        
        try
        {
            GameSaveData data = new GameSaveData();
            
            // Gather game state from StoryState and other systems
            var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
            var missionManager = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>();
            var hudManager = Scene.GetActor("HUDManager")?.GetComponent<HUDManager>();
            
            if (storyState != null)
            {
                // Save story flags
                // Note: In a real implementation, we would need a way to enumerate all flags
                // For now, we'll save some key ones
                data.StoryFlags["discovered_truth"] = storyState.GetFlag("discovered_truth");
                data.StoryFlags["escaped_canopy_hq"] = storyState.GetFlag("escaped_canopy_hq");
                data.StoryFlags["contacted_choir"] = storyState.GetFlag("contacted_choir");
                data.StoryFlags["choir_support_liberation"] = storyState.GetFlag("choir_support_liberation");
                data.StoryFlags["choir_support_reform"] = storyState.GetFlag("choir_support_reform");
                data.StoryFlags["choir_neutral"] = storyState.GetFlag("choir_neutral");
                data.StoryFlags["guard_executed"] = storyState.GetFlag("guard_executed");
                data.StoryFlags["guard_subdued"] = storyState.GetFlag("guard_subdued");
                data.StoryFlags["guard_released"] = storyState.GetFlag("guard_released");
                data.StoryFlags["relay_station_hacked"] = storyState.GetFlag("relay_station_hacked");
                
                // Save faction reputation
                data.FactionReputation[StoryState.Faction.Choir.ToString()] = storyState.GetReputation(StoryState.Faction.Choir);
                data.FactionReputation[StoryState.Faction.CanopyCorp.ToString()] = storyState.GetReputation(StoryState.Faction.CanopyCorp);
                data.FactionReputation[StoryState.Faction.UnderveilGangs.ToString()] = storyState.GetReputation(StoryState.Faction.UndeveilGangs);
                
                // Save ending weights
                data.EndingWeights[StoryState.EndingType.Liberation.ToString()] = storyState.GetEndingWeight(StoryState.EndingType.Liberation);
                data.EndingWeights[StoryState.EndingType.Reform.ToString()] = storyState.GetEndingWeight(StoryState.EndingType.Reform);
                data.EndingWeights[StoryState.EndingType.Ascension.ToString()] = storyState.GetEndingWeight(StoryState.EndingType.Ascension);
            }
            
            if (missionManager != null)
            {
                var activeMission = missionManager.GetActiveMission;
                if (activeMission != null)
                {
                    data.LastMissionCompleted = activeMission.MissionID;
                }
                
                // Track act progress based on completed missions
                // This would be more sophisticated in a real implementation
                if (missionManager.GetCompletedMissions.Count >= 1)
                    data.ActProgress = 1; // Act I complete
                if (missionManager.GetCompletedMissions.Count >= 2)
                    data.ActProgress = 2; // Act II complete
                if (missionManager.GetCompletedMissions.Count >= 3)
                    data.ActProgress = 3; // Act III complete
            }
            
            data.PlayTime = _playTimeTimer;
            data.SaveTime = DateTime.Now;
            
            // Save to file
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(SavePath, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
            
            Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Game saved to {SavePath}");
            Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Play time: {data.PlayTime/60f:F1} minutes");
        }
        catch (Exception e)
        {
            Debug.LogError($"�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Save failed: {e.Message}");
        }
        finally
        {
            _isSaving = false;
        }
    }
    
    public void LoadGame()
    {
        if (_isLoading) return;
        if (!File.Exists(SavePath))
        {
            Debug.Log("�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� No save file found.");
            _isLoading = false;
            return;
        }
        
        _isLoading = true;
        
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(SavePath, FileMode.Open))
            {
                GameSaveData data = (GameSaveData)formatter.Deserialize(stream);
                
                // Apply loaded state to game systems
                var storyState = Scene.GetActor("StoryState")?.GetComponent<StoryState>();
                var missionManager = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>();
                
                if (storyState != null)
                {
                    // Load story flags
                    foreach (var kvp in data.StoryFlags)
                    {
                        storyState.SetFlag(kvp.Key, kvp.Value);
                    }
                    
                    // Load faction reputation
                    foreach (var kvp in data.FactionReputation)
                    {
                        if (Enum.TryParse(typeof(StoryState.Faction), kvp.Key, true, out object factionObj))
                        {
                            StoryState.Faction faction = (StoryState.Faction)factionObj;
                            storyState.AddReputation(faction, kvp.Value - storyState.GetReputation(faction)); // Delta
                        }
                    }
                    
                    // Load ending weights
                    foreach (var kvp in data.EndingWeights)
                    {
                        if (Enum.TryParse(typeof(StoryState.EndingType), kvp.Key, true, out object endingObj))
                        {
                            StoryState.EndingType ending = (StoryState.EndingType)endingObj;
                            storyState.AddEndingWeight(ending, kvp.Value - storyState.GetEndingWeight(ending)); // Delta
                        }
                    }
                }
                
                // Note: Mission progress loading would be more complex
                // For simplicity, we'll just note what was last completed
                if (missionManager != null && !string.IsNullOrEmpty(data.LastMissionCompleted))
                {
                    Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Last mission was: {data.LastMissionCompleted}");
                }
                
                _playTimeTimer = data.PlayTime;
                
                Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Loaded game from {SavePath}");
                Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Play time: {data.PlayTime/60f:F1} minutes");
                Debug.Log($"�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Act progress: {data.ActProgress}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Load failed: {e.Message}");
        }
        finally
        {
            _isLoading = false;
        }
    }
    
    // Public methods for other systems to call
    public static void SaveGameStatic()
    {
        var saveSystem = Scene.GetActor("SaveLoadSystem")?.GetComponent<SaveLoadSystem>();
        saveSystem?.SaveGame();
    }
    
    public static void LoadGameStatic()
    {
        var saveSystem = Scene.GetActor("SaveLoadSystem")?.GetComponent<SaveLoadSystem>();
        saveSystem?.LoadGame();
    }
}
