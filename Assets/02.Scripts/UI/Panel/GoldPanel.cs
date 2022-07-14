using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPanel : MonoBehaviour
{
    [SerializeField] private Text _goldText;

    public void SetText()
    {
        long gold = DataManager.Inst.CurrentPlayer.gold;
        string text;
        if (gold > 9999999999)
        {
            text = "Gold : 9999999999+";
        }

        else
        {
            text = $"Gold : {gold.ToString()}";
        }

        _goldText.text = text;
    }
}
