// EMPPulse.cs
// Non-lethal EMP weapon that disables electronics and robots
using FlaxEngine;

public class EMPPulse : WeaponBase
{
    public override void OnStart()
    {
        base.OnStart();
        
        WeaponID = "WEAP_EMP_001";
        WeaponName = "EMP Pulse";
        Description = "Emits an electromagnetic pulse that temporarily disables electronic devices and robots.";
        Damage = 0f; // EMP doesn't do direct damage
        FireRate = 0.2f; // Slow firing rate
        Range = 8f;
        Accuracy = 0.9f;
        ReloadTime = 3.0f;
        MagazineSize = 1; // Single shot before reload
        HeatGenerated = 15f; // Moderate heat generation
    }
    
    protected override void PerformFire()
    {
        // EMP effect: disable electronics in radius
        var empCenter = Transform.Position;
        float empRadius = 6f;
        
        // Find all actors in radius
        var hits = Physics.SphereCastAll(empCenter, empRadius, Vector3.Up, 0f);
        foreach (var hit in hits)
        {
            // Check if hit actor has electronic components to disable
            var electronicComp = hit.Actor.GetComponent<IElectronic>();
            if (electronicComp != null)
            {
                electronicComp.DisableForDuration(5.0f); // Disable for 5 seconds
                Debug.Log($"EMP affected: {hit.Actor.Name}");
            }
            
            // Specifically affect robots/NPCs
            var npcController = hit.Actor.GetComponent<NPCController>();
            if (npcController != null)
            {
                // Stun robot for short duration
                var robotStun = hit.Actor.AddComponent<RobotStunEffect>();
                robotStun.StunDuration = 3.0f;
                Debug.Log($"EMP stunned robot: {hit.Actor.Name}");
            }
            
            // Affect cameras, terminals, etc.
            var hackable = hit.Actor.GetComponent<IHackable>();
            if (hackable != null)
            {
                // Temporary hack - disable for short time
                var tempDisable = hit.Actor.AddComponent<TemporaryDisableEffect>();
                tempDisable.Duration = 4.0f;
                Debug.Log($"EMP disabled hackable: {hit.Actor.Name}");
            }
        }
        
        // Play EMP effect (visual/sound)
        // Would spawn EMP wave visual effect here
        Debug.Log("EMP Pulse discharged!");
    }
}

// Interface for electronic objects that can be disabled by EMP
public interface IElectronic
{
    void DisableForDuration(float duration);
}

// Simple effect components for EMP
public class RobotStunEffect : Script
{
    public float StunDuration = 3.0f;
    private float _stunTimer;
    private NPCController _npcController;
    
    public override void OnStart()
    {
        _npcController = Actor.GetComponent<NPCController>();
        _stunTimer = StunDuration;
        
        if (_npcController != null)
        {
            // Temporarily disable AI
            // In a full implementation, we would change state to stunned
            Debug.Log($"Robot stunned for {StunDuration}s");
        }
    }
    
    public override void OnUpdate()
    {
        if (_stunTimer > 0)
        {
            _stunTimer -= Time.DeltaTime;
            if (_stunTimer <= 0)
            {
                // End stun effect
                Destroy(this);
                // Resume normal AI behavior
            }
        }
    }
}

public class TemporaryDisableEffect : Script
{
    public float Duration = 4.0f;
    private float _timer;
    
    public override void OnStart()
    {
        _timer = Duration;
    }
    
    public override void OnUpdate()
    {
        if (_timer > 0)
        {
            _timer -= Time.DeltaTime;
            if (_timer <= 0)
            {
                Destroy(this);
                // Object is re-enabled
            }
        }
    }
}
