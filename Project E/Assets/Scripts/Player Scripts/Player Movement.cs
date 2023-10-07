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
    public Playerinput grabScript;
    
    //public Animator animator;
    
    private InputAction move;
    private InputAction jump;
    private InputAction dash;
    private InputAction grab;

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
    public float dashTime;
    public float dashGravity;
    public float vertDashDamp;
    private bool isDashing = false;
    private int dashsLeft;
    
    
    [Header("Jump Settings")]
    public float jumpForce;
    public int extraJumps;
    private int jumpsLeft;

    [Header("Wall Settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float slipMultiplier;
    private bool isSliding = false;
    private bool isWallJumping = false;
    [SerializeField] private float wallJumpSpeed;
    [SerializeField] private float wallJumpTime;
    [SerializeField] private float wallJumpForce;

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
        rb.gravityScale *= normalCharGravity;
        currentSpeed = movementSpeed;
        lastRespawnPoint = RespawnPoint.transform.position;

    }

    #region necessary input system calls
    private void Awake()
    {
        playerMovement = new();
        jumpScript = new();
        dashScript = new();
        grabScript = new();
    }
    private void OnEnable()
    {
        move = playerMovement.Player.Move;
        move.Enable();

        jump = jumpScript.Player.Jump;
        jump.Enable();

        dash = dashScript.Player.Dash;
        dash.Enable();

        grab = grabScript.Player.Grab;
        grab.Enable();


    }


    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        dash.Disable();
        grab.Disable();
    }

#endregion

    void Update()
    {

        Flip();

        Sliding();
        StartCoroutine(Jump());
        StartCoroutine(Dash());
        if (transform.position.y < threshold)
            Respawn();
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
        if (!(isDashing||isSliding||isWallJumping))
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

    
    IEnumerator Jump()
    {
        if(isSliding && !IsGrounded())
        {
            if(jump.WasPressedThisFrame()) 
            {   
                isWallJumping = true;
                isFacingRight = !isFacingRight;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                
                rb.velocity = new Vector2(wallJumpSpeed*transform.localScale.x,wallJumpForce);
                rb.gravityScale /= 2;
                yield return new WaitForSecondsRealtime(wallJumpTime);
                rb.gravityScale = normalCharGravity;
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                isWallJumping = false;
                
                
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
       if (collidingWith)
       {
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*slipMultiplier);
            
            isSliding = true;
            lastPlatformTouched = "ground";
       }
        else
        {
            
            isSliding = false;
        }

    }

    private void Flip()
    {
        if(!isWallJumping)
        {
            float movementSpeed = PlayerInput().x;
            if (isFacingRight && movementSpeed < 0f || !isFacingRight && movementSpeed > 0f)
            {
                isFacingRight = !isFacingRight;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
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