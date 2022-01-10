using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ally : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;

    [SerializeField]
    private bool currentFacingRight = true; // For determining which way the player is currently facing.

    private bool isFacingRight = true;
    public float life = 10;
    public float speed = 5f;

    public bool isInvincible = false;
    private bool isHit = false;

    [SerializeField] private float m_DashForce = 25f;
    private bool isDashing = false;

    public GameObject enemy;
    private float distToPlayer;
    private float distToPlayerY;
    public float meleeDist = 1.5f;
    public float rangeDist = 5f;
    private bool canAttack = true;
    [SerializeField] private bool canDash = true;
    private Transform attackCheck;
    public float dmgValue = 4;

    public GameObject throwableObject;

    private float randomDecision = 0;
    private bool doOnceDecision = true;
    private bool endDecision = false;
    private Animator anim;

    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        attackCheck = transform.Find("AttackCheck").transform;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        isFacingRight = currentFacingRight; // sync at the start
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (life <= 0)
        {
            StartCoroutine(DestroyEnemy());
        }
        else if (enemy != null) // predefined enemy
        {
            if (isDashing)
            {
                m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
            }
            else if (!isHit)
            {
                distToPlayer =
                    enemy.transform.position.x - transform.position.x; // >0 is right to player, <0 is left to player
                distToPlayerY = enemy.transform.position.y - transform.position.y;

                if (Mathf.Abs(distToPlayer) < 0.25f)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
                    anim.SetBool("IsWaiting", true);
                }
                else if (Mathf.Abs(distToPlayer) > 0.25f && Mathf.Abs(distToPlayer) < meleeDist &&
                         Mathf.Abs(distToPlayerY) < 2f)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
                    if ((distToPlayer > 0f && transform.localScale.x > 0f) || // facing right but player is left
                        (distToPlayer < 0f && transform.localScale.x < 0f)) // facing left but player is right
                        Flip();
                    if (canAttack)
                    {
                        MeleeAttack();
                    }
                }
                else if (Mathf.Abs(distToPlayer) > meleeDist && Mathf.Abs(distToPlayer) < rangeDist)
                {
                    anim.SetBool("IsWaiting", false);
                    m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed,
                        m_Rigidbody2D.velocity.y);
                }
                else
                {
                    if (!endDecision)
                    {
                        if ((distToPlayer > 0f && transform.localScale.x < 0f) ||
                            (distToPlayer < 0f && transform.localScale.x > 0f))
                            Flip();

                        if (randomDecision < 0.4f)
                            Run();
                        else if (randomDecision >= 0.4f && randomDecision < 0.6f)
                            Jump();
                        else if (randomDecision >= 0.6f && randomDecision < 0.8f && canDash)
                            StartCoroutine(Dash());
                        else if (randomDecision >= 0.8f && randomDecision < 0.95f)
                            RangeAttack();
                        else
                            Idle();
                    }
                    else
                    {
                        endDecision = false;
                    }
                }
            }
            else if (isHit)
            {
                if ((distToPlayer > 0f && transform.localScale.x > 0f) ||
                    (distToPlayer < 0f && transform.localScale.x < 0f))
                {
                    Flip();
                    if (canDash)
                    {
                        StartCoroutine(Dash());
                    }
                }
                else if (canDash)
                {
                    StartCoroutine(Dash());
                }
            }
        }
        else
        {
            enemy = GameObject.Find("Player"); // TODO FIXME find player
        }

        if (transform.localScale.x * m_Rigidbody2D.velocity.x > 0 && !currentFacingRight && life > 0)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (transform.localScale.x * m_Rigidbody2D.velocity.x < 0 && currentFacingRight && life > 0)
        {
            // ... flip the player.
            Flip();
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ApplyDamage(float damage)
    {
        if (!isInvincible)
        {
            float direction = damage / Mathf.Abs(damage);
            damage = Mathf.Abs(damage);
            anim.SetBool("Hit", true);
            life -= damage;
            transform.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            transform.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 300f, 100f));
            StartCoroutine(HitTime());
        }
    }

    public void MeleeAttack()
    {
        // transform.GetComponent<Animator>().SetBool("Attack", true);
        anim.SetBool("Attack", true);
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Enemy" && collidersEnemies[i].gameObject != gameObject)
            {
                if (transform.localScale.x < 1)
                {
                    dmgValue = -dmgValue;
                }

                collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
            }
            else if (collidersEnemies[i].gameObject.tag == "Player")
            {
                collidersEnemies[i].gameObject.GetComponent<CharacterController2D>()
                    .ApplyDamage(2f, transform.position);
            }
        }

        StartCoroutine(WaitToAttack(0.5f));
    }

    public void RangeAttack()
    {
        if (doOnceDecision)
        {
            GameObject throwableProj = Instantiate(throwableObject,
                transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f),
                Quaternion.identity) as GameObject;
            throwableProj.GetComponent<ThrowableProjectile>().owner = gameObject;
            Vector2 direction = new Vector2(transform.localScale.x, 0f);
            throwableProj.GetComponent<ThrowableProjectile>().direction = direction;
            StartCoroutine(NextDecision(0.5f));
        }
    }

    public void Run()
    {
        anim.SetBool("IsWaiting", false);
        m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
        if (doOnceDecision)
            StartCoroutine(NextDecision(0.5f));
    }

    public void Jump()
    {
        Vector3 targetVelocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
        Vector3 velocity = Vector3.zero;
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, 0.05f);
        if (doOnceDecision)
        {
            anim.SetBool("IsWaiting", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, 850f));
            StartCoroutine(NextDecision(1f));
        }
    }

    public void Idle()
    {
        m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
        if (doOnceDecision)
        {
            anim.SetBool("IsWaiting", true);
            StartCoroutine(NextDecision(1f));
        }
    }

    public void EndDecision()
    {
        randomDecision = Random.Range(0.0f, 1.0f);
        endDecision = true;
    }

    IEnumerator HitTime()
    {
        isInvincible = true;
        isHit = true;
        yield return new WaitForSeconds(0.1f);
        isHit = false;
        isInvincible = false;
    }

    IEnumerator WaitToAttack(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    IEnumerator Dash()
    {
        anim.SetBool("IsDashing", true);
        isDashing = true;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        EndDecision();
    }

    IEnumerator NextDecision(float time)
    {
        doOnceDecision = false;
        yield return new WaitForSeconds(time);
        EndDecision();
        doOnceDecision = true;
        anim.SetBool("IsWaiting", false);
    }

    IEnumerator DestroyEnemy()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        capsule.size = new Vector2(1f, 0.25f);
        capsule.offset = new Vector2(0f, -0.8f);
        capsule.direction = CapsuleDirection2D.Horizontal;
        transform.GetComponent<Animator>().SetBool("IsDead", true);
        yield return new WaitForSeconds(0.25f);
        m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}