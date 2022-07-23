using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private InventoryPanel _inventoryPanelTemp;
    [SerializeField] private int _generateCnt = 5;

    private List<InventoryPanel> _inventoryPanelList;

    private void Start()
    {
        _inventoryPanelList = new List<InventoryPanel>();
        Generate();
    }

    private void Generate()
    {
        InventoryPanel panel;


        for (int i = 0; i < _generateCnt; i++)
        {
            panel = Instantiate(_inventoryPanelTemp, _inventoryPanelTemp.transform.parent);
            panel.name = _inventoryPanelTemp.name;
            _inventoryPanelList.Add(panel);

            panel.Init();

            int towerID = DataManager.Inst.CurrentPlayer.equipTowerIdArr[i];

            if (towerID == -1)
            {
                panel.EmptyTowerData();
            }
            else
            {
                TowerData data = DataManager.Inst.CurrentPlayer.GetTowerData(towerID);
                panel.SetTowerData(data);
            }

            panel.gameObject.SetActive(true);
        }
    }

    public void EquipTower(TowerData data)
    {
        foreach (var panel in _inventoryPanelList)
        {
            if (panel.IsEmpty)
            {
                panel.SetTowerData(data);
                return;
            }
        }
    }

    public void EmptyPanel(string key)
    {
        for (int i = 0; i < _inventoryPanelList.Count; i++)
        {
            if (_inventoryPanelList[i].ContainKey(key))
            { 
                _inventoryPanelList[i].EmptyTowerData();
                return;
            }
        }
    }
}
