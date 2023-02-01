using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopItemCountInputNumber : MonoBehaviour
{
    // ������ ���Ž� �ʿ��� UI Script


    // ������ �Է��� ��
    [SerializeField] private TMP_InputField text_Input;
    //  �� ���� ǥ��
    [SerializeField] private TMP_Text text_Preview;
    // ������ ����
    [SerializeField] private TMP_Text text_ItemPrice;
    // Ȱ��ȭ �� �� �ؽ�Ʈ ����� �ʱ�ȭ �ؾ��� text ���� inputfield �������� �ҷ��ͼ� ����� ����
    [SerializeField] private TMP_InputField if_text;
    // inputfield ui������Ʈ�� �Ҵ� ��
    [SerializeField] private GameObject go_Base;
    
    private Item item;
    private int itemCount;
   
    public int itemTotalPrice;

    private void Update()
    {
        // ���°� ����Ƽ ������ ���ӿ�����Ʈ SetActive = false�� �ǰ� �Ͽ���.
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
    
    public void PriceSetting() // ����Ƽ���� ȣ�� On Value Changed
    {
        if (string.IsNullOrEmpty(text_Input.text)) // ���ڿ��� null�̰ų� ������ ��
        {
            text_ItemPrice.text = item.itemPrice.ToString();
            return;
        }
        text_Input.text = text_Input.text.Replace(',', ' '); // ġȯ
        text_Input.text = text_Input.text.Trim(); // ��������
        text_Input.text = int.Parse(text_Input.text).ToString(); // ��ȯ
        text_ItemPrice.text = item.itemPrice.ToString();

        itemCount = int.Parse(text_Input.text);

        if (item.itemType != Item.ItemType.Equipment)
        {
            if (itemCount >= 100)
            {
                text_Input.text = 99.ToString();
                itemCount = 99;
                itemTotalPrice = item.itemPrice * itemCount;
                text_ItemPrice.text = SetTotalPrice(item.itemPrice, itemCount); // ������ ���߾� �ش�. 
            }
            else
            {
                itemTotalPrice = item.itemPrice * itemCount;
                text_ItemPrice.text = SetTotalPrice(item.itemPrice, itemCount); // ������ ���߾� �ش�. 
            }
        }
        else
        {
            text_Input.text = 1.ToString();
            itemCount = 1;
            itemTotalPrice = item.itemPrice;
            text_ItemPrice.text = SetTotalPrice(item.itemPrice, 1); // ������ ���߾� �ش�. 
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

    // ����Ƽ���� �����
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
