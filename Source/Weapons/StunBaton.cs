// StunBaton.cs
// Melee non-lethal weapon for close range takedowns
using FlaxEngine;

public class StunBaton : WeaponBase
{
    public override void OnStart()
    {
        base.OnStart();
        
        WeaponID = "WEAP_STUN_001";
        WeaponName = "Stun Baton";
        Description = "Close-range electroshock weapon for incapacitating targets without lethal force.";
        Damage = 5f; // Low damage, mainly for stagger
        FireRate = 2.0f; // Fast melee swings
        Range = 1.5f;  // Melee range
        Accuracy = 0.95f; // High accuracy for melee
        ReloadTime = 0f; // No ammo, energy-based
        MagazineSize = 1; // Conceptual - energy based
        HeatGenerated = 8f; // Low heat generation
        
        // For melee, we'll use a different system
        // CurrentAmmo will represent energy/charge
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        
        // Recharge energy over time (instead of ammo)
        if (CurrentAmmo < MagazineSize)
        {
            CurrentAmmo += 5f * Time.DeltaTime; // Recharge 5 units per second
            CurrentAmmo = Mathf.Min(CurrentAmmo, MagazineSize);
        }
    }
    
    public override bool Fire()
    {
        // For melee, we check if we have enough energy and if we're in range
        if (_isReloading) return false; // Not really reloading, but cooldown
        if (CurrentAmmo < 20f) return false; // Need 20 energy to use
        if (Time.Time - _lastShotTime < (1f / FireRate)) return false;
        
        // Check if target is in melee range
        var ray = new Ray(Transform.Position + Vector3.Up * 0.5f, Transform.Forward);
        if (Physics.Raycast(ray, out var hit, Range))
        {
            // Check if hit is a valid target (NPC, robot, etc.)
            var target = hit.Actor;
            if (target != null)
            {
                // Apply stun effect
                var stunEffect = target.AddComponent<StunEffect>();
                stunEffect.StunDuration = 4.0f;
                
                // Apply some damage
                var healthComp = target.GetComponent<PlayerStats>(); // Or NPC health component
                if (healthComp != null)
                {
                    healthComp.ApplyDamage(Damage);
                }
                
                // Generate heat
                var hudManager = Scene.GetActor("HUDManager")?.GetComponent<HUDManager>();
                if (hudManager != null)
                {
                    hudManager.IncreaseHeat(HeatGenerated);
                }
                
                _lastShotTime = Time.Time;
                CurrentAmmo -= 20f; // Use energy
                
                Debug.Log($"Stun Baton hit: {target.Name}");
                return true;
            }
        }
        
        return false;
    }
    
    protected override void PerformFire()
    {
        // For melee, the logic is in Fire() method due to raycasting
        // This method is kept for compatibility with base class
        Fire();
    }
}

// Simple stun effect
public class StunEffect : Script
{
    public float StunDuration = 4.0f;
    private float _stunTimer;
    
    public override void OnStart()
    {
        _stunTimer = StunDuration;
    }
    
    public override void OnUpdate()
    {
        if (_stunTimer > 0)
        {
            _stunTimer -= Time.DeltaTime;
            if (_stunTimer <= 0)
            {
                Destroy(this);
                // Target can act again
            }
        }
    }
}
