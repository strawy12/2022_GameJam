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
        _upgradeBtn.text = $"강화\n{_currentData.needGold} Gold";
        
        SetUpgradeButton();
    }

    public virtual void UpgradeItem()
    {
        if (DataManager.Inst.CurrentPlayer.gold <= _currentData.needGold) return;

        // 수정 필요할거같은 코드
        _currentData.itemLevel++;

        GameManager.Inst.SetGold(-_currentData.needGold);

        _currentData.needGold = (int)(_currentData.needGold * 1.5f);

        SetUI();
        SetUpgradeButton();
    }

    public virtual void SetUpgradeButton()
    {
        _upgradeBtn.interactable = DataManager.Inst.CurrentPlayer.gold >= _currentData.needGold;
    }
}
