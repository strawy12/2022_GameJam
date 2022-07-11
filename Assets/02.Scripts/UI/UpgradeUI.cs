using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private RectTransform _currentContent;
    private ScrollRect _scrollRect;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    public void ChangeContent(RectTransform rectTransform)
    {
        _scrollRect.content.gameObject.SetActive(false);
        _scrollRect.content = rectTransform;
        _scrollRect.content.gameObject.SetActive(true);
    }

}
