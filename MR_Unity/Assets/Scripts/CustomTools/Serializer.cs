using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Serializer : MonoBehaviour
{
    public static byte[] SerializeMesh(Object o)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(o);
                writer.Flush();
            }
            return ms.ToArray();
        }
    }

    public static string[] DeserializeMesh(byte[] data, int length)
    {
        string o;
        using (MemoryStream ms = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(ms))
            {
                o = new string(reader.ReadChars(length));
            }
        }
        char[] delimiters = { '\r', '\n' };
        return o.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
    }

}
