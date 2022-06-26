# Features
## Controls
- when holding a PlaceableObject close to a SnapPoint there should be a holographic preview of the snapped position
- "AR Passthrough" for VR Headsets to simulate the features of the AR Headset

## Framework
- basic structure to quickly implement different games into the app (Mainly structural functions like snapping game pieces to predefined places)
	- PlaceableObject: an extension to the Unity GameObject to handly everything MR related and the snapping logic
	- GameBoard: a base for all the snapping points and rendering of a gameboard
	- SnapPoints: points you can place a game piece to disable wonky physics-related problems


## GameImporter:
Adjust Snapoint size automatically according to minimum distance to similar points.
