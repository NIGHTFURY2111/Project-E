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

    private float currentSpeed;
    private int jumpsLeft;
    private bool isFacingRight = true;
    private bool isDashing = false;
    private int dashsLeft;



    private void Start()
    {
        jumpsLeft = extraJumps;
        rb.gravityScale *= normalCharGravity ;
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
        MovementReset();
    }

    private void PlayerMovement()
    {
        if (!isDashing)
        {
            Vector2 direction = move.ReadValue<Vector2>();

            rb.velocity = new Vector2(currentSpeed * direction.x, rb.velocity.y);
            //animator.SetFloat("Speed", math.abs(rb.velocity.x));
        }
    }


    IEnumerator Dash()
    {
        if (dash.WasPressedThisFrame() && (!isDashing) && dashsLeft!=0)
        {
            Vector2 direction = move.ReadValue<Vector2>();
            //Vector2 storing = rb.velocity;
            
            isDashing = true;
            dashsLeft--;

            rb.gravityScale = dashGravity;
            rb.velocity = (direction != Vector2.zero ) ? new Vector2(dashSpeed * direction.x, dashSpeed * direction.y* 0.6f):
                          (isFacingRight) ? new Vector2(dashSpeed,0) : new Vector2(-dashSpeed,0);

            yield return new WaitForSecondsRealtime(dashTime);
            
            rb.gravityScale = normalCharGravity;
            //rb.velocity = new Vector2( rb.velocity.x, 0);
            isDashing = false;
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

    private void MovementReset()
    {
        if (IsGrounded())
        {
            if (!isDashing && dashsLeft != extraDashs)
            {
                dashsLeft = extraDashs * Convert.ToInt32(IsGrounded() && dashsLeft != extraDashs);
            }

            if (jumpsLeft != extraJumps)
                jumpsLeft = extraJumps * Convert.ToInt32(IsGrounded() && jumpsLeft != extraJumps);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        float movementSpeed = move.ReadValue<Vector2>().x;
        if (isFacingRight && movementSpeed < 0f || !isFacingRight && movementSpeed > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
