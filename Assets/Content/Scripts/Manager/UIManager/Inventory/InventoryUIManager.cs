using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class InventoryUIManager : MonoBehaviour
{
    // ������ ���� �� Ÿ���� ���� ����ġ �ο�
    private readonly static Dictionary<Item.ItemType, int> sortWeightDict = new Dictionary<Item.ItemType, int>
        {
            { Item.ItemType.Used, 10000 },
            { Item.ItemType.Equipment, 20000 },
            { Item.ItemType.ETC, 30000 }
        };
    private class ItemComparer : IComparer<Item> // ���ϱ� ���� Ŭ����
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

    [SerializeField] private Toggle toggleInventoryUsed; // �Ҹ�ǰ�� Ȱ��ȭ
    [SerializeField] private Toggle toggleInventoryEquipment; // ����۸� Ȱ��ȭ
    [SerializeField] private Toggle toggleInventoryAll; // ���� ����� ���� ��

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
        // ���� �����
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

        // null�� ������ Ÿ�� �˻� ���� ���� Ȱ��ȭ
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
        /* ���� ���� �迭 ����� ä��� �˰���

         i Ŀ���� j Ŀ��
         i Ŀ�� : ���� �տ� �ִ� ��ĭ�� ã�� Ŀ��
         j Ŀ�� : i Ŀ�� ��ġ�������� �ڷ� �̵��ϸ� ������ �������� ã�� Ŀ��

         iĿ���� ��ĭ�� ã���� j Ŀ���� i+1 ��ġ���� Ž��
         jĿ���� �������� ã���� �������� �ű��, i Ŀ���� i+1 ��ġ�� �̵�
         jĿ���� slots.Length�� �����ϸ� ���� ��� ����*/

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
        // Trim �˰���
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

        /*// 2. Sort ������.
        Array.Sort(slots[i].item, 0, i, itemComparer);

        // 3. Update
        UpdateAllSlot();
        _inventoryUI.UpdateAllSlotFilters(); // ���� ���� ������Ʈ*/
    }
    // ------------------------------------------------------------------------------------------------------------------------
    public Toggle setupToggleCurrentSelection
    {
        get { return inventoryToggleGroup.ActiveToggles().FirstOrDefault(); }
    }

    public void SetUpToggleChoice()
    {
        if (inventoryToggleGroup.ActiveToggles().Any()) // Toggles �� �ϳ��� Active�� Toggle�� �ִٸ�
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
