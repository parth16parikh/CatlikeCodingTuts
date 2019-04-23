using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataReader
{
    private BinaryReader reader;
    public int m_version;

    public GameDataReader(BinaryReader reader, int l_Version)
    {
        this.reader = reader;
        this.m_version = l_Version;
    }

    public int ReadInt()
    {
        return reader.ReadInt32();
    }

    public float ReadFloat()
    {
        return reader.ReadSingle();
    }

    public Vector3 ReadVector()
    {
        Vector3 p;
        p.x = reader.ReadSingle();
        p.y = reader.ReadSingle();
        p.z = reader.ReadSingle();
        return p;
    }

    public Quaternion ReadQuaternion()
    {
        Quaternion q;
        q.x = reader.ReadSingle();
        q.y = reader.ReadSingle();
        q.z = reader.ReadSingle();
        q.w = reader.ReadSingle();
        return q;
    }

    public Color ReadColor()
    {
        Color color;
        color.r = reader.ReadSingle();
        color.g = reader.ReadSingle();
        color.b = reader.ReadSingle();
        color.a = reader.ReadSingle();
        return color;
    }
}
