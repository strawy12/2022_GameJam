using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipPanel : MonoBehaviour
{
    [SerializeField] private Transform _arrowTrm;
    [SerializeField] private Text _tipText;

    [SerializeField] private float _textSpeed;

    private CanvasGroup _canvasGroup;

    private bool _writingTip;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(string tipTextData, float arrowAngle, Vector2 pos)
    {
        if (_writingTip)
        {
            _tipText.DOKill(true);
        }
        _writingTip = true;
        _canvasGroup.alpha = 0f;
        transform.position = pos;

        _canvasGroup.alpha = 1f;
        _tipText.text = "";
        _tipText.DOText(tipTextData, tipTextData.Length * _textSpeed).OnComplete(() =>
        _writingTip = false);
    }

    public void ClickNextBtn()
    {

    }
}
