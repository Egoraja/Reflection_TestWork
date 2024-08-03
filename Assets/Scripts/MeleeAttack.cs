using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{    
    [SerializeField] private GameObject swordWeapon;
    [SerializeField] private Transform fireGun;
    [SerializeField] private float weaponSpeed;
    private bool directionBullet;

    private void Start()
    {
        directionBullet = true;
    }

    public bool DirectionBullet
    {
        set { directionBullet = value; }
    }    

    public void SwordAttack()
    {
        GameObject currentWeapon = Instantiate(swordWeapon, fireGun.position, Quaternion.identity);
        Rigidbody2D currentBulletVelocity = currentWeapon.GetComponent<Rigidbody2D>();
        if (directionBullet == true)
            currentBulletVelocity.velocity = new Vector2(weaponSpeed * 1, currentBulletVelocity.velocity.y);
        else
            currentBulletVelocity.velocity = new Vector2(weaponSpeed * -1, currentBulletVelocity.velocity.y);
        Destroy(currentWeapon, 0.2f);
    }
}
