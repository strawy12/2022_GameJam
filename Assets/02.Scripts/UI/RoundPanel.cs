using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoundPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Text _roundText;
    [SerializeField] private Transform _roundImageTrm;

    public float ShowNowRoundUI(int round)
    {
        _roundImageTrm.localScale = Vector3.zero; 
        _roundText.text = $"Round {round.ToString()}";
        float duration = 0f;
        Sequence seq = DOTween.Sequence();
        seq.Append(_canvasGroup.DOFade(1f, 0.5f)); duration += 0.5f;
        seq.Append(_roundImageTrm.DOScale(Vector3.one, 0.75f)); duration += 0.75f;
        seq.AppendInterval(1f); duration += 1f;
        seq.Append(_canvasGroup.DOFade(0f, 0.3f)); duration += 0.3f;

        return duration;
    }
}
