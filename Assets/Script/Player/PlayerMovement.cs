using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private Collision_Check collisionCheck;
     // Reference to the Collision_Check script

    private float horizontal;
    private float speed = 6.8f;
    private float jumpForce = 10f;
    private float coyoteTime = 0.2f;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private int doubleJump = 2;
    private bool canDash = true;
    private bool isDashing;
    private bool isWallSliding; // New variable for wall slide
    public bool isFacingRight = true;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private int doubleJumpLeft = 0;
    [SerializeField] private float coyoteTimer = 0f;
    [SerializeField] private TrailRenderer tr;

    private enum MovementState { idle, running, jumping, falling, dashing, wallSliding } // Added wallSliding state

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource walksoundEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collisionCheck = GetComponent<Collision_Check>(); // Get the Collision_Check component
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if(rb.velocity.x != 0 && IsGrounded())
        {
            if(!walksoundEffect.isPlaying)
            {
                walksoundEffect.Play();
            }
        }
        else{
            walksoundEffect.Stop();
        }
        

        // Jump
        if (!collisionCheck.onGround && collisionCheck.onWall && Input.GetButtonDown("Jump"))
        {
            jumpSoundEffect.Play();
            WallJump();
        }
        else if (!isJumping && Input.GetButtonDown("Jump") && doubleJumpLeft > 0)
        {   
            jumpSoundEffect.Play();
            Jump();
        }

        if (Input.GetButtonDown("Jump") && (collisionCheck.onGround || coyoteTimer > 0f))
        {
            jumpSoundEffect.Play();
            Jump();
        }

        coyoteTimer -= Time.deltaTime;
        if (collisionCheck.onGround)
        {
            coyoteTimer = coyoteTime;
            doubleJumpLeft = doubleJump;
            isJumping = false;
            isWallSliding = false; // Reset wall sliding state
        }

        if (collisionCheck.onWall && !collisionCheck.onGround && rb.velocity.y < 0f) // Check if on wall and falling
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        UpdateAnimationState();
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!isWallSliding || doubleJumpLeft == 0)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, -speed * 0.5f); // Set the velocity to zero horizontally and apply the wall slide velocity vertically
        }

    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void UpdateAnimationState()
    {
        MovementState state;


        if ((horizontal != 0f && IsGrounded()) || isJumping || isWallSliding && IsGrounded())
        {   
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (isDashing || (horizontal > 0f && isDashing) || (horizontal < 0f && isDashing))
        {   
            state = MovementState.dashing;
        }
        else if (rb.velocity.y > .1f && !IsGrounded() && !isDashing && !isWallSliding)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f && !IsGrounded() && !isDashing && !isWallSliding && !collisionCheck.onGround)
        {
            state = MovementState.falling;
        }

        if (isWallSliding && !collisionCheck.onGround && !IsGrounded())
        {
            state = MovementState.wallSliding;
        }

        anim.SetInteger("state", (int)state);
    }

    
    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f && !isWallSliding) || (!isFacingRight && horizontal > 0f && !isWallSliding))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;

            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        doubleJumpLeft--;
    }

    private void WallJump()
    {
        rb.velocity = new Vector2(-collisionCheck.wallSide * speed, jumpForce); // Jump away from the wall
        Flip(); // Flip the player's direction when wall jumping
        isJumping = true;
        doubleJumpLeft = doubleJump;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        tr.emitting = false;
        canDash = true;
    }
}
