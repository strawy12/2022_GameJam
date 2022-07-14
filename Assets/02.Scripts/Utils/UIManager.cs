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
    [SerializeField] private RoundPanel _failRoundPanel;

    [SerializeField] private GameObject _quitPanel;
    [SerializeField] private GameObject _settingPanel;

    private List<NextTowerPanel> _nextTowerPanelList = new List<NextTowerPanel>();
    private List<UpgradePanel> _upgradePanelList = new List<UpgradePanel>();

    [SerializeField] private List<StatInfoPanel> _statInfoPanelList;

    private GameManager.GameState _beforeState = GameManager.GameState.Game;
    private void Start()
    {
        _goldPanel.SetText();
    }

    public void OnUI()
    {
        _beforeState = GameManager.Inst.gameState;
        GameManager.Inst.gameState = GameManager.GameState.UI;
        Time.timeScale = 0f;
    }

    public void OffUI()
    {
        GameManager.GameState state = GameManager.Inst.gameState;
        GameManager.Inst.gameState = _beforeState;
        _beforeState = state;
        Time.timeScale = 1f;

    }

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

        OpenUpgradeUI();
    }

    public void GoGameScene()
    {
        GameManager.Inst.gameState = GameManager.GameState.Game;

        CloseUpgradeUI();
    }
    public void OpenUpgradeUI()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_nextTowerUI.DOFade(0f, 0.75f));
        seq.Append(_upgradeUI.OpenUI());
    }

    public void CloseUpgradeUI()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(_upgradeUI.CloseUI());
        seq.Join(_nextTowerUI.DOFade(1f, 0.75f));
    }

    public void EffectOnOff()
    {
            audioMixer.SetFloat("Effect", !_effectToggle.isOn ? 0f : -80f);
    }
    public void BgmOnOff()
    {
        audioMixer.SetFloat("Music", !_bgmToggle.isOn? 0f : -80f);
    }

    public float ShowRoundUI(int round)
    {
      return  _roundPanel.ShowNowRoundUI(round);
    }

    public float ShowFailRoundUI(int round)
    {
        return _failRoundPanel.ShowNowRoundUI(round);
    }
    public void SetStatInfoPanel(PlayerStatData.EPlayerStat type)
    {
        _statInfoPanelList.Find(x => x.StatType == type).SetInfo();
    }

    public void OnClickBackBtn()
    {
        if(_quitPanel.activeSelf)
        {
            _quitPanel.SetActive(false);
            return;
        }

        if(_settingPanel.activeSelf)
        {
            _settingPanel.SetActive(false);
            OffUI();
        }

        else
        {
            _settingPanel.SetActive(true);
            OnUI();
        }
    }
}
