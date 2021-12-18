using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    public bool reachLedge;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Vector2 checkLedgeOffset, drawLineRight, drawLineLeft;
    private Color debugCollisionColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer) || Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset + new Vector2(0.5f, 0f), collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;

        RaycastHit2D canClimbRight = Physics2D.Raycast((Vector2)transform.position + checkLedgeOffset, Vector2.right);
        RaycastHit2D canClimbLeft = Physics2D.Raycast((Vector2)transform.position + checkLedgeOffset, Vector2.left);

        Debug.DrawRay((Vector2)transform.position + checkLedgeOffset, Vector2.right, Color.red);
        Debug.DrawRay((Vector2)transform.position + checkLedgeOffset, Vector2.left, Color.red);

        reachLedge = (canClimbRight == false && onRightWall) || (canClimbLeft == false && onLeftWall);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset + new Vector2(0.5f, 0f), collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

       // Gizmos.DrawLine((Vector2)transform.position + checkLedgeOffset, (Vector2)transform.position + checkLedgeOffset + drawLineLeft);
      //  Gizmos.DrawLine((Vector2)transform.position + checkLedgeOffset, (Vector2)transform.position + checkLedgeOffset + drawLineRight);
    }
}
