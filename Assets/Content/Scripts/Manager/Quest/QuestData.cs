using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    public string questName;
    public int[] npcID;

    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcID = npc;
    }
}
