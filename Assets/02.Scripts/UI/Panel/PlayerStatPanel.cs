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
                infoText = string.Format("Atk Factor : x {0:N2}", statData.value);
                break;
            case PlayerStatData.EPlayerStat.Critical:

                
                infoText = string.Format("Critical Percent : {0:N2}%", statData.value);
                break;
            case PlayerStatData.EPlayerStat.MaxHp:
                infoText = string.Format("Max Hp : {0}", ((int)statData.value).ToString());
                break;
        }

        _infoText.text = infoText;
    }

    public override void UpgradeItem()
    {
        PlayerStatData statData = _currentData as PlayerStatData;
        switch (statData.statType)
        {
            case PlayerStatData.EPlayerStat.DamageFactor:
                statData.value *= 1.1f;
                break;
            case PlayerStatData.EPlayerStat.Critical:
                if(statData.value == 0f)
                {
                    statData.value = 1f;
                }
                statData.value *= 1.1f;
                break;
            case PlayerStatData.EPlayerStat.MaxHp:
                statData.value *= 1.1f;
                break;
        }

        UIManager.Inst.SetStatInfoPanel(statData.statType);
    }
}
