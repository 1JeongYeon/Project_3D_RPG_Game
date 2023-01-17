using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questID; // questid 저장할 변수
    public int questActionIndex; // 현재 진행중인 퀘스트 인덱스를 저장할 변수
    public GameObject[] questObjects; // 퀘스트에 필요한 오브젝트들을 저장할 변수

    Dictionary<int, QuestData> questList; // 퀘스트 내용을 저장할 변수

    void Start()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("사람들과 대화", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("슬라임을 잡아라~", new int[] { 5000, 2000 }));
        questList.Add(30, new QuestData("퀘스트 클리어", new int[] { 0 }));
    }
    
    void NextQuest() // 퀘를 깻을때 id를 증가시켜주고 인덱스를 0으로 초기화하여 다음 퀘스트가 이어지게 해준다.
    {
        questID += 10;
        questActionIndex = 0;
    }
    
    void ControlObject() // questobjects에 있는 오브젝을 컨트롤 하게 해줌 id가 10이고 인덱스가 2라면 첫번쨰 퀘를 활성화해주고 인덱스가 1이면 첫번쨰 퀘 비활성화
    {
        switch (questID)
        {
            case 10:
                if (questActionIndex == 2)
                    questObjects[0].SetActive(true);
                break;
            case 20:
                if (questActionIndex == 1)
                    questObjects[0].SetActive(false);
                break;
        }
    }

    public int GetQuestTalkIndex(int id) // questid에 인덱스를 더해준 값 반환
    {
        return questID + questActionIndex;
    }

    public string CheckQuest(int id) // npc와 플레이어가 대화하는 순서가 맞으면 인덱스를 증가시켜주고 마지막이라면 담 퀘 시작해주는 함수
    {
        if (id == questList[questID].npcID[questActionIndex])
        {
            questActionIndex++;
        }

        ControlObject();

        if (questActionIndex == questList[questID].npcID.Length)
        {
            NextQuest();
            Debug.Log(questID);
        }

        return questList[questID].questName;
    }

    public string CheckQuest() // 매개변수 달라
    {
        return questList[questID].questName;
    }
}
