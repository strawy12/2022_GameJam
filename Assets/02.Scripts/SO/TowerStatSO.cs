using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETowerType
{
    ActiveType,
    PassiveType,
    FixingType
}

[CreateAssetMenu(menuName = "SO/Tower/Stats")]

public class TowerStatSO : ScriptableObject
{

    public PoolableMono poolPrefab;
    public int damage = 1;
    [Range(1, 5)]public float weight = 1;

    public ETowerType towerType = ETowerType.PassiveType;
}
