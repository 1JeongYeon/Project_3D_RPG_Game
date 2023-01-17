using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint2 : MonoBehaviour
{
    public string startPoint; // 맵이 이동하면 플레이어 시작될 위치
    private Player player;
    private GameManager gameManager;
    private void Start()
    {
        GameManager.Instance.CurrentMap = Map.Dungeon_2;
        player = FindObjectOfType<Player>();

        if (startPoint == player.currentMapName)
        {
            player.transform.position = this.transform.position;
        }
    }
}
