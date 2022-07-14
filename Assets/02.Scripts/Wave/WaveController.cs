using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WaveController : MonoBehaviour
{
    public List<WaveDataSO> waves;
    public UnityEvent OnStartWave;
    public UnityEvent OnEndWave;
    private bool _isWave = false;
    public bool IsWave
    {
        get
        {
            return _isWave;
        }
    }
    private int _waveLevel = 1; // 이거랑
    private int _waveIndex = 0; // 이거 가지고 있어야함
    private int _waveMonsterCount = 0;
    private int _randomIndex = 0;
    public int TotalWave
    {
        get
        {
            return _waveLevel * waves.Count + _waveIndex;
        }
    }
    public GameObject nextUIPanel;
    public UnityEvent OnFailedWave;
    private void Awake()
    {
        OnEndWave.AddListener(UIManager.Inst.GoUpgradeUI);
    }
    public void StartWave()
    {
        if (_isWave) return;
        nextUIPanel.SetActive(false);
        _isWave = true;
        float delay = UIManager.Inst.ShowRoundUI(TotalWave);

        StartCoroutine(StartWaveDelay(delay));
    }

    private IEnumerator StartWaveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        UIManager.Inst.GoGameScene();
        _waveMonsterCount = waves[_waveIndex].RemainMonsterCnt;
        OnStartWave?.Invoke();
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
                e.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
                e.SetEnemyStat(_waveLevel);
                yield return new WaitForSeconds(pattern.spawnDelay);
            }
        }
    }

    public void KillWaveMonster()
    {
        Debug.Log(_waveMonsterCount);
        _waveMonsterCount--;
        if (_waveMonsterCount <= 0)
        {
            EndWave();
        }
    }

    public void FailWave()
    {
        _isWave = false;
        if (TotalWave - 1 > 0)
        {
            if (_waveIndex - 1 < 0)
            {
                _waveLevel--;
                _waveIndex = waves.Count - 1;
            }
            else
            {
                _waveIndex--;
            }
        }
        OnFailedWave?.Invoke();
        OnEndWave?.Invoke();
        nextUIPanel.SetActive(true);
    }
    public void EndWave()
    {
        _isWave = false;
        OnEndWave?.Invoke();
        nextUIPanel.SetActive(true);
        _waveIndex++;
        if (_waveIndex > waves.Count - 1)
            _waveIndex = 0;
        _waveLevel++;
    }
}
