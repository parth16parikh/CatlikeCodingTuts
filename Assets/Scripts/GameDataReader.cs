using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataReader {

    private BinaryReader reader;
    public int Version {
        get;
        set;
    }

    public GameDataReader(BinaryReader reader, int version) {
        this.reader = reader;
        this.Version = version;
    }

    public int ReadInt() {
        return reader.ReadInt32();
    }

    public float ReadFloat() {
        return reader.ReadSingle();
    } 

    public Vector3 ReadVector3() {
        Vector3 value;
        value.x = reader.ReadSingle();
        value.y = reader.ReadSingle();
        value.z = reader.ReadSingle();
        return value;
    }

    public Quaternion ReadQuaternion() {
        Quaternion value;
        value.x = reader.ReadSingle();
        value.y = reader.ReadSingle();
        value.z = reader.ReadSingle();
        value.w = reader.ReadSingle();
        return value;
    }

    public Color ReadColor()
    {
        Color value;
        value.r = reader.ReadSingle();
        value.g = reader.ReadSingle();
        value.b = reader.ReadSingle();
        value.a = reader.ReadSingle();
        return value;
    }
}
