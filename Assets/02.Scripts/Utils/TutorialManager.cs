using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public class TipData
{
    [TextArea]
    public string tipTextData;

    [Space]
    public Vector3 targetingPos;
    public Vector3 position;
    public UnityEvent action;

    public float Angle
    {
        get
        {
            Vector2 dir = targetingPos - position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            return angle;
        }
    }

}


public class TutorialManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _tipUI;
    [SerializeField] private TipPanel _tipPanel;

    [SerializeField] private List<TipData> _tipDataList;

    public void StartTutorial()
    {
        TipData tipData = _tipDataList[0];
        _tipDataList.RemoveAt(0);


        _tipUI.DOFade(1f, 0.5f).OnComplete(() =>
        {
            _tipUI.blocksRaycasts = true;
            _tipUI.interactable = true;
            _tipPanel.Setup(tipData.tipTextData, tipData.Angle, tipData.position);
        });

        tipData.action?.Invoke();
        Debug.Log(_tipPanel);
        Debug.Log(tipData);

    }
}
