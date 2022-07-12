using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    private List<NextTowerPanel> _nextTowerPanelList = new List<NextTowerPanel>();
    private List<UpgradePanel> _upgradePanelList = new List<UpgradePanel>();


    public void AddUpgradePanel(UpgradePanel panel)
    {
        _upgradePanelList.Add(panel);
    }
    
    public void AddNextTowerPanel(NextTowerPanel panel)
    {
        _nextTowerPanelList.Add(panel);
    }

    public void SetNextTowerPanels(Sprite[] sprites)
    {
        for(int i = 0; i < sprites.Length; i++)
        {
            return;
            _nextTowerPanelList[i].SetSprite(sprites[i]);
        }
    }

    public void SetNextTowerPanel(int idx, Sprite towerSprite)
    {
        _nextTowerPanelList[idx].SetSprite(towerSprite);
    }

    public void GoldEvent()
    {
        _upgradePanelList.ForEach(panel => panel.SetUI());
    }

}
