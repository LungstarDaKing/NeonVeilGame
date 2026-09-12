# Act I: Awakening - Setup and Testing Guide

This guide shows you how to set up and test the first mission of Neon Veil in the Flax Editor.

## Overview
Act I: Awakening consists of 7 objectives:
1. Discover the hidden unpersoning log during audit work
2. Escape Canopy HQ after being noticed
3. Reach the Underveil and make contact with the Choir
4. Make your first faction-reputation decision with the Choir
5. Hijack a vehicle to escape Midlayer patrol drones
6. Infiltrate the Midlayer relay station and plant hacking evidence
7. Decide the fate of the captured guard at the relay station

## Step 1: Create the Test Scene
1. In Flax Editor, create a new scene (File -> New Scene)
2. Save it as Act1_TestScene.flaxscene in Content/Levels/
3. Set up a simple test environment that represents the opening area

## Step 2: Set Up Required Actors
Create these actors in your scene:

### 1. Mission System
- Create Empty Actor named MissionManager
  * Add Component -> Scripts -> MissionManager

### 2. Story System  
- Create Empty Actor named StoryState
  * Add Component -> Scripts -> StoryState

### 3. HUD System
- Create Empty Actor named HUDManager
  * Add Component -> Scripts -> HUDManager

### 4. Player
- Create Empty Actor named Player
  * Add Component -> Physics -> CharacterController
    - Height: 2.0, Radius: 0.5, Step Offset: 0.5
  * Add Component -> Scripts -> CipherController
  * Add Component -> Scripts -> PlayerStats

### 5. NPC (for testing detection)
- Create Empty Actor named Guard_NPC
  * Add Component -> Physics -> CharacterController
    - Height: 1.8, Radius: 0.4, Step Offset: 0.5
  * Add Component -> Scripts -> DetectorComponent
  * Add Component -> Scripts -> NPCController
  * Add Component -> Scripts -> Source.AI.NPCStates.PatrolState

### 6. Objective 1: Unpersoning Log
- Create Empty Actor named UnpersoningLog
  * Add Component -> Scripts -> Source.Missions.UnpersoningLog
  * (Optional) Add a visual indicator like a small box or document model

### 7. Test Triggers (for Objectives 2 & 3)
- Create Empty Actor named EscapeHQ_TestTrigger
  * Add Component -> Physics -> BoxCollider (set as IsTrigger)
    - Size: 3,1,3 positioned at your exit point
  * Add Component -> Scripts -> Source.Missions.TestTriggers.EscapeHQTrigger
  
- Create Empty Actor named UnderveilContact_TestTrigger
  * Add Component -> Physics -> BoxCollider (set as IsTrigger)
    - Size: 3,1,3 positioned in your Underveil area
  * Add Component -> Scripts -> Source.Missions.TestTriggers.UnderveilContactTrigger

## Step 3: Connect the Mission
1. In the MissionManager actor, the Act1Awakening mission should automatically be detected
2. Or you can manually add it:
   - Select MissionManager actor
   - In Properties, click Add Component -> Scripts -> Act1Awakening

## Step 4: Testing the Mission
1. Press Play (F5) to enter game mode
2. Open the Console (View -> Console) to see logs

### Testing Objective 1: Discover Log
- Walk your player to the UnpersoningLog actor
- Press the Interact key (default: E) when near it
- You should see:
  - "�������������������������������� Unpersoning log discovered!" in console
  - "���������������������������������������������������������������� Objective completed: Discover the unpersoning log"
  - Story updates: "Story updated: truth discovered, Choir reputation +5"

### Testing Objective 2: Escape HQ
- Walk your player to the EscapeHQ_TestTrigger actor
- When the player enters the trigger:
  - You should see: "�������������������������������������������������������������������������������������������� Player escaped Canopy HQ!"
  - And: "���������������������������������������������������������������� Objective completed: Escape Canopy HQ"

### Testing Objective 3: Contact Choir
- Walk your player to the UnderveilContact_TestTrigger actor
- When the player enters the trigger:
  - You should see: "�������������������������������������������������������������������������������������������� Player contacted the Choir in Underveil! Reputation +10"
  - And: "���������������������������������������������������������������� Objective completed: Reach Underveil and contact Choir"

## Step 5: Verify Mission Completion
After completing the first three objectives, check the console for:
- Mission progress updates
- Story state changes (reputation, ending weights)
- When all objectives are complete, you should see the ACT I COMPLETED message

## Step 6: Building and Running
1. Stop game mode (Shift+F5)
2. Build the project (Ctrl+F5 or Build -> Build Project)
3. If successful, run the executable to test standalone

## Next Steps
Once Act I objectives 1-3 are working:
1. Implement Objective 4: Create a dialogue system interaction for the Choir decision
2. Implement Objective 5: Add vehicle hijacking mechanics and test with VehicleManager
3. Implement Objective 6: Create the relay station infiltration area with hackable objects
4. Implement Objective 7: Add morality choice system for the captured guard
5. Replace test triggers with proper level design and narrative triggers

## Troubleshooting
- If you don't see console messages, make sure all scripts have OnStart() methods
- Check that actors are properly named (case-sensitive in some checks)
- Verify that components are correctly attached to the right actors
- Ensure the MissionManager is finding and activating the Act1Awakening mission
- If triggers don't work, check that colliders are set as IsTrigger and have proper size/position

Remember: This is just the beginning! The actual game will have proper level design, narrative cutscenes, and polished gameplay systems replacing these test implementations.
