using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField]
    SpawnZone m_SpawnZone;

    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.m_SpawnZoneOfLevel = m_SpawnZone;
    }

}
