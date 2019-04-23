using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : PersistingObject
{
    [SerializeField]
    private ShapeFactory m_ShapeFactory;
    [SerializeField]
    private KeyCode m_CreateKey = KeyCode.C;
    [SerializeField]
    private KeyCode m_NewGameKey = KeyCode.N;
    [SerializeField]
    private KeyCode m_SaveGameKey = KeyCode.S;
    [SerializeField]
    private KeyCode m_LoadGameKey = KeyCode.L;
    [SerializeField]
    private KeyCode m_DestroyGameKey = KeyCode.X;
    [SerializeField]
    private PersistentStorage m_Storage;
    [SerializeField]
    private int m_LevelCount = 2;

    public SpawnZone m_SpawnZoneOfLevel {
        get; set;
    }

    public float CreationSpeed
    {
        get; set;
    }

    public float DestructionSpeed
    {
        get; set;
    }

    public static Game Instance { get; private set;}

    private float m_CreationProgress = 0.0f;
    private float m_DestructionProgress = 0.0f;

    List<Shape> m_Shapes;
    string m_SaveFile;

    const int m_SaveVersion = 2;

    private bool enable = true;
    private int loadedLevelBuildIndex = 0;

    private void OnEnable()
    {
        Instance = this;
    }

    private void Start()
    {
        m_Shapes = new List<Shape>();

        if(Application.isEditor)
        {
            for(int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if(loadedScene.name.Contains("Level "))
                {
                    SceneManager.SetActiveScene(loadedScene);
                    loadedLevelBuildIndex = loadedScene.buildIndex;
                }
            }
        }
    }

    IEnumerator LoadLevel(int levelBuildIndex)
    {
        enable = false;

        if(loadedLevelBuildIndex > 0)
        {
            yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
        }

        yield return SceneManager.LoadSceneAsync(
                levelBuildIndex, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(
                SceneManager.GetSceneByBuildIndex(levelBuildIndex)
            );

        loadedLevelBuildIndex = levelBuildIndex;

        enable = true;
    }

    // Update is called once per frame
    void Update()
    {
        m_CreationProgress += Time.deltaTime * CreationSpeed;

        while (m_CreationProgress >= 1.0f)
        {
            m_CreationProgress -= 1.0f;
            CreateShape();
        }

        m_DestructionProgress += Time.deltaTime * DestructionSpeed;

        while (m_DestructionProgress >= 1.0f)
        {
            m_DestructionProgress -= 1.0f;
            DestroyShape();
        }

        if (Input.GetKeyDown(m_NewGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(m_SaveGameKey))
        {
            m_Storage.Save(this, m_SaveVersion);
        }
        else if (Input.GetKeyDown(m_LoadGameKey))
        {
            BeginNewGame();
            m_Storage.Load(this);
        }
        else
        {
            for(int i = 1; i <= m_LevelCount; i++)
            {
                if(Input.GetKeyDown(KeyCode.Alpha0+i))
                {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }

    }

    private void DestroyShape()
    {
        if (m_Shapes.Count > 0)
        {
            int l_Index = Random.Range(min: 0, m_Shapes.Count);
            m_ShapeFactory.Reclaim(m_Shapes[l_Index]);
            int l_LastIndex = m_Shapes.Count - 1;
            m_Shapes[l_Index] = m_Shapes[l_LastIndex];
            m_Shapes.RemoveAt(l_LastIndex);
        }
    }

    void CreateShape()
    {
        Shape l_Shape = m_ShapeFactory.GetRandom();
        Transform t = l_Shape.transform;
        t.localPosition = m_SpawnZoneOfLevel.m_SpawnZone;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        l_Shape.SetColor(Random.ColorHSV(
            hueMin: 0f, hueMax: 1f,
            saturationMin: 0.5f, saturationMax: 1f,
            valueMin: 0.25f, valueMax: 1f,
            alphaMin: 1f, alphaMax: 1f
        ));
        m_Shapes.Add(l_Shape);
    }

    void BeginNewGame()
    {
        for (int i = 0; i < m_Shapes.Count; i++)
        {
            m_ShapeFactory.Reclaim(m_Shapes[i]);
        }

        m_Shapes.Clear();
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(m_Shapes.Count);
        writer.Write(loadedLevelBuildIndex);
        for (int i = 0; i < m_Shapes.Count; i++)
        {
            writer.Write(m_Shapes[i].p_ShapeID);
            writer.Write(m_Shapes[i].m_MaterialId);
            m_Shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.m_version;
        int count = reader.ReadInt();
        StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
        for (int i = 0; i < count; i++)
        {
            int shapeID = version > 0 ? reader.ReadInt() : 0;
            int materialID = version > 0 ? reader.ReadInt() : 0;
            Shape o = m_ShapeFactory.Get(shapeID, materialID);
            o.Load(reader);
            m_Shapes.Add(o);
        }
    }
}
