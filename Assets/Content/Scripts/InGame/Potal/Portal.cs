using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public string mapName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<Player>().currentMapName = mapName;
            GameManager.Instance.dataMgr.SaveGameData();
            SceneController.Instance.GoDungeon1();
        }
    }
}
