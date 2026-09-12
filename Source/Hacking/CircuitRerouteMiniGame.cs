// CircuitRerouteMiniGame.cs
// Simple circuit rerouting hacking minigame
using FlaxEngine;
using System.Collections.Generic;

public class CircuitRerouteMiniGame : Script
{
    // Puzzle settings
    public int GridWidth = 5;
    public int GridHeight = 5;
    public float TileSize = 1.0f;
    public float TimeLimit = 15.0f; // seconds
    
    // References to puzzle tiles (set in editor or generated)
    private List<CircuitTile> _tiles = new List<CircuitTile>();
    private float _remainingTime;
    private bool _isActive;
    private bool _isSolved;
    
    public override void OnStart()
    {
        GeneratePuzzle();
        StartMinigame();
    }
    
    public override void OnUpdate()
    {
        if (!_isActive || _isSolved) return;
        
        _remainingTime -= Time.DeltaTime;
        if (_remainingTime <= 0.0f)
        {
            FailMinigame();
        }
        
        // Check for input to rotate tiles (placeholder)
        if (Input.GetButtonDown("Interact"))
        {
            // In a full implementation, detect which tile is under cursor and rotate it
            RotateRandomTile();
            CheckSolution();
        }
    }
    
    private void GeneratePuzzle()
    {
        // Generate a random solvable circuit puzzle (simplified)
        _tiles.Clear();
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                var tile = new CircuitTile();
                tile.Position = new Vector3(x * TileSize, 0, y * TileSize);
                tile.Type = (CircuitTileType)Random.Range(0, 4); // 0-3: straight, corner, etc.
                _tiles.Add(tile);
                // In a full implementation, spawn a visual tile actor here
            }
        }
        
        // Ensure puzzle is solvable (placeholder logic)
        // In a full implementation, use a known solvable pattern
        Debug.Log("Circuit puzzle generated");
    }
    
    private void StartMinigame()
    {
        _remainingTime = TimeLimit;
        _isActive = true;
        _isSolved = false;
        Debug.Log("Circuit reroute minigame started");
    }
    
    private void FailMinigame()
    {
        _isActive = false;
        Debug.Log("Circuit reroute minigame failed");
        // Trigger failure: increase Heat, alarm, etc.
    }
    
    private void CheckSolution()
    {
        // Placeholder: check if all tiles form a complete path from start to end
        // For simplicity, assume solved after a few rotations
        if (Random.Range(0, 10) > 7) // 30% chance to solve on any rotation
        {
            _isSolved = true;
            _isActive = false;
            Debug.Log("Circuit reroute minigame solved!");
            // Trigger success: unlock door, disable camera, etc.
        }
    }
    
    private void RotateRandomTile()
    {
        if (_tiles.Count == 0) return;
        int index = Random.Range(0, _tiles.Count);
        _tiles[index].Rotation = (_tiles[index].Rotation + 90) % 360;
    }
}

// Supporting enum and struct for the minigame
public enum CircuitTileType
{
    Straight,
    Corner,
    T_Junction,
    Cross
}

public class CircuitTile
{
    public Vector3 Position;
    public float Rotation; // degrees
    public CircuitTileType Type;
}
