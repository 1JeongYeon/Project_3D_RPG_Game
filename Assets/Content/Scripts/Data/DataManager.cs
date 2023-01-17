using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static System.Action Save;
    public static System.Action Load;

    // json 파일 이름 지정
    private string GameDataFileName = "GameData.json";

    public Data data = new Data();

    public bool LoadGameData()
    {
        // 파일 경로를 정해줌 persistentDataPath = 로컬 저장 경로
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // 경로에 파일이 있다면
        if (File.Exists(filePath))
        {
            // 경로에 있는 파일의 모든 내용을 가져온뒤 data에 적용 시키고 불러온다.
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);

            WJYLoadData();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SaveGameData()
    {
        data = new Data();
        WJYSaveData();
        string ToJsonData = JsonUtility.ToJson(data, true); // true로 해주면 눈으로 보기 쉽게 정리해줌
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // 경로에 있는 .txt 파일에 json 문자열을 기록한다
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log(ToJsonData);
    }

    // data 경로에 있는 파일을 삭제하는 함수.
    public void DeleteGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            File.Delete(filePath);
        }
    }

    private void WJYSaveData()
    {
        // Player 현재 씬 이름 저장
        data.saveSceneName = GameManager.Instance.Player.currentScene;

        // 상태 매니저를 받아와서 각 캐릭터 수치 레벨 coin 저장
        StatusManager sm = GameManager.Instance.statusMgr;

        data.maxHP = sm.maxHp;
        data.curHP = sm.currentHp;
        data.maxMP = sm.maxMp;
        data.curMP = sm.currentMp;
        data.maxExp = sm.maxExp;
        data.exp = sm.currentExp;

        data.level = sm.level;
        data.coin = GameManager.Instance.Coin;

        // 위치정보 저장
        //CharacterController thePlayer = FindObjectOfType<CharacterController>();

        //data.savePlayerPos = thePlayer.transform.position;
        //data.savePlayerRot = thePlayer.transform.rotation.eulerAngles;
        Inventory theInventory = FindObjectOfType<Inventory>();


        // Inventory  정보 저장-----------------------------------------
        Slot[] slots = theInventory.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                data.invenArrayNumber.Add(i);
                data.invenItemName.Add(slots[i].item.itemName);
                data.invenItemNumber.Add(slots[i].itemCount);
            }
        }

        // QuickSlot 정보 저장-----------------------------------------
        Slot[] quickSlots = theInventory.GetQuickSlots();
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (quickSlots[i].item != null)
            {
                data.quickSlotArrayNumber.Add(i);
                data.quickSlotItemName.Add(quickSlots[i].item.itemName);
                data.quickSlotItemNumber.Add(quickSlots[i].itemCount);
            }
        }
    }

    private void WJYLoadData()
    {
        // WJYSaveData() 와 반대 개념으로 불러와 준다.
        GameManager.Instance.Player.currentScene = data.saveSceneName;
        StatusManager sm = GameManager.Instance.statusMgr;

        sm.maxHp = (int)data.maxHP;
        sm.currentHp = (int)data.curHP;
        sm.maxMp = (int)data.maxMP;
        sm.currentMp = (int)data.curMP;
        sm.maxExp = data.maxExp;
        sm.currentExp = data.exp;

        sm.level = data.level;
        GameManager.Instance.Coin = data.coin;

        Inventory theInventory = FindObjectOfType<Inventory>();

        theInventory.coin.text = data.coin.ToString();
        UIManager.Instance.levelTxt.text = data.level.ToString();

        //CharacterController thePlayer = FindObjectOfType<CharacterController>();
        //thePlayer.transform.position = data.savePlayerPos;
        //thePlayer.transform.eulerAngles = data.savePlayerRot;

        // Inventory data를 받아와 업데이트 해준다.
        for (int i = 0; i < data.invenItemName.Count; i++)
        {
            theInventory.LoadToInven(data.invenArrayNumber[i], data.invenItemName[i], data.invenItemNumber[i]);
        }
        // QuickSlot도 똑같이 받아와 업데이트 해준다.
        for (int i = 0; i < data.quickSlotItemName.Count; i++)
        {
            theInventory.LoadToQuickSlot(data.quickSlotArrayNumber[i], data.quickSlotItemName[i], data.quickSlotItemNumber[i]);
        }
    }
    
}