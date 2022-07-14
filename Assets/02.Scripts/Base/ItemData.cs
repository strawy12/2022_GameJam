using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int itemNum;
    public string itemName;
    public Sprite itemSprite;

    public int itemLevel;

    public double needGold;

    [TextArea]
    public string itemExplanation;

    public ItemData(ItemData data)
    {
        itemNum = data.itemNum;
        itemName = data.itemName;
        itemSprite = data.itemSprite;
        itemLevel = data.itemLevel;
        needGold = data.needGold;
        itemExplanation = data.itemExplanation;
    }
}
