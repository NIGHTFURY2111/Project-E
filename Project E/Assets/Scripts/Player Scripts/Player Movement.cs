using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [Header("General Settings")]
    public Rigidbody2D rb;
    public Playerinput playerMovement;
    public Playerinput jumpScript;
    public Playerinput dashScript;
<<<<<<< HEAD
=======
    public Playerinput grabScript;
<<<<<<< Updated upstream
=======
>>>>>>> 5bb20c742c4577e39e65b3b820a891e3858104ff
>>>>>>> Stashed changes
    
    //public Animator animator;
    
    private InputAction move;
    private InputAction jump;
    private InputAction dash;
    

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    
    [Header("Movement Settings")]
    [SerializeField] private float normalCharGravity;
    public float movementSpeed;
    public float stickySpeed;
    public float currentSpeed;
    private bool isFacingRight = true;
    
    
    [Header("Dash Settings")]
    public int extraDashs;
    public float dashSpeed;
<<<<<<< Updated upstream
    public float dashTime;
    public float dashGravity;
=======
    public float wallJumpSpeed;
    public float dashTime;
    public float wallJumpTime;
    public float dashGravity;
<<<<<<< HEAD
    //public float slideGravity;
=======
>>>>>>> 5bb20c742c4577e39e65b3b820a891e3858104ff
>>>>>>> Stashed changes
    public float vertDashDamp;
    private bool isDashing = false;
    private bool isSliding = false;
    private bool isWallJumping = false;
    private int dashsLeft;
    
    
    [Header("Jump Settings")]
    public float jumpForce;
    public int extraJumps;
    private int jumpsLeft;

    [Header("Wall Settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float slipMultiplier;
    public float grabGravity;
    private bool isGrabing = false;
    //[SerializeField] private string collidedWith;
    //private bool isTouchingWall = false;



    [Header("Respawn Settings")]
    public GameObject RespawnPoint;
    public float threshold;
    private string lastPlatformTouched;
    public Vector3 lastRespawnPoint;





    private void Start()
    {
        jumpsLeft = extraJumps;
        rb.gravityScale = normalCharGravity;
        currentSpeed = movementSpeed;
        lastRespawnPoint = RespawnPoint.transform.position;

    }

    #region necessary input system calls
    private void Awake()
    {
        playerMovement = new();
        jumpScript = new();
        dashScript = new();
        
    }
    private void OnEnable()
    {
        move = playerMovement.Player.Move;
        move.Enable();

        jump = jumpScript.Player.Jump;
        jump.Enable();

        dash = dashScript.Player.Dash;
        dash.Enable();

       


    }


    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        dash.Disable();
       
    }

#endregion

    void Update()
    {

        Flip();
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
        Sliding();
        StartCoroutine(Jump());
        StartCoroutine(Dash());
        
        
=======
>>>>>>> Stashed changes
        WallJump();
        Grabbing();
        Jump();
        StartCoroutine(Dash());
        if (transform.position.y < threshold)
            Respawn();
<<<<<<< Updated upstream
=======
>>>>>>> 5bb20c742c4577e39e65b3b820a891e3858104ff
>>>>>>> Stashed changes
        //if (IsGrounded()) Debug.Log("grounded");

        /*        animator.SetBool("isJumping", !IsGrounded());

        if (IsGrounded() && jumpsLeft != extraJumps)
        {
            jumpsLeft = extraJumps;
            animator.SetBool("isJumping", false);
        }*/
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void LateUpdate()
    {
        if (IsGrounded())
        {
            JumpReset();
            DashReset();
        }
    }


    public void Respawn()
    {
        transform.position = RespawnPoint.transform.position;
    }


    private void PlayerMovement()
    {
        if (!(isDashing || isWallJumping||isSliding))
        {// Changed If-else conditions to switch cases

            
            switch (lastPlatformTouched)
            {   
                case "Slippery platform": rb.AddForce(new Vector2(currentSpeed * PlayerInput().x * slipMultiplier, rb.velocity.y));
                    break;

                case "Sticky platform": rb.velocity = new Vector2(stickySpeed * PlayerInput().x, rb.velocity.y);
                    break;
                
                default: rb.velocity = new Vector2(currentSpeed * PlayerInput().x, rb.velocity.y);
                    break;
            }
            //if (isTouchingWall&&)
                //if (IsGrounded("Slippery platform"))
            //{
            //    rb.AddForce(new Vector2(currentSpeed * PlayerInput().x * slipMultiplier, rb.velocity.y));
            //}
            //else if(IsGrounded("Sticky platform"))
            //{
            //    rb.velocity = new Vector2(stickySpeed * PlayerInput().x, rb.velocity.y);
            //}
            //else /*if(IsGrounded())*/
            //{
            //    rb.velocity =  new Vector2(currentSpeed * PlayerInput().x, rb.velocity.y);
            //}
            //animator.SetFloat("Speed", math.abs(rb.velocity.x));
        }

    }

    Vector2 PlayerInput()
    {
        return move.ReadValue<Vector2>();
    }
      
    IEnumerator Dash()
    {
        if (dash.WasPressedThisFrame() && (!isDashing) && dashsLeft!=0)
        {
            //Vector2 direction = move.ReadValue<Vector2>();
            //Vector2 storing = rb.velocity;
            
            isDashing = true;
            dashsLeft--;
            rb.velocity = Vector2.zero;
            rb.gravityScale = dashGravity;
            yield return new WaitForSecondsRealtime(0.05f);
            rb.velocity = (PlayerInput() != Vector2.zero ) ? new Vector2(dashSpeed * PlayerInput().x, dashSpeed * PlayerInput().y*vertDashDamp):
                          (isFacingRight) ? new Vector2(dashSpeed,0) : new Vector2(-dashSpeed,0);

            yield return new WaitForSecondsRealtime(dashTime);
            
            rb.gravityScale = normalCharGravity;
            /*rb.velocity =  Vector2.zeronew Vector2( rb.velocity.x, 0)*/;
            isDashing = false;
            lastPlatformTouched = "ground";
        }
    }

<<<<<<< Updated upstream
    private void WallJump()
    {
        if (isGrabing && jumpsLeft > 0 && jump.WasPressedThisFrame())
        {
            rb.velocity = new Vector2(-transform.localScale.x * movementSpeed*100, jumpForce);
            rb.gravityScale /= 2;
            --jumpsLeft;
        }
        if (jump.WasReleasedThisFrame())
            rb.gravityScale = normalCharGravity;
    }
    private void Jump()
=======
<<<<<<< HEAD
    IEnumerator Jump()
    {   if (isSliding && !IsGrounded()) 
=======
    private void WallJump()
>>>>>>> Stashed changes
    {
        if (isGrabing && jumpsLeft > 0 && jump.WasPressedThisFrame())
        {
            rb.velocity = new Vector2(-transform.localScale.x * movementSpeed*100, jumpForce);
            rb.gravityScale /= 2;
            --jumpsLeft;
        }
        if (jump.WasReleasedThisFrame())
            rb.gravityScale = normalCharGravity;
    }
    private void Jump()
    {
        if (jumpsLeft > 0 && jump.WasPressedThisFrame())
>>>>>>> 5bb20c742c4577e39e65b3b820a891e3858104ff
        {
            if(jump.WasPressedThisFrame())
            {
                if(isFacingRight)
                {   isWallJumping = true;
                    
                    isFacingRight = !isFacingRight;
                    Vector3 localScale = transform.localScale;
                    localScale.x *= -1f;
                    transform.localScale = localScale;
                    rb.velocity = new Vector2(wallJumpSpeed*transform.localScale.x, jumpForce);

                    rb.gravityScale /= 2;
                    yield return new WaitForSecondsRealtime(wallJumpTime);
                    isWallJumping = false;
                    

                }
                //else
                //{
                //    isFacingRight = !isFacingRight;
                //    Vector3 localScale = transform.localScale;
                //    localScale.x *= -1f;
                //    transform.localScale = localScale;
                //    rb.velocity = new Vector2(wallJumpSpeed, jumpForce);

                //    rb.gravityScale /= 2;
                //    yield return new WaitForSecondsRealtime(1f);
                //}
            }
        }
        else
        {
            if (jumpsLeft > 0 && jump.WasPressedThisFrame())
            {
                //animator.SetBool("isJumping", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.gravityScale /= 2;
                --jumpsLeft;
            }

            if (jump.WasReleasedThisFrame())
                rb.gravityScale = normalCharGravity;
        }
        
    }

    void DashReset()
    {
        if (!isDashing && dashsLeft != extraDashs)
            dashsLeft = extraDashs * Convert.ToInt32(IsGrounded()/* && dashsLeft != extraDashs*/);

    }

    void JumpReset()
    {
        if (jumpsLeft != extraJumps)
            jumpsLeft = extraJumps * Convert.ToInt32(IsGrounded()/* && jumpsLeft != extraJumps*/);
    }


    bool IsGrounded( string testAgainst = "ground")
    {
        Collider2D collidingWith = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);

        if (collidingWith)
            lastPlatformTouched = collidingWith.gameObject.tag;

        if (testAgainst == "ground")
            return collidingWith;
        else
            return lastPlatformTouched == testAgainst;
    }

    void Sliding()
    {
       Collider2D collidingWith = Physics2D.OverlapCircle(wallCheck.position, 0.3f, groundLayer);
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
       
       
            if (collidingWith)
            {

                //rb.gravityScale = slideGravity;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
                isSliding = true;
                lastPlatformTouched = "ground";

=======
>>>>>>> Stashed changes
       if (grab.IsPressed() && collidingWith)
       {
            rb.velocity = new Vector2(rb.velocity.x,0f);
            rb.gravityScale = grabGravity;
            isGrabing = true;
       }
        else if (grab.WasReleasedThisFrame() || !collidingWith)
        {
            rb.gravityScale = normalCharGravity;
            isGrabing = false;
<<<<<<< Updated upstream
=======
>>>>>>> 5bb20c742c4577e39e65b3b820a891e3858104ff
>>>>>>> Stashed changes
        }

            else
            {
                //rb.gravityScale = normalCharGravity;
                isSliding = false;
                
            } 
        
           
        

    }

    private void Flip()
    {
        float movementSpeed = PlayerInput().x;
        if (isFacingRight && movementSpeed < 0f || !isFacingRight && movementSpeed > 0f)
        {
            isFacingRight = !isFacingRight;
            //Vector3 localScale = transform.localScale;
            //localScale.x *= -1f;
            //transform.localScale = localScale;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.3f);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{


    //    if (collision.gameObject.tag == "Sticky platform")
    //    {
    //        currentSpeed = stickySpeed;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Sticky platform")
    //    {
    //        currentSpeed = movementSpeed;
    //    }

    //}

}   