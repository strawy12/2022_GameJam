using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public RectTransform rectTransform { get; private set; }

    private RectTransform _currentContent;
    private ScrollRect _scrollRect;

    private bool _isOn;

    private void Awake()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void ChangeContent(RectTransform rectTransform)
    {
        _scrollRect.content.gameObject.SetActive(false);
        _scrollRect.content = rectTransform;
        _scrollRect.content.gameObject.SetActive(true);
    }


}
