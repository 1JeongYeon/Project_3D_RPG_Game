using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!isRock && collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isMelee && other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        /*else if(!isMelee && other.CompareTag("Player"))
        {
            Debug.Log("´ê¾Ò´Ù");
            player.PlayerHit();
            if(StatusManager.Instance.currentHp <= 0)
            {
                player.PlayerDie();
            }
        }*/
    }

    
}
