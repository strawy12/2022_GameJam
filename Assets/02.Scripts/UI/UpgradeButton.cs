using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : Button
{
    [SerializeField] private Text _buttonText;
    public string text
    {
        get => _buttonText.text;
        set => _buttonText.text = value;
    }

}
