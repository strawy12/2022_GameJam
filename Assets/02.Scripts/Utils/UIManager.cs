using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    private List<NextTowerPanel> _nextTowerPanelList = new List<NextTowerPanel>();
    private List<UpgradePanel> _upgradePanelList = new List<UpgradePanel>();
    [SerializeField] private UpgradeUI _upgradeUI;

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
        int cnt = Mathf.Max(_nextTowerPanelList.Count, sprites.Length);
        for (int i = 0; i < cnt; i++)
        {
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

    public void GoUpgradeUI()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(Define.MainCam.transform.DOMoveX(-2.1f, 0.75f));
        seq.Append(_upgradeUI.rectTransform.DOAnchorPosX(0f, 0.5f));
    }
    public void GoGameScene()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(_upgradeUI.rectTransform.DOAnchorPosX(-_upgradeUI.rectTransform.rect.width, 0.5f));
        seq.Append(Define.MainCam.transform.DOMoveX(13f, 0.75f));
    }
}
