using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStatPanel : MonoBehaviour
{
    [SerializeField] private PlayerStatPanel _statPanelTemp;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        PlayerStatPanel panel;
        PlayerStatData statData;
        int cnt = DataManager.Inst.CurrentPlayer.statDataList.Count;

        for (int i = 0; i < cnt; i++)
        {
            panel = Instantiate(_statPanelTemp, _statPanelTemp.transform.parent);
            panel.name = _statPanelTemp.name;
            statData = DataManager.Inst.CurrentPlayer.statDataList[i];
            panel.Init(statData);
        }

    }
}
