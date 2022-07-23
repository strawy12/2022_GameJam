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

    private bool _isRefeat = false;
    private bool _isWave = false;
    public bool IsWave
    {
        get
        {
            return _isWave;
        }
    }
    public int WaveLevel
    {
        get => DataManager.Inst.CurrentPlayer.waveLevel;
        set => DataManager.Inst.CurrentPlayer.waveLevel = value;
    }

    public int WaveIndex
    {
        get => DataManager.Inst.CurrentPlayer.waveIndex;
        set => DataManager.Inst.CurrentPlayer.waveIndex = value;
    }

    private int _waveMonsterCount = 0;
    private int _randomIndex = 0;
    public int TotalWave
    {
        get
        {
            return WaveLevel * (WaveIndex + 1);
        }
    }
    public GameObject nextUIButton;
    public GameObject refeatRoundButton;
    public UnityEvent OnClearWave;
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

            pattern.count = Random.Range(1, 20);
            pattern.spawnDelay = Random.Range(0.5f, 1f);

            waveData.patterns.Add(pattern);
        }

        return waveData;
    }

    public void StartWave()
    {
        if (_isWave) return;
        nextUIButton.SetActive(false);
        refeatRoundButton.SetActive(false);
        _isWave = true;
        float delay = UIManager.Inst.ShowRoundUI(TotalWave);
        OnStartWave?.Invoke();

        if (Random.Range(0, 2) == 0)
        {
            _currentWave = waves[WaveIndex];
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
        StartCoroutine(SpawnMonsterCoroutine(WaveLevel));
    }

    private IEnumerator SpawnMonsterCoroutine(int level)
    {
        foreach (PatternData pattern in _currentWave.patterns)
        {
            for (int i = 0; i < pattern.count; i++)
            {
                _randomIndex = Random.Range(0, pattern.enemies.Count);
                Enemy e = PoolManager.Instance.Pop(pattern.enemies[_randomIndex].gameObject.name) as Enemy;
                e.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
                e.SetEnemyStat(level);
                OnFailedWave.AddListener(e.FailWaveDeath); //일단 이렇게 ㄱㄱ
                yield return new WaitForSeconds(pattern.spawnDelay);
            }
        }
    }

    public void KillWaveMonster(Enemy e)
    {
        OnFailedWave.RemoveListener(e.FailWaveDeath); //일단 이렇게 ㄱㄱ
        _waveMonsterCount--;
        if (_waveMonsterCount <= 0)
        {
            ClearWave();
        }
    }

    public void FailWave()
    {
        _isWave = false;
        GameManager.Inst.gameState = GameManager.GameState.UI;
        OnFailedWave?.Invoke();
        EndWave();
    }

    public void EndWave()
    {
        OnEndWave?.Invoke();
        nextUIButton.SetActive(true);
        refeatRoundButton.SetActive(false);
    }

    public void ClearWave()
    {
        _isWave = false;
        EndWave();
        OnClearWave?.Invoke();
        if(_isRefeat)
        {
            _isRefeat = false;
            return;
        }
        WaveIndex++;
        if (WaveIndex > waves.Count - 1)
            WaveIndex = 0;
        WaveLevel++;
    }
    private IEnumerator RefeatWaveCoroutine(float delay, int level, int index)
    {
        yield return new WaitForSeconds(delay);
        UIManager.Inst.GoGameScene();
        _waveMonsterCount = waves[index].RemainMonsterCnt;
        StartCoroutine(SpawnMonsterCoroutine(level));
    }

    public void RefeatWave()
    {
        if (_isWave) return;

        nextUIButton.SetActive(false);
        refeatRoundButton.SetActive(false);
        _isWave = true;
        _isRefeat = true;
        int level = WaveLevel;
        int index = WaveIndex;
        float delay = UIManager.Inst.ShowRoundUI(TotalWave - 1);
        OnStartWave?.Invoke();

        if(index - 1 < 0)
        {
            level--;
            index = waves.Count - 1;
        }
        else
        {
            index = index- 1;
        }
        StartCoroutine(RefeatWaveCoroutine(delay, level, index));

    }
}
