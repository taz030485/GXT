using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    private void Start()
    {
        GridManager.AddEnemy(this);
    }

    public void TakeDamage(int damage)
    {
        
    }
}
