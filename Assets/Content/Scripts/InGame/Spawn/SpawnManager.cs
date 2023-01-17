using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    List<Spawn> spawns = new List<Spawn>();

    [SerializeField] Player player = null;

    void Start()
    {
        if (FindObjectOfType<Player>() == null)
            Instantiate(player);

        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            switch(GameManager.Instance.CurrentMap)
            {
                case Map.Field:
                    {
                        if (Contents.monster1_spawnCount < Contents.MONSTER1_MAXCOUNT)
                        {
                            Contents.monster1_spawnCount++;
                            spawns[(int)MONSTER.MONSTER1].MonsterSpawn();
                        }
                    }
                    break;
                case Map.Dungeon_1:
                    {
                        if (Contents.monster2_spawnCount < Contents.MONSTER2_MAXCOUNT)
                        {
                            Contents.monster2_spawnCount++;
                            spawns[(int)MONSTER.MONSTER1].MonsterSpawn();
                        }
                    }
                    break;
                case Map.Dungeon_2:
                    {
                        if (Contents.monster3_spawnCount < Contents.MONSTER3_MAXCOUNT)
                        {
                            Contents.monster3_spawnCount++;
                            spawns[(int)MONSTER.MONSTER1].MonsterSpawn();
                        }
                    }
                    break;
            }
        }
    }
}
