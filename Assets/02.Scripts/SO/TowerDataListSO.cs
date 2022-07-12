using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerData : ItemData
{
    public int damage = 1;
    public float weight = 1;
    public ETowerType towerType = ETowerType.PassiveType;
}

[CreateAssetMenu(menuName = "SO/DataList/TowerDataList")]
public class TowerDataListSO : ScriptableObject
{
    public List<TowerData> towerDataList;

    public TowerData this[int idx]
    {
        get => towerDataList[idx];
        set => towerDataList[idx] = value;
    }

    public int Count => towerDataList.Count;

}
