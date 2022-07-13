using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PyramidTower : Tower
{
    [SerializeField] private Animator deletePyramidTower;
    [SerializeField] private PyramidSwamp _swampObj;

    public UnityEvent OnBrokeTower;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        EventManager.StartListening(Constant.CLICK_SCREEN, UseSkill);
    }

    IEnumerator SpawnSwamp()
    {
        PyramidSwamp swamp = PoolManager.Instance.Pop("PyramidSwamp") as PyramidSwamp;
        swamp.transform.position = new Vector2(transform.position.x, -7);
        swamp.gameObject.SetActive(true);

        deletePyramidTower.Play("PyramidDust");
        OnBrokeTower?.Invoke();

        yield return new WaitForSeconds(.7f);
        Debug.Log("피라미드타워삭제");

        DestroyTower();
    }

    public override void UseSkill()
    {
        if (gameObject.activeSelf == false) return;
        StartCoroutine(SpawnSwamp());
    }

    public override void DestroyTower()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);
        base.DestroyTower();
    }

    public override void Reset()
    {
        base.Reset();
        EventManager.StartListening(Constant.CLICK_SCREEN, UseSkill);

    }
    private void OnDestroy()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);

    }
}
