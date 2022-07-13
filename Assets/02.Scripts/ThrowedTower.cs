using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ThrowedTower : MonoBehaviour
{
    [SerializeField] private float _reloadDelay;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _forceOffset;
    [SerializeField] private float _throwStartDelay;
    [SerializeField] private ThrowLine _throwLine = null;
    [SerializeField] private Transform _throwPos;
    [SerializeField] private Transform _armTransform;


    private float _force;
    private Camera _mainCam;

    private Vector2 _throwDir;
    private bool _isPressed = false;
    private Vector2 _startMousePos;

    private Tower _currentTower;
    private bool _isReloading;

    private Animator _animator;

    private List<Tower> _nextTowerList = new List<Tower>();

    private int _hashThrow = Animator.StringToHash("Throw");

    public UnityEvent OnThrowStart;

    private void Awake()
    {
        _mainCam = Define.MainCam;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GenerateTower();
    }

    private void Update()
    {
        if (_isReloading) return;
        if (_isPressed)
        {
            Vector2 mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.x >= transform.position.x) return;

            Vector2 throwDir = mousePos - (Vector2)transform.position;
            throwDir.Normalize();

            float angle = Mathf.Atan2(-throwDir.y, -throwDir.x) * Mathf.Rad2Deg;

            if (angle < -45f || angle > 70f) return;

            angle += -45f;

            _force = Vector2.Distance(_startMousePos, mousePos) * _forceOffset;
            _force = Mathf.Clamp(_force, 0f, _maxForce);
            _throwDir = throwDir;

            _armTransform.rotation = Quaternion.Euler(0f, 0f, angle);

            _throwLine.DrawGuideLine(_currentTower.Rigid, _throwPos.position, -_throwDir * _force, 300);
        }


    }

    private void OnMouseDown()
    {
        if (GameManager.Inst.gameState != GameManager.GameState.Game) return;
        if (_isReloading) return;

        GameManager.Inst.gameState = GameManager.GameState.ThrowReady;
        GameManager.Inst.MainCameraMove.SetCameraPos(new Vector3(13f, 0f, -10f));

        _animator.speed = 0;
        _isPressed = true;
        _currentTower.Rigid.isKinematic = true;
        _throwPos.position = Vector3.zero;
        _startMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (GameManager.Inst.gameState != GameManager.GameState.Game && 
            GameManager.Inst.gameState != GameManager.GameState.ThrowReady) return;

        if (_isReloading) return;
        if (_isPressed == false) return;


        _isPressed = false;

        _isReloading = true;

        _animator.speed = 1;
        _animator.SetTrigger(_hashThrow);
        OnThrowStart?.Invoke();
        _currentTower.OnEndThrow += Release;
        GameManager.Inst.gameState = GameManager.GameState.Throwing;
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
        GameManager.Inst.StartFollow(_currentTower.transform);

        _currentTower.Collider.enabled = true;
        _currentTower.Rigid.isKinematic = false;
        _currentTower.Rigid.velocity = (-_throwDir * _force);
        _force = 0f;
        _throwLine.ClearLine();


    }

    private void Release()
    {
        _currentTower.OnEndThrow -= Release;
        _currentTower = null;

        StartCoroutine(ReleaseCoroutine());
    }


    private IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(_reloadDelay);

        GenerateTower();
        _isReloading = false;
    }

    private void GenerateTower()
    {
        
        _currentTower = GetTower();
        _currentTower.transform.SetParent(_throwPos);
        _currentTower.transform.localPosition = Vector3.zero;
        _currentTower.transform.localRotation = Quaternion.identity;

        _isReloading = false;

        _currentTower.Collider.enabled = false;
        _currentTower.Rigid.isKinematic = true;
        _currentTower.gameObject.SetActive(true);
    }

    private Tower GetTower()
    {
        if (_nextTowerList.Count <= 5)
            SetNextTowerList();

        Tower tower = _nextTowerList[0];
        _nextTowerList.RemoveAt(0);

        SettingNextTowerUI();

        return tower;
    }

    private void SettingNextTowerUI()
    {
        Sprite[] sprites = new Sprite[4];
        for (int i = 0; i < 4; i++)
        {
            sprites[i] = _nextTowerList[i].Data.itemSprite;
        }
        UIManager.Inst.SetNextTowerPanels(sprites);
    }

    public void ResetNextTowerList()
    {
        _nextTowerList.Clear();
        SetNextTowerList();
    }

    private void SetNextTowerList()
    {
        List<string> towerList = new List<string>();
        foreach (var data in DataManager.Inst.CurrentPlayer.towerDataList)
        {
            if (data.isLock)
                continue;
                
            string towerName = data.prefabName;
            towerList.Add(towerName);
        }

        while (_nextTowerList.Count < 7)
        {
            var rnd = new System.Random();
            towerList = towerList.OrderBy(item => rnd.Next()).ToList();

            for (int i = 0; i < towerList.Count; i++)
            {
                Tower tower = PoolManager.Instance.Pop(towerList[i]) as Tower;
                tower.gameObject.SetActive(false);
                _nextTowerList.Add(tower);
            }
        }
    }
}
