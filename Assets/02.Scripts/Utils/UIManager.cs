using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private UpgradeUI _upgradeUI;
    [SerializeField] private GoldPanel _goldPanel;
    [SerializeField] private CanvasGroup _nextTowerUI;

    [SerializeField] private Toggle _effectToggle, _bgmToggle;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private RoundPanel _roundPanel;

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
        GameManager.Inst.gameState = GameManager.GameState.UI;

        Sequence seq = DOTween.Sequence();
        seq.Append(GameManager.Inst.MainCameraMove.MoveCameraPos(new Vector3(-2.1f,0,-10f), 0.75f));
        seq.Join(_nextTowerUI.DOFade(0f, 0.75f));
        seq.Append(_upgradeUI.OpenUI());
    }
    public void GoGameScene()
    {
        GameManager.Inst.gameState = GameManager.GameState.Game;
        Sequence seq = DOTween.Sequence();

        seq.Append(_upgradeUI.CloseUI());
        seq.Append(GameManager.Inst.MainCameraMove.MoveCameraPos(new Vector3(13f, 0, -10f), 0.75f));
        seq.Join(_nextTowerUI.DOFade(1f, 0.75f));
    }

    public void EffectOnOff()
    {
            audioMixer.SetFloat("Effect", !_effectToggle.isOn ? -20f : -80f);
    }
    public void BgmOnOff()
    {
        audioMixer.SetFloat("Music", !_bgmToggle.isOn? -20f : -80f);
    }

    public float ShowRoundUI(int round)
    {
      return  _roundPanel.ShowNowRoundUI(round);
    }
}
