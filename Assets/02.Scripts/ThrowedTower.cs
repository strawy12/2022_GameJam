using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class ThrowedTower : MonoBehaviour
{
    [SerializeField] private float _reloadDelay;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _forceOffset;
    [SerializeField] private float _throwStartDelay;
    [SerializeField] private ThrowLine _throwLine = null;
    [SerializeField] private Transform _throwPos;
    [SerializeField] private Transform _armTransform;
    [SerializeField] private Collider2D _eventBoundsCol;

    private float _force;
    private Camera _mainCam;

    private Vector2 _throwDir;
    private Vector2 _startMousePos;

    private bool _isPressed = false;
    private bool _isLoad;
    private bool _canThrow;

    private Tower _currentTower;
    private Dictionary<string, Tower> _towerDict = new Dictionary<string, Tower>();

    private Animator _animator;

    private int _hashThrow = Animator.StringToHash("Throw");

    public UnityEvent OnThrowStart;
    private float _throwChargingDelay = 0f;

    private void Awake()
    {
        _mainCam = Define.MainCam;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GenerateTowers();
        EventManager<string>.StartListening(Constant.CLICK_TOWER_BUTTON, LoadTower);
    }

    private void Update()
    {
        if (!_isLoad) return;

        if (_isPressed)
        {
            _throwChargingDelay += Time.deltaTime;
            bool canThrow = true;
            Vector2 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.x >= transform.position.x)
            {
                canThrow = false;
            }

            Vector2 throwDir = mousePos - (Vector2)transform.position;
            throwDir.Normalize();

            float angle = Mathf.Atan2(-throwDir.y, -throwDir.x) * Mathf.Rad2Deg;

            if (angle < -45f || angle > 70f)
            {
                canThrow = false;
            }


            angle += -45f;

            _force = Vector2.Distance(_startMousePos, mousePos) * _forceOffset;
            _force = Mathf.Clamp(_force, 0f, _maxForce);

            if (_force < 20f)
            {
                canThrow = false;
            }

            _canThrow = canThrow;
            _throwDir = throwDir;

            _armTransform.rotation = Quaternion.Euler(0f, 0f, angle);

            _throwLine.DrawGuideLine(_currentTower.Rigid, _throwPos.position, -_throwDir * _force, 300);
        }


    }

    private void OnMouseDown()
    {
        // if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.IsPointerOverGameObject()) return;
        if (Define.IsPointerOverUIObject()) return;
        if (GameManager.Inst.gameState != GameManager.GameState.Game) return;
        if (!_isLoad) return;

        _animator.speed = 0;
        _isPressed = true;
        _canThrow = true;
        _currentTower.Rigid.isKinematic = true;
        _throwPos.position = Vector3.zero;
        _startMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (_throwChargingDelay < 0.1f) return;
        if (GameManager.Inst.gameState != GameManager.GameState.Game) return;

        if (!_isLoad) return;
        if (_isPressed == false) return;

        _isPressed = false;

        if (_canThrow == false)
        {
            GameManager.Inst.gameState = GameManager.GameState.Game;
            _armTransform.DORotate(Vector3.zero, 0.3f);
            _throwLine.ClearLine();
            return;
        }

        _isLoad = false;
        _throwChargingDelay = 0f;
        _animator.speed = 1;
        _animator.SetTrigger(_hashThrow);

        EventManager<string>.TriggerEvent(Constant.START_THROW_TOWER, _currentTower.Data.prefabName);

        StartCoroutine(StartThrowDelay());
    }

    private IEnumerator StartThrowDelay()
    {
        yield return new WaitForSeconds(_throwStartDelay);

        StartThrow();
    }

    private void StartThrow()
    {
        _currentTower.StartThrow();

        _currentTower.Collider.enabled = true;
        _currentTower.Rigid.isKinematic = false;
        _currentTower.Rigid.velocity = (-_throwDir * _force);
        _force = 0f;
        _throwLine.ClearLine();

        Release();
    }

    private void Release()
    {
        string key = _currentTower.Data.prefabName;
        _currentTower = null;
        _towerDict[key] = GenerateTower(key);

        _armTransform.DORotate(Vector3.zero, 0.3f);
    }

    public void ReleaseTower()
    {
        if (_currentTower != null)
            PoolManager.Instance.Push(_currentTower);

        _currentTower = null;
    }

    public void LoadTower(string key)
    {
        if (_currentTower != null && _currentTower.gameObject.activeSelf)
        {
            _currentTower.gameObject.SetActive(false);

            if (_currentTower.Data.prefabName.Equals(key))
            {
                _isLoad = false;
                return;
            }
        }

        _currentTower = GenerateTower(key);
        _currentTower.gameObject.SetActive(true);

        _isLoad = true;
    }

    public void UnLoadTower()
    {
        _currentTower.gameObject.SetActive(false);

    }

    private void GenerateTowers()
    {
        foreach (var data in DataManager.Inst.CurrentPlayer.towerDataList)
        {
            GenerateTower(data.prefabName);
        }
    }

    private Tower GenerateTower(string key)
    {
        Tower tower = null;

        if (_towerDict.TryGetValue(key, out tower))
        {
            if (tower == null)
            {
                tower = PoolManager.Instance.Pop(key) as Tower;
            }
            else
            {
                return tower;
            }
        }

        else
        {
            tower = PoolManager.Instance.Pop(key) as Tower;
            _towerDict.Add(key, tower);
        }

        tower.transform.SetParent(_throwPos);
        tower.transform.localPosition = Vector3.zero;
        tower.transform.localRotation = Quaternion.identity;
        tower.Reset();

        tower.gameObject.SetActive(false);

        return tower;
    }

    public void OnDestroy()
    {
        EventManager<string>.StopListening(Constant.CLICK_TOWER_BUTTON, LoadTower);
    }

    private void OnApplicationQuit()
    {
        EventManager<string>.StopListening(Constant.CLICK_TOWER_BUTTON, LoadTower);
    }
}
