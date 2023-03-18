using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public GameObject enemy;
    public Rigidbody2D enemyBody;
    //public Collider2D meleeAttackCollider;
    public int hp;
    public TextMeshProUGUI hpText;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject;
        enemyBody = GetComponent<Rigidbody2D>();
        //meleeAttackCollider = gameObject.transform.Find("atcke").GetComponentInChildren<Collider2D>();
        hp = 5;
        hpText = GetComponentInChildren<TextMeshProUGUI>();
        hpText.text = hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if (enemyBody.IsTouchingLayers(3))
        //{
            //enemy.SetActive(false);
        //}
        /*else
        {
            enemy.SetActive(true);
        }*/
        if (hp == 0) enemy.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "atcke")
        {
            //enemy.SetActive(false);
            hp--;
            hpText.text = hp.ToString();
        }
    }
}
