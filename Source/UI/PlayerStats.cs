// PlayerStats.cs
// Component to track player statistics that the HUD can read
using FlaxEngine;

public class PlayerStats : Script
{
    // Stats
    public float MaxHealth = 100f;
    public float CurrentHealth = 100f;
    public float MaxEnergy = 100f;
    public float CurrentEnergy = 100f;
    public float MaxHeat = 100f;
    public float CurrentHeat = 0f;
    
    // Regeneration rates
    public float EnergyRechargeRate = 10f; // per second
    public float HeatCoolDownRate = 5f;   // per second
    
    public override void OnStart()
    {
        // Initialize to max values
        CurrentHealth = MaxHealth;
        CurrentEnergy = MaxEnergy;
        CurrentHeat = 0f;
    }
    
    public override void OnUpdate()
    {
        // Regenerate energy
        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += EnergyRechargeRate * Time.DeltaTime;
            CurrentEnergy = Mathf.Min(CurrentEnergy, MaxEnergy);
        }
        
        // Cool down heat
        if (CurrentHeat > 0f)
        {
            CurrentHeat -= HeatCoolDownRate * Time.DeltaTime;
            CurrentHeat = Mathf.Max(CurrentHeat, 0f);
        }
    }
    
    // Public methods for modifying stats
    public void ApplyDamage(float amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);
        if (CurrentHealth <= 0f)
        {
            // Trigger death event
            var hudManager = Scene.GetActor("HUDManager")?.GetComponent<HUDManager>();
            hudManager?.OnPlayerDeath();
        }
    }
    
    public void ApplyHeal(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }
    
    public void UseEnergy(float amount)
    {
        CurrentEnergy = Mathf.Max(CurrentEnergy - amount, 0f);
    }
    
    public void AddEnergy(float amount)
    {
        CurrentEnergy = Mathf.Min(CurrentEnergy + amount, MaxEnergy);
    }
    
    public void IncreaseHeat(float amount)
    {
        CurrentHeat = Mathf.Min(CurrentHeat + amount, MaxHeat);
    }
    
    public void DecreaseHeat(float amount)
    {
        CurrentHeat = Mathf.Max(CurrentHeat - amount, 0f);
    }
}
