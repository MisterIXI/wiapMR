using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Json;

public class GameImporter : MonoBehaviour
{
    private string GAME_DATA_PATH;
    private const float GAMEBOARD_THICKNESS = 5f;
    public GameObject snapPointPrefab;
    public GameObject gamePiecePrefab;
    public struct GameData
    {
        public string name;
        public int height;
        public int width;
        public string texture;
        public GamePiece[] gamePieces;
        public SnapPointStruct[] snapPoints;
        public SnapGrid[] snapGrids;
        [System.Serializable]
        public struct GamePiece
        {
            public string name;
            public string path;
            public string color;
            public float metallic;
            public float smoothness;
        }
        [System.Serializable]
        public struct SnapPointStruct
        {
            public int posX;
            public int posY;
            public int posZ;
        }
        [System.Serializable]
        public struct SnapGrid
        {
            public float startX;
            public float startY;
            public float startZ;
            public float endX;
            public float endY;
            public float endZ;
            public int countX;
            public int countY;
            public int countZ;

        }
    }

    void Start()
    {
        // //read go.json from Assets/Games/
        // string path = Application.dataPath + "/Games/";
        // string[] files = Directory.GetFiles(path);
        // // print all files
        // foreach (string file in files)
        // {
        //     if (file.EndsWith(".json"))
        //     {
        //         JsonUtility.FromJson<GameData>(File.ReadAllText(file));
        //     }
        // }
        GAME_DATA_PATH = Application.dataPath + "/Games/";
        string path = Application.dataPath + "/Games/" + "go.json";
        // GameData gamedata = JsonUtility.FromJson<GameData>(File.ReadAllText(path));
        try
        {
            GameData gameData = ImportGameData(path);
            ImportGame(gameData);
        }
        catch(System.Exception e)
        {
            Debug.Log("Game data invalid or not found!");
            Debug.Log(e.Message);
        }
    }

