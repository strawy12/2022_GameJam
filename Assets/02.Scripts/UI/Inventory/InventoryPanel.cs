using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventoryPanel : MonoBehaviour, IPointerClickHandler
{
    private TowerData _currentTowerData;

    private Image _towerImage;
    private Image _cooltimeImage;

    public UnityEvent OnClick;

    /// <summary>
    /// [CoolTime Timer]
    /// If this var's value is not 0,
    /// Play this action => _coolTimer -= Time.deltaTime;
    /// </summary>
    private float _coolTimer;
    private bool _interactable;
    private int _currentIndex;

    public int Index => _currentIndex;

    public bool IsEmpty => _currentTowerData == null;


    public void Init()
    {
        _towerImage    ??= transform.Find("TowerImage").GetComponent<Image>();
        _cooltimeImage ??= transform.Find("CooltimeImage").GetComponent<Image>();

        _currentIndex = transform.GetSiblingIndex() - 1;

        _coolTimer = 0f;

        EmptyTowerData();

        EventManager<string>.StartListening(Constant.START_THROW_TOWER, UseTower);

    }

    public void SetTowerData(TowerData data)
    {
        if (data != null)
        {
            _currentTowerData = data;
            _towerImage.enabled = true;
            _cooltimeImage.enabled = true;
            _interactable = true;

            _towerImage.sprite = _currentTowerData.itemSprite;
            SetCoolTimeImage();
        }
    }
    public void EmptyTowerData()
    {
        _currentTowerData = null;
        _towerImage.enabled = false;
        _cooltimeImage.enabled = false;
        _interactable = false;
    }

    public void SetCoolTimeImage()
    {
        float value = _coolTimer / _currentTowerData.cooltime;
        _cooltimeImage.fillAmount = value;
    }

    public void UseTower(string key)
    {
        if (_currentTowerData.prefabName.Equals(key) == false) return;

        #region Debuging
        if (_interactable == false)
        {
            Debug.LogError("This Action is abnormal. Check this logic");
            return;
        }
        #endregion

        _interactable = false;
        _coolTimer = _currentTowerData.cooltime;
        StartCoroutine(CoolTimerCoroutine());
    }

    private IEnumerator CoolTimerCoroutine()
    {
        WaitForEndOfFrame waitEndFrame = new WaitForEndOfFrame();
        while(_coolTimer >= 0f && !_interactable)
        {
            _coolTimer -= Time.deltaTime;
            SetCoolTimeImage();
            yield return waitEndFrame;
        }

        _interactable = true;
    }

    public bool ContainKey(string key)
    {
        if (_currentTowerData == null)
            return false;

        return _currentTowerData.prefabName.Equals(key);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_interactable) return;

        EventManager<string>.TriggerEvent(Constant.CLICK_TOWER_BUTTON, _currentTowerData.prefabName);
    }

    private void OnDestroy()
    {
        EventManager<string>.StopListening(Constant.START_THROW_TOWER, UseTower);
    }

    private void OnApplicationQuit()
    {
        EventManager<string>.StopListening(Constant.START_THROW_TOWER, UseTower);
    }
}
