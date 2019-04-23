using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{

    public Mesh[] m_Meshs;
    public Material m_Material;
    public int m_MaxDepth;
    public float m_ChildScale;
    public float m_SpawnProbablity;
    public float m_MaxRotation;
    public float m_MaxTwist;

    private int m_Depth;
    private float m_Rotation;

    private static Vector3[] m_ChildDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back,
    };

    private static Quaternion[] m_ChildRotations =
    {
        Quaternion.identity,
        Quaternion.Euler(0.0f,0.0f,-90.0f),
        Quaternion.Euler(0.0f,0.0f,90.0f),
        Quaternion.Euler(90.0f,0.0f,0.0f),
        Quaternion.Euler(-90.0f,0.0f,0.0f),
    };

    private Material[,] m_Materials;

    // Start is called before the first frame update
    void Start()
    {
        if(m_Materials == null)
        {
            InitializedMaterials();
        }
        m_Rotation = Random.Range(-m_MaxRotation, m_MaxRotation);
        transform.Rotate(Random.Range(-m_MaxTwist,m_MaxTwist),0.0f,0.0f);
        gameObject.AddComponent<MeshFilter>().mesh = m_Meshs[Random.Range(0,m_Meshs.Length)];
        gameObject.AddComponent<MeshRenderer>().material = m_Material;
        GetComponent<MeshRenderer>().material = m_Materials[m_Depth, Random.Range(0,2)];
        if(m_Depth < m_MaxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private void Update()
    {
        transform.Rotate(0.0f, m_Rotation * Time.deltaTime, 0.0f);
    }

    void InitializedMaterials()
    {
        m_Materials = new Material[m_MaxDepth + 1,2];

        for (int i = 0; i <= m_MaxDepth; i++)
        {
            float t = (float)i / ((float)m_MaxDepth - 1.0f);
            t *= t;
            m_Materials[i,0] = new Material(m_Material);
            m_Materials[i,0].color = Color.Lerp(Color.white, Color.yellow, t);
            m_Materials[i, 1] = new Material(m_Material);
            m_Materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        m_Materials[m_MaxDepth,0].color = Color.magenta;
        m_Materials[m_MaxDepth,1].color = Color.red;
    }

    IEnumerator CreateChildren()
    {
        for(int i = 0; i < m_ChildDirections.Length; i++)
        {
            if(Random.value < m_SpawnProbablity)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
        }
    }

    private void Initialize(Fractal parent, int childIndex)
    {
        m_Meshs = parent.m_Meshs;
        m_Materials = parent.m_Materials;
        m_MaxDepth = parent.m_MaxDepth;
        m_Depth = parent.m_Depth + 1;
        m_ChildScale = parent.m_ChildScale;
        m_MaxRotation = parent.m_MaxRotation;
        m_MaxTwist = parent.m_MaxTwist;
        m_SpawnProbablity = parent.m_SpawnProbablity;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * m_ChildScale;
        transform.localPosition = m_ChildDirections[childIndex] * (0.5f + 0.5f * m_ChildScale);
        transform.localRotation = m_ChildRotations[childIndex];
    }
}
