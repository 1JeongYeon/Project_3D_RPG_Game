using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static System.Action Save;
    public static System.Action Load;

    // json ���� �̸� ����
    private string GameDataFileName = "GameData.json";

    public Data data = new Data();

    public bool LoadGameData()
    {
        // ���� ��θ� ������ persistentDataPath = ���� ���� ���
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // ��ο� ������ �ִٸ�
        if (File.Exists(filePath))
        {
            // ��ο� �ִ� ������ ��� ������ �����µ� data�� ���� ��Ű�� �ҷ��´�.
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
        string ToJsonData = JsonUtility.ToJson(data, true); // true�� ���ָ� ������ ���� ���� ��������
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // ��ο� �ִ� .txt ���Ͽ� json ���ڿ��� ����Ѵ�
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log(ToJsonData);
    }

    // data ��ο� �ִ� ������ �����ϴ� �Լ�.
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
        // Player ���� �� �̸� ����
        data.saveSceneName = GameManager.Instance.Player.currentScene;

        // ���� �Ŵ����� �޾ƿͼ� �� ĳ���� ��ġ ���� coin ����
        StatusManager sm = GameManager.Instance.statusMgr;

        data.maxHP = sm.maxHp;
        data.curHP = sm.currentHp;
        data.maxMP = sm.maxMp;
        data.curMP = sm.currentMp;
        data.maxExp = sm.maxExp;
        data.exp = sm.currentExp;

        data.level = sm.level;
        data.coin = GameManager.Instance.Coin;

        // ��ġ���� ����
        //CharacterController thePlayer = FindObjectOfType<CharacterController>();

        //data.savePlayerPos = thePlayer.transform.position;
        //data.savePlayerRot = thePlayer.transform.rotation.eulerAngles;
        Inventory theInventory = FindObjectOfType<Inventory>();


        // Inventory  ���� ����-----------------------------------------
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

        // QuickSlot ���� ����-----------------------------------------
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
        // WJYSaveData() �� �ݴ� �������� �ҷ��� �ش�.
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

        // Inventory data�� �޾ƿ� ������Ʈ ���ش�.
        for (int i = 0; i < data.invenItemName.Count; i++)
        {
            theInventory.LoadToInven(data.invenArrayNumber[i], data.invenItemName[i], data.invenItemNumber[i]);
        }
        // QuickSlot�� �Ȱ��� �޾ƿ� ������Ʈ ���ش�.
        for (int i = 0; i < data.quickSlotItemName.Count; i++)
        {
            theInventory.LoadToQuickSlot(data.quickSlotArrayNumber[i], data.quickSlotItemName[i], data.quickSlotItemNumber[i]);
        }
    }
    
}