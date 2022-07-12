using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatPanel : UpgradePanel 
{
    public override void SetUI()
    {
        base.SetUI();

        PlayerStatData statData = _currentData as PlayerStatData;

        _infoText.text = $"{statData.statType.ToString()} : {statData.value.ToString()}";
    }
}
