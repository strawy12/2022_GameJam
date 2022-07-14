using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradePanel : MonoBehaviour
{
    [SerializeField] protected Image _itemImage;

    [SerializeField] protected Text _nameText;
    [SerializeField] protected Text _infoText;
    [SerializeField] protected Text _explanationText;
    [SerializeField] protected UpgradeButton _upgradeBtn;

    protected ItemData _currentData;

    public virtual void Init(ItemData data)
    {
        _currentData = data;

        SetUI();

        UIManager.Inst.AddUpgradePanel(this);
        gameObject.SetActive(true);
    }

    public virtual void SetUI()
    {
        _itemImage.sprite = _currentData.itemSprite;
        _nameText.text = $"Lv.{_currentData.itemLevel.ToString()} {_currentData.itemName}";
        _explanationText.text = _currentData.itemExplanation;
        _upgradeBtn.text = $"강화\n<size=16>{(long)_currentData.needGold}</size>";
        
        SetUpgradeButton();
    }

    public void ClickUpgradeBtn()
    {
        if (DataManager.Inst.CurrentPlayer.gold < _currentData.needGold) return;

        // 수정 필요할거같은 코드
        _currentData.itemLevel++;

        GameManager.Inst.SetGold(-(int)_currentData.needGold);

        _currentData.needGold = (_currentData.needGold * 1.5f);

        UpgradeItem();
        SetUI();
        SetUpgradeButton();

    }

    public abstract void UpgradeItem();

    public virtual void SetUpgradeButton()
    {
        _upgradeBtn.interactable = DataManager.Inst.CurrentPlayer.gold >= _currentData.needGold;
    }
}
