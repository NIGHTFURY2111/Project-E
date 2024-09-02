using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    #region variables

    [System.Serializable]
    public class GeneralSettings
    {
    public GameObject currentTeleporter;
    public Playerinput playerMovement;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    }


    [System.Serializable]
    public class MovementSettings
    {
        public float normalCharGravity;
        public float movementSpeed;
        public float stickySpeed;
        public float currentSpeed;
        public float direction;
        public bool isFacingRight = true;
    }


    [System.Serializable]
    public class IceSettings
    {
        public float slipMultiplier;
        public float maxIceSpeed;
        public float iceGripValue;
    }


    [System.Serializable]
    public class DashSettings
    {
        public int extraDashs;
        public float dashSpeed;
        public float dashTime;
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
        public int extraJumps;
        public int jumpsLeft;
        public bool isJumping = false;
    }


    [System.Serializable]
    public class WallSettings 
    { 
        public Transform wallCheck;
        public float wallSlidingSpeed = 2f;
        public float wallJumpingTime;
        public float wallPush;
        public bool isWallSliding = false;
        public bool isWallJumping;
        public Vector2 wallJumpingPower;
    }


    [System.Serializable]
    public class RespawnSettings
    {
        public GameObject RespawnPoint;
        public float threshold;
        public string lastPlatformTouched;
        public Vector3 lastRespawnPoint;
    }


    StateFactory stateFactory;
    BaseState _currentState;
    Rigidbody2D rb;
    [SerializeField] GeneralSettings generalSettings;
    [SerializeField] MovementSettings movementSettings;
    [SerializeField] IceSettings iceSettings;
    [SerializeField] DashSettings dashSettings;
    [SerializeField] JumpSettings jumpSettings;
    [SerializeField] WallSettings wallSettings;
    [SerializeField] RespawnSettings respawnSettings;
    InputAction _move;
    InputAction jump;
    InputAction dash;
    InputAction grab;
    Vector2 _moveDirection = Vector2.zero;
    public event Action<bool> PlayerGroundedEvent;


    #endregion
    #region necessary input system calls
    private void Awake()
    {
        stateFactory = new StateFactory(this);
        rb= GetComponent<Rigidbody2D>();

        generalSettings.playerMovement = new();

        _move = generalSettings.playerMovement.Player.Move;
        jump = generalSettings.playerMovement.Player.Jump;
        dash = generalSettings.playerMovement.Player.Dash;
        grab = generalSettings.playerMovement.Player.Grab;

    }
    private void OnEnable()
    {
        _move.Enable();
        jump.Enable();
        dash.Enable();
        grab.Enable();
    }


    private void OnDisable()
    {
        _move.Disable();
        jump.Disable();
        dash.Disable();
        grab.Disable();
    }

    #endregion

    private void Start()
    {

        PlayerGroundedEvent += OnGroundCheck;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentState = stateFactory.Idle();
        _currentState.EnterState();

        respawnSettings.lastRespawnPoint = respawnSettings.RespawnPoint.transform.position;
        Respawn();

    }

    void Update()
    {
        FlipCheck();
        CheckRespawn();
        Artificialgravity();
        _currentState.UpdateState();

        PlayerGroundedEvent?.Invoke(Physics2D.Raycast(transform.position, Vector2.down, (transform.localScale.y / 2), generalSettings.groundLayer));            
        Debug.Log(Physics2D.Raycast(transform.position, Vector2.down, (transform.localScale.y / 2), generalSettings.groundLayer).collider.gameObject.name);

    }


    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
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
        if (isGrounded)
        {
            _moveDirection.y = Math.Clamp(_moveDirection.y, -2, int.MaxValue);
        }
        else
        {
            _moveDirection.y -= movementSettings.normalCharGravity * Time.deltaTime;
        }
    }

    void CheckRespawn()
    {
        if (transform.position.y < respawnSettings.threshold)
            Respawn();
    }
    public void Respawn()
    {
        transform.position = respawnSettings.RespawnPoint.transform.position;
        rb.velocity = Vector2.zero;

    }

    public void Teleport()
    {
        if (generalSettings.currentTeleporter != null && dashSettings.isDashing)
        {
            transform.position = generalSettings.currentTeleporter.GetComponent<TeleportScript>().Destination.position;
            dashSettings.isDashing = false;
        }
    }

    public void GroundCheck()
    {
        Physics2D.Raycast(transform.position, Vector2.down, (transform.localScale.y / 2) + 0.01f, generalSettings.groundLayer);
    }

    private void OnGroundCheck(bool IsGrounded)
    {
        isGrounded = IsGrounded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);
    }

    #region Getters Setters


    public Vector2 playerInput { get => _move.ReadValue<Vector2>(); }
    public float playerMoveSpeed { get => movementSettings.movementSpeed; }
    public BaseState currentState { get { return _currentState; } set { _currentState = value; } }
    public float moveDirectionX { get { return _moveDirection.x; } set { _moveDirection.x = value; } }
    public float moveDirectionY { get { return _moveDirection.y; } set { _moveDirection.y = value; } }
    public InputAction moveInput { get=>_move; set => _move = value; }
    public static bool isGrounded { get; private set; }


    #endregion
}