// SportsCar.cs
// Fast/fragile sports model vehicle
using FlaxEngine;

public class SportsCar : VehicleController
{
    public override void OnStart()
    {
        base.OnStart();
        
        VehicleID = "VEH_SPORT_001";
        VehicleName = "Venom GT";
        VehicleClass = "Sports";
        
        // Sports car tuning
        Acceleration = 25f;          // Very fast acceleration
        BrakeStrength = 20f;
        MaxSpeed = 45f;              // High top speed
        TurnSpeed = 120f;            // Quick turning
        Mass = 1200f;                // Lightweight
        
        // Sports cars are easier to hijack but have alarms
        HijackTimeRequired = 2.5f;   // Faster to hijack
        
        Debug.Log("SportsCar initialized");
    }
    
    // Sports car specific behavior could go here
    // Like better handling, more fragile, etc.
}
