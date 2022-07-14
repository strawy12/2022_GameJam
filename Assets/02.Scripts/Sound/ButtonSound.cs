using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : AudioPlayer
{
    [SerializeField] private AudioClip _startGame,
                                       _normalButton,
                                       _upgrageButton,
                                       _toggleButton;

    public void ButtonClick()
    {
        PlayClip(_normalButton);
    }

    public void UpgrageButton()
    {
        PlayClip(_upgrageButton);
    } 
    public void StartButton()
    {
        PlayClip(_startGame);
    }
    public void ToggleButton()
    {
        PlayClip(_toggleButton);
    }
}
