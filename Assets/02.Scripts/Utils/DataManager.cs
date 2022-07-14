using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] private float _defaultSound = 0.5f;
    [SerializeField] private PlayerData _player;
    [SerializeField] private TowerDataListSO _towerDataList;
    [SerializeField] private PlayerStatDataSO _statDataList;

    public PlayerData CurrentPlayer => _player;

    private string SAVE_PATH = "";
    private const string SAVE_FILE = "/SaveFile.Json";

    private void Awake()
    {
        DataManager[] dmanagers = FindObjectsOfType<DataManager>();
        if (dmanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        SAVE_PATH = Application.persistentDataPath + "/Save";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();
        //SoundVolumeUpdate();
    }

    private void LoadFromJson()
    {
        if (File.Exists(SAVE_PATH + SAVE_FILE))
        {
            string stringJson = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            _player = JsonUtility.FromJson<PlayerData>(stringJson);

            if(_player.versionCheck == false)
            {
                _player = new PlayerData();

                InitTowerDataList();
                InitStatDataList();
                SaveToJson();
                return;
            }

            SetTowerItemSprite();
            SetStatItemSprite();
        }
        else
        {
            _player = new PlayerData();

            InitTowerDataList();
            InitStatDataList();

        }
        SaveToJson();
    }
    public void SaveToJson()
    {
        string stringJson = JsonUtility.ToJson(_player, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, stringJson, System.Text.Encoding.UTF8);
    }
    public void DataReset()
    {
        _player = new PlayerData();

        InitTowerDataList();
        InitStatDataList();

        SaveToJson();
        Application.Quit();
    }

    private void SetStatItemSprite()
    {
        foreach (var data in _player.statDataList)
        {
            if (data.itemSprite == null)
            {
                data.itemSprite = FindStatData(data.itemNum).itemSprite;
            }
        }
    }

    private void SetTowerItemSprite()
    {
        foreach (var data in _player.towerDataList)
        {
            if (data.itemSprite == null)
            {
                data.itemSprite = FindTowerData(data.itemNum).itemSprite;
            }
        }
    }

    private void InitTowerDataList()
    {
        for (int i = 0; i < _towerDataList.Count; i++)
        {
            _player.towerDataList.Add(new TowerData(_towerDataList[i]));
        }
    }

    private void InitStatDataList()
    {
        for (int i = 0; i < _statDataList.Count; i++)
        {
            _player.statDataList.Add(new PlayerStatData(_statDataList[i]));
        }
    }

    public TowerData FindTowerData(int towerNum)
    {
        TowerData data = _towerDataList.towerDataList.Find(tower => tower.itemNum == towerNum);
        return data;
    }

    public PlayerStatData FindStatData(int statNum)
    {
        PlayerStatData data = _statDataList.statDataList.Find(stat => stat.itemNum == statNum);
        return data;
    }
}   