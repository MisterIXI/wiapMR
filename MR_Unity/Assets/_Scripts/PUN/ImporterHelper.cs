using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using WiapMR.GameScripts;

namespace WiapMR.PUN
{
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
            for (int i = 0; i < gameData.GamePieces.Length; i++)
            {
                if (!deduplicatedGamePieces.Contains(gameData.GamePieces[i].Path))
                {
                    deduplicatedGamePieces.Add(gameData.GamePieces[i].Path);
                }
            }
            return deduplicatedGamePieces;
        }

        public static void ScaleUp(GameObject objectToScale, Vector3 scale)
        {
            // scale up the game object to match mesh bounding box and scale
            Vector3 meshScale = objectToScale.GetComponent<MeshRenderer>().bounds.size;
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
            Vector3 objectScale = objectToScale.transform.localScale;
            Vector3 meshScaleScaled = new Vector3(objectScale.x * scaleFactor[arg], objectScale.y * scaleFactor[arg], objectScale.z * scaleFactor[arg]);
            objectToScale.transform.localScale = meshScaleScaled;
            Destroy(objectToScale.GetComponent<BoxCollider>());
            objectToScale.AddComponent<BoxCollider>();
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
}