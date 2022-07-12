using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : AudioPlayer
{
    [SerializeField] private AudioClip _hitClip = null, _deathClip = null, _attackSound = null;

    public void PlayHitSound()
    {
        PlayClipWithVariablePitch(_hitClip);
    }
    public void PlayDeathSound()
    {
        PlayClip(_deathClip);
    }
    public void PlayAttackSound()
    {
        PlayClip(_attackSound);
    }
}
