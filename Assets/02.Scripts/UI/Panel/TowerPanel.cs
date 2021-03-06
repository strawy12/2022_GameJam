using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPanel : UpgradePanel
{
    [SerializeField] private TowerIcon _towerIcon;
    [SerializeField] private Image _backgroundImage;

    private TowerData _towerData;

    public override void Init(ItemData data)
    {
        _towerData = data as TowerData;

        if(DataManager.Inst.CurrentPlayer.EqualArrValue(_towerData.itemNum))
        {
            _towerIcon.isOn = true;
            _towerIcon.onValueChanged.AddListener(ClickTowerIcon);
        }

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
        _towerIcon.interactable = false;

        SetUpgradeButton();
    }
    public void UnLockUI()
    {
        _itemImage.color = Color.white;
        _backgroundImage.color = Color.white;
        _towerIcon.interactable = true;
        base.SetUI();
        
        _infoText.text = string.Format("Atk : {0:N2}", _towerData.damage);
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
            _upgradeBtn.interactable = true;

        }
    }

    public override void UpgradeItem()
    {
        _towerData.damage *= 1.1f;
    }

    public void ClickTowerIcon(bool isOn)
    {
        if(isOn)
        {
            UIManager.Inst.EquipTower(_towerData);
        }

        else
        {
            UIManager.Inst.EmptyInventoryPanel(_towerData.prefabName);
        }
    }

}
