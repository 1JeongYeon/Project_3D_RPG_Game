using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public string startPoint2; // 맵이 이동하면 플레이어 시작될 위치
    private Player player;
    private GameManager gameManager;
    private void Start()
    {
        GameManager.Instance.CurrentMap = Map.Dungeon_2;
        player = FindObjectOfType<Player>();

        if (startPoint2 == player.currentMapName)
        {
            player.transform.position = this.transform.position;
        }
    }
}
