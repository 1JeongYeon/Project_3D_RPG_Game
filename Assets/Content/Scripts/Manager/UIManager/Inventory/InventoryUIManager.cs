using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class InventoryUIManager : MonoBehaviour
{
    // 정렬을 위한 각 타입의 대한 가중치 부여
    private readonly static Dictionary<Item.ItemType, int> sortWeightDict = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Used, 10000 },
            { Item.ItemType.Equipment, 20000 },
            { Item.ItemType.ETC, 30000 }
        };
    private class ItemComparer : IComparer<Item> // 비교하기 위한 클래스
    {
        public int Compare(Item a, Item b)
        {
            return (a.index + sortWeightDict[a.itemType])
                 - (b.index + sortWeightDict[b.itemType]);
        }
    }

    private HashSet<int> indexSetUpdate = new HashSet<int>();
    private static readonly ItemComparer itemComparer = new ItemComparer();

    [SerializeField] private Button ButtontoQuit;

    public ToggleGroup inventoryToggleGroup;
    public Toggle[] toggles;

    [SerializeField] private Toggle toggleInventoryUsed; // 소모품만 활성화
    [SerializeField] private Toggle toggleInventoryEquipment; // 장비템만 활성화
    [SerializeField] private Toggle toggleInventoryAll; // 정렬 기능을 넣을 것

    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] private GameObject background;



    private Inventory inventory;
    public Item[] items;
    public Slot[] slots;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        slots = inventory.GetSlots();
        ButtontoQuit.onClick.AddListener(() =>
        {
            inventoryScreen.SetActive(false);
            background.SetActive(false);
        });

        InitToggleEvents();

    }

    private void Update()
    {
        // 구조 고민중
        return;
        SetUpToggleChoice();
    }

    private enum FilterOption
    {
        All, Equipment, Used
    }
    private FilterOption currentFilterOption = FilterOption.All;

    public void UpdateAllSlotFilters()
    {
        int capacity = slots.Length;

        for (int i = 0; i < capacity; i++)
        {
            Item itemData = ItemManager.Instance.items[i];
            UpdateSlotFilterState(i, itemData);
        }
    }
    public void UpdateSlotFilterState(int index, Item item)
    {
        bool isFiltered = true;

        // null인 슬롯은 타입 검사 없이 필터 활성화
        if (item != null)
            switch (currentFilterOption)
            {
                case FilterOption.Equipment:
                    isFiltered = (item.itemType == Item.ItemType.Equipment);
                    break;

                case FilterOption.Used:
                    isFiltered = (item.itemType == Item.ItemType.Used);
                    break;
            }

        slots[index].SetItemAccessibleState(isFiltered);
    }

    private void Awake()
    {
        // ...
        InitToggleEvents();
    }

    private void InitToggleEvents()
    {
        toggleInventoryAll.onValueChanged.AddListener(p => UpdateFilter(p, FilterOption.All));
        toggleInventoryEquipment.onValueChanged.AddListener(p => UpdateFilter(p, FilterOption.Equipment));
        toggleInventoryUsed.onValueChanged.AddListener(p => UpdateFilter(p, FilterOption.Used));

        // Local Method
        void UpdateFilter(bool flag, FilterOption option)
        {
            if (flag)
            {
                currentFilterOption = option;
                // UpdateAllSlotFilters();
            }
        }
    }


    public void TrimAllSlot()
    {
        /* 가장 빠른 배열 빈공간 채우기 알고리즘

         i 커서와 j 커서
         i 커서 : 가장 앞에 있는 빈칸을 찾는 커서
         j 커서 : i 커서 위치에서부터 뒤로 이동하며 기존재 아이템을 찾는 커서

         i커서가 빈칸을 찾으면 j 커서는 i+1 위치부터 탐색
         j커서가 아이템을 찾으면 아이템을 옮기고, i 커서는 i+1 위치로 이동
         j커서가 slots.Length에 도달하면 루프 즉시 종료*/

        indexSetUpdate.Clear();

        int i = -1;
        while (slots[i].item != null);
        int j = i;

        while (true)
        {
            while (++j < slots.Length && slots[j].item == null);

            if (j == slots.Length)
            {
                break;
            }

            indexSetUpdate.Add(i);
            indexSetUpdate.Add(j);

            slots[i].item = slots[j].item;
            slots[j].item = null;
            i++;
        }

        /*foreach (var index in slots.Length)
        {
            UpdateSlots(index);
        }*/
    }
    public void SortAllSlot()
    {
        // Trim 알고리즘
        int i = -1;
        while (slots[i].item != null) ;
        int j = i;

        while (true)
        {
            while (++j < slots.Length && slots[j].item == null) ;

            if (j == slots.Length)
            {
                break;
            }

            slots[i].item = slots[j].item;
            slots[j].item = null;
            i++;
        }

        /*// 2. Sort 정렬함.
        Array.Sort(slots[i].item, 0, i, itemComparer);

        // 3. Update
        UpdateAllSlot();
        _inventoryUI.UpdateAllSlotFilters(); // 필터 상태 업데이트*/
    }
    // ------------------------------------------------------------------------------------------------------------------------
    public Toggle setupToggleCurrentSelection
    {
        get { return inventoryToggleGroup.ActiveToggles().FirstOrDefault(); }
    }

    public void SetUpToggleChoice()
    {
        if (inventoryToggleGroup.ActiveToggles().Any()) // Toggles 중 하나라도 Active된 Toggle이 있다면
        {
            if (setupToggleCurrentSelection.name.Equals("Toggle : Equipment"))
            {
                SelectionActive(true, false, false);
            }
            else if (setupToggleCurrentSelection.name.Equals("Toggle : Used"))
            {
                SelectionActive(false, true, false);
            }
            else if (setupToggleCurrentSelection.name.Equals("Toggle : ETC"))
            {
                SelectionActive(false, false, true);
            }
        }
    }
    void SelectionActive(bool equipType, bool usedType, bool etcType)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (equipType == true)
                {
                    if (slots[i].item.itemType != Item.ItemType.Equipment)
                    {
                        Debug.Log("Equip");
                        slots[i].SetColor(0f);
                    }
                }
                else
                {
                    slots[i].SetColor(1f);
                }

                if (usedType == true)
                {
                    if (slots[i].item.itemType != Item.ItemType.Used)
                    {
                        Debug.Log("Used");
                        slots[i].SetColor(0f);
                    }
                }
                else
                {
                    slots[i].SetColor(1f);
                }

                if (etcType == true)
                {
                    if (slots[i].item.itemType != Item.ItemType.ETC)
                    {
                        Debug.Log("ETC");
                        slots[i].SetColor(0f);
                    }
                }
                else
                {
                    slots[i].SetColor(1f);
                }
            }
     
        }
    }
}
