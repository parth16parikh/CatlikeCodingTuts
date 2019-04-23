using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawnZone : SpawnZone
{

    [SerializeField]
    bool m_SurfaceOnly;

    public override Vector3 m_SpawnZone
    {
        get
        {
            return transform.TransformPoint(m_SurfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, 1.0f);
    }
}
