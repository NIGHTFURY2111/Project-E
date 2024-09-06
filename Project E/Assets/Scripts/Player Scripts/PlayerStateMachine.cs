using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static PlayerStateMachine;

public class PlayerStateMachine : MonoBehaviour
{
    #region variables

    [System.Serializable]
    public class GeneralSettings
    {
        public float maxFallingVelocity;
        public Playerinput playerMovement;
        public Transform groundCheck;
        public LayerMask groundLayer;
    }


    [System.Serializable]
    public class MovementSettings
    {
        public float playerGravity;
        public float movementSpeed;
        public float currentSpeed;
        public float direction;
        public bool isFacingRight = true;
    }


    [System.Serializable]
    public class DashSettings
    {
        //public int extraDashs;
        public float dashSpeed;
        public float _dashTime;
        public float dashGravity;
        public float vertDashDamp;
        public float vertMaxDash;
        public bool isDashing = false;
        public int dashsLeft;
    }
    

    [System.Serializable]
    public class JumpSettings
    {
        public float jumpForce;
        public float jumpGravity;
        public int extraJumps;
        public int jumpsLeft;
        public bool isJumping = false;
    }
    #endregion

    [SerializeField] GeneralSettings generalSettings;
    [SerializeField] MovementSettings movementSettings;
    [SerializeField] DashSettings dashSettings;
    [SerializeField] JumpSettings jumpSettings;
    //[SerializeField] IceSettings iceSettings;
    //[SerializeField] WallSettings wallSettings;
    //[SerializeField] RespawnSettings respawnSettings;
    StateFactory stateFactory;
    BaseState _currentState;
    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rb;
    InputAction _move;
    InputAction _jump;
    InputAction _dash;
    InputAction grab;
    Vector2 _moveDirection = Vector2.zero;
    public float gravity;
    float distanceToGround;
    public event Action<bool> PlayerGroundedEvent;
    public event Action<bool> playerWallEvent;


    #region necessary input system calls
    private void Awake()
    {

        stateFactory = new StateFactory(this);
        rb= GetComponent<Rigidbody2D>();
        capsuleCollider= GetComponent<CapsuleCollider2D>();

        generalSettings.playerMovement = new();

        _move = generalSettings.playerMovement.Player.Move;
        _jump = generalSettings.playerMovement.Player.Jump;
        _dash = generalSettings.playerMovement.Player.Dash;
        grab = generalSettings.playerMovement.Player.Grab;

    }
    private void OnEnable()
    {
        _move.Enable();
        _jump.Enable();
        _dash.Enable();
        grab.Enable();
    }


    private void OnDisable()
    {
        _move.Disable();
        _jump.Disable();
        _dash.Disable();
        grab.Disable();
    }

    #endregion

    private void Start()
    {

        PlayerGroundedEvent += OnGroundCheck;
        gravity = movementSettings.playerGravity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentState = stateFactory.Idle();
        _currentState.EnterState();

        //calculating the distance to ground cuz local scale n capsule collider are both lying (something about vectors n all that)
        distanceToGround = capsuleCollider.size.y * transform.localScale.y / 2; 
    }

    void Update()
    {
        FlipCheck();
        //CheckRespawn();
        _currentState.UpdateState();

        RayCasts();

    }


    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
        Artificialgravity();
        rb.velocity = _moveDirection;
    }

    private void LateUpdate()
    {

    }
    private void FlipCheck()
    {
        float movementSpeed = rb.velocity.x;
        if ((movementSettings.isFacingRight && movementSpeed < 0f )|| (!movementSettings.isFacingRight && movementSpeed > 0f))
        {
            Flip();
        }
    }
    private void Flip()
    {
        movementSettings.isFacingRight = !movementSettings.isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    public void Artificialgravity()
    {
        _moveDirection.y = Math.Clamp
            (
                _moveDirection.y - gravity,
                (isGrounded)? -0.2f : generalSettings.maxFallingVelocity, // -.2f is applied on ground, dont set it too high
                int.MaxValue
            );
    }

    //void CheckRespawn()
    //{
    //    if (transform.position.y < respawnSettings.threshold)
    //        Respawn();
    //}
    //public void Respawn()
    //{
    //    transform.position = respawnSettings.RespawnPoint.transform.position;
    //    rb.velocity = Vector2.zero;

    //}

    public void RayCasts()
    {
        //raycsast for ground check
        PlayerGroundedEvent?.Invoke(Physics2D.Raycast(transform.position, Vector2.down, distanceToGround + 0.1f, generalSettings.groundLayer));
        Debug.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y- distanceToGround + 0.1f),Color.red);

        // raycast for wall Check

    }

    private void OnGroundCheck(bool IsGrounded)
    {
        isGrounded = IsGrounded;
    }


    #region Getters Setters

    
    public Vector2 playerInput { get => _move.ReadValue<Vector2>(); }
    public float moveDirectionX { get => _moveDirection.x; set => _moveDirection.x = value; }
    public float moveDirectionY { get => _moveDirection.y; set => _moveDirection.y = value; }
    public float playerMoveSpeed { get => movementSettings.movementSpeed; }
    public float dashSpeedMultiplier { get => dashSettings.dashSpeed; }
    public float dashTime { get => dashSettings._dashTime; }
    public float playerJumpingForce { get => jumpSettings.jumpForce; }
    public float jumpGravity { get => jumpSettings.jumpGravity; }
    public float dashGravity { get => dashSettings.dashGravity; }
    public float normalGravity { get => movementSettings.playerGravity; }
    //public float currentGravity { get => movementSettings.playerGravity; set => movementSettings.playerGravity = value; }
    public BaseState currentState { get => _currentState; set => _currentState = value; }
    public InputAction moveInput { get=>_move; set => _move = value; }
    public InputAction jumpInput { get=>_jump; set => _jump = value; }
    public InputAction dashInput { get=>_dash; set => _dash = value; }

    public bool isGrounded { get; private set; }

    #endregion
}