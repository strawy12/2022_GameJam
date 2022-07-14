using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatInfoPanel : MonoBehaviour
{
    [SerializeField] private PlayerStatData.EPlayerStat _statType;
    [SerializeField] private Text _infoText;

    public PlayerStatData.EPlayerStat StatType => _statType;

    public void SetInfo()
    {
        float value = DataManager.Inst.CurrentPlayer.GetStat(_statType);
        switch (_statType)
        {
            case PlayerStatData.EPlayerStat.DamageFactor:
                _infoText.text  = string.Format("x {0:N2}", value);
                break;
            case PlayerStatData.EPlayerStat.Critical:
                if(value > 100)
                {
                    _infoText.text = string.Format("{0:N2}% / 100%", (150f + (value - 100f)));

                }
                else
                {
                    _infoText.text = string.Format("150% / {0:N2}% ", (150f + (value - 100f)));
                }
                break;
            case PlayerStatData.EPlayerStat.MaxHp:
                _infoText.text = string.Format("{0}", ((int)value).ToString());

                break;
        }
    }
}