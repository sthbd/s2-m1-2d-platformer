using UnityEngine;

public class Enemy : MonoBehaviour

{    //use rigid to mvoe Ai
    public Rigidbody2D rb2d;
    // which layers does raycast respect
    public LayerMask layerMask;
    public float distanceCheckWall= 1;
    public float distanceCheckWallOffsetY    = -0.5f;
    public float distanceCheckLedge= 1;

    //
    public SpriteRenderer spriteRenderer;
    public float patrolSpeedX = 5;
    public bool moveRight = true;

    public Player player;
    public float playerChaseRadius = 3;

    void Update()
    {

        float distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);  

        if (distanceToPlayer <= playerChaseRadius)
        {

            Chase();
                }
        else

            {
                Patrol();
            }

            spriteRenderer.flipX = moveRight;
        
           
    }

    void Chase()
    {

        moveRight = player.transform.position.x > this.transform.position.x;

        float linearVelocityX = moveRight ? +patrolSpeedX : -patrolSpeedX;
    }

    void Patrol()
    {
        // will shoot ray to detect walls from centre of enemy
        Vector2 wallDetectedOrigin = transform.position;
        wallDetectedOrigin.y += distanceCheckWallOffsetY;
        Vector2 wallDetectedDir = moveRight ? Vector2.right : Vector2.left;
        bool willHitWall = Physics2D.Raycast(wallDetectedOrigin, wallDetectedDir, distanceCheckWall,layerMask);
        Debug.DrawLine(wallDetectedOrigin, wallDetectedOrigin + wallDetectedDir * distanceCheckWall);


        Vector2 ledgeDetectedOffsetDir = moveRight ? Vector2.right : Vector2.left;
        Vector2 ledgeDetectedOrigin = (Vector2)transform.position + ledgeDetectedOffsetDir;
        Vector2 ledgeDetectedDir = Vector2.down;

        bool willWalkOffLedge = !Physics2D.Raycast(ledgeDetectedOrigin, ledgeDetectedDir, distanceCheckLedge, layerMask);
        Debug.DrawLine(ledgeDetectedOrigin, ledgeDetectedOrigin + ledgeDetectedDir * distanceCheckLedge);

        if ( willHitWall == true || willWalkOffLedge == true)
        {
            //moveRight right is not what it currently is invert / flip bool
            moveRight = !moveRight;

            // flip on X axis if we are not moving right
           

        }

        // MOVE 
        // calculate which direction we need to move in
        float lineVelocityX = moveRight ? +patrolSpeedX : -patrolSpeedX;
        rb2d.linearVelocityX = lineVelocityX;
 
    }


    private void OnCollisionEnter2D(Collision2D collision)

    {
        if (collision.gameObject.CompareTag("Player") == true )

        {
            Debug.Log("Hit player");
        }
            
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, playerChaseRadius);
    }
}

