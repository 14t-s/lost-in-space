using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D body;
    public Vector2 velocity;
    public float maxSpeed;
    /*public float maxAcceleration;
    public Vector2 velocity, desiredVelocity;*/
    public bool jumpPressed;
    public bool desiredJump;
    //public Vector2 acceleration;

    public bool onGround;

    private Controls inputs;

    void Awake()
    {
        inputs = new Controls();
        inputs.Gameplay.Enable();
        inputs.Gameplay.Jump.performed += HandleJump;
        /*Debug.Log(inputs.Gameplay.Movement.bindings.ToArray().Length);
        var test = inputs.Gameplay.Movement.bindings.ToArray();
        for (int i = 0; i < test.Length; i++)
        {
            Debug.Log(test[i]);
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementXY = inputs.Gameplay.Movement.ReadValue<Vector2>();
        //acceleration = movementXY * maxSpeed;
        velocity = new Vector2(movementXY.x, 0f) * maxSpeed;
        desiredJump |= jumpPressed && onGround;
        if (jumpPressed) Debug.Log("AAA");
        if (onGround) Debug.Log("AAB");
        if (desiredJump) Debug.Log("JumpC");
        jumpPressed = false;
    }

    void FixedUpdate()
    {
        //velocity = body.velocity;
        //float maxSpeedChange = maxAcceleration * Time.deltaTime;
        //velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        if (desiredJump && onGround)
        {
            desiredJump = false;
            Debug.Log("jumpA");
            velocity.y = 5f;
        }
        
        body.velocity = new Vector2(velocity.x, body.velocity.y);
        body.velocity += new Vector2(0f, velocity.y);
        //velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
        /*velocity += acceleration * Time.deltaTime;
        player.velocity += acceleration * Time.deltaTime;// * 0.9f;*/
        //body.velocity = velocity;
        //player.velocity -= player.velocity * Time.deltaTime;
        //player.AddForce(acceleration, ForceMode2D.Impulse);
        onGround = false;
    }

    private void HandleJump(InputAction.CallbackContext ctx)
    {
        jumpPressed = true;
        Debug.Log("jumpB");
        //velocity.y += 5f;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        onGround = true;
    }
}
