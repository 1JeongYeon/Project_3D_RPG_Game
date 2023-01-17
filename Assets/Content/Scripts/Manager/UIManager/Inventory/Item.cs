using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject  // 게임 오브젝트에 붙일 필요 X 
{
    public enum ItemType  // 아이템 유형
    {
        Equipment,
        Used,
        ETC,
    }

    public int index; // 사용여부 대기중
    public string itemName; // 아이템의 이름
    public ItemType itemType; // 아이템 유형
    public Sprite itemImage; // 아이템의 이미지(인벤 토리 안에서 띄울)
    public GameObject itemPrefab;  // 아이템의 프리팹 (아이템 생성시 프리팹으로 찍어냄)
    public int itemPrice;
    public string itemDescript;

    public string weaponType;  // 무기 유형 

    public Item(string itemName, ItemType itemType, Sprite itemImage, GameObject itemPrefab, int itemPrice, string itemDescript)
    {
        this.itemName = itemName;
        this.itemType = itemType;
        this.itemImage = itemImage;
        this.itemPrefab = itemPrefab;
        this.itemPrice = itemPrice;
        this.itemDescript = itemDescript;
    }
}

