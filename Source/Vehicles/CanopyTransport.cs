// CanopyTransport.cs
// Slow armored transport used by the Canopy elite
using FlaxEngine;

public class CanopyTransport : VehicleController
{
    public override void OnStart()
    {
        base.OnStart();
        
        VehicleID = "VEH_CANOPY_001";
        VehicleName = "Fortress Hauler";
        VehicleClass = "CanopyTransport";
        
        // Canopy transport tuning
        Acceleration = 6f;           // Slow acceleration
        BrakeStrength = 12f;         // Adequate brakes
        MaxSpeed = 20f;              // Low top speed
        TurnSpeed = 60f;             // Slow turning
        Mass = 3500f;                // Very heavy, armored
        
        // Canopy transports are very hard to hijack (high security)
        HijackTimeRequired = 8.0f;   // Takes much longer to hijack
        
        Debug.Log("CanopyTransport initialized");
    }
    
    // Canopy transport specific behavior
    // Very damage resistant, might have mounted weapons
    // Could have passenger weapons in a full implementation
}
