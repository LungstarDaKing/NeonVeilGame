// SilencedPistol.cs
// Standard lethal sidearm with suppressor
using FlaxEngine;

public class SilencedPistol : WeaponBase
{
    public override void OnStart()
    {
        base.OnStart();
        
        WeaponID = "WEAP_PISTOL_001";
        WeaponName = "Silenced Pistol";
        Description = "Compact sidearm with suppressor for quiet eliminations.";
        Damage = 18f;
        FireRate = 0.3f; // ~3.3 shots per second
        Range = 25f;
        Accuracy = 0.85f;
        ReloadTime = 2.2f;
        MagazineSize = 12;
        HeatGenerated = 12f; // Moderate heat
    }
    
    protected override void PerformFire()
    {
        // Fire bullet with possible spread for accuracy
        Vector3 direction = Transform.Forward;
        
        // Apply accuracy - random spread based on accuracy value
        float spread = (1f - Accuracy) * 5f; // Convert accuracy to spread angle
        float spreadX = (Random.Range(0f, 1f) - 0.5f) * spread;
        float spreadY = (Random.Range(0f, 1f) - 0.5f) * spread;
        direction += Transform.Right * spreadX + Transform.Up * spreadY;
        direction.Normalize();
        
        // Fire raycast
        var ray = new Ray(Transform.Position + Transform.Up * 0.2f, direction);
        if (Physics.Raycast(ray, out var hit, Range))
        {
            var target = hit.Actor;
            if (target != null)
            {
                // Apply damage
                var healthComp = target.GetComponent<PlayerStats>(); // Or NPC health component
                if (healthComp != null)
                {
                    healthComp.ApplyDamage(Damage);
                    Debug.Log($"Silenced Pistol hit: {target.Name} for {Damage} damage");
                }
                
                // Optional: add blood effect, impact effect, etc.
            }
        }
        else
        {
            Debug.Log("Silenced Pistol shot missed");
        }
    }
}

// Additional weapon examples would go here:
// - SmartTrackingSMG.cs
// - CombatKnife.cs
// - DecoyHologram.cs
// etc.