    public void ImportGame(GameData gameData)
    {
        // Debug.Log(gameData.snapGrid.countX);
        GameObject parentObject = new GameObject("GameBoard");

        // create Board texture
        GameObject gameTexture = GameObject.CreatePrimitive(PrimitiveType.Quad);
        gameTexture.transform.rotation = Quaternion.Euler(90, 0, 0);
        gameTexture.transform.localScale = new Vector3(gameData.width, gameData.height, 1);
        gameTexture.transform.position = new Vector3(gameData.width / 2, 0.001f, gameData.height / 2);
        gameTexture.transform.parent = parentObject.transform;
        // create a new Texture and load the given texture from path
        Texture2D tex = new Texture2D(200, 200, TextureFormat.RGBA32, false);
        tex.LoadImage(System.IO.File.ReadAllBytes(GAME_DATA_PATH + gameData.texture));
        // tex.filterMode = FilterMode.Point;
        // Debug.Log("Path:" + GAMEBOARD_PATH + gameData.texture + " | tex: " + tex);
        // set the shader to texture to avoid a blurry endresult
        // gameTexture.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
        gameTexture.GetComponent<Renderer>().material.mainTexture = tex;
        // create board 3d Model
        GameObject game = GameObject.CreatePrimitive(PrimitiveType.Cube);
        game.name = "GameBoard_Cube";
        // scale the board according to the json data
        game.transform.localScale = new Vector3(gameData.width, GAMEBOARD_THICKNESS, gameData.height);
        game.transform.position = new Vector3(gameData.width / 2, -GAMEBOARD_THICKNESS / 2, gameData.height / 2);
        game.transform.parent = parentObject.transform;
        // take color of texture at 0,0 to try and make it fit better
        game.GetComponent<Renderer>().material.color = tex.GetPixel(0, 0);

        // loop through the custom snapoints
        GameObject tempObj;
        for (int i = 0; i < gameData.snapPoints.Length; i++)
        {
            tempObj = Instantiate(snapPointPrefab);
            tempObj.transform.position = new Vector3(gameData.snapPoints[i].posX, gameData.snapPoints[i].posY + 6, gameData.snapPoints[i].posZ);
            tempObj.transform.parent = parentObject.transform;
        }
        // loop through the given snapgrid
        for (int i = 0; i < gameData.snapGrids.Length; i++)
        {
            float stepSizeX = gameData.snapGrids[i].endX - gameData.snapGrids[i].startX;
            float stepSizeY = gameData.snapGrids[i].endY - gameData.snapGrids[i].startY;
            float stepSizeZ = gameData.snapGrids[i].endZ - gameData.snapGrids[i].startZ;
            stepSizeX /= (gameData.snapGrids[i].countX - 1) != 0 ? gameData.snapGrids[i].countX - 1 : 1;
            stepSizeY /= (gameData.snapGrids[i].countY - 1) != 0 ? gameData.snapGrids[i].countY - 1 : 1;
            stepSizeZ /= (gameData.snapGrids[i].countZ - 1) != 0 ? gameData.snapGrids[i].countZ - 1 : 1;

            // loop through countx, county, countz
            for (int x = 0; x < gameData.snapGrids[i].countX; x++)
            {
                for (int y = 0; y < gameData.snapGrids[i].countY; y++)
                {
                    for (int z = 0; z < gameData.snapGrids[i].countZ; z++)
                    {
                        tempObj = Instantiate(snapPointPrefab);
                        tempObj.transform.parent = parentObject.transform;
                        tempObj.transform.position = new Vector3(
                            gameData.snapGrids[i].startX + (x * stepSizeX),
                            gameData.snapGrids[i].startY + (y * stepSizeY) + 6,
                            gameData.snapGrids[i].startZ + (z * stepSizeZ)
                            );
                    }
                }
            }
        }

        // loop through the gamepieces
        for (int i = 0; i < gameData.gamePieces.Length; i++)
        {
            GameObject piece = Instantiate(gamePiecePrefab);
            GameObject obj = Resources.Load<GameObject>(gameData.gamePieces[i].path);
            Debug.Log("Path: " + gameData.gamePieces[i].path);
            Debug.Log("Mesh: " + obj);
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
            Debug.Log("Path: " + gameData.gamePieces[i].path);
            Debug.Log("Mesh: " + mesh);
            piece.GetComponent<MeshFilter>().mesh = mesh;
            piece.transform.position = new Vector3(10 + (i * 5), 10, 10);
            Material currentMat = piece.GetComponent<MeshRenderer>().material;
            currentMat.color = ConvertColor(gameData.gamePieces[i].color);
            currentMat.SetFloat("_Metallic", gameData.gamePieces[i].metallic);
            currentMat.SetFloat("_Glossiness", gameData.gamePieces[i].smoothness);
            piece.GetComponent<BoxCollider>().size = piece.GetComponent<MeshRenderer>().bounds.size;
            if(currentMat.color.a != 1){
                currentMat.SetFloat("_Mode", 3);
            }

            // without this line the shader will only show the correct color until something changes
            // with it, it seems to reload the variables and renders correctly
            currentMat.shader = Shader.Find("Standard");
        }

        parentObject.transform.localScale = Vector3.one * 0.1f;
    }

    /// <summary>
    /// Imports GameData from given path to json file.
    /// </summary>
    /// <param name="path">Path to json file (including file ending)</param>
    /// <returns>Imported GameData or null if non-optional fields were imported incorrectly</returns>
    private GameData ImportGameData(string path)
    {
        GameData result = JsonUtility.FromJson<GameData>(File.ReadAllText(path));
        // set default name
        if (result.name == null || result.name == "")
        {
            result.name = "New game";
        }
        // return null when invalid height or width are set
        if (result.height <= 0 || result.width <= 0)
        {
            throw new InvalidDataException("Height and width must be greater than 0");
        }
        // set empty arrays to "snapPoints" and "snapGrids" if they are not initialized
        if (result.snapPoints == null)
        {
            result.snapPoints = new GameData.SnapPointStruct[0];
        }
        if (result.snapGrids == null)
        {
            result.snapGrids = new GameData.SnapGrid[0];
        }

        // set every countX, countY, countZ to 1 if they are 0
        for (int i = 0; i < result.snapGrids.Length; i++)
        {
            if (result.snapGrids[i].countX <= 0)
                result.snapGrids[i].countX = 1;
            if (result.snapGrids[i].countY <= 0)
                result.snapGrids[i].countY = 1;
            if (result.snapGrids[i].countZ <= 0)
                result.snapGrids[i].countZ = 1;
        }

        return result;
    }

    private Color ConvertColor(string color)
    {
        Color result = new Color();
        string[] colorParts = color.Split(',');
        result.r = float.Parse(colorParts[0]) / 255;
        result.g = float.Parse(colorParts[1]) / 255;
        result.b = float.Parse(colorParts[2]) / 255;
        result.a = float.Parse(colorParts[3]);
        return result;
    }
}
