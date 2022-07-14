using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSound : AudioPlayer
{
    [SerializeField] private AudioClip _throwClip,
                                       _hitClip,
                                       _smileClip;
                                       

    public void PlayThrowSound()
    {
        PlayClipWithVariablePitch(_throwClip);
    }


    public void PlayHitSound()
    {
        PlayClipWithVariablePitch(_hitClip);
    }

    public void SmileSound()
    {
        PlayClip(_smileClip);
    }
}
