using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[DisallowMultipleComponent]
public class PersistingObject : MonoBehaviour
{
    public virtual void Load(GameDataReader reader)
    {
        transform.localPosition = reader.ReadVector();
        transform.localRotation = reader.ReadQuaternion();
        transform.localScale = reader.ReadVector();
    } 

    public virtual void Save(GameDataWriter write)
    {
        write.Write(transform.localPosition);
        write.Write(transform.localRotation);
        write.Write(transform.localScale);
    }
}
