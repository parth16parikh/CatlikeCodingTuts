using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersistentStorage : MonoBehaviour
{
    string m_SaveFile;
    // Start is called before the first frame update
    void Awake()
    {
        m_SaveFile = Path.Combine(Application.persistentDataPath, "saveFile");
    }

    public void Save(PersistingObject o, int l_Version)
    {
        using (var writer = new BinaryWriter(File.Open(m_SaveFile, FileMode.Create)))
        {
            writer.Write(-l_Version);
            o.Save(new GameDataWriter(writer));
        }
    }

    public void Load(PersistingObject o)
    {
        using (var reader = new BinaryReader(File.Open(m_SaveFile, FileMode.Open)))
        {
            o.Load(new GameDataReader(reader, -reader.ReadInt32()));
        }
    }
}
