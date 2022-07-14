using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class BarUI : MonoBehaviour
{
    [SerializeField]
    protected Transform _fillBar;

    public float FillAmout
    {
        get => _fillBar.transform.localScale.x;
    }

    public  void SetGuageUI(float value)
    {
        value = Mathf.Clamp(value, 0f, 1f);

        _fillBar.transform.DOScaleX(value, 0.3f);
    }

}
