using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonSpawner : MonoBehaviour
{
    public float m_TimebetweenSpawn;
    public float m_SpawnDistance;

    public Nucleon[] m_NucleonPrefab;

    float m_TimeSinceLastSpawn;

    // Update is called once per frame
    void Update()
    {
        m_TimeSinceLastSpawn += Time.deltaTime;

        if(m_TimeSinceLastSpawn >= m_TimebetweenSpawn)
        {
            m_TimeSinceLastSpawn -= m_TimebetweenSpawn;
            SpawnNucleon();
        }
    }

    private void SpawnNucleon()
    {
        Nucleon l_Prefab = m_NucleonPrefab[UnityEngine.Random.Range(0, m_NucleonPrefab.Length)];
        Nucleon l_Spawn = Instantiate<Nucleon>(l_Prefab);
        l_Spawn.transform.localPosition = UnityEngine.Random.onUnitSphere * m_SpawnDistance;
    }
}
