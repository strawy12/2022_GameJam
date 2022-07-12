using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant;

public class PlayerStatPanel : UpgradePanel 
{
    public override void SetUI()
    {
        base.SetUI();

        PlayerStatData statData = _currentData as PlayerStatData;

        string infoText = "";

        switch (statData.statType)
        {
            case PlayerStatData.EPlayerStat.DamageFactor:
                infoText = $"Atk Factor : {statData.value.ToString()}";
                break;
            case PlayerStatData.EPlayerStat.Critical:
                infoText = $"Critical Percent : {statData.value.ToString()}%";
                break;
            case PlayerStatData.EPlayerStat.MaxHp:
                infoText = $"Max Hp : {statData.value.ToString()}";
                break;
        }

        _infoText.text = infoText;
    }

    public override void UpgradeItem()
    {
    }
}
