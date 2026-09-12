// WeatherSystem.cs
// Manages weather conditions and effects
using FlaxEngine;
using System.Collections.Generic;

public class WeatherSystem : Script
{
    // Weather types
    public enum WeatherType
    {
        Clear,
        Overcast,
        Rain,
        Fog,
        Storm
    }
    
    // Current weather
    public WeatherType CurrentWeather = WeatherType.Clear;
    public WeatherType TargetWeather = WeatherType.Clear;
    
    // Transition properties
    public float ChangeIntervalMin = 300f; // 5 minutes
    public float ChangeIntervalMax = 900f; // 15 minutes
    public float TransitionDuration = 30f; // 30 seconds to change weather
    
    // Weather effects
    public ParticleSystem RainParticles;
    public ParticleSystem FogParticles;
    public float RainIntensity = 0f; // 0-1
    public float FogDensity = 0f;    // 0-1
    
    // Gameplay effects
    public float VisibilityModifier = 1f; // Affects NPC vision range
    public float SurfaceSlipperiness = 0f; // Affects vehicle handling
    public float AudioAmbiancce = 0f;     // Background audio level
    
    private float _timeUntilChange;
    private float _transitionTimer;
    private bool _isTransitioning;
    
    public override void OnStart()
    {
        // Initialize weather
        _timeUntilChange = Random.Range(ChangeIntervalMin, ChangeIntervalMax);
        _transitionTimer = 0f;
        _isTransitioning = false;
        
        UpdateWeatherEffects();
        
        Debug.Log("WeatherSystem initialized");
    }
    
    public override void OnUpdate()
    {
        // Update weather change timer
        if (!_isTransitioning)
        {
            _timeUntilChange -= Time.DeltaTime;
            if (_timeUntilChange <= 0f)
            {
                StartWeatherChange();
            }
        }
        else
        {
            // Handle transition
            _transitionTimer += Time.DeltaTime;
            float t = _transitionTimer / TransitionDuration;
            t = Mathf.Clamp01(t);
            
            // Smooth step for more natural transition
            t = t * t * (3f - 2f * t);
            
            // Interpolate weather effects
            UpdateWeatherTransition(t);
            
            if (_transitionTimer >= TransitionDuration)
            {
                CompleteWeatherChange();
            }
        }
        
        // Update ambient effects based on weather
        UpdateAmbientEffects();
        
        // Update particle systems
        UpdateParticleSystems();
    }
    
    private void StartWeatherChange()
    {
        _isTransitioning = true;
        _transitionTimer = 0f;
        
        // Choose new target weather (weighted toward clear/overcast)
        float r = Random.Range(0f, 1f);
        if (r < 0.4f) TargetWeather = WeatherType.Clear;
        else if (r < 0.7f) TargetWeather = WeatherType.Overcast;
        else if (r < 0.9f) TargetWeather = WeatherType.Rain;
        else if (r < 0.95f) TargetWeather = WeatherType.Fog;
        else TargetWeather = WeatherType.Storm;
        
        Debug.Log($"Weather changing from {CurrentWeather} to {TargetWeather}");
    }
    
    private void CompleteWeatherChange()
    {
        _isTransitioning = false;
        CurrentWeather = TargetWeather;
        _timeUntilChange = Random.Range(ChangeIntervalMin, ChangeIntervalMax);
        
        Debug.Log($"Weather change complete: now {CurrentWeather}");
    }
    
    private void UpdateWeatherTransition(float t)
    {
        // Interpolate between current and target weather values
        float currentRain = GetWeatherValue(CurrentWeather, WeatherType.Rain);
        float targetRain = GetWeatherValue(TargetWeather, WeatherType.Rain);
        RainIntensity = Mathf.Lerp(currentRain, targetRain, t);
        
        float currentFog = GetWeatherValue(CurrentWeather, WeatherType.Fog);
        float targetFog = GetWeatherValue(TargetWeather, WeatherType.Fog);
        FogDensity = Mathf.Lerp(currentFog, targetFog, t);
    }
    
    private float GetWeatherValue(WeatherType weather, WeatherType typeToCheck)
    {
        // Returns how much of the specified weather type is present in the given weather
        switch (weather)
        {
            case WeatherType.Clear:
                return typeToCheck == WeatherType.Clear ? 1f : 0f;
            case WeatherType.Overcast:
                return typeToCheck == WeatherType.Overcast ? 1f : 
                       typeToCheck == WeatherType.Rain ? 0.3f : 0f;
            case WeatherType.Rain:
                return typeToCheck == WeatherType.Rain ? 1f : 
                       typeToCheck == WeatherType.Overcast ? 0.5f : 0f;
            case WeatherType.Fog:
                return typeToCheck == WeatherType.Fog ? 1f : 0f;
            case WeatherType.Storm:
                return typeToCheck == WeatherType.Storm ? 1f : 
                       typeToCheck == WeatherType.Rain ? 0.7f : 
                       typeToCheck == WeatherType.Overcast ? 0.3f : 0f;
            default:
                return 0f;
        }
    }
    
