using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect // 한 아이템에 대한 효과를 위한 class
{
    // 아이템의 이름  -> Key
    public string itemName;

    // DP 는 포션으로 회복이 되면 안된다.
    [Tooltip("HP, MP 만 구현")]
    // 효과. 어느 부분을 회복하거나 버프를 줄 포션인지. 포션 하나당 미치는 효과가 여러개일 수 있어 배열로 함.
    public string[] part;
    // 수치. 포션 하나당 미치는 효과가 여러개일 수 있어 배열로 함. 그에 따른 수치 정하기.
    public int[] num;  
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects; // 여러가지 추가 효과를 위한 배열
    //ItemEffectDatabase.cs 에서 여러 포션 아이템들을 이 `itemEffects` 배열 멤버에서 한번에 관리하게 됨.

    private const string HP = "HP", MP = "MP"/*, DP = "DP"*/;  // DP 는 포션으로 회복 불가능 하게 
    
    private StatusManager thePlayerStatus; 
    [SerializeField]
    private WeaponManager theWeaponManager;

    [SerializeField]
    private ItemQuickSlotController theItemQuickSlotController;

    void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusManager>();
    }
    // QuickSlotController -> Slot의 징검다리 역할
    public void IsActivatedquickSlot(int _num)
    {
        theItemQuickSlotController.IsActivatedQuickSlot(_num);
    }

    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
          // 장비템일 경우 장착하는 코드지만 아직 장착템 구현 x
          // StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
        }
        if (_item.itemType == Item.ItemType.Used) // 소모품일때
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == _item.itemName)
                {
                    for (int j = 0; j < itemEffects[i].part.Length; j++)
                    {
                        switch (itemEffects[i].part[j])
                        {
                            case HP:
                                if (thePlayerStatus.maxHp == thePlayerStatus.currentHp)
                                {
                                    Debug.Log("HP가 최대치입니다.");
                                }
                                else
                                {
                                    Debug.Log("HP 회복");
                                    thePlayerStatus.IncreaseHP(itemEffects[i].num[j]);
                                }
                                break;
                            case MP:
                                if (thePlayerStatus.maxMp == thePlayerStatus.currentMp)
                                {
                                    Debug.Log("MP가 최대치입니다.");
                                }
                                else
                                {
                                    Debug.Log("MP 회복");
                                    thePlayerStatus.IncreaseMana(itemEffects[i].num[j]);
                                }
                                break;
                            
                            default:
                                Debug.Log("HP, MP 만 가능합니다.");
                                break;
                        }
                        Debug.Log(_item.itemName + " 을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("itemEffectDatabase에 일치하는 itemName이 없습니다.");
        }
    }
}