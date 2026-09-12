// IHackable.cs
// Interface for objects that can be hacked
using FlaxEngine;

public interface IHackable
{
    /// <summary>
    /// Starts the hacking mini-game for this object
    /// </summary>
    void StartHack();
    
    /// <summary>
    /// Called when hacking is successful
    /// </summary>
    void OnHackSuccess();
    
    /// <summary>
    /// Called when hacking fails
    /// </summary>
    void OnHackFailure();
    
    /// <summary>
    /// Gets whether this object is currently being hacked
    /// </summary>
    bool IsBeingHacked { get; }
}
