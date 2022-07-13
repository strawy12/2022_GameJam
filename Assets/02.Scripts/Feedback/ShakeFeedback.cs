using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeFeedback : Feedback
{
    [SerializeField] private GameObject _objectToShake;
    [SerializeField] private float _duration = 0.2f, _strength = 1f, _randomness = 90f;
    [SerializeField] private int _vibrato = 10;
    [SerializeField] private bool _snapping = false, _fadeOut = true;
    // snapping : 격자단위로 떨리게 하는거, fadeout : 진동후 원래 자리로 돌아가나
    public override void CompletePrevFeedBack()
    {
        _objectToShake.transform.DOComplete();
        // 모든 트윈을 즉시 완료, 완료 트윈 갯수 반환
    }

    public override void CreateFeedBack()
    {
        CompletePrevFeedBack();
        _objectToShake.transform.DOShakePosition(_duration, _strength, _vibrato, _randomness, _snapping, _fadeOut);
    }
}
