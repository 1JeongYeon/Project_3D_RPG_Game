using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopItemCountInputNumber : MonoBehaviour
{
    // 아이템 구매시 필요한 UI Script


    // 유저가 입력한 수
    [SerializeField] private TMP_InputField text_Input;
    //  템 갯수 표시
    [SerializeField] private TMP_Text text_Preview;
    // 아이템 가격
    [SerializeField] private TMP_Text text_ItemPrice;
    // 활성화 시 전 텍스트 지우고 초기화 해야함 text 말고 inputfield 형식으로 불러와서 덮어씌임 방지
    [SerializeField] private TMP_InputField if_text;
    // inputfield ui오브젝트가 할당 됨
    [SerializeField] private GameObject go_Base;
    
    private Item item;
    private int itemCount;
   
    public int itemTotalPrice;

    private void Update()
    {
        // 끄는건 유니티 내에서 게임오브젝트 SetActive = false가 되게 하였다.
        if (go_Base)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                OnBuy();
                go_Base.SetActive(false);
            }
        }
    }

    public void OnSetData(Item argItem)
    {
        item = argItem;
    }
    
    public void PriceSetting() // 유니티에서 호출 On Value Changed
    {
        if (string.IsNullOrEmpty(text_Input.text)) // 문자열이 null이거나 공백일 때
        {
            text_ItemPrice.text = item.itemPrice.ToString();
            return;
        }
        text_Input.text = text_Input.text.Replace(',', ' '); // 치환
        text_Input.text = text_Input.text.Trim(); // 공백제거
        text_Input.text = int.Parse(text_Input.text).ToString(); // 변환
        text_ItemPrice.text = item.itemPrice.ToString();

        itemCount = int.Parse(text_Input.text);

        if (item.itemType != Item.ItemType.Equipment)
        {
            if (itemCount >= 100)
            {
                text_Input.text = 99.ToString();
                itemCount = 99;
                itemTotalPrice = item.itemPrice * itemCount;
                text_ItemPrice.text = SetTotalPrice(item.itemPrice, itemCount); // 가격을 맞추어 준다. 
            }
            else
            {
                itemTotalPrice = item.itemPrice * itemCount;
                text_ItemPrice.text = SetTotalPrice(item.itemPrice, itemCount); // 가격을 맞추어 준다. 
            }
        }
        else
        {
            text_Input.text = 1.ToString();
            itemCount = 1;
            itemTotalPrice = item.itemPrice;
            text_ItemPrice.text = SetTotalPrice(item.itemPrice, 1); // 가격을 맞추어 준다. 
        }
    }

    string SetTotalPrice(int price, int count)
    {
        if(price < 1000)
        {
            if (price * count >= 1000)
            {
                return string.Format("{0:0,000}", price * count);
            }
            return string.Format("{0}", price * count);
        }
        else
        {
            return string.Format("{0:0,000}", price * count);
        }
    }

    // 유니티에서 사용중
    public void OnBuy()
    {
        int coin = GameManager.Instance.Coin;
        if (coin >= itemTotalPrice)
        {
            Inventory.Instance.AcquireItem(item, itemCount);
            
            if (itemTotalPrice != 0)
            {
                if (coin >= itemTotalPrice)
                {
                    GameManager.Instance.Coin -= itemTotalPrice;
                    Inventory.Instance.coin.text = string.Format("{0:0,000}", GameManager.Instance.Coin.ToString());
                }
            }
            text_Input.text = 1.ToString();
        }
    }
}
