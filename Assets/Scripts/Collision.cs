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

    

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;

    // ledge stuff
    public Vector2 checkLedgeOffset, drawLineRight, drawLineLeft;
    private Color debugCollisionColor = Color.red;
    public bool onRightLedge, onLeftLedge;
    public bool reachLedge, reachLedgeRight, reachLedgeLeft;
    public GameObject ledgeCheckerObject;

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

        
        // Ledge checker
        RaycastHit2D canClimbRight = Physics2D.Raycast(ledgeCheckerObject.transform.position, Vector2.right, 2f);
        RaycastHit2D canClimbLeft = Physics2D.Raycast(ledgeCheckerObject.transform.position, Vector2.left, 2f);

        Debug.DrawRay(ledgeCheckerObject.transform.position, Vector2.right, Color.red);
        Debug.DrawRay(ledgeCheckerObject.transform.position, Vector2.left, Color.red);

        reachLedgeRight = (canClimbRight == false && onRightWall);
        reachLedgeLeft = (canClimbLeft == false && onLeftWall);

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
