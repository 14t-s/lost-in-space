using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    public int hp;

    public GameObject meleeAttack;
    public Collider2D meleeAttackCollider;
    
    public TextMeshProUGUI textElement;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        meleeAttack = gameObject.transform.Find("atcke").gameObject;
        meleeAttackCollider = gameObject.transform.Find("atcke").GetComponentInChildren<Collider2D>();
        meleeAttack.SetActive(false);
        //meleeAttackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = string.Concat("HP: ", hp.ToString());
    }

    public void HandleMeleeStart(InputAction.CallbackContext ctx)
    {
        meleeAttack.SetActive(true);
        //meleeAttackCollider.enabled = true;
    }

    public void HandleMeleeEnd(InputAction.CallbackContext ctx)
    {
        meleeAttack.SetActive(false);
        //meleeAttackCollider.enabled = false;
    }
}
