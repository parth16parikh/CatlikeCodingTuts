using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject {

    [SerializeField]
    Shape[] prefabs;

    [SerializeField]
    Material[] materials;

    public Shape Get(int shapeId = 0, int materialID = 0) {
        Shape instance = Instantiate(prefabs[shapeId]);
        instance.ShapeID = shapeId;
        instance.SetMaterial(materials[shapeId],materialID);
        return instance;
    }
	

    public Shape GetRandom() {
        return Get(
                Random.Range(0, prefabs.Length) , 
                Random.Range(0, materials.Length));
    }
}
