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


    [System.Serializable]
    public class WallSettings
    {
        public float wallJumpForce;
        public float wallGravityClamp;
    }
    #endregion
    
    [SerializeField] GeneralSettings generalSettings;
    [SerializeField] MovementSettings movementSettings;
    [SerializeField] DashSettings dashSettings;
    [SerializeField] JumpSettings jumpSettings;
    [SerializeField] WallSettings wallSettings;
    //[SerializeField] IceSettings iceSettings;
    //[SerializeField] WallSettings wallSettings;
    //[SerializeField] RespawnSettings respawnSettings;
    [SerializeReferenceDropdown]
    [SerializeReference]public StateFactory stateFactory;
    BaseState _currentState;
    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rb;
    InputAction _move;
    InputAction _jump;
    InputAction _dash;
    InputAction _fire;
    Vector2 _moveDirection = Vector2.zero;
    public float gravity;
    float distanceToGround;
    float distanceToWall;
    public event Action<bool> PlayerGroundedEvent;
    public event Action<bool> playerWallEvent;
    public bool canAttack;
   


    #region necessary input system calls
    private void Awake()
    {

        //stateFactory = new StateFactory(this);
        rb= GetComponent<Rigidbody2D>();
        capsuleCollider= GetComponent<CapsuleCollider2D>();
        //stateFactory = new StateFactory(this);
        Debug.Log(stateFactory);
        generalSettings.playerMovement = new();
        
        _currentState = stateFactory.Idle();

        _move = generalSettings.playerMovement.Player.Move;
        _jump = generalSettings.playerMovement.Player.Jump;
        _dash = generalSettings.playerMovement.Player.Dash;
        _fire = generalSettings.playerMovement.Player.Fire;

    }
    private void OnEnable()
    {
        _move.Enable();
        _jump.Enable();
        _dash.Enable();
        _fire.Enable();
    }


    private void OnDisable()
    {
        _move.Disable();
        _jump.Disable();
        _dash.Disable();
        _fire.Disable();
    }

    #endregion

    private void Start()
    {
        PlayerGroundedEvent += OnGroundCheck;
        playerWallEvent += OnWallCheck;
        gravity = movementSettings.playerGravity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //_currentState.EnterState();

        //calculating the distance to ground cuz local scale n capsule collider are both lying (something about vectors n all that)
        distanceToGround = capsuleCollider.size.y * transform.localScale.y / 2; 
        distanceToWall = capsuleCollider.size.x * transform.localScale.x / 2;
    }

    void Update()
    {
        FlipCheck();
        //CheckRespawn();
        stateFactory.update();

        RayCasts();
        attack();

    }


    private void FixedUpdate()
    {
        stateFactory.fixedUpdate();
        Artificialgravity();
        rb.velocity = _moveDirection;
    }

    private void LateUpdate()
    {

    }
    private void FlipCheck()
    {
        float movementSpeed = rb.velocity.x;
        float Playerinput = moveInput.ReadValue<Vector2>().x;

        //if ((movementSettings.isFacingRight && movementSpeed < 0f )|| (!movementSettings.isFacingRight && movementSpeed > 0f))
        if ((movementSettings.isFacingRight && movementSpeed < 0f && Playerinput <0) || (!movementSettings.isFacingRight && movementSpeed > 0f && Playerinput > 0))
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
        //Debug.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y- distanceToGround + 0.1f),Color.red);

        // raycast for wall Check
        playerWallEvent?.Invoke(Physics2D.Raycast(transform.position, (movementSettings.isFacingRight ? Vector2.right:Vector2.left), distanceToWall + 0.5f, generalSettings.groundLayer));
        Debug.Log((bool)Physics2D.Raycast(transform.position, (movementSettings.isFacingRight ? Vector2.right:Vector2.left), distanceToWall + 0.5f, generalSettings.groundLayer)+"    "+touchingWall);
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + ((movementSettings.isFacingRight ? 1 : -1) * (distanceToWall + 0.5f)), transform.position.y), Color.blue);
    }

    private void OnGroundCheck(bool IsGrounded)
    {
        isGrounded = IsGrounded;
    }
    private void OnWallCheck(bool TouchingWall)
    {
        touchingWall = TouchingWall;
    }
    
    void attack()
    {
        if(canAttack && _fire.WasPressedThisFrame())
        {
            Debug.Log("attack");
        }
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
    public float setGravityClamp { get => generalSettings.maxFallingVelocity; set=> generalSettings.maxFallingVelocity = value; }
    public float wallGravityClamp { get => wallSettings.wallGravityClamp; }
    //public float currentGravity { get => movementSettings.playerGravity; set => movementSettings.playerGravity = value; }
    public BaseState currentState { get => _currentState; set => _currentState = value; }
    public InputAction moveInput { get=>_move; set => _move = value; }
    public InputAction jumpInput { get=>_jump; set => _jump = value; }
    public InputAction dashInput { get=>_dash; set => _dash = value; }

    public bool isGrounded { get; private set; }
    public bool touchingWall { get; private set; }

    #endregion
}