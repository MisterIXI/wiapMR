using System.IO;

namespace WiapMR.GameScripts
{
    /// <summary>
    /// Contains all imported game data.
    /// With this class, the game data can be serialized and sent to other players.
    /// </summary>
    public class GameData
    {
        public string Name;
        public int Height;
        public int Width;
        public string Texture;
        public GamePiece[] GamePieces;
        public SnapPointStruct[] SnapPoints;
        public SnapGrid[] SnapGrids;
        [System.Serializable]
        public struct GamePiece
        {
            public string Name;
            public string Path;
            public string Color;
            public float Metallic;
            public float Smoothness;
        }
        [System.Serializable]
        public struct SnapPointStruct
        {
            public int PosX;
            public int PosY;
            public int PosZ;
        }
        [System.Serializable]
        public struct SnapGrid
        {
            public float StartX;
            public float StartY;
            public float StartZ;
            public float EndX;
            public float EndY;
            public float EndZ;
            public int CountX;
            public int CountY;
            public int CountZ;
        }

        public override string ToString()
        {
            return Name + "(" + Height + "," + Width + ")";
        }

        public GameData()
        {
            Name = "";
            Height = 0;
            Width = 0;
            Texture = "";
            GamePieces = new GamePiece[0];
            SnapPoints = new SnapPointStruct[0];
            SnapGrids = new SnapGrid[0];
        }

        /// <summary>
        /// Converts the game data to a byte array.
        /// </summary>
        /// <returns>The converted gameData as byte[]</returns>
        public byte[] ToByteArray()
        {
            // use a memorystream to save everything in one byte array
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(Name);
                    writer.Write(Height);
                    writer.Write(Width);
                    writer.Write(Texture);
                    writer.Write(GamePieces.Length);
                    for (int i = 0; i < GamePieces.Length; i++)
                    {
                        writer.Write(GamePieces[i].Name);
                        writer.Write(GamePieces[i].Path);
                        writer.Write(GamePieces[i].Color);
                        writer.Write(GamePieces[i].Metallic);
                        writer.Write(GamePieces[i].Smoothness);
                    }
                    writer.Write(SnapPoints.Length);
                    for (int i = 0; i < SnapPoints.Length; i++)
                    {
                        writer.Write(SnapPoints[i].PosX);
                        writer.Write(SnapPoints[i].PosY);
                        writer.Write(SnapPoints[i].PosZ);
                    }
                    writer.Write(SnapGrids.Length);
                    for (int i = 0; i < SnapGrids.Length; i++)
                    {
                        writer.Write(SnapGrids[i].StartX);
                        writer.Write(SnapGrids[i].StartY);
                        writer.Write(SnapGrids[i].StartZ);
                        writer.Write(SnapGrids[i].EndX);
                        writer.Write(SnapGrids[i].EndY);
                        writer.Write(SnapGrids[i].EndZ);
                        writer.Write(SnapGrids[i].CountX);
                        writer.Write(SnapGrids[i].CountY);
                        writer.Write(SnapGrids[i].CountZ);
                    }
                    writer.Flush();
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts a byte array to a game data object.
        /// </summary>
        /// <param name="data">the binary data received</param>
        public GameData(byte[] data)
        {
            // use a memorystream to load everything from the byte array
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    Name = reader.ReadString();
                    Height = reader.ReadInt32();
                    Width = reader.ReadInt32();
                    Texture = reader.ReadString();
                    int gamePieceCount = reader.ReadInt32();
                    GamePieces = new GamePiece[gamePieceCount];
                    for (int i = 0; i < gamePieceCount; i++)
                    {
                        GamePieces[i].Name = reader.ReadString();
                        GamePieces[i].Path = reader.ReadString();
                        GamePieces[i].Color = reader.ReadString();
                        GamePieces[i].Metallic = reader.ReadSingle();
                        GamePieces[i].Smoothness = reader.ReadSingle();
                    }
                    int snapPointCount = reader.ReadInt32();
                    SnapPoints = new SnapPointStruct[snapPointCount];
                    for (int i = 0; i < snapPointCount; i++)
                    {
                        SnapPoints[i].PosX = reader.ReadInt32();
                        SnapPoints[i].PosY = reader.ReadInt32();
                        SnapPoints[i].PosZ = reader.ReadInt32();
                    }
                    int snapGridCount = reader.ReadInt32();
                    SnapGrids = new SnapGrid[snapGridCount];
                    for (int i = 0; i < snapGridCount; i++)
                    {
                        SnapGrids[i].StartX = reader.ReadSingle();
                        SnapGrids[i].StartY = reader.ReadSingle();
                        SnapGrids[i].StartZ = reader.ReadSingle();
                        SnapGrids[i].EndX = reader.ReadSingle();
                        SnapGrids[i].EndY = reader.ReadSingle();
                        SnapGrids[i].EndZ = reader.ReadSingle();
                        SnapGrids[i].CountX = reader.ReadInt32();
                        SnapGrids[i].CountY = reader.ReadInt32();
                        SnapGrids[i].CountZ = reader.ReadInt32();
                    }
                }
            }
        }
    }
}
