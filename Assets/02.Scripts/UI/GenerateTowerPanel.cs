using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTowerPanel : MonoBehaviour
{
    [SerializeField] private TowerPanel _towerPanelTemp;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        TowerPanel panel;
        TowerData towerData;
        int cnt = DataManager.Inst.CurrentPlayer.towerStatDataList.Count;
        for(int i = 1; i <= cnt; i++)
        {
            panel = Instantiate(_towerPanelTemp, _towerPanelTemp.transform.parent);
            panel.name = _towerPanelTemp.name;
            towerData = DataManager.Inst.FindTowerData(i);
            panel.Init(towerData);
        }

    }
}
