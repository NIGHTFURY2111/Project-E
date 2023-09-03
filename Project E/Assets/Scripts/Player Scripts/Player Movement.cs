using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public int extraJumps = 2;
    public float movementSpeed;
    public float jumpForce;

    public Rigidbody2D rb;
    public Playerinput playerMovement;
    public Playerinput jumpScript;
    //public Animator animator;

    private int jumpsLeft;
    private float jumpGravity = 2f;
    private float normalGravity = 5f;
    private bool isFacingRight = true;

    private InputAction move;
    private InputAction jump;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    private void Start()
    {
        jumpsLeft = extraJumps;
        //rb.gravityScale = normalGravity;

    }
    private void Awake()
    {
        playerMovement = new();
        jumpScript = new();
    }
    private void OnEnable()
    {
        move = playerMovement.Player.Move;
        move.Enable();

        jump = jumpScript.Player.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
    void Update()
    {
        Flip();
        //animator.SetBool("isJumping", !IsGrounded());


        //if (IsGrounded() && jumpsLeft != extraJumps)
        //{
        //    jumpsLeft = extraJumps; 
        //    animator.SetBool("isJumping",false);
        //}

        if (jumpsLeft > 0 && jump.WasPressedThisFrame())
        {
            //animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = jumpGravity;
            --jumpsLeft;
        }

        if (jump.WasReleasedThisFrame() && !IsGrounded())
            rb.gravityScale = normalGravity;

    }

    private void LateUpdate()
    {
        if (IsGrounded() && jumpsLeft != extraJumps)
        {
            jumpsLeft = extraJumps * Convert.ToInt32(IsGrounded() && jumpsLeft != extraJumps);
        }
    }
    private void FixedUpdate()
    {
        Vector2 direction = move.ReadValue<Vector2>();

        rb.velocity = new Vector2(movementSpeed * direction.x, rb.velocity.y);
        //animator.SetFloat("Speed", math.abs(rb.velocity.x));
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
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
