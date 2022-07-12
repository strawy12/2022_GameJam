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
    public bool isLock;

    public int needGold;

    [TextArea]
    public string itemExplanation;
}
