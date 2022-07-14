using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSound : AudioPlayer
{
    [SerializeField] private AudioClip _gameStartSound;

    public void PlayStartSound()
    {
        PlayClip(_gameStartSound);
    }
}
