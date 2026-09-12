// TranqPistol.cs
// Non-lethal ranged weapon that fires tranquilizer darts
using FlaxEngine;

public class TranqPistol : WeaponBase
{
    public override void OnStart()
    {
        base.OnStart();
        
        WeaponID = "WEAP_TRANQ_001";
        WeaponName = "Tranquilizer Pistol";
        Description = "Silent ranged weapon that fires tranquilizer darts to incapacitate targets.";
        Damage = 2f; // Very low damage
        FireRate = 0.8f; // Moderate fire rate
        Range = 15f;
        Accuracy = 0.7f; // Less accurate than regular pistols due to dart drop
        ReloadTime = 2.5f;
        MagazineSize = 8;
        HeatGenerated = 5f; // Very low heat (silent)
    }
    
    protected override void PerformFire()
    {
        // Fire tranquilizer dart
        var ray = new Ray(Transform.Position + Transform.Up * 0.3f, Transform.Forward);
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var target = hit.Actor;
            if (target != null)
            {
                // Apply tranq effect
                var tranqEffect = target.AddComponent<TranqEffect>();
                tranqEffect.Duration = 8.0f; // Tranquilized for 8 seconds
                
                // Apply minimal damage
                var healthComp = target.GetComponent<PlayerStats>(); // Or NPC health
                if (healthComp != null)
                {
                    healthComp.ApplyDamage(Damage);
                }
                
                Debug.Log($"Tranq Pistol hit: {target.Name}");
            }
        }
        else
        {
            // Dart hit nothing - could mark impact position for visual effect
            Debug.Log("Tranq dart missed target");
        }
    }
}

// Tranquilizer effect
public class TranqEffect : Script
{
    public float Duration = 8.0f;
    private float _timer;
    private NPCController _npcController;
    private CharacterController _characterController;
    
    public override void OnStart()
    {
        _npcController = Actor.GetComponent<NPCController>();
        _characterController = Actor.GetComponent<CharacterController>();
        _timer = Duration;
        
        Debug.Log($"Target tranquilized for {Duration}s");
        
        // If it's an NPC, temporarily disable or change state
        if (_npcController != null)
        {
            // In a full implementation, we would change AI state to unconscious/sleeping
        }
        
        // If it's the player, apply effects
        if (_characterController != null)
        {
            // Player would be affected too if hit by friendly fire
        }
    }
    
    public override void OnUpdate()
    {
        if (_timer > 0)
        {
            _timer -= Time.DeltaTime;
            if (_timer <= 0)
            {
                Destroy(this);
                // Target wakes up
                
                // If NPC, resume normal behavior
                var npcController = Actor.GetComponent<NPCController>();
                if (npcController != null)
                {
                    // Resume normal AI
                }
            }
        }
    }
}
