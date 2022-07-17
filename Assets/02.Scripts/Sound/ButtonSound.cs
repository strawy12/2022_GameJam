using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : AudioPlayer
{
    [SerializeField] private AudioClip _startGame,
                                       _normalButton,
                                       _upgrageSuccessButton,
                                       _upgrageFailedButton,
                                       _toggleButton,
                                       _openButton;

    public void ButtonClick()
    {
        PlayClip(_normalButton);
    }

    public void OpenButton()
    {
        PlayClip(_openButton);
    }

    public void UpgrageSuccessButton()
    {
        PlayClip(_upgrageSuccessButton);
    }
    public void UpgrageFailedButton()
    {
        PlayClip(_upgrageFailedButton);
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
