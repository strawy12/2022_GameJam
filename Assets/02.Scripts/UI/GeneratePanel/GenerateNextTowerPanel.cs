using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNextTowerPanel : MonoBehaviour
{
    [SerializeField] private NextTowerPanel _nextTowerPanelTemp;
    [SerializeField] private int _generateCnt = 4;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        NextTowerPanel panel;

        for (int i = 0; i < _generateCnt; i++)
        {
            panel = Instantiate(_nextTowerPanelTemp, _nextTowerPanelTemp.transform.parent);
            panel.name = _nextTowerPanelTemp.name;
            panel.gameObject.SetActive(true);
        }

    }
}
