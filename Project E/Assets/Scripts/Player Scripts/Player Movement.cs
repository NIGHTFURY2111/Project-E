using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [Header("General Settings")]
    public GameObject currentTeleporter;
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
    [SerializeField] private LayerMask wallLayer;


    [Header("Movement Settings")]
    [SerializeField] private float normalCharGravity;
    public float movementSpeed;
    public float stickySpeed;
    public float currentSpeed;
    private bool isFacingRight = true;
    public float direction;

    [Header("Ice Settings")]
    [SerializeField] private float slipMultiplier;
    public float maxIceSpeed;
    public float iceGripValue;


    [Header("Dash Settings")]
    public int extraDashs;
    public float dashSpeed;
    public float dashTime;
    public float dashGravity;
    public float vertDashDamp;
    public float vertMaxDash;
    private bool isDashing = false;
    private int dashsLeft;


    [Header("Jump Settings")]
    public float jumpForce;
    public int extraJumps;
    private int jumpsLeft;
    private bool isJumping = false;

    [Header("Wall Settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] float wallSlidingSpeed = 2f;
    public float wallJumpingTime;
    public float wallPush;
    public Vector2 wallJumpingPower;
    private bool isWallJumping;
    private bool isWallSliding = false;
    //private bool isSliding = false;
    //private bool isWallJumping = false;
    //[SerializeField] private float wallJumpSpeed;
    //[SerializeField] private float wallJumpTime;
    //[SerializeField] private float wallJumpForce;
    //public float wallSlidingSpeed;

    //[SerializeField] private string collidedWith;
    ////private bool isTouchingWall = false;

    //private float wallJumpingDirection;
    //private float wallJumpingCounter;
    //private float wallJumpingDuration = 0.4f;



    [Header("Respawn Settings")]
    public GameObject RespawnPoint;
    public float threshold;
    private string lastPlatformTouched;
    public Vector3 lastRespawnPoint;





    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        jumpsLeft = extraJumps;
        rb.gravityScale *= normalCharGravity;
        currentSpeed = movementSpeed;
        lastRespawnPoint = RespawnPoint.transform.position;
        Respawn();
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

        FlipCheck();
        Teleport();
        Sliding();
        Jump();
        //StartCoroutine(Jump());
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
        rb.velocity = Vector2.zero;
    }


    private void PlayerMovement()
    {
        if (!(isDashing || isWallSliding || isWallJumping))
        //if (!(isDashing))
        {// Changed If-else conditions to switch cases


            switch (lastPlatformTouched)
            {
                case "Slippery platform":
                    rb.AddForce(new Vector2(currentSpeed * PlayerInput().x * slipMultiplier, rb.velocity.y));
                    rb.velocity = new Vector2(Mathf.Clamp(Mathf.MoveTowards(rb.velocity.x, 0, iceGripValue), -maxIceSpeed, maxIceSpeed), rb.velocity.y/*Mathf.MoveTowards(rb.velocity.y, 0, iceGripValue)*/);
                    break;

                case "Sticky platform":
                    
                    break;

                default:
                    rb.velocity = new Vector2(currentSpeed * PlayerInput().x, rb.velocity.y);

                    break;
            }

            //animator.SetFloat("Speed", math.abs(rb.velocity.x));
        }

    }

    Vector2 PlayerInput()
    {
        return move.ReadValue<Vector2>();
    }

    IEnumerator Dash()
    {
        if (dash.WasPressedThisFrame() && (!isDashing) && dashsLeft != 0)
        {
            //Vector2 direction = move.ReadValue<Vector2>();
            //Vector2 storing = rb.velocity;

            isDashing = true;
            dashsLeft--;
            rb.velocity = Vector2.zero;
            rb.gravityScale = dashGravity;
            yield return new WaitForSecondsRealtime(0.05f);
            rb.velocity = (PlayerInput() != Vector2.zero) ? new Vector2(dashSpeed * PlayerInput().x, dashSpeed * PlayerInput().y * vertDashDamp) :
                          new Vector2(dashSpeed * (transform.localScale.x / 4), 0);

            yield return new WaitForSecondsRealtime(dashTime);

            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, float.MinValue, vertMaxDash));
            rb.gravityScale = normalCharGravity;
            isDashing = false;
            lastPlatformTouched = "ground";
        }
    }

    IEnumerator WallJump()
    {
        if (!IsGrounded())
        {
            isWallJumping = true;
            Flip();
            rb.velocity = Vector2.zero;
            jumpsLeft = 0;
            rb.velocity = new Vector2(wallJumpingPower.x * (transform.localScale.x / 4), wallJumpingPower.y);
            yield return new WaitForSecondsRealtime(wallJumpingTime);
            isWallJumping = false;
        }
    }
    void Jump()
    {
        if (jumpsLeft > 0 && jump.WasPressedThisFrame() && !isWallJumping)
        {

            if (isWallSliding)
            {

                //isWallJumping = true;
                //isFacingRight = !isFacingRight;
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                //rb.velocity = new Vector2(wallJumpSpeed * transform.localScale.x, wallJumpForce);
                //rb.gravityScale /= 2;
                //yield return new WaitForSecondsRealtime(wallJumpTime);
                //rb.gravityScale = normalCharGravity;
                //rb.velocity = new Vector2(rb.velocity.x, 0f);
                //isWallJumping = false;
                StartCoroutine(WallJump());


            }


            else
            {
                isJumping = true;

                //animator.SetBool("isJumping", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                --jumpsLeft;
                //rb.gravityScale /= 2;   


            }
        }
        if (rb.velocity.y <= 0f)
            isJumping = false;

        //    rb.gravityScale = normalCharGravity;
    }



    //private void WallJump()
    //{
    //    if (isWallSliding)
    //    {
    //        isWallJumping = false;
    //        wallJumpingDirection = -transform.localScale.x;
    //        wallJumpingCounter = wallJumpingTime;

    //        CancelInvoke(nameof(StopWallJumping));
    //    }
    //    else
    //    {
    //        wallJumpingCounter -= Time.deltaTime;
    //    }

    //    if (jump.WasPressedThisFrame() && wallJumpingCounter > 0f)
    //    {
    //        isWallJumping = true;
    //        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
    //        wallJumpingCounter = 0f;

    //        if (transform.localScale.x != wallJumpingDirection)
    //        {
    //            isFacingRight = !isFacingRight;
    //            Vector3 localScale = transform.localScale;
    //            localScale.x *= -1f;
    //            transform.localScale = localScale;
    //        }

    //        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    //    }
    //}

    //private void StopWallJumping()
    //{
    //    isWallJumping = false;
    //}

    void DashReset()
    {
        if (!isDashing && dashsLeft != extraDashs)
            dashsLeft = extraDashs * Convert.ToInt32(IsGrounded()/* && dashsLeft != extraDashs*/);

    }

    void JumpReset()
    {
        if (jumpsLeft != extraJumps)
            jumpsLeft = extraJumps * 1/* && jumpsLeft != extraJumps*/;
    }


    bool IsGrounded(string testAgainst = "ground")
    {
        Collider2D collidingWith = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);

        if (collidingWith)
            lastPlatformTouched = collidingWith.gameObject.tag;

        if (testAgainst == "ground")
            return collidingWith;
        else
            return lastPlatformTouched == testAgainst;
    }

    //void Sliding()
    //{
    //   Collider2D collidingWith = Physics2D.OverlapCircle(wallCheck.position, 0.3f, groundLayer);
    //   if (collidingWith && !IsGrounded() && movementSpeed != 0f)
    //   {
    //        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed,float.MaxValue));
    //        //rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*slipMultiplier);

    //        isSliding = true;
    //        lastPlatformTouched = "ground";
    //   }
    //    else
    //    {

    //        isSliding = false;
    //    }

    //}
    private void Sliding()
    {
        Collider2D collidingWith = Physics2D.OverlapCircle(wallCheck.position, 0.3f, wallLayer);
        if (collidingWith)
        {
            if (isJumping)
            {
                jumpsLeft = 0;
            }
            if (!IsGrounded() && movementSpeed != 0f && !isJumping)
            {
                isWallSliding = true;
                rb.velocity = Vector2.zero;
                rb.velocity = new Vector2(rb.velocity.x + (transform.localScale.x * wallPush), Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                lastPlatformTouched = "ground";
                JumpReset();
            }

        }
        else
        {
            isWallSliding = false;
        }
    }
    public void Teleport()
    {
        if (currentTeleporter != null && isDashing)
        {
            transform.position = currentTeleporter.GetComponent<TeleportScript>().Destination.position;
            isDashing = false;
        }
    }

    private void FlipCheck()
    {
        if (!isWallJumping)
        {
            float movementSpeed = PlayerInput().x;
            if (isFacingRight && movementSpeed < 0f || !isFacingRight && movementSpeed > 0f)
            {
                Flip();
            }
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (IsGrounded("Sticky Platform"))
    //    {
    //        rb.velocity += new Vector2(direction, 0f);

    //    }
    //}
}