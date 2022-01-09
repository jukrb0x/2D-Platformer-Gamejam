using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float dmgValue = 4;
    public GameObject throwableObject;
    public Transform attackCheck;
    private Rigidbody2D _rigidbody2D;
    public Animator animator;
    public bool canAttack = true;
    public bool canThrow = false;
    public bool isTimeToCheck = false;
    [SerializeField] private GameManager gameManager;

    public GameObject cam;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager ??= GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsPaused) return;

        // sword attack
        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            canAttack = false;
            animator.SetBool("IsAttacking", true);
            // HasExitTime will exit Attacking State in animator
            StartCoroutine(AttackCooldown());
        }

        // throwable attack
        if (Input.GetKeyDown(KeyCode.K) && canThrow)
        {
            Transform objTransform = transform;

            // instantiate a throwable
            GameObject throwableWeapon =
                Instantiate(throwableObject,
                    objTransform.position + new Vector3(objTransform.localScale.x * 0.5f, -0.2f),
                    Quaternion.identity);

            // set direction of the throwable 
            Vector2 direction = new Vector2(objTransform.localScale.x, 0);
            throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
            throwableWeapon.name = "ThrowableWeapon";
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }

    public void DoDashDamage()
    {
        dmgValue = Mathf.Abs(dmgValue);
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.CompareTag("Enemy"))
            {
                if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
                {
                    dmgValue = -dmgValue;
                }

                collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
                cam.GetComponent<CameraFollow>().ShakeCamera();
            }
        }
    }
}