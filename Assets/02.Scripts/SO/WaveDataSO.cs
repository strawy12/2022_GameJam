using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class PatternData
{
    public List<Enemy> enemies;
    public int count;
    public float spawnDelay = 0.5f;

}

[CreateAssetMenu(menuName = "SO/Wave/WaveDataSO")]
public class WaveDataSO : ScriptableObject
{
    public List<PatternData> patterns;

    public int RemainMonsterCnt
    {
        get
        {
            int _count = 0;
            foreach(PatternData pattern in patterns)
            {
                _count += pattern.count;
            }
            return _count;
        }
    } 
}
