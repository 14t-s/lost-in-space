using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemy;
    public Rigidbody2D enemyBody;
    
    // Start is called before the first frame update
    void Start()
    {
        if (enemyBody.IsTouchingLayers(3))
        {
            enemy.SetActive(false);
        } else
        {
            enemy.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
