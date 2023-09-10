using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Playerinput playerMovement;
    public Playerinput jumpScript;
    public Playerinput dashScript;
    //public Animator animator;

    private InputAction move;
    private InputAction jump;
    private InputAction dash;

    [SerializeField] private float normalCharGravity;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public float movementSpeed;
    public float jumpForce;
    public int extraJumps;
    public int extraDashs;
    public float dashSpeed;
    public float dashTime;
    public float dashGravity;
    public float vertDashDamp;
    public float stickySpeed;
    public float currentSpeed;
    private int jumpsLeft;
    private bool isFacingRight = true;
    private bool isDashing = false;
    private int dashsLeft;
    private string lastPlatformTouched;
    //[SerializeField] private string collidedWith;
    [SerializeField] private float slipMultiplier;



    private void Start()
    {
        jumpsLeft = extraJumps;
        rb.gravityScale *= normalCharGravity;
        currentSpeed = movementSpeed;

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
        Jump();
        StartCoroutine(Dash());

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

    private void PlayerMovement()
    {
        if (!isDashing)
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
            rb.velocity = (PlayerInput() != Vector2.zero ) ? new Vector2(dashSpeed * PlayerInput().x, dashSpeed * PlayerInput().y*vertDashDamp):
                          (isFacingRight) ? new Vector2(dashSpeed,0) : new Vector2(-dashSpeed,0);

            yield return new WaitForSecondsRealtime(dashTime);
            
            rb.gravityScale = normalCharGravity;
            /*rb.velocity =  Vector2.zeronew Vector2( rb.velocity.x, 0)*/;
            isDashing = false;
            lastPlatformTouched = "ground";
        }
    }

    private void Jump()
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
    private void Flip()
    {
        float movementSpeed = PlayerInput().x;
        if (isFacingRight && movementSpeed < 0f || !isFacingRight && movementSpeed > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
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