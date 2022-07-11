using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPanel : MonoBehaviour
{
    [SerializeField] private Image _towerImage;

    [SerializeField] private Text _nameText;
    [SerializeField] private Text _infoText;
    [SerializeField] private Text _explanationText;

    private TowerData _currentData;

    public void Init(TowerData data)
    {
        _currentData = data;

        _towerImage.sprite = _currentData.towerSprite;
        _nameText.text = $"Lv.{_currentData.towerStat.level.ToString()}{_currentData.towerName}";
        _infoText.text = _currentData.towerSkillExplanation;
        _explanationText.text = $"Atk : {_currentData.towerStat.damage.ToString()} \n Weight : {_currentData.weight.ToString()}";

        gameObject.SetActive(true);
    }
}
