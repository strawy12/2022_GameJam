using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{

    private List<TowerPanel> _towerPanelList = new List<TowerPanel>();

    public void AddTowerPanel(TowerPanel panel)
    {
        _towerPanelList.Add(panel);
    }

    public void AddGoldEvent()
    {
        _towerPanelList.ForEach(panel => panel.SetUpgradeButton());
    }

}
