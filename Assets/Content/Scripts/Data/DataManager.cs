using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static System.Action Save;
    public static System.Action Load;

    private string GameDataFileName = "GameData.json";

    public Data data = new Data();

    public bool LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        if (File.Exists(filePath))
        {
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
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
        Debug.Log(ToJsonData);
    }

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
        data.saveSceneName = GameManager.Instance.Player.currentScene;

        StatusManager sm = GameManager.Instance.statusMgr;

        data.maxHP = sm.maxHp;
        data.curHP = sm.currentHp;
        data.maxMP = sm.maxMp;
        data.curMP = sm.currentMp;
        data.maxExp = sm.maxExp;
        data.exp = sm.currentExp;

        data.level = sm.level;
        data.coin = GameManager.Instance.Coin;

        //CharacterController thePlayer = FindObjectOfType<CharacterController>();
        Inventory theInventory = FindObjectOfType<Inventory>();

        //data.savePlayerPos = thePlayer.transform.position;
        //data.savePlayerRot = thePlayer.transform.rotation.eulerAngles;

        // Inventory  -----------------------------------------
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

        // QuickSlot -----------------------------------------
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

        //CharacterController thePlayer = FindObjectOfType<CharacterController>();
        Inventory theInventory = FindObjectOfType<Inventory>();

        Debug.Log(theInventory);
        theInventory.coin.text = data.coin.ToString();
        UIManager.Instance.levelTxt.text = data.level.ToString();

        //thePlayer.transform.position = data.savePlayerPos;
        //thePlayer.transform.eulerAngles = data.savePlayerRot;

        for (int i = 0; i < data.invenItemName.Count; i++)
        {
            theInventory.LoadToInven(data.invenArrayNumber[i], data.invenItemName[i], data.invenItemNumber[i]);
        }

        for (int i = 0; i < data.quickSlotItemName.Count; i++)
        {
            theInventory.LoadToQuickSlot(data.quickSlotArrayNumber[i], data.quickSlotItemName[i], data.quickSlotItemNumber[i]);
        }
    }
    
}