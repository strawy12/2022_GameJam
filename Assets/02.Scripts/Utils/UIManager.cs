using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private UpgradeUI _upgradeUI;
    [SerializeField] private GoldPanel _goldPanel;

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
        int cnt = Mathf.Max(_nextTowerPanelList.Count, sprites.Length);
        for (int i = 0; i < cnt; i++)
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
        _goldPanel.SetText();
    }

    public void GoUpgradeUI()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(GameManager.Inst.MainCameraMove.MoveCameraPos(new Vector3(-2.1f,0,-10f), 0.75f));
        seq.Append(_upgradeUI.rectTransform.DOAnchorPosX(0f, 0.5f));
    }
    public void GoGameScene()
    {
        GameManager.Inst.gameState = GameManager.GameState.Game;
        Sequence seq = DOTween.Sequence();

        seq.Append(_upgradeUI.rectTransform.DOAnchorPosX(-_upgradeUI.rectTransform.rect.width, 0.5f));
        seq.Append(GameManager.Inst.MainCameraMove.MoveCameraPos(new Vector3(13f, 0, -10f), 0.75f));
    }
}
