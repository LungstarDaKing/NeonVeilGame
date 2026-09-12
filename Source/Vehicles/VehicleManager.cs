// VehicleManager.cs
// Manages vehicles in the game world
using FlaxEngine;
using System.Collections.Generic;

public class VehicleManager : Script
{
    // Singleton
    public static VehicleManager Instance { get; private set; }
    
    private List<VehicleController> _vehicles = new List<VehicleController>();
    private List<VehicleController> _availableVehicles = new List<VehicleController>(); // Not hijacked/locked
    
    public override void OnStart()
    {
        // Set up singleton
        if (Instance != null && Instance != this)
        {
            Destroy(Actor);
            return;
        }
        Instance = this;
        
        // Find all vehicles in the scene
        FindAllVehicles();
    }
    
    private void FindAllVehicles()
    {
        _vehicles.Clear();
        _availableVehicles.Clear();
        
        var vehicleActors = Scene.GetAllActors()
                            .SelectMany(a => a.GetComponents<VehicleController>());
        
        _vehicles.AddRange(vehicleActors);
        
        foreach (var vehicle in _vehicles)
        {
            if (vehicle.CanBeHijacked)
            {
                _availableVehicles.Add(vehicle);
            }
        }
        
        Debug.Log($"Found {_vehicles.Count} total vehicles, {_availableVehicles.Count} available for hijacking");
    }
    
    public void RegisterVehicle(VehicleController vehicle)
    {
        if (!_vehicles.Contains(vehicle))
        {
            _vehicles.Add(vehicle);
            if (vehicle.CanBeHijacked)
            {
                _availableVehicles.Add(vehicle);
            }
        }
    }
    
    public void UnregisterVehicle(VehicleController vehicle)
    {
        _vehicles.Remove(vehicle);
        _availableVehicles.Remove(vehicle);
    }
    
    public VehicleController GetClosestAvailableVehicle(Vector3 position, float maxRadius = 15f)
    {
        VehicleController closest = null;
        float closestDistance = maxRadius;
        
        foreach (var vehicle in _availableVehicles)
        {
            float distance = Vector3.Distance(position, vehicle.Position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = vehicle;
            }
        }
        
        return closest;
    }
    
    public void ReportStolenVehicle(VehicleController vehicle)
    {
        // When a vehicle is hijacked, remove from available list
        _availableVehicles.Remove(vehicle);
        // Could trigger police response, increase heat, etc.
        Debug.Log($"Vehicle reported stolen: {vehicle.VehicleName}");
        
        // Increase player heat for stealing a vehicle
        var hudManager = Scene.GetActor("HUDManager")?.GetComponent<HUDManager>();
        if (hudManager != null)
        {
            float heatIncrease = vehicle.VehicleClass switch
            {
                "Sports" => 15f,
                "UnderveilRunner" => 10f,
                "CanopyTransport" => 25f, // Stealing a canopy vehicle is a big deal
                _ => 10f
            };
            hudManager.IncreaseHeat(heatIncrease);
        }
    }
    
    public IReadOnlyList<VehicleController> GetAllVehicles => _vehicles.AsReadOnly();
    public IReadOnlyList<VehicleController> GetAvailableVehicles => _availableVehicles.AsReadOnly();
}
