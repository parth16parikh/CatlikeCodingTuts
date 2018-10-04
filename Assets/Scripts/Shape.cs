using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject {

    int shapeID = int.MinValue;

    public int ShapeID {
        get{
            return shapeID;
        }
        set{
            if(shapeID == int.MinValue && value != int.MinValue) {
                shapeID = value;    
            }
            else
            {
                Debug.LogError("Not allowed to change shapeId.");
            }
        }
    }

    public int MaterialID
    {
        get;
        private set;

    }

    private MeshRenderer meshRenderer;
    Color color;

    static int colorPropertyID = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock materialPropertyBlock;

    public void SetMaterial(Material material, int materialID) {
        meshRenderer.material = material;
        MaterialID = materialID;
    }

    public void SetColor(Color color) {
        this.color = color;

        if(materialPropertyBlock == null) {
            materialPropertyBlock = new MaterialPropertyBlock();
        }
        materialPropertyBlock.SetColor(colorPropertyID,color);
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.write(color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
    }
}
