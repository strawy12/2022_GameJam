using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public RectTransform rectTransform { get; private set; }

    private CanvasGroup _canvasGroup;
    private RectTransform _currentContent;
    private ScrollRect _scrollRect;

    private bool _isOpen;

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

    public void ClickOpenButton()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            OpenUI();
        }

        else
        {
            CloseUI();
        }
    }

    public Tween OpenUI()
    {
        _isOpen = true;
        return rectTransform.DOAnchorPosX(0f, 0.5f);
    }
    public Tween CloseUI()
    {
        _isOpen = false;
        return rectTransform.DOAnchorPosX(-rectTransform.rect.width, 0.5f);
    }

}
