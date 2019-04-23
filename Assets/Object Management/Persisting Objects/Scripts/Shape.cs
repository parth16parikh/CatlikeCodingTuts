using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistingObject
{
    int m_ShapeId = int.MinValue;
    public int m_MaterialId { get; private set; }
    Color m_Color;
    MeshRenderer m_MeshRenderer;
    static int m_ColorPropertyId = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock m_SharedMaterialPropertyBlock;

    public int p_ShapeID
    {
        get {
            return m_ShapeId;
        }
        set {
            if(m_ShapeId == int.MinValue && value != int.MinValue)
            {
                m_ShapeId = value;
            }
            else
            {
                Debug.LogError("Not allowed to change shapeId.");
            }
        }
    }

    private void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material l_Material, int l_MaterialId)
    {
        m_MeshRenderer.material = l_Material;
        m_MaterialId = l_MaterialId;
    }

    public void SetColor(Color l_Color)
    {
        m_Color = l_Color;
        if(m_SharedMaterialPropertyBlock == null)
        {
            m_SharedMaterialPropertyBlock = new MaterialPropertyBlock();
        }
        m_SharedMaterialPropertyBlock.SetColor(m_ColorPropertyId, l_Color);
        m_MeshRenderer.SetPropertyBlock(m_SharedMaterialPropertyBlock);
    }

    public override void Save(GameDataWriter write)
    {
        base.Save(write);
        write.Write(m_Color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.m_version > 0 ? reader.ReadColor() : Color.white);
    }
}
