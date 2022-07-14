using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParticle : PoolableMono
{
    protected ParticleSystem _particle;

    protected void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    public virtual void OnEnableParticle(Vector2 pos)
    {
        transform.position = pos;
        _particle.Play();

        Invoke("PushParticle", _particle.main.duration);
    }
    public void PushParticle() 
    { 
        PoolManager.Instance.Push(this);
    }
    public override void Reset()
    {
        _particle.Stop();
    }
    public void SetParticleVelocity(Vector2 pos, Vector2 targetPos)
    {
        var vm = _particle.velocityOverLifetime;
        vm.x = pos.x - targetPos.x;
        OnEnableParticle(pos);
    }
}
