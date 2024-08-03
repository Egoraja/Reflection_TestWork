using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;    
    [SerializeField, Range(0.2f, 0.6f)] private float freezeMoveTimer; // небольшой таймер для отскока от стены
    [SerializeField, Range(0.1f, 5f)] private float slideSpeed;

    [Space(10)]
    [Header("Other settings")]
    [SerializeField] private float jumpOffset;
    [SerializeField] private float wallOffset;
    [SerializeField] private Transform groundColliderTransform;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private AnimationCurve curveMovement;

    private PlayerState playerState;
    private Vector2 wallNormal = Vector2.zero;
    private bool isGround;
    private bool isOnWall;
    private float currentFreezTime = 0; 
    private Rigidbody2D playerRB;

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
        playerRB = GetComponent<Rigidbody2D>();
        isGround = false;
        isOnWall = false;
    } 
    
    public bool IsGround
    { get { return isGround; } }


    public void PlayerMove(float direction, bool isJumpButtonPressed)
    {
        if (isJumpButtonPressed == true)       
            Jump();        
        if (Mathf.Abs(direction) > 0.01f)
            HorizontalMovement(direction);
    }    

    /// <summary>
    /// Два вида прыжка от стены и от пола
    /// </summary>
    private void Jump()
    {
        if (playerState.IsAlive == true)
        {
            if (isGround == true)
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);

            if (isGround == false && isOnWall == true)
            {
                currentFreezTime = freezeMoveTimer;
                isOnWall = false;
                playerRB.velocity = new Vector2(wallNormal.x * jumpForce, jumpForce);
                wallNormal = Vector2.zero;
            }
        }
    }

    private void HorizontalMovement(float direction)
    {
        if (playerState.IsAlive == true && currentFreezTime <= 0)        
            playerRB.velocity = new Vector2(curveMovement.Evaluate(direction) * movementSpeed, playerRB.velocity.y);               
    }

    /// <summary>
    /// "Отлипание" от стены с помощью кнопки, противоположной расположению стены от игрока
    /// </summary>
    private void IsOnTheWall()
    {
        if (playerRB.velocity.x < 0 && wallNormal.x > 0 || playerRB.velocity.x > 0 && wallNormal.x < 0)
            isOnWall = true;
        else
        {
            isOnWall = false;
            wallNormal = Vector2.zero;
        }
    }

    /// <summary>
    /// Скольжение по стене, восстановление таймера прыжка от стены
    /// </summary>
    private void MovingOnWall()
    {
        if (isOnWall == true && isGround == false && playerRB.velocity.y <= 0)
            playerRB.velocity = new Vector2(0, -slideSpeed);
        if (currentFreezTime > 0)
            currentFreezTime -= Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7 && isGround == false) // Wall
        {
            wallNormal = collision.contacts[0].normal;
        }
    }

    private void FixedUpdate()
    {
        Vector3 overLapCirclePositionGround = groundColliderTransform.transform.position;
        isGround = Physics2D.OverlapCircle(overLapCirclePositionGround, jumpOffset, groundMask);
        IsOnTheWall();
        MovingOnWall();
    }
}

