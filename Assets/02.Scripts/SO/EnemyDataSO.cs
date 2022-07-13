using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SO/Enemy/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public PoolableMono poolPrefab;
    public int maxHealth = 3;
    public float knockbackRegist = 1f; // 넉백 저항력
    public int dropCoin = 2;
    //공격 관련 데이터
    public int damage = 1;
    public float attackDelay = 1;
    public float hitRange = 0;
}