    private void UpdateWeatherEffects()
    {
        // Set effects based on current weather
        switch (CurrentWeather)
        {
            case WeatherType.Clear:
                RainIntensity = 0f;
                FogDensity = 0f;
                break;
            case WeatherType.Overcast:
                RainIntensity = 0.1f; // Light drizzle possible
                FogDensity = 0.05f;   // Slight haze
                break;
            case WeatherType.Rain:
                RainIntensity = 0.8f;
                FogDensity = 0.1f;    // Rain often reduces visibility
                break;
            case WeatherType.Fog:
                RainIntensity = 0.05f; // Light rain in fog
                FogDensity = 0.9f;     // Heavy fog
                break;
            case WeatherType.Storm:
                RainIntensity = 1.0f;  // Heavy rain
                FogDensity = 0.2f;     // Some fog from rain
                break;
        }
        
       <SPECIAL_359>Effects();
    }
    
    private void UpdateAmbientEffects()
    {
        // Modify gameplay variables based on weather
        switch (CurrentWeather)
        {
            case WeatherType.Clear:
                VisibilityModifier = 1.2f; // Better visibility
                SurfaceSlipperiness = 0.0f; // Dry roads
                AudioAmbiancce = 0.2f;     // Light ambient
                break;
            case WeatherType.Overcast:
                VisibilityModifier = 1.0f; // Normal visibility
                SurfaceSlipperiness = 0.1f; // Slightly damp
                AudioAmbiancce = 0.3f;     // Bit more ambient
                break;
            case WeatherType.Rain:
                VisibilityModifier = 0.7f; // Reduced visibility
                SurfaceSlipperiness = 0.3f; // Wet roads
                AudioAmbiancce = 0.6f;     // Rain sounds
                break;
            case WeatherType.Fog:
                VisibilityModifier = 0.4f; // Poor visibility
                SurfaceSlipperiness = 0.2f; // Damp but not flooded
                AudioAmbiancce = 0.4f;     // Muffled sounds
                break;
            case WeatherType.Storm:
                VisibilityModifier = 0.5f; // Poor visibility from heavy rain
                SurfaceSlipperiness = 0.5f; // Very wet, slippery
                AudioAmbiancce = 0.8f;     // Loud rain/thunder
                break;
        }
        
        // Update HUD or game systems with these modifiers
        var hudManager = Scene.GetActor("HUDManager")?.GetComponent<HUDManager>();
        if (hudManager != null)
        {
            // Could affect NPC vision range through story state or direct modification
            // For now, just log
            Debug.Log($"Weather effects: Visibility {VisibilityModifier:F2}, Slipperiness {SurfaceSlipperiness:F2}");
        }
    }
    
    private void UpdateParticleSystems()
    {
        // Update particle system emissions based on weather intensity
        if (RainParticles != null)
        {
            // RainParticles.EmissionRate = RainIntensity * 100f; // Example scaling
            // In Flax, we would set the emission rate or module properties
            Debug.Log($"Rain intensity: {RainIntensity:F2}");
        }
        
        if (FogParticles != null)
        {
            // FogParticles.EmissionRate = FogDensity * 50f; // Example scaling
            Debug.Log($"Fog density: {FogDensity:F2}");
        }
    }
    
    /// <summary>
    /// Forces a specific weather type (for testing or story events)
    /// </summary>
    public void ForceWeather(WeatherType weather)
    {
        CurrentWeather = weather;
        TargetWeather = weather;
        _isTransitioning = false;
        _timeUntilChange = Random.Range(ChangeIntervalMin, ChangeIntervalMax);
        UpdateWeatherEffects();
        Debug.Log($"Weather forced to: {CurrentWeather}");
    }
    
    /// <summary>
    /// Gets whether it's currently precipitating
    /// </summary>
    public bool IsPrecipitating => CurrentWeather == WeatherType.Rain || CurrentWeather == WeatherType.Storm;
    
    /// <summary>
    /// Gets whether visibility is significantly reduced
    /// </summary>
    public bool IsVisibilityReduced => VisibilityModifier < 0.8f;
}
