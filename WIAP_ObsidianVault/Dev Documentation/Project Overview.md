This is a short overview of all scripts present in the project.
The headlines are the corresponding folders inside
`MR_Unity/Assets/_Scripts`  

# GameScripts
This folder generally contains all scripts belonging to the game as a whole. E.g. everything directly corellated to creating/importing the gameboard.

### GameData.cs
The data class for the gameimport json files.
- Holds all information necessary for a successfull import
- Holds converter functions for `GameData->Byte[]` and `Byte[]->GameData` (used in GameImporter.cs)

### GameImporter.cs
Handles everything necessary for a succesfull import from a game.json file (including spreading the information to other clients)
- Triggers to spawn Go and Chess respectively
- Import information from json file `json->GameData.cs`
- Spread imported information via PUN and DataTransfer.cs
- Receive the spread information coming from different client
- Build gameboard with the texture and cube body from json
- Spawn the snappoints and parent them correctly
- Save all gamepieces to data structures for later spawning
- Build menu next to gameboard

### PlaceableObject.cs
Marks an object to interact with snappoints and holds the necessary functions.
- Handles collisions with snappoints
- Holds (and updates) information of grabbing state
- (currently disabled) snap to defined point of snappoint on release

### SnapPoint.cs
Holds the methods to interact with PlaceableObjects and everything pertaining to the snappoints themselves.
- (currently disabled) Handle the holographic preview with the corresponding meshes
- toggle visibility of all snappoints when triggered

# GUI
Like the name suggests it contains all scripts belonging to the GUI logic.

### ConsoleToGUI.cs
The code is slightly modified, but taken from unityanswers. It syncs all Unity console output to a Textbox in the app for ingame debugging.
- Receive Unity Console data and set it to a defined TextMeshPro

### StartPlate.cs
"Welcome screen". Starts one of the two defined games when certain conditions are met.
- Checks for platform and disables buttons and updates text on hololens (hololens can't spawn the board)
- Monitors state until other player joins. Currently waiting only for player 2 and then enables spawning.

# PlayerScripts
All scripts belonging to the playerinteractions are found here.
### GamePieceLoader.cs
Local build of gamepieces to spawn them correctly.
- On PUN RPC call loads the locally stored information into the gameobject and builds the mesh.
- With a delayed call resizes the mesh to fit inside a defined size (without this they spawn way too big | without the delay it is not calculated correctly) (the delay could probably be decreased and calculated in the next frame)

### HeadSync.cs
Handles tracking of the player heads. (including the local one)
- Initialization of tracking with the correct information as soon as its available
- Syncs helper object to local camera (position of VR/AR headset)
- Calculates positions of all other players according to helper object and board object positions.

### PieceSpawnController.cs
Builds and handles the spawnbuttons spawned next to the board object.
- Build and populate spawnbuttons in a instantiated prefab
- Add click actions to each button

### PlayerManager.cs
General player management class. Currently only handles tracker objects.
- spawn Boardhelper and Headhelper for local player respectively (happens on the PhotonNetwork)

# PUN
All networking related scripts are found here.

### DataTransfer.cs
Custom class with PUN RPC data transfer. Splits bigger data into chunks automatically and offers a trigger for received data. (only byte arrays accepted)
- Split big data into chunks and assign IDs to them
- Send the data chunks via PUN RPC
- Receives and reconstructs the data based on send IDs and package tags

### FileReader.cs
Helper class to the `ObjectLoader.cs`. Is taken from a github gist.
- Takes a string[] and converts it to an "ObjectFile" object

### ImporterHelper.cs
Contains various helper functions mainly used in `GameImporter.cs`.
- Converts a given string into a `Color` object.
- Builds a deduplicated (removed duplicates) List of gamePieces from a given gameData
- (currently unused) Scales a mesh in a gameobject to a new size and rebuilds the boxCollider
- (currently unused) Converterfunctions to convert `GameData->Byte[]` and back. but it didn't work as expected 

### ObjectLoader.cs
Modified script taken from a github gist that takes meshdata and assigns it on runtime to a the attached gameobject. Uses the `FileReader.cs` class for file conversion.
- Takes string[] and initiates an import
- Builds and populates the local meshfilter with a new mesh.

### PUN_Controller.cs
General handling of PUN server and room connection. Ownershiphandling is also done here.
- Automatically tries to connect to master server and joins (or creates) a room titled "Room".
- On join trigger several functions to track the heads of all other players and spawn own trackers.
- Transfers ownership of gamepiece when requested, but only if it is not currently grabbed.
- Disables the ObjectManipulator when transfering ownership of gamepiece to another player.

### SyncPos.cs
A modular class syncing the helperobjects of heads and boards.
- Spreads the given name to other instances via RPC.
- Spawns head of other Players

# \_zzArchive
This folder is basically an archive for old and "deleted" scripts.
All these scripts can be considered removed, but still browsed for inspiration/help

# \_zzPrefabsOld
This folder holds all prefabs that were used either in prototyping or in tutorials for testing. These prefabs also are still used in zzArchive projects/scripts.