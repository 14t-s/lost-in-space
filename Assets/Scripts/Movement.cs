﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Collision coll;
    [HideInInspector]
    public Rigidbody2D player;
    private AnimationScript anim;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    public float kaioatTime = 1f;
    public float kaioatTimeCounter;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    [Space]

    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    // Raycasts for corner correction and ledge grabbing
    private float topRaycastLength;
    public Vector3 edgeRaycastOffset;
    public Vector3 innerRaycastOffset;
    private bool canCornerCorrect;

    private Controls playerControls;
    private Controls playerControlsAction;

    void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        //playerControls = GetComponent<Controls>();

        //Reference
        /*
        PlayerInputActions playerInputActions = new PlayerInputActions();
        controls.Gameplay.Jump.performed += Jump;
        */

        playerControlsAction = new Controls();
        playerControlsAction.Gameplay.Enable();
        playerControlsAction.Gameplay.Movement.performed += Movement_performed;
    }

    public void Movement_performed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        float x = inputVector.x;
        float y = inputVector.y;
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, player.velocity.y);

        //Archive
        /*
         float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, player.velocity.y);*/
    }

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        player = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        // keys:
        // arrow keys - movement
        // z - jump
        // shift - wall grab
        // x - melee / s - ranged attckes
        // c - special/dash
        // d - ability

        Vector2 updatedInputVector = playerControlsAction.Gameplay.Movement.ReadValue<Vector2>();
        float x = updatedInputVector.x;
        float y = updatedInputVector.y;
        float xRaw = 0;
        float yRaw = 0;

        if (updatedInputVector.x > 0)
        {
            xRaw = 1;
        }
        else
        {
            xRaw = 0;
        }

        if (updatedInputVector.y > 0)
        {
            yRaw = 1;
        }
        else
        {
            yRaw = 0;
        }

        /*
        //  Wall grab 
        if (coll.onWall && Input.GetKey(KeyCode.LeftShift) && canMove) // [kcc] also instead of putting keycodes here just make vars at top of script
        {
            GrabWall();
        }
        */

        //if (Input.GetKeyUp(KeyCode.LeftShift) || !coll.onWall || !canMove)
        if (!coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        // Resets coyote time or counts down the grace period
        if (coll.onGround == true)
        {
            kaioatTimeCounter = kaioatTime;
        }
        else
        {
            kaioatTimeCounter -= Time.deltaTime;
        }

        // Resets better jumping?
        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }


        if (wallGrab && !isDashing)
        {
            player.gravityScale = 0;
            if (x > .2f || x < -.2f)
                player.velocity = new Vector2(player.velocity.x, 0);

            float speedModifier;
            if (coll.reachLedge == true)
                speedModifier = y > 0 ? 0f : 1;
            else
                speedModifier = y > 0 ? .5f : 1;


            player.velocity = new Vector2(player.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            player.gravityScale = 3;
        }


        /*
        if (coll.onWall && !coll.onGround && Input.GetKeyUp(KeyCode.LeftShift)) // [kcc]
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }
        */

        if (!coll.onWall || coll.onGround)
            wallSlide = false;

        /*
        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Z)) // [kcc]
        {
            anim.SetTrigger("jump");

            if (kaioatTimeCounter > 0f)
            {
                Jump(Vector2.up, false);
            }

            if (coll.onWall && !coll.onGround)
                WallJump();
        }
        */
        
        /*
        if (Input.GetKeyDown(KeyCode.C) && !hasDashed) // [kcc]
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw, side);
        }*/

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;

        if (x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
        }


    }

    public void HandleJump()
    {
        anim.SetTrigger("jump");

        if (kaioatTimeCounter > 0f)
        {
            Jump(Vector2.up, false);
        }

        if (coll.onWall && !coll.onGround)
            WallJump();
    }

    // Handles the wall grabbing 
    public void GrabWall()
    {
        if (side != coll.wallSide)
        {
            side *= -1;
            anim.Flip(side);
        }
        wallGrab = true;
        wallSlide = false;
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    private void Dash(float x, float y, float xdir)
    {
        if (x == -xdir && wallGrab) return;
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        anim.SetTrigger("dash");

        player.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        player.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        dashParticle.Play();
        player.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        player.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.onGround)
            hasDashed = false;
    }

    private void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    private void WallSlide()
    {
        if (coll.wallSide != side)
            anim.Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((player.velocity.x > 0 && coll.onRightWall) || (player.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : player.velocity.x;

        player.velocity = new Vector2(push, -slideSpeed);
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            player.velocity = new Vector2(dir.x * speed, player.velocity.y);
        }
        else
        {
            player.velocity = Vector2.Lerp(player.velocity, (new Vector2(dir.x * speed, player.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        player.velocity = new Vector2(player.velocity.x, 0);
        player.velocity += dir * jumpForce;

        particle.Play();

        kaioatTimeCounter = 0f;
    }

    private void CornerCorrect(float yVelocity)
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position - innerRaycastOffset + Vector3.up * topRaycastLength, Vector3.left, topRaycastLength, groundTouch)
        return;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        player.drag = x;
    }

    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }

    // Enable player only if GameState is in Gameplay
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
