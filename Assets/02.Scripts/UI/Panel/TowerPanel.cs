using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPanel : UpgradePanel
{
    [SerializeField] private Image _backgroundImage;

    private TowerData _towerData;

    public override void Init(ItemData data)
    {
        _towerData = data as TowerData;

        base.Init(data);

    }

    public override void SetUI()
    {
        if (_towerData.isLock)
        {
            TowerData beforeData = DataManager.Inst.CurrentPlayer.towerDataList[Mathf.Max(0,_currentData.itemNum - 1)];
            if (beforeData.isLock == false && beforeData.itemLevel >= 10)
            {
                _towerData.isLock = false;
                UnLockUI();
            }

            else
            {
                LockUI();
            }


        }

        else
        {
            UnLockUI();
        }
    }

    public void LockUI()
    {
        _nameText.text = "????";
        _explanationText.text = string.Format("조건: {0}의 Lv. 10 이상 달성", DataManager.Inst.CurrentPlayer.towerDataList[_towerData.itemNum - 1].itemName);
        _itemImage.color = Color.black;
        _infoText.text = "";
        _backgroundImage.color = Color.gray;

        SetUpgradeButton();
    }
    public void UnLockUI()
    {
        _itemImage.color = Color.white;
        _backgroundImage.color = Color.white;
        base.SetUI();
        _infoText.text = $"Atk : {_towerData.damage.ToString()} \n Weight : {_towerData.weight.ToString()}";
    }

    public override void SetUpgradeButton()
    {
        TowerData data = _currentData as TowerData;
        if (data.isLock)
        {


            _upgradeBtn.interactable = false;
            _upgradeBtn.text = "X";
        }
        else
        {
            base.SetUpgradeButton();
        }
    }

    public override void UpgradeItem()
    {
    }
}
