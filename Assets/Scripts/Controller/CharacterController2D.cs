using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f; // How much to smooth out the movement
    [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
    [SerializeField] private Transform groundCheck; // A position marking where to check if the player is grounded.
    [SerializeField] private Transform wallCheck; // Position controlling if the character touches a wall


    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool grounded; // Whether or not the player is grounded.
    private Rigidbody2D _rigidbody2D;
    private bool facingRight = true; // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;
    private float limitFallSpeed = 25f; // Limit fall speed

    public bool canDoubleJump = true; // If player can double jump

    [SerializeField] private float dashForce = 25f;
    private bool canDash = true;
    private bool isDashing = false; // If player is dashing
    private bool isWall = false; // If there is a wall in front of the player
    private bool isWallSliding = false; // If player is sliding in a wall
    private bool oldWallSliding = false; // If player is sliding in a wall in the previous frame
    private float prevVelocityX = 0f;
    private bool canCheck = false; // For check if player is wallsliding

    public float life = 10f; // Life of the player
    public bool invincible = false; // If player can die
    private bool canMove = true; // If player can move

    private Animator animator;
    public ParticleSystem particleJumpUp; // Trail particles
    public ParticleSystem particleJumpDown; // Explosion particles

    private float jumpWallStartX = 0;
    private float jumpWallDistX = 0; // Distance between player and wall
    private bool limitVelOnWallJump = false; // For limit wall jump distance with low fps

    // doing what on ...
    [Header("Events")] [Space] public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool>
    {
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        OnFallEvent ??= new UnityEvent(); // when OnFallEvent == null
        OnLandEvent ??= new UnityEvent();
    }


    private void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, k_GroundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }

            if (!wasGrounded)
            {
                OnLandEvent.Invoke();
                if (!isWall && !isDashing)
                    particleJumpDown.Play();
                canDoubleJump = true;
                if (_rigidbody2D.velocity.y < 0f)
                    limitVelOnWallJump = false;
            }
        }

        isWall = false;

        if (!grounded)
        {
            OnFallEvent.Invoke();
            Collider2D[] collidersWall =
                Physics2D.OverlapCircleAll(wallCheck.position, k_GroundedRadius, whatIsGround);
            for (int i = 0; i < collidersWall.Length; i++)
            {
                if (collidersWall[i].gameObject != null)
                {
                    isDashing = false;
                    isWall = true;
                }
            }

            prevVelocityX = _rigidbody2D.velocity.x;
        }

        if (limitVelOnWallJump)
        {
            if (_rigidbody2D.velocity.y < -0.5f)
                limitVelOnWallJump = false;
            jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
            if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
            {
                canMove = true;
            }
            else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
            {
                canMove = true;
                _rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, _rigidbody2D.velocity.y);
            }
            else if (jumpWallDistX < -2f)
            {
                limitVelOnWallJump = false;
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            }
            else if (jumpWallDistX > 0)
            {
                limitVelOnWallJump = false;
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            }
        }
    }


    /// <summary>
    /// Move is called in FixedUpdate
    /// </summary>
    /// <param name="moveSpeed">speed to move</param>
    /// <param name="jump">invoke a jump</param>
    /// <param name="dash">invoke a dash</param>
    public void Move(float moveSpeed, bool jump, bool dash)
    {
        if (!canMove) return;
        if (dash && canDash && !isWallSliding)
        {
            // dashing
            // Rigidbody2D.AddForce(new Vector2(transform.localScale.x * dashForce, 0f));
            StartCoroutine(DashCooldown());
        }

        // If crouching, check to see if the character can stand up
        if (isDashing)
        {
            _rigidbody2D.velocity = new Vector2(transform.localScale.x * dashForce, 0);
        }
        // only control the player if grounded or airControl is turned on
        else if (grounded || airControl)
        {
            if (_rigidbody2D.velocity.y < -limitFallSpeed)
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -limitFallSpeed);
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref velocity,
                movementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (moveSpeed > 0 && !facingRight && !isWallSliding)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (moveSpeed < 0 && facingRight && !isWallSliding)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (grounded && jump)
        {
            // Add a vertical force to the player.
            animator.SetBool("IsJumping", true);
            animator.SetBool("JumpUp", true);
            grounded = false;
            _rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            canDoubleJump = true;
            particleJumpDown.Play();
            particleJumpUp.Play();
        }
        else if (!grounded && jump && canDoubleJump && !isWallSliding)
        {
            canDoubleJump = false;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            _rigidbody2D.AddForce(new Vector2(0f, jumpForce / 1.2f));
            animator.SetBool("IsDoubleJumping", true);
        }

        else if (isWall && !grounded)
        {
            if (!oldWallSliding && _rigidbody2D.velocity.y < 0 || isDashing)
            {
                isWallSliding = true;
                wallCheck.localPosition =
                    new Vector3(-wallCheck.localPosition.x, wallCheck.localPosition.y, 0);
                Flip();
                StartCoroutine(WaitToCheck(0.1f));
                canDoubleJump = true;
                animator.SetBool("IsWallSliding", true);
            }

            isDashing = false;

            if (isWallSliding)
            {
                if (moveSpeed * transform.localScale.x > 0.1f)
                {
                    StartCoroutine(WaitToEndSliding());
                }
                else
                {
                    oldWallSliding = true;
                    _rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
                }
            }

            if (jump && isWallSliding)
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("JumpUp", true);
                _rigidbody2D.velocity = new Vector2(0f, 0f);
                _rigidbody2D.AddForce(new Vector2(transform.localScale.x * jumpForce * 1.2f, jumpForce));
                jumpWallStartX = transform.position.x;
                limitVelOnWallJump = true;
                canDoubleJump = true;
                isWallSliding = false;
                animator.SetBool("IsWallSliding", false);
                oldWallSliding = false;
                wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x),
                    wallCheck.localPosition.y, 0);
                canMove = false;
            }
            else if (dash && canDash)
            {
                isWallSliding = false;
                animator.SetBool("IsWallSliding", false);
                oldWallSliding = false;
                wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x),
                    wallCheck.localPosition.y, 0);
                canDoubleJump = true;
                StartCoroutine(DashCooldown());
            }
        }
        else if (isWallSliding && !isWall && canCheck)
        {
            isWallSliding = false;
            animator.SetBool("IsWallSliding", false);
            oldWallSliding = false;
            wallCheck.localPosition =
                new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
            canDoubleJump = true;
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ApplyDamage(float damage, Vector3 position)
    {
        if (!invincible)
        {
            animator.SetBool("Hit", true);
            life -= damage;
            Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(damageDir * 10);
            if (life <= 0)
            {
                StartCoroutine(WaitToDead());
            }
            else
            {
                StartCoroutine(Stun(0.25f));
                StartCoroutine(MakeInvincible(1f));
            }
        }
    }

    IEnumerator DashCooldown()
    {
        animator.SetBool("IsDashing", true);
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        yield return new WaitForSeconds(0.5f);
        canDash = true;
    }

    IEnumerator Stun(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator MakeInvincible(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    IEnumerator WaitToMove(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator WaitToCheck(float time)
    {
        canCheck = false;
        yield return new WaitForSeconds(time);
        canCheck = true;
    }

    IEnumerator WaitToEndSliding()
    {
        yield return new WaitForSeconds(0.1f);
        canDoubleJump = true;
        isWallSliding = false;
        animator.SetBool("IsWallSliding", false);
        oldWallSliding = false;
        wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
    }

    IEnumerator WaitToDead()
    {
        animator.SetBool("IsDead", true);
        canMove = false;
        invincible = true;
        GetComponent<Attack>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}