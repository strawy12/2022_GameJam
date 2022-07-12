using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    private  int _hashThrow = Animator.StringToHash("Throw");

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


            _throwDir = mousePos - (Vector2)_throwPos.position;
            _throwDir.Normalize();

            _force = Vector2.Distance(_startMousePos, mousePos) * _forceOffset;

            _force = Mathf.Clamp(_force, 0f, _maxForce);

            float angle = Mathf.Atan2(-_throwDir.y, -_throwDir.x) * Mathf.Rad2Deg - 45f;

            _armTransform.rotation = Quaternion.Euler(0f, 0f, angle);

            _throwLine.DrawGuideLine(_currentTower.Rigid, _throwPos.position, -_throwDir * _force, 300);
        }


    }

    private void OnMouseDown()
    {
        if (_isReloading) return;

        _animator.speed = 0;
        _isPressed = true;
        _currentTower.Rigid.isKinematic = true;
        _throwPos.position = Vector3.zero;
        _startMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (_isReloading) return;

        _isPressed = false;

        _isReloading = true;

        _animator.speed = 1;
        _animator.SetTrigger(_hashThrow);
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
        _currentTower = null;
        _throwLine.ClearLine();

        StartCoroutine(Release());
    }


    private IEnumerator Release()
    {
        yield return new WaitForSeconds(_reloadDelay);

        // 풀매니저 사용
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
            SetTowerList();

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

    private void SetTowerList()
    {
        List<string> towerList = new List<string>();
        foreach (var data in DataManager.Inst.CurrentPlayer.towerDataList)
        {
            if (data.isLock)
                continue;

            string towerName = data.prefabName;
            towerList.Add(towerName);
        }

        while(_nextTowerList.Count < 7)
        {
            var rnd = new System.Random();
            towerList = towerList.OrderBy(item => rnd.Next()).ToList();

            for(int i = 0; i < towerList.Count; i++)
            {
                Tower tower = PoolManager.Instance.Pop(towerList[i]) as Tower;
                tower.gameObject.SetActive(false);
                _nextTowerList.Add(tower);
            }
        }

    }
}
