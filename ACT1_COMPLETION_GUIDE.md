# Act I: Awakening - Complete Implementation Guide

This guide shows you how to set up, test, and complete all 7 objectives of Act I: Awakening in Neon Veil.

## Overview
Act I: Awakening consists of 7 interconnected objectives that teach core mechanics and set up the story:
1. Discover the hidden unpersoning log during audit work
2. Escape Canopy HQ after being noticed
3. Reach the Underveil and make contact with the Choir
4. Make your first faction-reputation decision with the Choir
5. Hijack a vehicle to escape Midlayer patrol drones
6. Infiltrate the Midlayer relay station and plant hacking evidence
7. Decide the fate of the captured guard at the relay station

Each objective impacts StoryState (reputation, ending weights, story flags) and contributes to the eventual ending.

## Systems Implemented
- Mission System (Mission.cs, MissionObjective.cs, MissionManager.cs)
- StoryState System (StoryState.cs) - tracks faction reputation, ending weights, story flags
- HUD System (HUDManager.cs, PlayerStats.cs) - health, energy, heat bars
- AI System (DetectorComponent.cs, NPCController.cs + states)
- Player Controller (CipherController.cs) - movement, jump, crouch, sprint
- Hacking System (CircuitRerouteMiniGame.cs, IHackable.cs, HackableDoor.cs, HackableTerminal.cs)
- Weapons System (WeaponBase.cs + specific weapons)
- Vehicles System (VehicleController.cs + vehicle types + VehicleManager.cs)
- Environment System (DayNightCycle.cs, WeatherSystem.cs)

## Step-by-Step Setup and Testing

### 1. Create the Test Scene
1. In Flax Editor, create a new scene (File -> New Scene)
2. Save it as Act1_CompleteTest.flaxscene in Content/Levels/
3. Build a simple test environment representing the Act I progression:
   - Starting area (Canopy HQ audit office)
   - Escape route to city outskirts
   - Underveil district
   - Midlayer relay station
   - Use primitive shapes for now (boxes, cylinders) - replace with final assets later

### 2. Set Up Required Core Systems Actors
Create these actors in your scene (only need one of each):

#### Mission System
- Create Empty Actor named MissionManager
  * Add Component -> Scripts -> MissionManager

#### Story System  
- Create Empty Actor named StoryState
  * Add Component -> Scripts -> StoryState

#### HUD System
- Create Empty Actor named HUDManager
  * Add Component -> Scripts -> HUDManager

#### Player
- Create Empty Actor named Player
  * Add Component -> Physics -> CharacterController
    - Height: 2.0, Radius: 0.5, Step Offset: 0.5
  * Add Component -> Scripts -> CipherController
  * Add Component -> Scripts -> PlayerStats

### 3. Set Up Objective-Specific Actors

#### Objective 1: Unpersoning Log
- Create Empty Actor named UnpersoningLog
  * Add Component -> Scripts -> Source.Missions.UnpersoningLog
  * Position: In the starting audit office area
  * (Optional) Add a visual indicator like a small box or document model

#### Objective 2: Escape Canopy HQ Trigger
- Create Empty Actor named EscapeHQ_TestTrigger
  * Add Component -> Physics -> BoxCollider (set as IsTrigger)
    - Size: 5,3,5 positioned at your exit point from starting area
  * Add Component -> Scripts -> Source.Missions.TestTriggers.EscapeHQTrigger

#### Objective 3: Underveil Contact Trigger
- Create Empty Actor named UnderveilContact_TestTrigger
  * Add Component -> Physics -> BoxCollider (set as IsTrigger)
    - Size: 5,3,5 positioned in your Underveil area
  * Add Component -> Scripts -> Source.Missions.TestTriggers.UnderveilContactTrigger

#### Objective 4: Choir Decision Point
- Create Empty Actor named ChoirDecision_Point
  * Add Component -> Scripts -> Source.Missions.ChoirDecision
  * Position: In the Underveil area, after contacting the Choir
  * (Optional) Add a visual indicator for the Choir representative

#### Objective 5: Vehicle Hijacking
- Create Empty Actor named VehicleSpawner (for testing)
  * Add Component -> Scripts -> Source.Vehicles.SportsCar (or any vehicle type)
  * Position: Accessible but not too close to start
  * Add Component -> Scripts -> Source.Missions.VehicleHijackTracker
  * (Optional) Add multiple vehicle types for variety

#### Objective 6: Relay Station Terminal
- Create Empty Actor named RelayStation_Terminal
  * Add Component -> Scripts -> Source.Missions.RelayStationTerminal
  * Position: In your relay station area
  * (Optional) Add a visual indicator like a computer terminal model

#### Objective 7: Captured Guard Decision
- Create Empty Actor named CapturedGuard_Decision
  * Add Component -> Scripts -> Source.Missions.CapturedGuardDecision
  * Position: Near the relay station terminal (after hacking)
  * (Optional) Add a visual indicator like a restrained guard model

### 4. Connect the Mission
1. In the MissionManager actor, the Act1Awakening mission should automatically be detected
2. Or you can manually add it:
   - Select MissionManager actor
   - In Properties, click Add Component -> Scripts -> Act1Awakening

