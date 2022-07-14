using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeUI : MonoBehaviour
{
    public RectTransform rectTransform { get; private set; }

    private CanvasGroup _canvasGroup;
    private RectTransform _currentContent;
    private ScrollRect _scrollRect;

    [SerializeField] private Button _openButton;
    [SerializeField] private Sprite _openSprite, _closeSprite;
    public UnityEvent OnOpen;
    public UnityEvent OnClose;

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
        _openButton.image.sprite = _openSprite;
        OnOpen?.Invoke();
        return rectTransform.DOAnchorPosX(0f, 0.5f);
    }
    public Tween CloseUI()
    {
        _isOpen = false;
        _openButton.image.sprite = _closeSprite;
        OnClose?.Invoke();
        return rectTransform.DOAnchorPosX(-rectTransform.rect.width, 0.5f);
    }

}
