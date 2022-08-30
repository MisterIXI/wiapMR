using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using System.Runtime.InteropServices;
using System;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using System.Linq;
using WiapMR.PUN;

public class GameImporter : MonoBehaviourPunCallbacks
{
    private string GAME_DATA_PATH;
    private const float GAMEBOARD_THICKNESS = 5f;
    private readonly float GAMEPIECE_START_FACTOR = 0.01f;
    private readonly float GAMEBOARD_START_FACTOR = 0.1f;

    public GameObject snapPointPrefab;
    public GameObject gamePiecePrefab;
    public GameObject ScrollListPrefab;
    public GameObject ButtonPrefab;
    private int _waitingForData = 0;
    private byte[][] _gamePieceData;
    private byte[] _textureData;
    private string[] _gamePieceNames;
    private byte[] _serializedGameData;
    public GameObject GameRoot { get; private set; }
    public GameObject GameBoard { get; private set; }
    public GameData GameData;
    public Dictionary<string, string[]> GamePieceData { get; private set; }
    void Start()
    {
        // //read go.json from Assets/_Games/
        // string path = Application.dataPath + "/_Games/";
        // string[] files = Directory.GetFiles(path);
        // // print all files
        // foreach (string file in files)
        // {
        //     if (file.EndsWith(".json"))
        //     {
        //         JsonUtility.FromJson<GameData>(File.ReadAllText(file));
        //     }
        // }
        GamePieceData = new Dictionary<string, string[]>();
    }
    public void DoStuff()
    {
        if (_waitingForData != 0)
        {
            throw new Exception("Still waiting for data");
        }
        GAME_DATA_PATH = Application.dataPath + "/_Games/";
        string path = Application.dataPath + "/_Games/Go/";
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
    public void ImportGame()
    {
        Debug.Log("Importing game...");
        byte[] texData = _textureData;
        string[] gpNames = _gamePieceNames;
        byte[][] gpData = _gamePieceData;
        byte[] serializedGD = _serializedGameData;
        _textureData = null;
        _gamePieceNames = null;
        _gamePieceData = null;
        _serializedGameData = null;
        UnsubcribeFromDataEvents();
        // // Debug.Log(gameData.snapGrid.countX);
        GameObject gameRoot = new GameObject("GameRoot");
        GameObject boardObj = new GameObject("GameBoard");
        this.GameRoot = gameRoot;
        this.GameBoard = boardObj;
        boardObj.AddComponent<GameController>();
        GameData gameData = new GameData(serializedGD);
        GameData = gameData;
        var psc = gameRoot.AddComponent<PieceSpawnController>();
        psc.ButtonPrefab = ButtonPrefab;
        psc.ScrollListPrefab = ScrollListPrefab;
        CreateGameBoard(gameData, boardObj, texData);
        FillGamePieceData(gameData, boardObj, gpNames, gpData);
        CreateSnapPoints(gameData, boardObj);
        //set gameboard inactive to avoid snappoints colliding with gamepieces
        boardObj.SetActive(false);
        boardObj.transform.localScale = Vector3.one * 0.1f;
        boardObj.SetActive(true);
        BoxCollider cubeColl = boardObj.GetComponentInChildren<BoxCollider>();
        BoxCollider rBC = boardObj.AddComponent<BoxCollider>();
        // Debug.Log("CUBE SIZE| " + cubeColl.bounds.size);
        // Debug.Log("CUBE POS| " + cubeColl.gameObject.transform.position);
        rBC.size = cubeColl.bounds.size;
        rBC.center = cubeColl.transform.position;
        Destroy(cubeColl);
        boardObj.AddComponent<NearInteractionGrabbable>();
        var om = boardObj.AddComponent<ObjectManipulator>();
        om.TwoHandedManipulationType = TransformFlags.Move | TransformFlags.Rotate;
        // ScaleDown(boardObj, 0.01f);
        boardObj.transform.parent = gameRoot.transform;
        ScaleDown(boardObj, 0.01f);
        // CheckForPlayers(gameRoot);
        AddToBoardSyncer(boardObj);

    }

    private void CreateGameBoard(GameData gameData, GameObject parentObject, byte[] texData)
    {
        // create Board texture
        GameObject gameTexture = GameObject.CreatePrimitive(PrimitiveType.Quad);
        gameTexture.transform.rotation = Quaternion.Euler(90, 0, 0);
        gameTexture.transform.localScale = new Vector3(gameData.width, gameData.height, 1);
        gameTexture.transform.position = new Vector3(gameData.width / 2, 0.005f, gameData.height / 2);
        gameTexture.transform.parent = parentObject.transform;
        // create a new Texture and load the given texture from path
        Texture2D tex = new Texture2D(gameData.width, gameData.height, TextureFormat.RGBA32, false);
        // tex.LoadRawTextureData(texData);
        tex.LoadImage(texData);
        tex.Apply();
        // string testString = string.Join(",", texData);
        // Deb#ug.Log(testString.Length + "|" + (testString.Length-2)); // output 792485|792483
        // Debug.Log("..." + testString.Substring(testString.Length-100, 100));
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
        // PhotonView pv = game.AddComponent<PhotonView>();
        // pv.ViewID = 2500;
        // ScaleDown(gameTexture, GAMEBOARD_START_FACTOR);
        // ScaleDown(game, GAMEBOARD_START_FACTOR);
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

    // TODO: once the alternative is finished delete this method
    public void CheckForPlayers(GameObject parentObject)
    {
        if (parentObject == null)
            return;
        // move all *other* spawned players to GameBoard
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].GetComponent<HeadSync>().pv.IsMine)
            {
                if (players[i].transform.parent != parentObject.transform)
                {
                    players[i].transform.parent = parentObject.transform;
                }
            }
        }
    }

    public void AddToBoardSyncer(GameObject board)
    {
        var syncObjs = GameObject.FindObjectsOfType<SyncPos>();
        foreach (var syncObj in syncObjs)
        {
            if (syncObj.photonView.IsMine && syncObj.gameObject.name.StartsWith("BoardPosHelper"))
            {
                syncObj.otherToSync = board;
            }
        }
    }
    private void FillGamePieceData(GameData gameData, GameObject parentObject, string[] gpNames, byte[][] gpData)
    {
        List<string> gpNamesList = new List<string>(gpNames);
        GameObject[] result = new GameObject[gameData.gamePieces.Length];
        for (int i = 0; i < gpData.Length; i++)
        {
            GamePieceData.Add(gpNames[i], System.Text.Encoding.UTF8.GetString(gpData[i]).Split('\n'));
        }
        GameObject.FindObjectOfType<PieceSpawnController>().CreatePieceList(parentObject, gameData.gamePieces);
    }

    private IEnumerator TriggerGameImport(string gameDataPath, GameData gameData)
    {
        byte[] textureArr = System.IO.File.ReadAllBytes(gameDataPath + gameData.texture);
        List<byte[]> meshList = new List<byte[]>();

        List<string> deduplicatedGamePieces = ImporterHelper.DeduplicateGamePieces(gameData);
        byte[][] gamePiecesData_BYTES = new byte[deduplicatedGamePieces.Count][];
        for (int i = 0; i < deduplicatedGamePieces.Count; i++)
        {
            string[] stringData = System.IO.File.ReadAllLines(gameDataPath + deduplicatedGamePieces[i]);
            string data = "";
            for (int j = 0; j < stringData.Length; j++)
            {
                data += stringData[j] + "\n";
            }
            gamePiecesData_BYTES[i] = System.Text.Encoding.UTF8.GetBytes(data);
            //System.IO.File.ReadAllBytes(gameDataPath + deduplicatedGamePieces[i]);
            // ObjectLoader loader = tempObj.AddComponent<ObjectLoader>();
            // loader.Load(gameDataPath, deduplicatedGamePieces[i]);
            // while (!loader.isLoaded)
            // {
            //     //sleep for 100ms
            //     yield return new WaitForSeconds(0.1f);
            // }
        }
        int[] gdSizes = new int[] { gameData.gamePieces.Length, gameData.snapPoints.Length, gameData.snapGrids.Length };
        // Debug.Log("DEBUG:" + gamePiecesData_BYTES.Length + " " + gamePiecesData_BYTES[0].Length + "," + gamePiecesData_BYTES[1].Length);
        //TODO: split data up in smaller junks and send them to the clients via RPC
        // https://forum.photonengine.com/discussion/13276/any-way-to-send-large-data-via-rpcs-without-it-kicking-us-offline
        // max 512kb/message!
        //TODO: this could probably be optimized to send only to others, not also itself
        int calcChunks = 1 + 1 + gamePiecesData_BYTES.Length; // smalldata + texturedata + gamepiecesChunks
        this.photonView.RPC("SubscribeToDataEvents", RpcTarget.All, calcChunks);
        this.photonView.RPC("SendSmallData", RpcTarget.All, deduplicatedGamePieces.ToArray(), gameData.ToByteArray(), gamePiecesData_BYTES.Length);
        DataTransfer dt = GameObject.FindObjectOfType<DataTransfer>();
        for (int i = 0; i < gamePiecesData_BYTES.Length; i++)
        {
            dt.SendData(gamePiecesData_BYTES[i], "GamePiece_" + i);
        }
        dt.SendData(textureArr, "Texture");
        // this.photonView.RPC("ImportGame", RpcTarget.All, textureArr, deduplicatedGamePieces.ToArray(), gamePiecesData_BYTES, gameData.ToByteArray());

        yield return null;
    }
    [PunRPC]
    public void SendSmallData(string[] gamePieces, byte[] gameData, int gamePieceArrSize)
    {
        _gamePieceNames = gamePieces;
        _serializedGameData = gameData;
        _gamePieceData = new byte[gamePieceArrSize][];
        _waitingForData--;
        if (_waitingForData == 0)
        {
            ImportGame();
        }
    }

    public GameObject SpawnGamePiece(int pieceID)
    {
        GameObject piece = PhotonNetwork.Instantiate(gamePiecePrefab.name, Vector3.zero, Quaternion.identity);
        // Debug.Log("DEBUG: Spawning GamePiece " + pieceID + "  |  " + _gameData.gamePieces.Length);
        piece.GetPhotonView().RPC("LoadGamePiece", RpcTarget.All, pieceID);
        return piece;
    }


    [PunRPC]
    public void SubscribeToDataEvents(int calcChunks)
    {
        _waitingForData = calcChunks;
        GameObject.FindObjectOfType<DataTransfer>().OnDataReceived += HandlePieceData;
        GameObject.FindObjectOfType<DataTransfer>().OnDataReceived += HandleTextureData;
    }
    private void UnsubcribeFromDataEvents()
    {
        GameObject.FindObjectOfType<DataTransfer>().OnDataReceived -= HandlePieceData;
        GameObject.FindObjectOfType<DataTransfer>().OnDataReceived -= HandleTextureData;
    }
    private void HandlePieceData(string tag, byte[] data)
    {
        if (tag.StartsWith("GamePiece"))
        {
            int index = int.Parse(tag.Substring(tag.IndexOf("_") + 1));
            _gamePieceData[index] = data;
            _waitingForData--;
            if (_waitingForData == 0)
            {
                ImportGame();
            }
        }
    }

    private void HandleTextureData(string tag, byte[] data)
    {
        if (tag == "Texture")
        {
            _textureData = data;
            _waitingForData--;
            if (_waitingForData == 0)
            {
                ImportGame();
            }
        }
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

    public void ScaleDown(GameObject obj, float scale)
    {
        obj.transform.localScale = new Vector3(scale, scale, scale);
    }

}
