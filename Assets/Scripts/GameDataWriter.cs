using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataWriter {


    private BinaryWriter writer;

    public GameDataWriter(BinaryWriter writer) {
        this.writer = writer;    
    }

    public void write(int value) {
        writer.Write(value);
    }

    public void write(float value)
    {
        writer.Write(value);
    }

    public void write(Vector3 value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public void write(Quaternion value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }

    public void write(Color value) {
        writer.Write(value.r);
        writer.Write(value.g);
        writer.Write(value.b);
        writer.Write(value.a);
    }

}
