using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Text _touchText;
    
    void Update()
    {
        _touchText.color = 
            new Color(_touchText.color.r, _touchText.color.g, _touchText.color.b, Mathf.Sin(Time.time) * 0.5f + 0.5f);
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Main");
    }
}
