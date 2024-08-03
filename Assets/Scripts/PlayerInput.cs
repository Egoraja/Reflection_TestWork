using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMovement))]

public class PlayerInput : MonoBehaviour
{  
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    private MeleeAttack meleeAttack;
    private PlayerState playerState;      

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();              
        playerAnimation = GetComponent<PlayerAnimation>();
        meleeAttack = GetComponent<MeleeAttack>();
        playerState = GetComponent< PlayerState>();
    }    

    private void Update()
    {        
        float horizontalDirection = Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);
        bool isJumpButtonPressed = Input.GetButtonDown(GlobalStringVars.JUMP);

        playerAnimation.JumpAnim(isJumpButtonPressed);
        playerMovement.PlayerMove(horizontalDirection, isJumpButtonPressed);
        playerAnimation.PlayerTurnScale(horizontalDirection);

        if (playerState.IsAlive == true)
        {
            if (Input.GetButtonDown(GlobalStringVars.FIRE_1) && playerState.IsAttacking == true)
                playerAnimation.AttackAnim();

            if (Mathf.Abs(Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS)) > 0.15f)
                playerAnimation.RunAnim(true);
            else
                playerAnimation.RunAnim(false);
        }       

        if (horizontalDirection < 0)
            meleeAttack.DirectionBullet = false;
        else if (horizontalDirection > 0)
            meleeAttack.DirectionBullet = true;
    }
}
