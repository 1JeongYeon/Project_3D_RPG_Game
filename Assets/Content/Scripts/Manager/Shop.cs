using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    
    public Animator anim; // 상점 드가고 나갈때 npc가 보여주는 간단한 애니메이션 넣기 위함

    [SerializeField]
    private Player enterPlayer;

    public static bool shopActivated = false;  // 상점 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.
    private GameObject go_ShopBase; // Shop_Base 이미지

    public GameObject itemObject;

    public int itemPrice;

    public Transform itemSpawnPos;
    
    public ShopItemSlotUI shopItems;
    

    private Inventory inventory = null;
    
    public void Enter(Player player)
    {
        enterPlayer = player;
        shopActivated = true;
        
        if(go_ShopBase == null)
        {
            string path = "Canvas/Shop UI/Image : ShopFrame";
            go_ShopBase = GameManager.Instance.FindGameObject(path);
        }
        
        go_ShopBase.SetActive(true);

        FindObject();
        inventory.OpenInventory();
    }

    public void Exit()
    {
        if (go_ShopBase != null)
        {
            shopActivated = false;
            go_ShopBase.SetActive(false);
            FindObject();
            inventory.CloseInventory();
        }
    }

    void FindObject()
    {
        if (inventory == null)
        {
            string path = "Canvas/Inventory";
            inventory = GameManager.Instance.FindGameObject(path).GetComponent<Inventory>();
        }
        if (go_ShopBase == null)
        {
            string path = "Canvas/Shop UI/Image : ShopFrame";
            go_ShopBase = GameManager.Instance.FindGameObject(path);
        }
    }
}
