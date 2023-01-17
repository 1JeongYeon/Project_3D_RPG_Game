using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            other.GetComponent<EnemyHP>().TakeDamage(player.playerDamage);
        }
        else if(other.tag.Equals("Dragon"))
        {
            other.GetComponent<Dragon>().Damage((int)player.playerDamage);
        }

        Invoke("CollisionOff", 0.2f);
    }
    void CollisionOff()
    {
        gameObject.SetActive(false);
    }
}
