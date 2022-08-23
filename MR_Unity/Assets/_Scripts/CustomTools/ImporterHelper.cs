using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System;

public class ImporterHelper : MonoBehaviour
{
    public static Color ConvertColor(string color)
    {
        Color result = new Color();
        string[] colorParts = color.Split(',');
        result.r = float.Parse(colorParts[0]) / 255;
        result.g = float.Parse(colorParts[1]) / 255;
        result.b = float.Parse(colorParts[2]) / 255;
        result.a = float.Parse(colorParts[3]);
        return result;
    }

    public static List<string> DeduplicateGamePieces(GameData gameData)
    {
        List<string> deduplicatedGamePieces = new List<string>();
        for (int i = 0; i < gameData.gamePieces.Length; i++)
        {
            if (!deduplicatedGamePieces.Contains(gameData.gamePieces[i].path))
            {
                deduplicatedGamePieces.Add(gameData.gamePieces[i].path);
            }
        }
        return deduplicatedGamePieces;
    }

    public static void ScaleUp(GameObject gameObject, Vector3 scale)
    {
        // scale up the game object to match mesh bounding box and scale
        Vector3 meshScale = gameObject.GetComponent<MeshRenderer>().bounds.size;
        float[] scaleFactor = new float[] { scale.x / meshScale.x, scale.y / meshScale.y, scale.z / meshScale.z };
        int arg = 0;
        float max = meshScale[0];
        for (int i = 1; i < scaleFactor.Length; i++)
        {
            if (meshScale[i] > max)
            {
                max = meshScale[i];
                arg = i;
            }
        }
        Vector3 objectScale = gameObject.transform.localScale;
        Vector3 meshScaleScaled = new Vector3(objectScale.x * scaleFactor[arg], objectScale.y * scaleFactor[arg], objectScale.z * scaleFactor[arg]);
        gameObject.transform.localScale = meshScaleScaled;
        Destroy(gameObject.GetComponent<BoxCollider>());
        gameObject.AddComponent<BoxCollider>();
        // gameObject.GetComponent<MeshRenderer>();
        // Debug.Log("Scaled up to: " + gameObject.GetComponent<MeshRenderer>().bounds);

    }

    // https://stackoverflow.com/questions/3278827/how-to-convert-a-structure-to-a-byte-array-in-c
    public static byte[] SerializeGameData(GameData customObject)
    {

        int size = Marshal.SizeOf(customObject);
        Debug.Log("Size before: " + size);
        byte[] arr = new byte[size];

        IntPtr ptr = IntPtr.Zero;
        try
        {
            ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(customObject, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
        }

        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return arr;
    }

    public static GameData DeserializeGameData(int[] sizes, byte[] serializedCustomObject)
    {

        GameData gameData = new GameData();

        // gameData.gamePieces = new GameData.GamePiece[sizes[0]];
        // gameData.snapPoints = new GameData.SnapPointStruct[sizes[1]];
        // gameData.snapGrids = new GameData.SnapGrid[sizes[2]];

        int size = Marshal.SizeOf(gameData);
        Debug.Log("Size after: " + size);
        IntPtr ptr = IntPtr.Zero;
        try
        {
            ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(serializedCustomObject, 0, ptr, size);

            gameData = (GameData)Marshal.PtrToStructure(ptr, gameData.GetType());
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
        return gameData;
    }
}
