using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Button ButtontoQuit;

    public ToggleGroup inventoryToggleGroup;
    public Toggle[] toggles;

    [SerializeField] private Toggle toggleInventoryUsed;
    [SerializeField] private Toggle toggleInventoryEquipment;
    [SerializeField] private Toggle toggleInventoryAll;

    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] private GameObject background;



    private Inventory inventory;
    //public Item item;
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
    private FilterOption _currentFilterOption = FilterOption.All;

    private void Awake()
    {
        // ...
        InitToggleEvents();
    }

    private void InitToggleEvents()
    {
        toggleInventoryAll.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.All));
        toggleInventoryEquipment.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Equipment));
        toggleInventoryUsed.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Used));

        // Local Method
        void UpdateFilter(bool flag, FilterOption option)
        {
            if (flag)
            {
                _currentFilterOption = option;
                // UpdateAllSlotFilters();
            }
        }
    }

    public Toggle setupToggleCurrentSeletion
    {
        get { return inventoryToggleGroup.ActiveToggles().FirstOrDefault(); }
    }

    public void SetUpToggleChoice()
    {
        if (inventoryToggleGroup.ActiveToggles().Any()) // Toggles 중 하나라도 Active된 Toggle이 있다면
        {
            if (setupToggleCurrentSeletion.name.Equals("Toggle : Equipment"))
            {
                SelectionActive(true, false, false);
            }
            else if (setupToggleCurrentSeletion.name.Equals("Toggle : Used"))
            {
                SelectionActive(false, true, false);
            }
            else if (setupToggleCurrentSeletion.name.Equals("Toggle : ETC"))
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
                        Debug.Log("equip");
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
