// VehicleHijackTracker.cs
// Tracks vehicle hijacking for mission objectives
using FlaxEngine;

public class VehicleHijackTracker : Script
{
    // Reference to the mission
    private Act1Awakening _mission;
    
    // Tracking state
    private bool _hijackCompleted = false;
    private float _timeSinceHijack = 0f;
    private const float EscapeTimeRequired = 5.0f; // seconds to survive after hijack to complete objective
    
    public override void OnStart()
    {
        _mission = Scene.GetActor("MissionManager")?.GetComponent<MissionManager>()
                    ?.GetActiveMission as Act1Awakening;
        
        Debug.Log("VehicleHijackTracker initialized - monitoring for vehicle hijacking");
    }
    
    public override void OnUpdate()
    {
        // If we already completed the objective, no need to track
        if (_hijackCompleted) return;
        
        // Check if any vehicle has been successfully hijacked
        var vehicleManager = Scene.GetActor("VehicleManager")?.GetComponent<VehicleManager>();
        if (vehicleManager != null)
        {
            // Check all vehicles for hijack state
            foreach (var vehicle in vehicleManager.GetAllVehicles)
            {
                // Check if this vehicle was hijacked (not locked and engine on)
                if (!vehicle.IsLocked && vehicle.IsEngineOn)
                {
                    // Vehicle has been hijacked
                    if (!_hijackCompleted)
                    {
                        _hijackCompleted = true;
                        _timeSinceHijack = 0f;
                        Debug.Log("������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������ Vehicle hijacked!");
                        Debug.Log($"�������������������������������������������������������������������� Vehicle: {vehicle.VehicleName} ({vehicle.VehicleClass})");
                    }
                }
            }
        }
        
        // If hijack completed, start counting escape time
        if (_hijackCompleted)
        {
            _timeSinceHijack += Time.DeltaTime;
            
            // Optional: show escape progress in console every second
            if ((int)_timeSinceHijack != (int)(_timeSinceHijack - Time.DeltaTime))
            {
                int remaining = Mathf.CeilToInt(EscapeTimeRequired - _timeSinceHijack);
                if (remaining > 0)
                {
                    Debug.Log($"���������������������������������������������������������������������������������������������������������������������������������������������������� Escaping patrol drones... {remaining}s remaining");
                }
            }
            
            // If enough time has passed, consider escape successful
            if (_timeSinceHijack >= EscapeTimeRequired)
            {
                CompleteObjective();
                Debug.Log("�������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Successfully escaped Midlayer patrol drones!");
            }
        }
    }
    
    private void CompleteObjective()
    {
        if (_mission != null)
        {
            var objective = _mission.GetObjectiveByDescription(
                "Hijack a vehicle to escape Midlayer patrol drones");
            if (objective != null && !objective.IsComplete)
            {
                _mission.CompleteObjective(objective.ID);
                Debug.Log("���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������� Objective completed: Hijack a vehicle to escape Midlayer patrol drones");
            }
        }
        
        // Prevent further processing
        enabled = false;
    }
}
