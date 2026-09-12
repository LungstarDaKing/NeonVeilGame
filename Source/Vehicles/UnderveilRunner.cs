// UnderveilRunner.cs
// Rugged mid-tier vehicle common in the Underveil
using FlaxEngine;

public class UnderveilRunner : VehicleController
{
    public override void OnStart()
    {
        base.OnStart();
        
        VehicleID = "VEH_UNDER_001";
        VehicleName = "Raptor";
        VehicleClass = "UnderveilRunner";
        
        // Underveil runner tuning
        Acceleration = 12f;          // Decent acceleration
        BrakeStrength = 18f;         // Good brakes for rough terrain
        MaxSpeed = 32f;              // Moderate top speed
        TurnSpeed = 90f;             // decent turning
        Mass = 1800f;                // Heavier, more durable
        
        // Underveil runners are harder to hijack (more secure, less alarms)
        HijackTimeRequired = 4.0f;   // Takes longer to hijack
        
        Debug.Log("UnderveilRunner initialized");
    }
    
    // Underveil runner specific behavior
    // Better off-road capability, more damage resistant
}
