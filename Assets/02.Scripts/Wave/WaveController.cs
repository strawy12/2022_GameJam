using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WaveController : MonoBehaviour
{
    public List<WaveDataSO> waves;
    public UnityEvent OnEndWave;
    private bool _isWave = false;
    public bool IsWave
    {
        get 
        {
            return _isWave;
        }
    }
    private int _waveLevel = 1;
    private int _waveIndex = 0;
    private int _waveMonsterCount = 0;
    private int _randomIndex = 0;
    public GameObject nextUIPanel;

    public void StartWave()
    {
        if (_isWave) return;
        nextUIPanel.SetActive(false);
        _isWave = true;
        _waveMonsterCount = waves[_waveIndex].RemainMonsterCnt;
        StartCoroutine(SpawnMonsterCoroutine());
    }
    private IEnumerator SpawnMonsterCoroutine()
    {

        foreach (PatternData pattern in waves[_waveIndex].patterns)
        {
            for (int i = 0; i < pattern.count; i++)
            {
                _randomIndex = Random.Range(0, pattern.enemies.Count);
                Enemy e = PoolManager.Instance.Pop(pattern.enemies[_randomIndex].gameObject.name) as Enemy;
                e.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0,0,0));
                yield return new WaitForSeconds(pattern.spawnDelay);
            }
        }
    }

    public void KillWaveMonster()
    {
        _waveMonsterCount--;
        if(_waveMonsterCount <= 0)
        {
            EndWave();
        }
    }
    public void EndWave()
    {
        _isWave = false;
        OnEndWave?.Invoke();
        nextUIPanel.SetActive(true);
        _waveIndex++;
        if (_waveIndex > waves.Count)
        {
            _waveIndex = 0;
            _waveLevel++;
        }
    }
}
