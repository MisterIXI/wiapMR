using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using System;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using WiapMR.PUN;
using WiapMR.PlayerScripts;

namespace WiapMR.GameScripts
{
    public class GameImporter : MonoBehaviourPunCallbacks
    {
        private const float GameboardThickness = 5f;

        public GameObject SnapPointPrefab;
        public GameObject GamePiecePrefab;
        public GameObject ScrollListPrefab;
        public GameObject ButtonPrefab;
        private int _waitingForData;
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
            GamePieceData = new Dictionary<string, string[]>();
        }

        public void SpawnGo()
        {
            if (_waitingForData != 0)
            {
                throw new Exception("Still waiting for data");
            }
            string path = Application.dataPath + "/_Games/Go/";
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

        public void SpawnChess()
        {
            if (_waitingForData != 0)
            {
                throw new Exception("Still waiting for data");
            }
            string path = Application.dataPath + "/_Games/Chess/";
            Debug.Log("Loading Chess");
            try
            {
                GameData gameData = ImportGameData(path + "Chess.json");
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
            // Debug.Log(gameData.snapGrid.countX);
            GameObject gameRoot = new GameObject("GameRoot");
            GameObject boardObj = new GameObject("GameBoard");
            this.GameRoot = gameRoot;
            this.GameBoard = boardObj;
            boardObj.AddComponent<GameController>();
            GameData gameData = new GameData(serializedGD);
            GameData = gameData;
            PieceSpawnController psc = gameRoot.AddComponent<PieceSpawnController>();
            psc.ButtonPrefab = ButtonPrefab;
            psc.ScrollListPrefab = ScrollListPrefab;
            CreateGameBoard(gameData, boardObj, texData);
            FillGamePieceData(gameData, boardObj, gpNames, gpData);
            CreateSnapPoints(gameData, boardObj);
            //set gameboard inactive to avoid snappoints colliding with gamepieces
            boardObj.SetActive(false);
            boardObj.transform.localScale = Vector3.one * 1f;
            boardObj.SetActive(true);
            BoxCollider cubeColl = boardObj.GetComponentInChildren<BoxCollider>();
            BoxCollider rBC = boardObj.AddComponent<BoxCollider>();
            // Debug.Log("CUBE SIZE| " + cubeColl.bounds.size);
            // Debug.Log("CUBE POS| " + cubeColl.gameObject.transform.position);
            rBC.size = cubeColl.bounds.size;
            rBC.center = cubeColl.transform.position;
            Destroy(cubeColl);
            boardObj.AddComponent<NearInteractionGrabbable>();
            ObjectManipulator om = boardObj.AddComponent<ObjectManipulator>();
            om.TwoHandedManipulationType = TransformFlags.Move | TransformFlags.Rotate;
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
            gameTexture.transform.localScale = new Vector3(gameData.Width, gameData.Height, 1);
            gameTexture.transform.position = new Vector3(gameData.Width / 2, 0.005f, gameData.Height / 2);
            gameTexture.transform.parent = parentObject.transform;
            // create a new Texture and load the given texture from path
            Texture2D tex = new Texture2D(gameData.Width, gameData.Height, TextureFormat.RGBA32, false);
            // tex.LoadRawTextureData(texData);
            tex.LoadImage(texData);
            tex.Apply();
            // set the shader to texture to avoid a blurry endresult
            gameTexture.GetComponent<Renderer>().material.mainTexture = tex;
            gameTexture.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
            // create board 3d Model
            GameObject game = GameObject.CreatePrimitive(PrimitiveType.Cube);
            game.name = "GameBoard_Cube";
            // scale the board according to the json data
            game.transform.localScale = new Vector3(gameData.Width, GameboardThickness, gameData.Height);
            game.transform.position = new Vector3(gameData.Width / 2, -GameboardThickness / 2, gameData.Height / 2);
            game.transform.parent = parentObject.transform;
            // take color of texture at 0,0 to try and make it fit better
            game.GetComponent<Renderer>().material.color = tex.GetPixel(0, 0);
        }

        private void CreateSnapPoints(GameData gameData, GameObject parentObject)
        {
            // loop through the custom snapoints
            GameObject tempObj;
            for (int i = 0; i < gameData.SnapPoints.Length; i++)
            {
                tempObj = Instantiate(SnapPointPrefab);
                tempObj.transform.position = new Vector3(gameData.SnapPoints[i].PosX, gameData.SnapPoints[i].PosY + 6, gameData.SnapPoints[i].PosZ);
                tempObj.transform.parent = parentObject.transform;
            }
            // loop through the given snapgrid
            for (int i = 0; i < gameData.SnapGrids.Length; i++)
            {
                float stepSizeX = gameData.SnapGrids[i].EndX - gameData.SnapGrids[i].StartX;
                float stepSizeY = gameData.SnapGrids[i].EndY - gameData.SnapGrids[i].StartY;
                float stepSizeZ = gameData.SnapGrids[i].EndZ - gameData.SnapGrids[i].StartZ;
                stepSizeX /= (gameData.SnapGrids[i].CountX - 1) != 0 ? gameData.SnapGrids[i].CountX - 1 : 1;
                stepSizeY /= (gameData.SnapGrids[i].CountY - 1) != 0 ? gameData.SnapGrids[i].CountY - 1 : 1;
                stepSizeZ /= (gameData.SnapGrids[i].CountZ - 1) != 0 ? gameData.SnapGrids[i].CountZ - 1 : 1;

                // loop through countx, county, countz
                for (int x = 0; x < gameData.SnapGrids[i].CountX; x++)
                {
                    for (int y = 0; y < gameData.SnapGrids[i].CountY; y++)
                    {
                        for (int z = 0; z < gameData.SnapGrids[i].CountZ; z++)
                        {
                            tempObj = Instantiate(SnapPointPrefab);
                            tempObj.transform.parent = parentObject.transform;
                            tempObj.transform.position = new Vector3(
                                gameData.SnapGrids[i].StartX + (x * stepSizeX),
                                gameData.SnapGrids[i].StartY + (y * stepSizeY) + 6,
                                gameData.SnapGrids[i].StartZ + (z * stepSizeZ)
                                );
                        }
                    }
                }
            }
        }

        public void AddToBoardSyncer(GameObject board)
        {
            SyncPos[] syncObjs = GameObject.FindObjectsOfType<SyncPos>();
            foreach (SyncPos syncObj in syncObjs)
            {
                if (syncObj.photonView.IsMine && syncObj.gameObject.name.StartsWith("BoardPosHelper"))
                {
                    syncObj.OtherToSync = board;
                }
            }
        }
        private void FillGamePieceData(GameData gameData, GameObject parentObject, string[] gpNames, byte[][] gpData)
        {
            for (int i = 0; i < gpData.Length; i++)
            {
                GamePieceData.Add(gpNames[i], System.Text.Encoding.UTF8.GetString(gpData[i]).Split('\n'));
            }
            GameObject.FindObjectOfType<PieceSpawnController>().CreatePieceList(parentObject, gameData.GamePieces);
        }

        private IEnumerator TriggerGameImport(string gameDataPath, GameData gameData)
        {
            byte[] textureArr = System.IO.File.ReadAllBytes(gameDataPath + gameData.Texture);
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
            }
            // split data up in smaller junks and send them to the clients via RPC
            // https://forum.photonengine.com/discussion/13276/any-way-to-send-large-data-via-rpcs-without-it-kicking-us-offline
            // max 512kb/message!
            // this could probably be optimized to send only to others, not also itself
            int calcChunks = 1 + 1 + gamePiecesData_BYTES.Length; // smalldata + texturedata + gamepiecesChunks
            this.photonView.RPC("SubscribeToDataEvents", RpcTarget.All, calcChunks);
            this.photonView.RPC("SendSmallData", RpcTarget.All, deduplicatedGamePieces.ToArray(), gameData.ToByteArray(), gamePiecesData_BYTES.Length);
            DataTransfer dt = GameObject.FindObjectOfType<DataTransfer>();
            for (int i = 0; i < gamePiecesData_BYTES.Length; i++)
            {
                dt.SendData(gamePiecesData_BYTES[i], "GamePiece_" + i);
            }
            dt.SendData(textureArr, "Texture");
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
            GameObject piece = PhotonNetwork.Instantiate(GamePiecePrefab.name, Vector3.zero, Quaternion.identity);
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

        private void HandleTextureData(string oTag, byte[] data)
        {
            if (oTag == "Texture")
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
            if (String.IsNullOrEmpty(result.Name))
            {
                result.Name = "New game";
            }
            // return null when invalid height or width are set
            if (result.Height <= 0 || result.Width <= 0)
            {
                throw new InvalidDataException("Height and width must be greater than 0");
            }
            // set empty arrays to "snapPoints" and "snapGrids" if they are not initialized
            if (result.SnapPoints == null)
            {
                result.SnapPoints = Array.Empty<GameData.SnapPointStruct>();
            }
            if (result.SnapGrids == null)
            {
                result.SnapGrids = Array.Empty<GameData.SnapGrid>();
            }

            // set every countX, countY, countZ to 1 if they are 0
            for (int i = 0; i < result.SnapGrids.Length; i++)
            {
                if (result.SnapGrids[i].CountX <= 0)
                    result.SnapGrids[i].CountX = 1;
                if (result.SnapGrids[i].CountY <= 0)
                    result.SnapGrids[i].CountY = 1;
                if (result.SnapGrids[i].CountZ <= 0)
                    result.SnapGrids[i].CountZ = 1;
            }

            return result;
        }

        public void ScaleDown(GameObject obj, float scale)
        {
            obj.transform.localScale = new Vector3(scale, scale, scale);
        }

    }
}