using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool isPeace = false;

    void Update()
    {
        //if (GameManager.Inst.isPeaceMonster)
            //return;

        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);    
    }
}
