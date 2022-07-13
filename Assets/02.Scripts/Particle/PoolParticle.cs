using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolParticle : PoolableMono
{
    private ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    public void OnEnableParticle(Vector2 pos)
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
}