### 5. Testing the Full Mission Flow
1. Press Play (F5) to enter game mode
2. Open the Console (View -> Console) to see detailed logs

#### Objective 1: Discover Log
- Walk to UnpersoningLog actor
- Press Interact (E) when prompted
- Verify: Log discovery message, objective completion, StoryState updates

#### Objective 2: Escape Canopy HQ
- Walk to EscapeHQ_TestTrigger actor
- Enter the trigger zone
- Verify: Escape message, objective completion

#### Objective 3: Contact Choir
- Walk to UnderveilContact_TestTrigger actor
- Enter the trigger zone
- Verify: Contact message, +10 Choir reputation, objective completion

#### Objective 4: Choir Decision
- Walk to ChoirDecision_Point actor
- Press Interact (E) when prompted
- Choose 1, 2, or 3 via keyboard
- Verify: Decision message, reputation/weight changes, objective completion

#### Objective 5: Vehicle Hijacking
- Walk to VehicleSpawner actor
- Get close to vehicle and press Hijack key (default: H)
- Wait for hijack to complete (visual/audio cues in full game)
- Wait ~5 seconds for escape timer
- Verify: Hijack message, escape timer countdown, objective completion

#### Objective 6: Relay Station Hack
- Walk to RelayStation_Terminal actor
- Press Interact (E) when prompted
- Complete the circuit reroute mini-game (or wait for auto-success in testing)
- Verify: Hack success message, evidence planting effects, objective completion

#### Objective 7: Captured Guard Decision
- Walk to CapturedGuard_Decision actor
- Press Interact (E) when prompted
- Choose 1, 2, or 3 via keyboard
- Verify: Decision message, outcome summary, reputation/weight changes, objective completion
- Check console for final ending tendencies

### 6. Verify Mission Completion
After completing all 7 objectives, check the console for:
- Each objective completion message
- StoryState updates after each major decision
- Final ACT I COMPLETED: AWAKENING message with banner
- Final Story State showing accumulated reputation and ending weights
- Likely ending path indication based on your choices

### 7. Building and Running
1. Stop game mode (Shift+F5 or click stop button)
2. Build the project (Ctrl+F5 or Build -> Build Project)
3. If build succeeds, run the executable to test standalone

## Choice Impact on Endings

Your decisions throughout Act I influence which ending you're more likely to get:

### Liberation Ending (Burn down the Mark system)
- Increased by: Supporting Choir liberation, executing guard, successful relay station hack
- Decreased by: Showing mercy to enforcers, reform choices

### Reform Ending (Reform the Mark system)
- Increased by: Supporting Choir reform, subduing guard, releasing guard compassionately
- Decreased by: Violent actions, authoritarian choices

### Ascension Ending (Take control of the Mark system)
- Increased by: Neutral/refusing Choir, executing guard (shows power), failed hack (recklessness)
- Decreased by: Mercy, liberal choices, reform choices

## Testing Tips
- Use console logs (View -> Console) to verify all system interactions
- If you don't see expected messages, check:
  * That all required actors and components are present
  * That actor names match exactly (case-sensitive in some checks)
  * That components are properly assigned to correct actors
  * That the MissionManager is finding and activating the Act1Awakening mission
- For vehicle hijacking, ensure VehicleManager is present and working
- For hacking, ensure CircuitRerouteMiniGame is present on HackingManager actor

## Next Steps After Act I Completion
Once Act I is working correctly:
1. Replace primitive shapes with final assets using the pipeline in ASSET_README.md
2. Add audio and visual feedback for discoveries and mission milestones
3. Implement saving/loading system to persist progress between sessions
4. Begin implementing Act II: Revelation mission content
5. Enhance AI behaviors for different NPC types (maintenance drones, civilians, etc.)
6. Add more sophisticated mission triggers and narrative cutscenes
7. Polish mission flow with proper level design and pacing
8. Work on art pipeline using ASSET_README.md guidelines

## Troubleshooting Common Issues

### Missing Console Messages
- Verify that scripts have OnStart() and OnUpdate() methods
- Check that actors are not disabled or hidden
- Ensure Flax Editor is in play mode (not edit mode)

### Components Not Found
- Double-check that you added the correct component to the correct actor
- Verify namespace paths (e.g., Source.Missions.UnpersoningLog)
- Try removing and re-adding the component

### Mission Not Progressing
- Ensure MissionManager actor is present in the scene
- Verify that Act1Awakening component is on MissionManager or a separate actor
- Check that objectives are properly initialized in InitializeObjectives

### Vehicle Hijacking Not Working
- Verify VehicleManager actor is present
- Check that vehicle types (SportsCar, etc.) are properly configured
- Ensure physics components are set up on vehicle actors

### Hacking Not Triggering
- Verify that HackingManager actor exists with CircuitRerouteMiniGame
- Check that the terminal is properly set as hackable
- Ensure player is within interaction range when pressing Interact

Remember: This is just the beginning of your journey! The actual game will have proper level design, narrative cutscenes, polished gameplay systems, and final assets replacing these test implementations. But the core systems and mission flow are now functional and ready for expansion.
