using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    // 인벤토리 활성화 여부
    public static bool inventoryActivated = false;  

    [SerializeField]
    private GameObject inventoryBase;
    [SerializeField]
    private GameObject slotsParent;  // Slot들의 부모 Grid Setting 
    [SerializeField]
    private GameObject quickSlotParent; 

    [SerializeField]
    private GameObject sellItemUI;
    [SerializeField]
    private GameObject backgroundImg;
    
    private Slot[] slots;
    private Slot[] quickSlots; 

    public TMP_Text coin;

    [SerializeField]
    private Item[] save_Items;

    
    public Slot[] GetSlots() { return slots; }
    public Slot[] GetQuickSlots() { return quickSlots; }


    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < save_Items.Length; i++)
        {
            if (save_Items[i].itemName == _itemName)
            {
                slots[_arrayNum].AddItem(save_Items[i], _itemNum);
            }
        }
    }

    public void LoadToQuickSlot(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < save_Items.Length; i++)
        {
            if (save_Items[i].itemName == _itemName)
            {
                quickSlots[_arrayNum].AddItem(save_Items[i], _itemNum);
            }
        }
    }

    void Awake()
    {
        Instance = this;
        backgroundImg.SetActive(false);
        slots = slotsParent.GetComponentsInChildren<Slot>();
        quickSlots = quickSlotParent.GetComponentsInChildren<Slot>();
        coin.text = GameManager.Instance.Coin.ToString();
    }

    private void Start()
    {
       GameManager.Instance.dataMgr.LoadGameData();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInventory();
        }
    }

    public void OpenInventory()
    {
        backgroundImg.SetActive(true);
        inventoryBase.SetActive(true);
        inventoryActivated = true;

        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseInventory()
    {
        backgroundImg.SetActive(false);
        inventoryBase.SetActive(false);
        sellItemUI.SetActive(false);
        inventoryActivated = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // 같은 종류의 아이템이 이미 있는지를 검사한다. 있다면 아이템 갯수를 더해준다.
    public void AcquireItem(Item _item, int _count = 1) 
    {
        // 무기가 아닌 경우에만 진행함 (무기는 갯수를 업데이트 해주지 않음)
        if (Item.ItemType.Equipment != _item.itemType) 
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)  
                {
                    // 같은 아이템을 찾았다면 갯수만 업데이트
                    if (slots[i].item.itemName == _item.itemName) 
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }

            // 퀵슬롯에 있는 아이템이라면 퀵슬롯 아이템의 갯수를 업데이트
            for (int i = 0; i < quickSlots.Length; i++)
            {
                if (quickSlots[i].item != null)
                {
                    if (quickSlots[i].item.itemName == _item.itemName)
                    {
                        quickSlots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        //같은 종류의 아이템이 없다면 아이템을 저장할 새로운 슬롯을 마련
        for (int i = 0; i < slots.Length; i++)
        {
            // 빈 슬롯을 찾음
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
