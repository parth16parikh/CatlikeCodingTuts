using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PersistableObject : MonoBehaviour {
	
    public virtual void Save(GameDataWriter writer) {
        writer.write(transform.localPosition);
        writer.write(transform.localRotation);
        writer.write(transform.localScale);
    }

    public virtual void Load(GameDataReader reader) {
        transform.localPosition = reader.ReadVector3();
        transform.localRotation = reader.ReadQuaternion();
        transform.localScale = reader.ReadVector3();
    }
}
