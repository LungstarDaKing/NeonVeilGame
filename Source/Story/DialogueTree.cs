// DialogueTree.cs
// Complete dialogue tree containing nodes and management
using FlaxEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class DialogueTree
{
    public string ID;
    public string Title; // Display title for the conversation
    public string DefaultStartNodeID; // ID of the node to start with
    
    public List<DialogueNode> Nodes = new List<DialogueNode>();
    
    public DialogueTree()
    {
        ID = System.Guid.NewGuid().ToString();
    }
    
    /// <summary>
    /// Gets a node by its ID
    /// </>
    public DialogueNode GetNode(string nodeID)
    {
        return Nodes.FirstOrDefault(n => n.ID == nodeID);
    }
    
    /// <summary>
    /// Gets the entry point node
    /// </summary>
    public DialogueNode GetEntryPoint()
    {
        // First try to find node marked as entry point
        var entryNode = Nodes.FirstOrDefault(n => n.IsEntryPoint);
        if (entryNode != null) return entryNode;
        
        // Then try the default start node
        if (!string.IsNullOrEmpty(DefaultStartNodeID))
        {
            var defaultNode = GetNode(DefaultStartNodeID);
            if (defaultNode != null) return defaultNode;
        }
        
        // Finally, just return the first node if we have any
        return Nodes.FirstOrDefault();
    }
    
    /// <summary>
    /// Validates the dialogue tree for common issues
    /// </summary>
    public List<string> Validate()
    {
        var errors = new List<string>();
        
        if (Nodes.Count == 0)
        {
            errors.Add("Dialogue tree has no nodes");
            return errors;
        }
        
        // Check for duplicate IDs
        var ids = new HashSet<string>();
        foreach (var node in Nodes)
        {
            if (!string.IsNullOrEmpty(node.ID))
            {
                if (ids.Contains(node.ID))
                {
                    errors.Add($"Duplicate node ID: {node.ID}");
                }
                else
                {
                    ids.Add(node.ID);
                }
            }
            else
            {
                errors.Add("Found node with empty ID");
            }
        }
        
        // Check that all choice targets exist
        foreach (var node in Nodes)
        {
            foreach (var choice in node.Choices)
            {
                if (!string.IsNullOrEmpty(choice.TargetNodeID))
                {
                    if (GetNode(choice.TargetNodeID) == null)
                    {
                        errors.Add($"Node {node.ID} choice targets non-existent node: {choice.TargetNodeID}");
                    }
                }
            }
        }
        
        return errors;
    }
}
