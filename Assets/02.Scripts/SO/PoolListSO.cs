using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class PoolingPair
{
    public PoolableMono prefab;
    public int poolCnt;
}
[CreateAssetMenu(menuName = "SO/System/PoolList")]
public class PoolListSO : ScriptableObject
{
    public List<PoolingPair> list;
}
