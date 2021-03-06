using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETowerType
{
    ActiveType,
    PassiveType,
    FixingType
}

[System.Serializable]
public class TowerData : ItemData
{
    [Header("Ÿ?? ????")]
    public string prefabName;

    public float damage;
    public float cooltime;
    public float knockbackPower;

    public bool isLock;
    public ETowerType towerType;
        
    public TowerData(TowerData data) : base(data)
    {
        prefabName = data.prefabName;
        damage = data.damage;
        cooltime = data.cooltime;
        isLock = data.isLock;
        towerType = data.towerType;
        knockbackPower = data.knockbackPower;
    }
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
