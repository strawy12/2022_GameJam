using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WaveController : MonoBehaviour
{
    public List<WaveDataSO> waves;
    public UnityEvent OnStartWave;
    public UnityEvent OnEndWave;

    [SerializeField] private List<Enemy> _enemyList;
    [SerializeField] private WaveDataSO _randomWaveBase;

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
            return _waveLevel * (_waveIndex + 1);
        }
    }
    public GameObject nextUIPanel;
    public UnityEvent OnFailedWave;

    private WaveDataSO _currentWave;
    private void Awake()
    {
        OnEndWave.AddListener(UIManager.Inst.GoUpgradeUI);
    }

    private WaveDataSO GenerateRandomWave()
    {
        WaveDataSO waveData = _randomWaveBase;

        int patternCnt = Random.Range(1, 5);
        waveData.patterns = new List<PatternData>();
        for (int i = 0; i < patternCnt; i++)
        {
            PatternData pattern = new PatternData();
            pattern.enemies = new List<Enemy>();

            for (int j = 0; j < _enemyList.Count; j++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    pattern.enemies.Add(_enemyList[j]);
                }
            }
            if (pattern.enemies.Count == 0) continue;

            pattern.count = Random.Range(0, 50);
            pattern.spawnDelay = Random.Range(0.5f, 1f);

            waveData.patterns.Add(pattern);
        }

        return waveData;
    }

    public void StartWave()
    {
        if (_isWave) return;
        nextUIPanel.SetActive(false);
        _isWave = true;
        float delay = UIManager.Inst.ShowRoundUI(TotalWave);
        OnStartWave?.Invoke();

        if (Random.Range(1, 2) == 0)
        {
            _currentWave = waves[_waveIndex];
        }
        else
        {
            _currentWave = GenerateRandomWave();
        }
        StartCoroutine(StartWaveDelay(delay));
    }

    private IEnumerator StartWaveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        UIManager.Inst.GoGameScene();
        _waveMonsterCount = _currentWave.RemainMonsterCnt;
        StartCoroutine(SpawnMonsterCoroutine());
    }

    private IEnumerator SpawnMonsterCoroutine()
    {
        foreach (PatternData pattern in _currentWave.patterns)
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
            ClearWave();
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
        EndWave();
    }

    public void EndWave()
    {
        OnEndWave?.Invoke();
        nextUIPanel.SetActive(true);
    }
    public void ClearWave()
    {
        _isWave = false;
        EndWave();
        _waveIndex++;
        if (_waveIndex > waves.Count - 1)
            _waveIndex = 0;
        _waveLevel++;
    }

}
