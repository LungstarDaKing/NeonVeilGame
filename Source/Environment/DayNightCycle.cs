// DayNightCycle.cs
// Manages the day/night cycle and lighting
using FlaxEngine;

public class DayNightCycle : Script
{
    // Cycle properties
    public float DayLength = 1200f; // Seconds for a full day (20 minutes = 1200s)
    public float SunriseTime = 0.25f; // Normalized time of day for sunrise (0-1)
    public float SunsetTime = 0.75f;  // Normalized time of day for sunset
    
    // Lighting references
    public DirectionalLight SunLight;
    public LightGroup AmbientLight; // Or Skybox/Lightmap for ambient
    
    // Colors and intensities
    public Color DayAmbientColor = new Color(0.6f, 0.7f, 0.8f, 1.0f);
    public Color NightAmbientColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Color DaySunColor = new Color(1.0f, 0.95f, 0.8f);
    public Color NightSunColor = new Color(0.2f, 0.2f, 0.3f); // Moonlight
    
    public float DaySunIntensity = 1.5f;
    public float NightSunIntensity = 0.1f;
    
    // Current time
    private float _timeOfDay; // 0-1 representing time of day (0 = midnight, 0.5 = noon)
    private float _elapsedTime;
    
    // Events
    public event Action OnSunrise;
    public event Action OnSunset;
    public event Action<float> OnTimeOfDayChanged; // passes normalized time of day
    
    public override void OnStart()
    {
        // Start at random time of day
        _timeOfDay = Random.Range(0f, 1f);
        _elapsedTime = 0f;
        
        UpdateLighting();
        
        Debug.Log("DayNightCycle initialized");
    }
    
    public override void OnUpdate()
    {
        // Advance time
        _elapsedTime += Time.DeltaTime;
        _timeOfDay = (_elapsedTime / DayLength) % 1f;
        
        // Check for sunrise/sunset events
        CheckDayNightEvents();
        
        // Update lighting based on time of day
        UpdateLighting();
        
        // Notify listeners
        OnTimeOfDayChanged?.Invoke(_timeOfDay);
    }
    
    private void CheckDayNightEvents()
    {
        // Check if we just passed sunrise
        float previousTime = (_elapsedTime - Time.DeltaTime) / DayLength;
        previousTime = previousTime % 1f;
        if (previousTime < SunriseTime && _timeOfDay >= SunriseTime)
        {
            OnSunrise?.Invoke();
            Debug.Log("Sunrise!");
        }
        
        // Check if we just passed sunset
        if (previousTime < SunsetTime && _timeOfDay >= SunsetTime)
        {
            OnSunset?.Invoke();
            Debug.Log("Sunset!");
        }
    }
    
    private void UpdateLighting()
    {
        if (SunLight == null) return;
        
        // Calculate sun position based on time of day
        // Simple model: sun arcs across sky
        float sunAngle = (_timeOfDay * 360f) - 90f; // 0 at sunrise, 180 at sunset, 360 at next sunrise
        // Clamp to reasonable range for visual day/night
        float elevation = Mathf.Clamp01(Mathf.Sin(_timeOfDay * 3.14159f * 2f)) * 90f; // 0 at midnight, 90 at noon
        
        // Set sun direction
        Vector3 sunDirection = new Vector3(
            Mathf.Cos(sunAngle * 3.14159f / 180f) * Mathf.Cos(elevation * 3.14159f / 180f),
            Mathf.Sin(elevation * 3.14159f / 180f),
            Mathf.Sin(sunAngle * 3.14159f / 180f) * Mathf.Cos(elevation * 3.14159f / 180f)
        );
        SunLight.SetLocalRotation(Quaternion.FromToRotation(Vector3.Down, sunDirection));
        
        // Interpolate sun color and intensity
        float t = Mathf.Clamp01((_timeOfDay - SunriseTime) / (SunsetTime - SunriseTime)); // 0 at sunrise, 1 at sunset
        t = Mathf.Clamp01(t * 2f); // Make it peak at noon
        
        // Actually, let's do a proper day/night cycle
        float dayFactor = Mathf.Clamp01(Mathf.Sin(_timeOfDay * 3.14159f)); // 0 at night, 1 at day
        
        SunLight.Color = Color.Lerp(NightSunColor, DaySunColor, dayFactor);
        SunLight.Intensity = Mathf.Lerp(NightSunIntensity, DaySunIntensity, dayFactor);
        
        // Update ambient light
        if (AmbientLight != null)
        {
            AmbientLight.Color = Color.Lerp(NightAmbientColor, DayAmbientColor, dayFactor);
        }
        
        // Update skybox or fog color based on time of day
        // Would be implemented through rendering settings or skybox material
        Debug.Log($"Time of day: {_timeOfDay:F2}, Day factor: {dayFactor:F2}");
    }
    
    /// <summary>
    /// Gets the current time of day as a normalized value (0-1)
    /// </summary>
    public float GetTimeOfDay => _timeOfDay;
    
    /// <summary>
    /// Gets whether it's currently day time
    /// </summary>
    public bool IsDay => _timeOfDay > SunriseTime && _timeOfDay < SunsetTime;
    
    /// <summary>
    /// Gets whether it's currently night time
    /// </summary>
    public bool IsNight => !IsDay;
    
    /// <summary>
    /// Gets a formatted time string (HH:MM)
    /// </summary>
    public string GetTimeString()
    {
        float totalHours = _timeOfDay * 24f;
        int hours = Mathf.FloorToInt(totalHours);
        int minutes = Mathf.FloorToInt((totalHours - hours) * 60f);
        return $"{hours:D2}:{minutes:D2}";
    }
}
