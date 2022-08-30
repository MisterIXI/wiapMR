using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameData
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

    public override string ToString()
    {
        return name + "(" + height + "," + width + ")";
    }

    public GameData()
    {
        name = "";
        height = 0;
        width = 0;
        texture = "";
        gamePieces = new GamePiece[0];
        snapPoints = new SnapPointStruct[0];
        snapGrids = new SnapGrid[0];
    }

    public byte[] ToByteArray()
    {
        // use a memorystream to save everything in one byte array
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(name);
                writer.Write(height);
                writer.Write(width);
                writer.Write(texture);
                writer.Write(gamePieces.Length);
                for (int i = 0; i < gamePieces.Length; i++)
                {
                    writer.Write(gamePieces[i].name);
                    writer.Write(gamePieces[i].path);
                    writer.Write(gamePieces[i].color);
                    writer.Write(gamePieces[i].metallic);
                    writer.Write(gamePieces[i].smoothness);
                }
                writer.Write(snapPoints.Length);
                for (int i = 0; i < snapPoints.Length; i++)
                {
                    writer.Write(snapPoints[i].posX);
                    writer.Write(snapPoints[i].posY);
                    writer.Write(snapPoints[i].posZ);
                }
                writer.Write(snapGrids.Length);
                for (int i = 0; i < snapGrids.Length; i++)
                {
                    writer.Write(snapGrids[i].startX);
                    writer.Write(snapGrids[i].startY);
                    writer.Write(snapGrids[i].startZ);
                    writer.Write(snapGrids[i].endX);
                    writer.Write(snapGrids[i].endY);
                    writer.Write(snapGrids[i].endZ);
                    writer.Write(snapGrids[i].countX);
                    writer.Write(snapGrids[i].countY);
                    writer.Write(snapGrids[i].countZ);
                }
                writer.Flush();
            }
            return ms.ToArray();
        }
    }


    public GameData(byte[] data)
    {
        // use a memorystream to load everything from the byte array
        using (MemoryStream ms = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(ms))
            {
                name = reader.ReadString();
                height = reader.ReadInt32();
                width = reader.ReadInt32();
                texture = reader.ReadString();
                int gamePieceCount = reader.ReadInt32();
                gamePieces = new GamePiece[gamePieceCount];
                for (int i = 0; i < gamePieceCount; i++)
                {
                    gamePieces[i].name = reader.ReadString();
                    gamePieces[i].path = reader.ReadString();
                    gamePieces[i].color = reader.ReadString();
                    gamePieces[i].metallic = reader.ReadSingle();
                    gamePieces[i].smoothness = reader.ReadSingle();
                }
                int snapPointCount = reader.ReadInt32();
                snapPoints = new SnapPointStruct[snapPointCount];
                for (int i = 0; i < snapPointCount; i++)
                {
                    snapPoints[i].posX = reader.ReadInt32();
                    snapPoints[i].posY = reader.ReadInt32();
                    snapPoints[i].posZ = reader.ReadInt32();
                }
                int snapGridCount = reader.ReadInt32();
                snapGrids = new SnapGrid[snapGridCount];
                for (int i = 0; i < snapGridCount; i++)
                {
                    snapGrids[i].startX = reader.ReadSingle();
                    snapGrids[i].startY = reader.ReadSingle();
                    snapGrids[i].startZ = reader.ReadSingle();
                    snapGrids[i].endX = reader.ReadSingle();
                    snapGrids[i].endY = reader.ReadSingle();
                    snapGrids[i].endZ = reader.ReadSingle();
                    snapGrids[i].countX = reader.ReadInt32();
                    snapGrids[i].countY = reader.ReadInt32();
                    snapGrids[i].countZ = reader.ReadInt32();
                }
            }
        }
    }
}
