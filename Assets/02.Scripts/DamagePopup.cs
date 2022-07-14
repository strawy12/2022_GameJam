using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamagePopup : PoolableMono
{
    private TextMeshPro _textMesh;
    private Color _defaultColor;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _defaultColor = _textMesh.color;
    }

    public void Setup(int damageAmount, Vector3 pos, bool isCritical)
    {
        transform.position = pos;
        _textMesh.SetText(damageAmount.ToString());

        if (isCritical)
        {
            _textMesh.color = Color.red;
            _textMesh.fontSize = 40f;
        }
        else
        {
            _textMesh.color = _defaultColor;
            _textMesh.fontSize = 30f;
        }
        _textMesh.DOFade(1f, 0f);
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y + 0.7f, 1f));
        seq.Join(_textMesh.DOFade(0f, 1f));
        seq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }

    public override void Reset()
    {
        _textMesh.color = Color.white;
        _textMesh.fontSize = 7f;
    }

}
