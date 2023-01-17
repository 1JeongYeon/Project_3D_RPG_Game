using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    List<GameObject> monsterobjs = new List<GameObject>();

    [SerializeField]
    private GameObject monster1Prefab;
    [SerializeField]
    private GameObject plane;
    [SerializeField]
    private Transform parent;
    private BoxCollider boxColider;
    [SerializeField]
    private GameObject enemyHpPrefab;
    [SerializeField]
    private Transform canvasTransform;
    private void Awake()
    {
        boxColider = GetComponent<BoxCollider>();
    }
    public void MonsterSpawn()
    {
        monsterobjs.Add(Instantiate(monster1Prefab, RandomPosition(), Quaternion.identity));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Destroy(monsterobjs[0]);
            monsterobjs.RemoveAt(0);
            Contents.monster1_spawnCount--;
        }
    }
    private Vector3 RandomPosition()
    {
        Vector3 position = plane.transform.position;

        float x = boxColider.bounds.size.x;
        float z = boxColider.bounds.size.z;

        x = Random.Range((x / 2) * -1, x / 2);
        z = Random.Range((z / 2) * -1, z / 2);

        Vector3 Randomposition = new Vector3(x, 0.5f, z);

        return position + Randomposition;
    }
}
