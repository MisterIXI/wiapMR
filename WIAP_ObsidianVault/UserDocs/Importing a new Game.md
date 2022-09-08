# Things needed for import
## Game info JSON
Explanation of all possible datapoints, explained on the chess json (shortenend):
```json
{
	// version - currently not checked but should be 1
    "Version": 1, 
    // game name - mostly relevant for internal naming and later browsing menus
    "Name": "Chess",
    // a size around 200x200 is recommended with the current settings
    // y size of the gameboard
    "Height": 200,
    // x size of the gameboard
    "Width": 200,
    // path to the board texture
    "Texture": "Chessboard.png",
    // can be left empty
    // all gamepieces provided as an array and to be made spawnable
    "GamePieces": [
	    // defines a single gamePiece
        {
	        // gamepiece name for spawnbutton
            "Name": "Bishop Black",
            // path to triangulated .obj file
            "Path": "Bishop.obj",
            // unity coded color (r,g,b,a)
            "Color": "100,100,100,1",
            // optional
            // unity metallic value of standard shader (note that this is a string)
            "Metallic": "1",
            // optional
            // unity smoothness value of standard shader (note that this is a string)
            "Smoothness": "0.5"
        },
        [...]
    ],
    // can be left empty
    "SnapPoints": [
		// defines one snappoint at (30,50,10) -> (30,10) with height of 50
		{
			"PosX": 30,
			"PosY": 50,
			"PosZ": 10
		}
    ],
    // can be left empty
    "SnapGrids": [
	    // defines a single SnapGrid which spawns the given count of single snappoints
	    // all single points are optional, they will default to 0 when not included (as seen with the Y pos and counts in the real file)
        {
	        // the following four are for offsetting the grid to specific places on the gameboard
	        // x start position of grid to span
            "StartX": 12.5,
            // heigh of start point
            "StartY": 5,
	        // z start position of grid to span
            "StartZ": 12.5,
	        // x end position of grid to span
            "EndX": 187.5,
            // height of end point
            "EndY": 5,
	        // z end position of grid to span
            "EndZ": 187.5,
            // how many snappoints in one row (x direction)
            "CountX": 8,
            // how many stacked row x column there are (vertical direction)
            "CountY": 1,
            // how many snappoints in one column (z direction)
            "CountZ": 8
            // results in an 8x8 chess board with evenly distributed snappoint normally centered
        }
    ]
}
```
For a new game you will have to create your own json and use this template to create all necessary positions. To figure out the snappoint positions you will have to calculate and/or guess and try ingame.

## Gameboard surface texture image file
Must be a **PNG** file. Otherwise can be anything and as big as you want. (Should match the given gameboard size ratio)

## (Optional) Gamepiece OBJ files
> **tick the "triangulate" box in blender export if you use it**

All used OBJ files must be **triangulated** for use.