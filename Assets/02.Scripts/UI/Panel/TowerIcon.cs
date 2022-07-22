using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TowerIcon : Toggle
{
    [SerializeField] private Image _towerImage;

    protected override void Awake()
    {
        base.Awake();
        onValueChanged.AddListener(TowerImageChange); 
    }

    private void TowerImageChange(bool isOn)
    {
        _towerImage.color = isOn ? Color.gray : Color.white;
    }
}
