# Neon Veil

A sci-fi stealth-hacking open-world adventure game built with Flax Engine.

## Setup

1. Install Flax Launcher and Editor (GUI installer).
2. Install .NET SDK 8.0+.
3. Open the project in Flax Editor: double-click NeonVeil.flaxproj.
4. Follow the asset pipeline instructions in ASSET_README.md for importing custom assets.

## First Deliverable: Basic Player Movement, AI Detection, and Hacking Minigame

### Step 1: Create a Test Scene
1. In Flax Editor, create a new scene (File -> New Scene).
2. Save it as TestScene.flaxscene in the Content/Levels/ folder.

### Step 2: Set Up the Player Controller
1. In the scene, create an empty actor (right-click in Scene Outliner -> Create Empty).
2. Rename it to Player.
3. Add a CharacterController component to the Player actor (in the Properties window, click Add Component -> Physics -> CharacterController).
4. Set the CharacterController's properties:
   - Height: 2.0
   - Radius: 0.5
   - Step Offset: 0.5
5. Add the CipherController script to the Player actor (Add Component -> Scripts -> CipherController).
6. In the CipherController script properties, adjust WalkSpeed, RunSpeed, etc. as desired.

### Step 3: Create a Simple Test Level
1. Add a few Box actors (Create -> Primitive -> Box) to serve as obstacles and floor.
2. Scale one box to act as the floor (e.g., Scale X=20, Y=1, Z=20) and position it at (0,0,0).
3. Place other boxes as walls or obstacles to test movement and vaulting.

### Step 4: Test Movement
1. Press Play (F5) to enter game mode.
2. Use WASD to move, Left Shift to run, Space to jump, Left Ctrl to crouch.
3. Approach an obstacle and press the Vault key (default: V) to attempt a vault over low obstacles.

### Step 5: Set Up AI Detection
1. Create an empty actor for the NPC and name it Guard.
2. Add a CharacterController component to the Guard (same settings as player or adjust).
3. Add the DetectorComponent script to the Guard actor.
4. Create a simple target actor (e.g., a sphere) and place it within the guard's view range and angle.
5. In the DetectorComponent properties, adjust ViewAngle and ViewRange as needed.
6. To test detection, you can add a debug script to the Guard that logs when the player is detected.

### Step 6: Set Up the Hacking Minigame
1. Create an empty actor named HackingManager (or attach to an existing UI actor).
2. Add the CircuitRerouteMiniGame script to the HackingManager actor.
3. The minigame will start automatically on scene start (check the console for logs).
4. In a full implementation, you would trigger the minigame via interacting with a hackable object (e.g., a door or terminal).
5. For testing, you can modify the CircuitRerouteMiniGame script to start on a key press (e.g., press H to start).

### Notes
- The scripts are intentionally basic and meant as a starting point.
- Replace placeholder input axes (MoveX, MoveY, Run, Jump, Crouch, Vault, Interact) with your actual input mappings in the Flax Input Settings.
- The vault logic is simplified; in a full implementation, you would integrate with animation root motion.
- The detector component does not yet include line-of-sight checks; add raycast checks for obstacles as needed.
- The circuit reroute minigame uses a random solve chance for demonstration; replace with actual puzzle logic.

## Next Steps After First Deliverable
- Implement animation integration for player movement and parkour.
- Develop AI state machine (Patrol, Suspicious, Alert, etc.).
- Create hackable objects (doors, terminals) that trigger the minigame.
- Build the first mission (Act I: Awakening) using the Mission class structure.
- Import custom assets using the pipeline described in ASSET_README.md.

## Troubleshooting
- If scripts fail to compile, ensure you have .NET SDK 8.0+ installed and that the Flax Editor is configured to use it.
- If the CharacterController doesn't move, verify that the actor is not static and that gravity is applied correctly.
- For detection issues, check that the DetectionLayers include the player's layer.

Enjoy building Neon Veil!
