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

        SAVE_PATH = Application.dataPath + "/Save";

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();
        //SoundVolumeUpdate();
    }

    private void LoadFromJson()
    {
        //if (File.Exists(SAVE_PATH + SAVE_FILE))
        //{
        //    string stringJson = File.ReadAllText(SAVE_PATH + SAVE_FILE);
        //    _player = JsonUtility.FromJson<PlayerData>(stringJson);
        //}
        //else
        //{
            _player = new PlayerData(_defaultSound);

            for(int i = 1; i <= _towerDataList.Count; i++)
            { 
                _player.towerStatDataList.Add(new TowerStat(i));
            }
        //}
        SaveToJson();
    }
    public void SaveToJson()
    {
        string stringJson = JsonUtility.ToJson(_player, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILE, stringJson, System.Text.Encoding.UTF8);
    }
    public void DataReset()
    {
        _player = new PlayerData(_defaultSound);
        SaveToJson();
        Application.Quit();
    }


    public TowerData FindTowerData(int towerNum)
    {
        TowerData data = _towerDataList.towerDataList.Find(tower => tower.towerNum == towerNum);
        TowerStat stat = _player.towerStatDataList.Find(tower => tower.towerNum == towerNum);

        data.towerStat = stat;
        return data;
    }
}   