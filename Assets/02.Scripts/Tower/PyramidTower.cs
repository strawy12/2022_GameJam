using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidTower : Tower
{
    [SerializeField] private PyramidSwamp _swampObj;


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

        yield return new WaitForSeconds(.7f);
        DestroyTower();
    }

    public override void UseSkill()
    {
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
