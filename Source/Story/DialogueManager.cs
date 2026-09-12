// DialogueManager.cs
// Manages active conversations and dialogue UI
using FlaxEngine;
using System.Collections.Generic;

public class DialogueManager : Script
{
    // Singleton pattern
    public static DialogueManager Instance { get; private set; }
    
    private DialogueTree _currentTree;
    private DialogueNode _currentNode;
    private bool _isDialogueActive;
    
    // UI references (would be set in editor or found at runtime)
    private UIControl _dialoguePanel;
    private UIText _speakerText;
    private UIText _dialogueText;
    private UIControl _choicesPanel;
    
    public override void OnStart()
    {
        // Set up singleton
        if (Instance != null && Instance != this)
        {
            Destroy(Actor);
            return;
        }
        Instance = this;
        
        // Find UI elements (in a real game, these would be set up in the editor)
        FindDialogueUI();
        
        // Start with dialogue inactive
        HideDialogue();
    }
    
    private void FindDialogueUI()
    {
        // In a real implementation, we would find or instantiate UI controls
        // For now, we'll just log that we're ready
        Debug.Log("DialogueManager ready - UI would be set up in editor");
    }
    
    /// <summary>
    /// Starts a dialogue with the specified tree
    /// </summary>
    public void StartDialogue(string dialogueTreeID)
    {
        // In a full implementation, we would load the dialogue tree from assets
        // For now, we'll just log
        Debug.Log($"Starting dialogue: {dialogueTreeID}");
        _isDialogueActive = true;
        // ShowDialogue() would be called here
    }
    
    /// <summary>
    /// Ends the current dialogue
    /// </summary>
    public void EndDialogue()
    {
        _isDialogueActive = false;
        _currentTree = null;
        _currentNode = null;
        HideDialogue();
        Debug.Log("Dialogue ended");
    }
    
    /// <summary>
    /// Makes a dialogue choice
    /// </summary>
    public void MakeChoice(int choiceIndex)
    {
        if (!_isDialogueActive || _currentNode == null || choiceIndex < 0 || choiceIndex >= _currentNode.Choices.Count)
        {
            Debug.LogWarning("Invalid dialogue choice");
            return;
        }
        
        var choice = _currentNode.Choices[choiceIndex];
        
        // Execute choice actions
        foreach (var action in choice.Actions)
        {
            action.Execute(StoryState.Instance, null); // MissionManager would be passed in full implementation
        }
        
        // Move to target node
        if (!string.IsNullOrEmpty(choice.TargetNodeID))
        {
            _currentNode = _currentTree.GetNode(choice.TargetNodeID);
            if (_currentNode != null)
            {
                // Execute node actions
                foreach (var action in _currentNode.Actions)
                {
                    action.Execute(StoryState.Instance, null);
                }
                
                UpdateDialogueUI();
                
                // Check if conversation ends
                if (_currentNode.EndsConversation)
                {
                    EndDialogue();
                }
            }
            else
            {
                Debug.LogError($"Dialogue node not found: {choice.TargetNodeID}");
                EndDialogue();
            }
        }
        else
        {
            EndDialogue();
        }
    }
    
    private void UpdateDialogueUI()
    {
        if (!_isDialogueActive) return;
        
        // Update speaker text
        if (_speakerText != null && !string.IsNullOrEmpty(_currentNode.Speaker))
        {
            _speakerText.Text = _currentNode.Speaker;
        }
        
        // Update dialogue text
        if (_dialogueText != null && !string.IsNullOrEmpty(_currentNode.Text))
        {
            _dialogueText.Text = _currentNode.Text;
        }
        
        // Update choices UI
        // In a full implementation, we would instantiate choice buttons here
        if (_choicesPanel != null)
        {
            // Clear existing choices
            // Instantiate buttons for each choice in _currentNode.Choices
            // Set button text to choice.Text
            // Set button click handlers to MakeChoice(index)
            Debug.Log($"Updating choices UI with {_currentNode.Choices.Count} choices");
        }
    }
    
    private void ShowDialogue()
    {
        if (_dialoguePanel != null)
        {
            _dialoguePanel.IsVisible = true;
        }
        _isDialogueActive = true;
        Debug.Log("Dialogue shown");
    }
    
    private void HideDialogue()
    {
        if (_dialoguePanel != null)
        {
            _dialoguePanel.IsVisible = false;
        }
        _isDialogueActive = false;
        Debug.Log("Dialogue hidden");
    }
    
    public bool IsDialogueActive => _isDialogueActive;
}
