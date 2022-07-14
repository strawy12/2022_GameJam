using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
