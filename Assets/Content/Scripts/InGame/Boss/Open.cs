using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject dragon;
    [SerializeField]
    private GameObject dragonUI;
    [SerializeField]
    private GameObject parent;
    public int enemyCount;
    private bool isAllClear = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = Contents.MONSTER2_MAXCOUNT;
    }
    // Update is called once per frame
    void Update()
    {
        if(enemyCount == 0 && !isAllClear )
        {
            isAllClear = true;
            BossSpawn();
        }
    }
    public void BossSpawn()
    {
        StartCoroutine(CoBossSpawn());
    }
    IEnumerator CoBossSpawn()
    {
        yield return new WaitForSeconds(1.5f);
        door.SetActive(false);
        dragon.SetActive(true);
        GameObject temp = Resources.Load("Prefabs/CombatUI") as GameObject;
        Instantiate(temp, parent.transform);
    }
}
