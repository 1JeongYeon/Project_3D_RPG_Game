using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject openDoor;
    private void Awake()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("´ê¾Ò´Ù");
            openDoor.SetActive(true);
            //GameObject.Find("BlueDragon").GetComponent<Dragon>().Wait();
            GameObject.Find("BlueDragon").GetComponent<Dragon>().BossAni();
            Destroy(gameObject);
        }
        

    }
}
