using UnityEngine;

public class Player : MonoBehaviour
{
    // variables
    // we want to know about player's rigidbody and animator to synchronize the movement and animation
    public Rigidbody2D rb2d;
    //
    public CapsuleCollider2D capsuleCollider;
    public Animator animator;
    // we want to be able to control the sprite flipX with facing direction when we move
    public SpriteRenderer spriteRenderer;
    // how fast the player moves horizontally

    public float moveSpeed = 10f;

    public float jumpSpeed = 10f;
    public float maxJumpTime = 0.300f; // in seconds
    public float maxCoyoteTime = 0.100f; // in seconds
    public float fallGravity = -10;



    public LayerMask groundLayer;

    //
    public float raycastDistance = 0.05f;


    private float coyoteTimeRemaining;
    private float jumpTimeRemaining;

    // Physics / raycast variables
    Vector2 edgeClipTopOrigin;
    Vector2 edgeClipBotOrigin;
    Vector2 edgeClipRayDistance;




    void Update()
    {

        ///////////////////////
        ///////////////////// MOVE HORIZONTALLY/////////////////////////

        // get the player movement input from unity 
        float moveX = Input.GetAxis("Horizontal");
        // math.abs gives us the number's absolute value. 
        // eg. Abs(+1), Abs(-1) both gies us 1. 
        bool isMovingHorizontally = Mathf.Abs(moveX) > 0.1f;

        if (isMovingHorizontally)
        {
            // 
            bool isFacingLeft = moveX < 0; // move x is positive when we move to the right, negative if we move left
            spriteRenderer.flipX = isFacingLeft;

            // check to see if player is hitting a wall horizontally
            Vector2 centre = transform.position;
            Vector2 extents = capsuleCollider.bounds.extents;
            float extentsX = isFacingLeft ? -extents.x : +extents.x;
            edgeClipTopOrigin = centre + new Vector2(extentsX, +extents.y) ;
            edgeClipBotOrigin = centre + new Vector2(extentsX, -extents.y);
            Vector2 direction = Vector2.Normalize(new Vector2(extentsX,0));
            edgeClipRayDistance = direction * raycastDistance;
            bool hitTop = Physics2D.Raycast(edgeClipTopOrigin, direction, raycastDistance, groundLayer);
            bool hitBot = Physics2D.Raycast(edgeClipBotOrigin, direction, raycastDistance, groundLayer);
            if (hitTop == false && hitBot is false)

     
                {

                // set move speed horizontally directly
                rb2d.linearVelocityX = moveX * moveSpeed;
            }
            
            Debug.DrawLine(edgeClipTopOrigin, edgeClipTopOrigin + edgeClipRayDistance, hitTop ? Color.red : Color.green);
            Debug.DrawLine(edgeClipBotOrigin, edgeClipBotOrigin + edgeClipRayDistance, hitBot ? Color.red : Color.green);



            // set move speed horizontal directly   
            float force = moveX * moveSpeed;
            rb2d.linearVelocityX = moveX * moveSpeed;
        }
        // synchronize the animator parameter to this player's movement. 
        // automatically control the player animations.
        animator.SetFloat("moveSpeedX", Mathf.Abs(moveX));

        ///////////////////// JUMP /////////////////////////
        ///

        //Additionnal gravity when falling
        if (rb2d.linearVelocityY <0)
            {
            rb2d.AddForceY(fallGravity);
        }

        // decremet coyote time timer
        coyoteTimeRemaining -= Time.deltaTime;

        //

        Vector2 rayOrigin = this.transform.position;
        Vector2 rayDirection = Vector2.down;
        float distance = 1.05f;
        bool isGrounded = Physics2D.Raycast(rayOrigin, rayDirection, distance, groundLayer);

        if (isGrounded)
        {
            // reset  coyote time timer because we are on the ground
            coyoteTimeRemaining = maxCoyoteTime;
        }
        // check if jumping
        if (isGrounded == true || coyoteTimeRemaining > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // remove ability to coyote jump
                coyoteTimeRemaining = 0;
                // keep player knowleable of jumping state 
                jumpTimeRemaining = maxJumpTime;
            }
        }

        if (jumpTimeRemaining > 0 )
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // add force for jumping
                rb2d.linearVelocityY = jumpSpeed;
            }
            else
            {
                // if we release the jump button, we stop applying jump force
                jumpTimeRemaining = 0;
             }
            // decrement timer 
            jumpTimeRemaining -= Time.deltaTime;
        }
        animator.SetBool("isGrounded", isGrounded);
    }

    // runs everytime we change something in the inspector 
    // or reset is called or when Unity recompiles, etc.
    private void OnValidate()
    {
        if (rb2d == null) 
            rb2d = GetComponent<Rigidbody2D>();
        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Reset()
    {
        
    }
}   
