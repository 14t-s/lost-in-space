using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    public int hp;

    public GameObject meleeAttack;
    
    public TextMeshProUGUI textElement;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        meleeAttack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = string.Concat("HP: ", hp.ToString());
    }

    public void HandleMeleeStart(InputAction.CallbackContext ctx)
    {
        meleeAttack.SetActive(true);
    }

    public void HandleMeleeEnd(InputAction.CallbackContext ctx)
    {
        meleeAttack.SetActive(false);
    }
}
