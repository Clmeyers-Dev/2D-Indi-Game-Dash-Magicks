using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame updateusing System.Collections;
    public Abilities abilities;
    public PlayerManager playerManager;
    public bool jump;
   
    [SerializeField] public float xDirectionalInput;
    public UnityEvent OnLandEvent;
    [SerializeField] float airMoveSpeed = 10f;
    [Header("for wallSliding")]
    [SerializeField] public float wallSlideSpeed = 0;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public Transform wallCheckPoint;
    [SerializeField] public Vector2 wallCheckSize;
    [Header("for wall jumping")]
    [SerializeField] public float wallJumpForce = 18;
    public float wallJumpDirection = -1f;
    [SerializeField] public Vector2 wallJumpAngle;
    public bool isTouchingWall;
    public bool isWallSliding;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    const float k_GroundedRadius = .2f; //
                                        //
    const float k_CeilingRadius = .2f; //  can stand up
    public float falling;
    public float maxSpeed = 3.4f;
    public float jumpHeight = 6.5f;
    public float gravityScale = 1.5f;
    public Camera mainCamera;
    public bool airControl;
    bool facingRight = true;
    public float moveDirection = 0;
    public bool canDoubleJump;
    [SerializeField] public bool isGrounded = false;
    Vector3 cameraPos;
    public Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;
    public bool jumpend;
    public bool wallJumpKey = false;

    [Header("Animation Flags")]
    public bool isWalking;
    public bool isJumpingUp;
    public bool isFalling;

    // Use this for initialization
    void Start()
    {
        abilities = GetComponent<Abilities>();
        //Obtains the componets needed
        playerManager = GetComponent<PlayerManager>();
        wallJumpAngle.Normalize();
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        //sets our transform
        t = transform;
        //freezes the rotation and collsion detection for our rigid body if it has not already been set
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;
        //obtains the postion of the main player camera
        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }

    }

    // Update is called once per frame


    void Update()
    {
        xDirectionalInput = Input.GetAxis("Horizontal");
        falling = r2d.velocity.y;
        isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
        // Movement controls
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))||(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && (isGrounded || Mathf.Abs(r2d.velocity.x) > 0.01f))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
            isWalking = true;
        }
        else
        {
            isWalking = false;
            if (isGrounded || r2d.velocity.magnitude < 0.01f)
            {
                moveDirection = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z) && r2d.velocity.y > 0)
        {
            jumpend = true;
        }
        else
        {
            jumpend = false;
        }
        if(falling>0){
            isJumpingUp = true;
            isFalling = false;
        }
        if(falling<0){
            isFalling = true;
            isJumpingUp = false;
        }
        // 
        // Change facing direction
        if (moveDirection != 0)
        {
            if (xDirectionalInput > 0 && !facingRight)
            {
                if (!isWallSliding)
                {
                    wallJumpDirection *= -1;
                    facingRight = true;
                    t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
                }

            }
            if (xDirectionalInput < 0 && facingRight)
            {
                if (!isWallSliding)
                {
                    wallJumpDirection *= -1;
                    facingRight = false;
                    t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
                }
            }
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded)
        {
            jump = true;
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            canDoubleJump = true;
        }
        else
        {
            if(canDoubleJump&&Input.GetKeyDown(KeyCode.Z)){
                canDoubleJump = false;
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            }
            jump = false;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            wallJumpKey = true;
        }
        else
        {
            wallJumpKey = false;
        }

    }
   
    private void Awake()
    {
        r2d = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }
    void FixedUpdate()
    {
        wallSlide();
        wallJump();
        bool wasGrounded = isGrounded;
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
        if (isGrounded&&!abilities.dashing)
        {
            r2d.velocity = new Vector2((xDirectionalInput) * maxSpeed, r2d.velocity.y);
            //var move =new  Vector3 (Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
            //transform.position += move * maxSpeed * Time.deltaTime;
        }
        else if (!isGrounded && !isWallSliding && xDirectionalInput != 0&&!abilities.dashing)
        {
           r2d.AddForce(new Vector2(airMoveSpeed * xDirectionalInput, 0));
            //var move =new  Vector3 (Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
           //transform.position += move * airMoveSpeed * Time.deltaTime;
            if (Mathf.Abs(r2d.velocity.x) > maxSpeed)
            {
                 //var move =new  Vector3 (Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
                //transform.position += move * maxSpeed * Time.deltaTime;
                r2d.velocity = new Vector2(xDirectionalInput * maxSpeed, r2d.velocity.y);
            }
        }
        if(jumpend){
             r2d.velocity = new Vector2(r2d.velocity.x, r2d.velocity.y * .1f);
         }

    }
    void wallSlide()
    {
        if (isTouchingWall && !isGrounded && r2d.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
        //wall slide;
        if (isWallSliding)
        {
            r2d.velocity = new Vector2(r2d.velocity.x, wallSlideSpeed);
        }
    }
    void wallJump()
    {

        if ((isWallSliding || isTouchingWall) && wallJumpKey)
        {

            r2d.velocity = new Vector2(0, 0);
            r2d.AddForce(new Vector2(wallJumpForce * wallJumpDirection * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
            wallJumpDirection *= -1;
            facingRight = false;
            t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);

            wallJumpKey = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
    }
}
