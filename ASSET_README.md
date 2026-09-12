# Asset Pipeline

## Characters
- Use Reallusion Character Creator/iClone to build/customize base humanoid models.
- Export as FBX with standard humanoid bone naming.

## Animation
- Source animation clips from Mixamo.
- Retarget onto Reallusion-rigged skeletons.

## Weapons, Props & Robots
- Generate custom-styled 3D meshes using Meshy AI.
- Export as FBX/GLB.
- Import into Flax Editor's Content/Props_Weapons/ (or Content/Robots/).

## Import into Flax
1. Drag-and-drop FBX/GLB assets into Flax Editor's asset browser.
2. Flax auto-generates .flax asset wrappers.
3. Manually set up materials, animations, and prefabs in the Editor.

## Polygon Budgets
- Hero characters: <15k tris
- Background NPCs: <6k tris
- Weapons/props: <5k tris
- Vehicles: <12k tris

## Texture Sizes
- Hero characters/weapons: up to 2048x2048 with normal/roughness/metallic maps
- Background props: 512-1024
- Use texture atlasing for repeated small props.
