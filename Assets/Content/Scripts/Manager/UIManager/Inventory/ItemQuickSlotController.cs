using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemQuickSlotController : MonoBehaviour
{
    private ItemEffectDatabase theItemEffectDatabase;


    [SerializeField] private Slot[] quickSlots;  // 퀵슬롯들 (6개)
    [SerializeField] private Transform tf_parent;  // 퀵슬롯들의 부모 오브젝트

    private int selectedSlot;  // 선택된 퀵슬롯의 인덱스 (0~5)
    [SerializeField] private GameObject go_SelectedImage;  // 선택된 퀵슬롯 이미지
    
    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();

        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    private void Update()
    {
        TryUsingQuickSlot();
    }
    private void TryUsingQuickSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UsingSlotItem(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            UsingSlotItem(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            UsingSlotItem(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            UsingSlotItem(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            UsingSlotItem(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            UsingSlotItem(5);
    }
    private void UsingSlotItem(int _num)
    {
        SelectedSlot(_num);
        Execute();
    }

    private void SelectedSlot(int _num)
    {
        // 선택된 슬롯
        selectedSlot = _num;

        // 선택된 슬롯으로 선택 효과 이미지 이동하기
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    // 아이템 사용 처리하는 함수
    private void Execute()
    {
        StatusManager sm = GameManager.Instance.statusMgr;
        if (quickSlots[selectedSlot].item != null)
        {
            // 선택된 퀵슬롯의 아이템이 소모품일때
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)
            {
                // 체력이나 마나가 꽉 차있지 않을때
                if (!(sm.currentHp == sm.maxHp))
                {
                    theItemEffectDatabase.UseItem(quickSlots[selectedSlot].item);
                    quickSlots[selectedSlot].SetSlotCount(-1);
                }
                else if (!(sm.currentMp == sm.maxMp))
                {
                    theItemEffectDatabase.UseItem(quickSlots[selectedSlot].item);
                    quickSlots[selectedSlot].SetSlotCount(-1);
                }
                else
                {
                    Debug.Log("상태가 좋아서 사용할 수 없어요~");
                }
            }
            else
            {
                Debug.Log("소비 아이템만 사용 가능");
            }
        }
        else
        {
            Debug.Log("Item이 없어영~");
        }
    }

    public void IsActivatedQuickSlot(int _num)
    {
        if (selectedSlot == _num)
        {
            Execute();
            return;
        }
        if (ItemShadow.instance != null)
        {
            if (ItemShadow.instance.itemShadowSlot != null)
            {
                if (ItemShadow.instance.itemShadowSlot.GetQuickSlotIndex() == selectedSlot)
                {
                    Execute();
                    return;
                }
            }
        }
    }


}
