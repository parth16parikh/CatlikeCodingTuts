using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject {

    public ShapeFactory shapeFactory;
    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;
    public PersistentStorage storage;

    List<Shape> shapes;

    const int saveVersion = 1;

    private void Start()
    {
        shapes = new List<Shape>();
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
            storage.Save(this,saveVersion);
        }

        if(Input.GetKeyDown(loadKey)) {
            BeginNewGame();
            storage.Load(this);
        }
    }

    void CreateObject() {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * 5.0f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1.0f);
        instance.SetColor(Random.ColorHSV(hueMin: 0f, hueMax: 1f,
            saturationMin: 0.5f, saturationMax: 1f,
            valueMin: 0.25f, valueMax: 1f,
            alphaMin: 1f, alphaMax: 1f));
        shapes.Add(instance);
    }

    void BeginNewGame() {
        for (int i = 0; i < shapes.Count; i++) {
            Destroy(shapes[i].gameObject);
        }
        shapes.Clear();
    }

    public override void Save(GameDataWriter writer) {
        writer.write(shapes.Count);
        for (int i = 0; i < shapes.Count; i++) {
            writer.write(shapes[i].ShapeID);
            writer.write(shapes[i].MaterialID);
            shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader) {
        int version = -reader.ReadInt();
        if(version > saveVersion) {
            Debug.LogError("Unsupported future save version " + version);
            return;
        }

        int count = version <= 0 ? -version : reader.ReadInt();
        print(count);
        for (int i = 0; i < count; i++)
        {
            print("creating object");
            int shapeID = version <= 0 ? 0 : reader.ReadInt();
            int materialID = version <= 0 ? 0 : reader.ReadInt();
            Shape instance = shapeFactory.Get(shapeID,materialID);
            instance.Load(reader);
            shapes.Add(instance);
        }
    }
}
