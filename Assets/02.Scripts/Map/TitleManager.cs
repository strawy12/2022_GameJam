using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Text _startText;
    [SerializeField] private Text _touchText;

    private bool _isClick;
    void Update()
    {
        _startText.color =  
            new Color(_startText.color.r, _startText.color.g, _startText.color.b, Mathf.Sin(Time.time) * 0.5f + 0.5f);
        
    }

    public void StartButton()
    {
        if (_isClick) return;
        _isClick = true;

        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene("Main");
    }
}
