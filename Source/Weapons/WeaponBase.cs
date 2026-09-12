// WeaponBase.cs
// Base class for all weapons in the game
using FlaxEngine;

public abstract class WeaponBase : Script
{
    // Weapon properties
    public string WeaponID;
    public string WeaponName;
    public string Description;
    public Sprite Icon; // UI icon
    
    // Weapon stats
    public float Damage = 10f;
    public float FireRate = 0.5f; // Shots per second
    public float Range = 20f;     // Maximum effective range
    public float Accuracy = 0.8f; // 0-1, where 1 is perfect accuracy
    public float ReloadTime = 2.0f; // Seconds to reload
    public int MagazineSize = 30;   // Rounds per magazine
    public int CurrentAmmo;         // Current ammo in magazine
    
    // Heat properties
    public float HeatGenerated = 10f; // How much heat this weapon generates per shot
    
    // State
    protected bool _isReloading = false;
    protected float _lastShotTime;
    protected bool _isEquipped = false;
    
    public override void OnStart()
    {
        CurrentAmmo = MagazineSize;
        _lastShotTime = Time.Time;
    }
    
    public override void OnUpdate()
    {
        // Handle reload completion
        if (_isReloading && Time.Time - _lastShotTime >= ReloadTime)
        {
            FinishReload();
        }
    }
    
    /// <summary>
    /// Attempts to fire the weapon
    /// </summary>
    /// <returns>True if weapon was fired, false otherwise</returns>
    public virtual bool Fire()
    {
        // Check if we can fire
        if (_isReloading) return false;
        if (CurrentAmmo <= 0) return false;
        if (Time.Time - _lastShotTime < (1f / FireRate)) return false;
        
        // Fire the weapon
        _lastShotTime = Time.Time;
        CurrentAmmo--;
        
        // Generate heat
        var hudManager = Scene.GetActor("HUDManager")?.GetComponent<HUDManager>();
        if (hudManager != null)
        {
            hudManager.IncreaseHeat(HeatGenerated);
        }
        
        // Perform weapon-specific firing logic
        PerformFire();
        
        Debug.Log($"Fired {WeaponName}. Ammo remaining: {CurrentAmmo}");
        return true;
    }
    
    /// <summary>
    /// Weapon-specific firing logic - to be implemented by derived classes
    /// </summary>
    protected abstract void PerformFire();
    
    /// <summary>
    /// Starts reloading the weapon
    /// </summary>
    public virtual void StartReload()
    {
        if (_isReloading || CurrentAmmo >= MagazineSize) return;
        
        _isReloading = true;
        _lastShotTime = Time.Time;
        Debug.Log($"Started reloading {WeaponName}");
        
        // Play reload animation/sound
    }
    
    /// <summary>
    /// Finishes reloading the weapon
    /// </summary>
    protected virtual void FinishReload()
    {
        _isReloading = false;
        int ammoNeeded = MagazineSize - CurrentAmmo;
        CurrentAmmo += ammoNeeded;
        Debug.Log($"Finished reloading {WeaponName}. Ammo: {CurrentAmmo}");
    }
    
    /// <summary>
    /// Equips the weapon
    /// </summary>
    public virtual void Equip()
    {
        _isEquipped = true;
        Debug.Log($"Equipped {WeaponName}");
        // Show weapon model, enable scripts, etc.
    }
    
    /// <summary>
    /// Unequips the weapon
    /// </summary>
    public virtual void Unequip()
    {
        _isEquipped = false;
        Debug.Log($"Unequipped {WeaponName}");
        // Hide weapon model, disable scripts, etc.
    }
    
    /// <summary>
    /// Gets whether the weapon is ready to fire
    /// </>
    public bool IsReadyToFire => !_isReloading && CurrentAmmo > 0 && Time.Time - _lastShotTime >= (1f / FireRate);
    
    /// <summary>
    /// Gets the current ammo percentage (0-1)
    /// </summary>
    public float AmmoPercentage => (float)CurrentAmmo / MagazineSize;
}
