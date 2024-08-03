using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerPlayer : MonoBehaviour
{
    private bool isEnemy;

    private void Start()
    {
        isEnemy = false;
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.GetComponent<EnemyHealth>() == true && isEnemy == false)
        {
            isEnemy = true;
            collision.gameObject.GetComponent<EnemyHealth>().GotDamage();
        }
    }
}
