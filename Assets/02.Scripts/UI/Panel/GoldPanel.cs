using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPanel : MonoBehaviour
{
    [SerializeField] private Text _goldText;

    private void Awake()
    {
        _goldText = GetComponent<Text>();
    }
    public void SetText()
    {
        int gold = DataManager.Inst.CurrentPlayer.gold;
        string text;
        if (gold > 9999)
        {
            text = "Gold : 9999+G";
        }

        else
        {
            text = $"Gold : {gold.ToString()}G";
        }

        _goldText.text = text;
    }
}
