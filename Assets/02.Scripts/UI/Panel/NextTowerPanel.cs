using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTowerPanel : MonoBehaviour
{
    private Image _towerImage;

    private int _currentIndex;

    private void Awake()
    {
        _towerImage = transform.Find("TowerImage").GetComponent<Image>();
        _currentIndex = transform.GetSiblingIndex() - 1;

    }


    internal void SetSprite(Sprite towerSprite)
    {
        _towerImage.sprite = towerSprite;
    }
}
