using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    Shape[] m_Prefabs;
    [SerializeField]
    Material[] m_Materials;

    [SerializeField]
    private bool m_Recycle;

    List<Shape>[] m_Pools;

    Scene m_PoolScene;
    string m_SceneName = "Shape Factory";

    void CreatePools()
    {
        m_Pools = new List<Shape>[m_Prefabs.Length];

        for (int i = 0; i < m_Prefabs.Length; i++)
        {
            m_Pools[i] = new List<Shape>();
        }

        if(Application.isEditor)
        {
            m_PoolScene = SceneManager.GetSceneByName(m_SceneName);
            if (m_PoolScene.isLoaded)
            {
                GameObject[] l_RootObjects = m_PoolScene.GetRootGameObjects();
                for(int i = 0; i < l_RootObjects.Length; i++)
                {
                    Shape poolShape = l_RootObjects[i].GetComponent<Shape>();
                    if (!l_RootObjects[i].activeSelf)
                    {
                        m_Pools[poolShape.p_ShapeID].Add(poolShape);
                    }
                }
                return;
            }
        }

        m_PoolScene = SceneManager.CreateScene(m_SceneName);
    }

    public void Reclaim(Shape shapeToRecycle)
    {
        if(m_Recycle)
        {
            if(m_Pools == null)
            {
                CreatePools();
            }

            m_Pools[shapeToRecycle.p_ShapeID].Add(shapeToRecycle);
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        
        }
    }
    public Shape Get(int l_ShapeID = 0, int l_MaterialID = 0)
    {
        Shape instance;
        if (m_Recycle)
        {
            if (m_Pools == null)
            {
                CreatePools();
            }
            List<Shape> l_Pool = m_Pools[l_ShapeID];
            int lastIndex = l_Pool.Count - 1;
            if(lastIndex >= 0)
            {
                instance = l_Pool[lastIndex];
                l_Pool.RemoveAt(lastIndex);
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(m_Prefabs[l_ShapeID]);
                instance.p_ShapeID = l_ShapeID;
            }

            SceneManager.MoveGameObjectToScene(instance.gameObject, m_PoolScene);
        }
        else
        {
            instance = Instantiate(m_Prefabs[l_ShapeID]);
            instance.p_ShapeID = l_ShapeID;
            
        }
        instance.SetMaterial(m_Materials[l_MaterialID], l_MaterialID);
        return instance;
    }

    public Shape GetRandom()
    {
        return Get(Random.Range(0, m_Prefabs.Length),
                   Random.Range(0, m_Materials.Length));
    }
}
