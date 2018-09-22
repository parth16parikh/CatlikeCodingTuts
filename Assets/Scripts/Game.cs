using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject {

    public PersistableObject prefab;
    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;
    public PersistentStorage storage;

    List<PersistableObject> objects;

    private void Start()
    {
        objects = new List<PersistableObject>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(createKey)) {
            CreateObject();
        }

        if(Input.GetKeyDown(newGameKey)) {
            BeginNewGame();
        }

        if(Input.GetKeyDown(saveKey)) {
            storage.Save(this);
        }

        if(Input.GetKeyDown(loadKey)) {
            BeginNewGame();
            storage.Load(this);
        }
    }

    void CreateObject() {
        PersistableObject o = Instantiate(prefab);
        Transform t = o.transform;
        t.localPosition = Random.insideUnitSphere * 5.0f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1.0f);
        objects.Add(o);
    }

    void BeginNewGame() {
        for (int i = 0; i < objects.Count; i++) {
            Destroy(objects[i].gameObject);
        }
        objects.Clear();
    }

    public override void Save(GameDataWriter writer) {
        writer.write(objects.Count);
        for (int i = 0; i < objects.Count; i++) {
            objects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader) {
        int count = reader.ReadInt();
        print(count);
        for (int i = 0; i < count; i++)
        {
            print("creating object");
            PersistableObject o = Instantiate(prefab);
            o.Load(reader);
            objects.Add(o);
        }
    }
}
