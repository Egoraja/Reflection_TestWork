using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;   
    private PlayerMovement playerMovement;
    private PlayerState playerState;
    private float directionalFactor = -1;   

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerState = GetComponent<PlayerState>();      
        playerMovement = GetComponent<PlayerMovement>();       
    }

    public void PlayerTurnScale(float scale)
    {
        if (playerState.IsAlive == true)
        {
            if (scale < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(transform.localScale.x * directionalFactor, transform.localScale.y, transform.localScale.z);
            if (scale > 0 && transform.localScale.x < 0)
                transform.localScale = new Vector3(transform.localScale.x * directionalFactor, transform.localScale.y, transform.localScale.z);
        }        
    }

    public void AttackAnim()
    {
        if (playerState.IsAlive == true)
             playerAnimator.SetTrigger("Attack");       
    }

    public void RunAnim(bool isRun)
    {
        if (isRun == true)
        {           
            if (playerMovement.IsGround == true)           
                playerAnimator.SetBool("IsRun", true);            
            else
                playerAnimator.SetBool("IsRun", false);
        }

        if (isRun == false)                   
            playerAnimator.SetBool("IsRun", false);        
    }

    public void JumpAnim(bool isJump)
    {
        if (isJump == true && playerMovement.IsGround == true)
            playerAnimator.SetTrigger("Jump");
    }

    public void GotHitAnim()
    {
        playerAnimator.SetTrigger("GotHit");    
    }

    public void DeathAnim()
    {
        playerAnimator.SetBool("IsDead", true);        
    }   
}
