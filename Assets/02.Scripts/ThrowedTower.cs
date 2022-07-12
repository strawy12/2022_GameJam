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
    [SerializeField] private Tower _towerPref;
    [SerializeField] private ThrowLine _throwLine = null;

    private float _force;
    private Camera _mainCam;

    private Vector2 _throwDir;
    private bool _isPressed = false;
    private Vector2 _startMousePos;

    private Tower _currentTower;
    private bool _isReloading;

    private List<Tower> _nextTowerList = new List<Tower>();

    private void Awake()
    {
        _mainCam = Define.MainCam;

        _isReloading = false;
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

            _throwDir = mousePos - (Vector2)transform.position;
            _throwDir.Normalize();

            _force = Vector2.Distance(_startMousePos, mousePos) * _forceOffset;

            _force = Mathf.Clamp(_force, 0f, _maxForce);

            float angle = Mathf.Atan2(-_throwDir.y, -_throwDir.x) * Mathf.Rad2Deg - 90f;
            _currentTower.ChangeAngle(angle);
            _throwLine.DrawGuideLine(_currentTower.Rigid, transform.position, -_throwDir * _force, 300);
        }


    }

    private void OnMouseDown()
    {
        if (_isReloading) return;

        _isPressed = true;
        _currentTower.Rigid.isKinematic = true;
        _startMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (_isReloading) return;

        EventManager<Transform>.TriggerEvent(Constant.START_THROW_TOWER, _currentTower.transform);
        _currentTower.StartThrow();
        _isPressed = false;
        _currentTower.Collider.enabled = true;
        _currentTower.Rigid.isKinematic = false;
        _currentTower.Rigid.velocity = (-_throwDir * _force);
        _force = 0f;
        _currentTower = null;
        _isReloading = true;
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
        _currentTower.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        _currentTower.Init();
        _currentTower.Collider.enabled = false;
        _currentTower.Rigid.isKinematic = true;
        _currentTower.gameObject.SetActive(true);
    }

    private Tower GetTower()
    {
        if (_nextTowerList.Count <= 4)
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
                _nextTowerList.Add(tower);
            }
        }

    }
}
