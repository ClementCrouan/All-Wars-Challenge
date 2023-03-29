using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class EnemyAI : MonoBehaviour
{
    private float Distance;

    public Animations[] animations;
    
    private float DistanceBase;
    private Vector3 basePosition;

    [HideInInspector]
    public Transform Target;

    public float chaseRange = 10;

    public float attackRange = 2.2f;

    public float attackRepeatTime = 1;
    private float attackTime;

    public int xPWin;

    private NavMeshAgent agent;

    private Animator animator;

    public float enemyHealth;
    private bool isDead = false;
    [HideInInspector]
    public float maxHealth;

    GenerateEnemy generateEnemy;

    bool questEnemy;

    public GameObject[] loots;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        attackTime = Time.time;
        basePosition = transform.position;
        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].attackCooldown = animations[i].currentCooldown;
        }
        maxHealth = enemyHealth;
        if (gameObject.tag == "EnemyQuest")
            questEnemy = true;
        generateEnemy = GameObject.Find("Player").GetComponent<GenerateEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Target = GameObject.Find("Player").transform;

            Distance = Vector3.Distance(Target.position, transform.position);
            
            DistanceBase = Vector3.Distance(basePosition, transform.position);

            if (Distance > chaseRange && Distance > attackRange)
            {
                if (DistanceBase <= 1 | !questEnemy)
                    Idle();
            }

            if (Distance < chaseRange && Distance > attackRange)
            {
                Chase();
            }

            if (Distance < attackRange)
            {
                Attack();
                for (int i = 0; i < animations.Length; i++)
                {
                    if (animations[i].currentCooldown <= 0.4f && !animations[i].attack)
                    {
                        Target.GetComponent<PlayerInventory>().ApplyDamage(animations[i].TheDamage);
                        animations[i].attack = true;
                    }
                }
            }

            if (Distance > chaseRange && DistanceBase > 1 && questEnemy)
            {
                BackBase();
            }

            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i].isAttacking)
                {
                    animations[i].currentCooldown -= Time.deltaTime;
                }

                if (animations[i].currentCooldown <= 0f)
                {
                    animations[i].currentCooldown = animations[i].attackCooldown;
                    animations[i].isAttacking = false;
                    animations[i].attack = false;
                }
            }
        }
    }

    void Chase()
    {
        ChangeAnimation();
        animator.SetBool("Walking", true);
        agent.destination = Target.position;
    }

    void Attack()
    {
        agent.destination = transform.position;
        int randomAttacking = Random.Range(0, 100);

        if (Time.time > attackTime && !Target.GetComponent<Deplacement>().isDead)
        {
            if (randomAttacking < 41)
            {
                animations[0].isAttacking = true;
                ChangeAnimation();
                animator.SetBool("Attacking01", true);
                attackTime = Time.time + attackRepeatTime;
            }

            if (randomAttacking > 40)
            {
                animations[1].isAttacking = true;
                ChangeAnimation();
                animator.SetBool("Attacking02", true);
                attackTime = Time.time + attackRepeatTime;
            }
        }
        else if (Target.GetComponent<Deplacement>().isDead)
        {
            ChangeAnimation();
            animator.SetBool("IsVictory", true);
        }
    }

    void Idle()
    {
        ChangeAnimation();
        animator.SetBool("Idling", true);
    }

    public void ApplyDamage(float TheDamage)
    {
        if (!isDead)
        {
            enemyHealth -= TheDamage;
            
            if (enemyHealth <= 0)
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        isDead = true;
        ChangeAnimation();
        animator.SetBool("IsDead", true);

        int randomNumber = Random.Range(0, loots.Length);
        GameObject finalLoot = loots[randomNumber];

        Destroy(transform.gameObject, 5);

        Instantiate(finalLoot, transform.position, transform.rotation);
        GameObject.Find("Player").GetComponent<PlayerInventory>().currentXP += xPWin;
        if (!questEnemy)
            generateEnemy.enemyCount--;
    }

    public void BackBase()
    {
        ChangeAnimation();
        animator.SetBool("Walking", true);
        agent.destination = basePosition;
    }

    void ChangeAnimation()
    {
        animator.SetBool("IsDead", false);
        animator.SetBool("Attacking01", false);
        animator.SetBool("Idling", false);
        animator.SetBool("Walking", false);
        animator.SetBool("Attacking02", false);
        animator.SetBool("IsVictory", false);
        animator.SetBool("GettingHit", false);
        animator.SetBool("Idling01", false);
        animator.SetBool("Running", false);
        animator.SetBool("Resist", false);
    }
}

[System.Serializable]
public struct Animations
{
    public string animationName;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool attack;
    public float TheDamage;
    public float currentCooldown;
    [HideInInspector]public float attackCooldown;
}
