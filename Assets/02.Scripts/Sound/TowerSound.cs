using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSound : AudioPlayer
{
    [SerializeField]
    private AudioClip _brokeClip, skillClip, _attackClip;

    public void PlayBrokeSound()
    {
        PlayClipWithVariablePitch(_brokeClip);
    }

    public void PlaySkillSound()
    {
        PlayClipWithVariablePitch(skillClip);
    }
    public void PlayAttackSound()
    {
        PlayClipWithVariablePitch(_attackClip);
    }
}
