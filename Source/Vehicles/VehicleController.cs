// VehicleController.cs
// Base class for all vehicles in the game
using FlaxEngine;

public abstract class VehicleController : Script
{
    // Vehicle properties
    public string VehicleID;
    public string VehicleName;
    public string VehicleClass; // Sports, UnderveilRunner, CanopyTransport
    
    // Physics properties
    public float Acceleration = 10f;
    public float BrakeStrength = 15f;
    public float MaxSpeed = 30f; // meters per second
    public float TurnSpeed = 100f; // degrees per second
    public float Mass = 1500f;   // kilograms
    
    // Wheel colliders (would be set up in editor)
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public WheelCollider RearLeftWheel;
    public WheelCollider RearRightWheel;
    
    // Vehicle state
    public float CurrentSpeed; // m/s
    public float EngineRPM;    // revolutions per minute
    public bool IsEngineOn;
    public bool IsLocked;
    
    // Hijacking
    public bool IsBeingHijacked;
    public float HijackProgress;
    public float HijackTimeRequired = 3.0f; // seconds to hijack
    
    public override void OnStart()
    {
        // Initialize vehicle
        IsEngineOn = false;
        CurrentSpeed = 0f;
        EngineRPM = 0f;
        IsLocked = true;
        IsBeingHijacked = false;
        HijackProgress = 0f;
        
        // Set up wheel colliders if not assigned
        SetupWheels();
    }
    
    public override void OnUpdate()
    {
        // Update vehicle physics
        UpdateEngine();
        UpdateTransmission();
        UpdateWheels();
        
        // Handle hijacking
        UpdateHijacking();
        
        // Update visuals (position, rotation, wheel rotation)
        UpdateVisuals();
    }
    
    protected virtual void SetupWheels()
    {
        // In a full implementation, we would automatically find or create wheel colliders
        // For now, we'll assume they're set in the editor
        if (FrontLeftWheel == null || FrontRightWheel == null || RearLeftWheel == null || RearRightWheel == null)
        {
            Debug.LogWarning("VehicleController: Some wheel colliders not assigned. Please assign in editor.");
        }
    }
    
    protected virtual void UpdateEngine()
    {
        // Simple engine model
        float throttleInput = 0f;
        if (Input.GetButton("Accelerate")) throttleInput = 1f;
        if (Input.GetButton("Brake")) throttleInput = -0.5f;
        
        // Engine RPM based on speed and throttle
        float targetRPM = Mathf.Lerp(0f, 6000f, Mathf.Abs(CurrentSpeed) / MaxSpeed) * Mathf.Sign(CurrentSpeed);
        if (throttleInput > 0) EngineRPM = Mathf.Lerp(EngineRPM, targetRPM, Acceleration * Time.DeltaTime);
        else if (throttleInput < 0) EngineRPM = Mathf.Lerp(EngineRPM, 0f, BrakeStrength * Time.DeltaTime); // Engine braking
        else EngineRPM = Mathf.Lerp(EngineRPM, 0f, 5f * Time.DeltaTime); // Idle decay
        
        EngineRPM = Mathf.Clamp(EngineRPM, 0f, 7000f);
        
        // Determine if engine should be on
        IsEngineOn = Mathf.Abs(EngineRPM) > 100f;
    }
    
