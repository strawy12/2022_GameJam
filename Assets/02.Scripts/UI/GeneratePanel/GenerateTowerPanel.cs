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
        int cnt = DataManager.Inst.CurrentPlayer.towerDataList.Count;

        for(int i = 0; i < cnt; i++)
        {
            panel = Instantiate(_towerPanelTemp, _towerPanelTemp.transform.parent);
            panel.name = _towerPanelTemp.name;
            towerData = DataManager.Inst.CurrentPlayer.towerDataList[i];
            panel.Init(towerData);
        }

    }
}
