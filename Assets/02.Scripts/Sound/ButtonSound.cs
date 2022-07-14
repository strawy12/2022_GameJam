using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : AudioPlayer
{
    [SerializeField] private AudioClip _startGame,
                                       _normalButton,
                                       _upgrageButton;

    public void ButtonClick()
    {
        PlayClipWithVariablePitch(_normalButton);
    }

    public void UpgrageButton()
    {
        PlayClip(_upgrageButton);
    } 
    public void StartButton()
    {
        PlayClip(_startGame);
    }
}
