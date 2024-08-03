using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerEnemy : MonoBehaviour
{
    private bool isPlayer;

    private void Start()
    {
        isPlayer = false;
        Destroy(gameObject,0.5f);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.GetComponent< PlayerState>() == true && isPlayer == false)
        {
            isPlayer = true;
            collision.gameObject.GetComponent<PlayerState>().GotDamage();
        }
    }
}
