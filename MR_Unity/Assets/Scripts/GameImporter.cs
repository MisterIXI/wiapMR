using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using System.Runtime.InteropServices;
using System;


public class GameImporter : MonoBehaviourPunCallbacks
{
    private string GAME_DATA_PATH;
    private const float GAMEBOARD_THICKNESS = 5f;
    private readonly Vector3 GAMEPIECE_START_SIZE = new Vector3(10f, 10f, 10f);
    public GameObject snapPointPrefab;
    public GameObject gamePiecePrefab;


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

    }
    public void DoStuff()
    {
        GAME_DATA_PATH = Application.dataPath + "/Games/";
        string path = Application.dataPath + "/Games/Go/";
        // GameData gamedata = JsonUtility.FromJson<GameData>(File.ReadAllText(path));
        try
        {
            GameData gameData = ImportGameData(path + "Go.json");
            StartCoroutine(TriggerGameImport(path, gameData));
        }
        catch (System.Exception e)
        {
            Debug.Log("Game data invalid or not found!");
            Debug.Log(e.Message);
        }
    }
    [PunRPC]
    public void ImportGame(byte[] texData, string[] gpNames, string[][] gpData, byte[] serializedGD)
    {
        // Debug.Log(gameData.snapGrid.countX);
        GameObject parentObject = new GameObject("GameBoard");
        GameData gameData = new GameData(serializedGD);
        CreateGameBoard(gameData, parentObject, texData);
        CreateGamePieces(gameData, parentObject, gpNames, gpData);
        CreateSnapPoints(gameData, parentObject);
        //set gameboard inactive to avoid snappoints colliding with gamepieces
        parentObject.SetActive(false);


        parentObject.transform.localScale = Vector3.one * 0.1f;
        parentObject.SetActive(true);
    }

    private void CreateGameBoard(GameData gameData, GameObject parentObject, byte[] texData)
    {
        // create Board texture
        GameObject gameTexture = GameObject.CreatePrimitive(PrimitiveType.Quad);
        gameTexture.transform.rotation = Quaternion.Euler(90, 0, 0);
        gameTexture.transform.localScale = new Vector3(gameData.width, gameData.height, 1);
        gameTexture.transform.position = new Vector3(gameData.width / 2, 0.001f, gameData.height / 2);
        gameTexture.transform.parent = parentObject.transform;
        // create a new Texture and load the given texture from path
        Texture2D tex = new Texture2D(gameData.width, gameData.height, TextureFormat.RGBA32, false);
        //print texData


        // tex.LoadRawTextureData(texData);
        tex.LoadImage(texData);
        tex.Apply();
        Debug.Log(tex);
        // tex.filterMode = FilterMode.Point;
        // Debug.Log("Path:" + GAMEBOARD_PATH + gameData.texture + " | tex: " + tex);
        // set the shader to texture to avoid a blurry endresult
        gameTexture.GetComponent<Renderer>().material.mainTexture = tex;
        gameTexture.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
        // create board 3d Model
        GameObject game = GameObject.CreatePrimitive(PrimitiveType.Cube);
        game.name = "GameBoard_Cube";
        // scale the board according to the json data
        game.transform.localScale = new Vector3(gameData.width, GAMEBOARD_THICKNESS, gameData.height);
        game.transform.position = new Vector3(gameData.width / 2, -GAMEBOARD_THICKNESS / 2, gameData.height / 2);
        game.transform.parent = parentObject.transform;
        // take color of texture at 0,0 to try and make it fit better
        game.GetComponent<Renderer>().material.color = tex.GetPixel(0, 0);
    }

    private void CreateSnapPoints(GameData gameData, GameObject parentObject)
    {
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
    }

    private GameObject[] CreateGamePieces(GameData gameData, GameObject parentObject, string[] gpNames, string[][] gpData)
    {
        List<string> gpNamesList = new List<string>(gpNames);
        GameObject[] result = new GameObject[gameData.gamePieces.Length];
        // loop through the gamepieces
        for (int i = 0; i < gameData.gamePieces.Length; i++)
        {
            GameObject piece = Instantiate(gamePiecePrefab);
            piece.name = gameData.gamePieces[i].name;
            string meshPath = gameData.gamePieces[i].path;
            // GameObject obj = Resources.Load<GameObject>(gameData.gamePieces[i].path);
            // GameObject obj = new GameObject();
            ObjectLoader loader = piece.AddComponent<ObjectLoader>();
            loader.Load(gpData[gpNamesList.IndexOf(meshPath)]);
            piece.transform.position = new Vector3(-10 - (i / 10 * 10), 0, i % 10 * 10);
            Material currentMat = piece.GetComponent<MeshRenderer>().material;
            currentMat.color = ImporterHelper.ConvertColor(gameData.gamePieces[i].color);
            currentMat.SetFloat("_Metallic", gameData.gamePieces[i].metallic);
            currentMat.SetFloat("_Glossiness", gameData.gamePieces[i].smoothness);
            Destroy(piece.GetComponent<BoxCollider>());
            piece.AddComponent<BoxCollider>();
            if (currentMat.color.a != 1)
            {
                currentMat.SetFloat("_Mode", 3);
            }

            // without this line the shader will only show the correct color until something changes
            // with it, it seems to reload the variables and renders correctly
            currentMat.shader = Shader.Find("Standard");
            piece.transform.parent = parentObject.transform;
            Debug.Log(piece.GetComponent<MeshRenderer>().bounds);
            ImporterHelper.ScaleUp(piece, GAMEPIECE_START_SIZE);
            result[i] = piece;
        }
        return result;
    }

    private IEnumerator TriggerGameImport(string gameDataPath, GameData gameData)
    {
        byte[] texArr = System.IO.File.ReadAllBytes(gameDataPath + gameData.texture);
        List<byte[]> meshList = new List<byte[]>();

        GameObject tempObj = new GameObject();
        tempObj.SetActive(false);
        List<string> deduplicatedGamePieces = ImporterHelper.DeduplicateGamePieces(gameData);
        string[][] gamePiecesData = new string[deduplicatedGamePieces.Count][];
        for (int i = 0; i < deduplicatedGamePieces.Count; i++)
        {
            gamePiecesData[i] = System.IO.File.ReadAllLines(gameDataPath + deduplicatedGamePieces[i]);
            // ObjectLoader loader = tempObj.AddComponent<ObjectLoader>();
            // loader.Load(gameDataPath, deduplicatedGamePieces[i]);
            // while (!loader.isLoaded)
            // {
            //     //sleep for 100ms
            //     yield return new WaitForSeconds(0.1f);
            // }
        }
        int[] gdSizes = new int[] { gameData.gamePieces.Length, gameData.snapPoints.Length, gameData.snapGrids.Length };
        this.photonView.RPC("ImportGame", RpcTarget.All, texArr, deduplicatedGamePieces.ToArray(), gamePiecesData, gameData.ToByteArray());
        yield return null;
    }


    /// <summary>
    /// Imports GameData from given path to json file.
    /// </summary>
    /// <param name="path">Path to json file (including file ending)</param>
    /// <returns>Imported GameData or null if non-optional fields were imported incorrectly</returns>
    private GameData ImportGameData(string path)
    {
        GameData result = JsonUtility.FromJson<GameData>(File.ReadAllText(path));
        // Debug.Log("Imported as: " + result);
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


}
