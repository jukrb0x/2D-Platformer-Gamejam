using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private int scoreAward = 5;
    public float life = 10;
    private bool isPlat;
    private bool isObstacle;
    private Transform fallCheck;
    private Transform wallCheck;
    public LayerMask turnLayerMask;
    private Rigidbody2D rb;

    [SerializeField] private bool facingRight = true;

    public float speed = 5f;

    public bool isInvincible = false;
    private bool isHit = false;

    void Awake()
    {
        fallCheck = transform.Find("FallCheck");
        wallCheck = transform.Find("WallCheck");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameManager ??= FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (life <= 0)
        {
            transform.GetComponent<Animator>().SetBool("IsDead", true);
            StartCoroutine(DestroyEnemy());
        }

        isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
        isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

        if (!isHit && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f) // be blocked by obstacle
        {
            if (isPlat && !isObstacle && !isHit)
            {
                if (facingRight)
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
            }
            else
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ApplyDamage(float damage)
    {
        if (isInvincible) return;
        float direction = damage / Mathf.Abs(damage);
        damage = Mathf.Abs(damage);
        transform.GetComponent<Animator>().SetBool("Hit", true);
        life -= damage;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(direction * 500f, 100f));
        StartCoroutine(HitTime());
        if (life <= 0)
        {
            gameManager.Score += scoreAward; // add score to player
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && life > 0)
        {
            collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
        }
    }

    IEnumerator HitTime()
    {
        isHit = true;
        isInvincible = true;
        yield return new WaitForSeconds(0.1f);
        isHit = false;
        isInvincible = false;
    }

    IEnumerator DestroyEnemy()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        capsule.size = new Vector2(1f, 0.25f);
        capsule.offset = new Vector2(0f, -0.8f);
        capsule.direction = CapsuleDirection2D.Horizontal;
        yield return new WaitForSeconds(0.25f);
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}