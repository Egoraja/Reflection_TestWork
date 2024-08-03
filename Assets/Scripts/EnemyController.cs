using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Логика для противника (движение, проверка есть ли игрок рядом, атака)
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("Movement settings")]
    [Space(5)]
    [SerializeField] private float speedWalk;
    [Space(5)]
    [SerializeField] private float timeToRevert;
    [Space(5)]
    [SerializeField, Range(1,2)] private float speedAttack;

    [Space(10)]
    [Header("Other settings")]
    [SerializeField] private float attackTime;
    [SerializeField] private float distanceCheck;    
    [SerializeField] private Transform detectorPosition;
    [SerializeField] private LevelController levelController;
    private EnemyHealth enemyHealth;   
   
    private Vector2 directionFrontView;
    private Vector2 directionRearView;    
    private Animator enemyAnim;
    private Rigidbody2D enemyRB;
    private Transform enemyTransform;
    private MeleeAttack meleeAttack;
    
    private bool attackingBySword;
    private float currentAction, currentTimeToRevert, currentAttackTime;    

    private const float IDLE_STATE = 0;
    private const float WALK_STATE = 1;   
    private const float REVERT_STATE = 2;

    private void Start()
    {       
        attackingBySword = false;           
        enemyAnim = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        meleeAttack = GetComponent<MeleeAttack>();
        enemyHealth = GetComponent<EnemyHealth>(); 
        directionFrontView = Vector2.left;
        directionRearView = Vector2.right;
        enemyAnim.speed *= speedAttack;
        currentAction = IDLE_STATE;     
        currentTimeToRevert = 0;
        currentAttackTime = attackTime;        
        SetBulletDirection();
    }

    private void EnemyWalking()
    {       
        if (currentTimeToRevert >= timeToRevert)
        {
            currentTimeToRevert = 0;
            currentAction = REVERT_STATE;
        }
        switch (currentAction)
        {
            case IDLE_STATE:
                enemyAnim.SetBool("IsRun", false);
                currentTimeToRevert += Time.deltaTime;
                break;
            case WALK_STATE:
                enemyRB.velocity = new Vector2(speedWalk * -1, enemyRB.velocity.y);
                enemyAnim.SetBool("IsRun", true);
                break;           
            case REVERT_STATE:                
                enemyTransform.localScale = new Vector3(enemyTransform.localScale.x * (-1), enemyTransform.localScale.y, enemyTransform.localScale.z);
                directionFrontView *= -1;
                directionRearView *= -1;
                speedWalk *= -1;               
                currentAction = WALK_STATE;
                SetBulletDirection();
                break;
        }
    }

    private void SetBulletDirection()
    {
        if (enemyTransform.localScale.x < 0)
           meleeAttack.DirectionBullet = false;
        else if (enemyTransform.localScale.x > 0)
            meleeAttack.DirectionBullet = true;
    }

    /// <summary>
    /// Проверка на нахождение игрока рядом, поворот к игроку, переход в боевой режим
    /// </summary>
    private void CheckPlayer()
    {
        RaycastHit2D[] hitsInRear = Physics2D.RaycastAll(detectorPosition.position, directionRearView, distanceCheck);

        for (int i = 0; i < hitsInRear.Length; i++)
        {
            if (hitsInRear[i].transform.gameObject.GetComponent<PlayerInput>() == true)
            {
                currentAction = REVERT_STATE;
                break;
            }
        }

        RaycastHit2D[] hitsInFront = Physics2D.RaycastAll(detectorPosition.position, directionFrontView, distanceCheck);

        for (int i = 0; i < hitsInFront.Length; i++)
        {
            if (hitsInFront[i].transform.gameObject.GetComponent<PlayerInput>() == true)
            {
                attackingBySword = true;
                currentAction = IDLE_STATE;
                enemyAnim.SetBool("IsRun", false);
            }
            else
            {
                attackingBySword = false;                   
            }
        }
    }

    /// <summary>
    /// "Физически" атака реализована на подходящем кадре анимации AttackEnemy
    /// </summary>
    private void SwordAttack()
    {
        if (currentAttackTime >= attackTime)
        {
            RaycastHit2D[] hitsInFront = Physics2D.RaycastAll(detectorPosition.position, directionFrontView, distanceCheck);
            for (int i = 0; i < hitsInFront.Length; i++)
            {
                if (hitsInFront[i].transform.gameObject.GetComponent<PlayerInput>())
                {
                    
                    float distance = Vector3.Distance(hitsInFront[i].transform.position, transform.position);
                    if (distance < 3)
                    {                       
                        enemyAnim.SetTrigger("Attack");
                        currentAttackTime = 0;
                    }
                }
            }
        }
        else
            currentAttackTime += Time.deltaTime;
    }

    private void Death()
    {
        enemyAnim.SetBool("IsAlive", false);
        Destroy(gameObject, 0.5f);        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyStopper"))
        {                    
            currentAction = IDLE_STATE;
            attackingBySword = false;             
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7) //Wall
        {
            currentAction = IDLE_STATE;
            attackingBySword = false;
        }
    }


    private void Update()
    {
        if (enemyHealth.IsAlive==true)
        {
            CheckPlayer();
            if (attackingBySword == false)            
                EnemyWalking();

            if (attackingBySword == true)
                SwordAttack();
        }      
        else
        {
            Death();
            levelController.PlayerWIn();
        }         
    }
}
