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

    void Update()
    {
        // 터치아직안함
        if (Input.GetMouseButtonDown(0) && !GameManager.Inst.isGround)
        {
            UseSkill();
        }
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


}