    protected virtual void UpdateTransmission()
    {
        // Simple speed calculation based on wheel RPM
        if (FrontLeftWheel != null && FrontRightWheel != null)
        {
            float leftRPM = FrontLeftWheel.RPM;
            float rightRPM = FrontRightWheel.RPM;
            float avgRPM = (leftRPM + rightRPM) * 0.5f;
            
            // Convert RPM to m/s (simplified)
            float wheelRadius = 0.3f; // 30cm radius
            CurrentSpeed = avgRPM * (wheelRadius * 2f * 3.14159f / 60f); // RPM to m/s
        }
        else
        {
            // Fallback: estimate speed from engine RPM
            CurrentSpeed = (EngineRPM / 6000f) * MaxSpeed;
        }
        
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, -MaxSpeed/2f, MaxSpeed); // Allow some reverse speed
    }
    
    protected virtual void UpdateWheels()
    {
        // Apply engine torque to wheels
        if (!IsEngineOn) return;
        
        float engineTorque = Mathf.Lerp(0f, 300f, EngineRPM / 6000f); // Simplified torque curve
        float throttleInput = Mathf.Clamp01(Input.GetAxis("Accelerate"));
        float brakeInput = Mathf.Clamp01(-Input.GetAxis("Accelerate")); // Negative axis for brake
        
        float torque = engineTorque * throttleInput;
        float brakeTorque = brakeInput * 500f; // Brake strength
        
        // Apply to wheels
        if (FrontLeftWheel != null)
        {
            FrontLeftWheel.EngineTorque = torque * 0.5f; // Split torque between wheels
            FrontLeftWheel.BrakeTorque = brakeTorque * 0.5f;
            FrontLeftWheel.SteerAngle = GetSteerAngle();
        }
        
        if (FrontRightWheel != null)
        {
            FrontRightWheel.EngineTorque = torque * 0.5f;
            FrontRightWheel.BrakeTorque = brakeTorque * 0.5f;
            FrontRightWheel.SteerAngle = GetSteerAngle();
        }
        
        // Rear wheels Get brake and possibly limited slip
        if (RearLeftWheel != null)
        {
            RearLeftWheel.BrakeTorque = brakeTorque * 0.5f;
        }
        
        if (RearRightWheel != null)
        {
            RearRightWheel.BrakeTorque = brakeTorque * 0.5f;
        }
    }
    
    protected virtual float GetSteerAngle()
    {
        float steerInput = Input.GetAxis("Steer"); // Left/right or left stick X
        float maxSteerAngle = 30f; // degrees
        return steerInput * maxSteerAngle;
    }
    
    protected virtual void UpdateHijacking()
    {
        // Check if player is attempting to hijack
        if (Input.GetButtonDown("Hijack") && IsLocked && !IsBeingHijacked)
        {
            // Check if player is close enough to vehicle
            var player = Scene.GetActor("Player");
            if (player != null)
            {
                float distance = Vector3.Distance(player.Position, Transform.Position);
                if (distance < 3.0f) // Within 3 meters
                {
                    StartHijacking();
                }
            }
        }
        
        if (IsBeingHijacked)
        {
            HijackProgress += Time.DeltaTime;
            if (HijackProgress >= HijackTimeRequired)
            {
                CompleteHijack();
            }
        }
    }
    
    protected virtual void StartHijacking()
    {
        IsBeingHijacked = true;
        HijackProgress = 0f;
        Debug.Log("Hijacking started");
        
        // Show hijacking UI/progress bar
        var hudManager = Scene.GetActor("HUDManager")?.GetComponent<HUDManager>();
        if (hudManager != null)
        {
            // In a full implementation, we would show hijacking progress
            Debug.Log("Show hijacking UI");
        }
    }
    
    protected virtual void CompleteHijack()
    {
        IsBeingHijacked = false;
        HijackProgress = 0f;
        IsLocked = false;
        IsEngineOn = true; // Start engine
        Debug.Log("Hijack complete - vehicle unlocked and engine started");
        
        // Hide hijacking UI
        // Play hijack complete sound/effect
    }
    
    protected virtual void UpdateVisuals()
    {
        // Update wheel rotation visuals
        if (FrontLeftWheel != null)
        {
            // Rotate wheel mesh based on wheel rotation and steer angle
            // This would be done by attaching visible wheel meshes to the wheel colliders
            // For simplicity, we'll just log
        }
        
        // Update vehicle position and rotation from physics
        // Actually, the CharacterController or Rigidbody handles this
        // WheelController would handle the wheel movement
    }
    
    // Public methods for external control
    public void Accelerate(float amount) { /* Implemented in UpdateEngine */ }
    public void Brake(float amount) { /* Implemented in UpdateEngine */ }
    public void Steer(float amount) { /* Implemented in GetSteerAngle */ }
    
    public bool CanBeHijacked => IsLocked && !IsBeingHijacked;
}
