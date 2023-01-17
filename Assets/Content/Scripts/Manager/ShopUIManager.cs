using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private ItemManager itemManager;

    [SerializeField] private GameObject shop_Base;
    [SerializeField] private GameObject purchaseFrame;
    [SerializeField] private TMP_Text[] priceText;
    [SerializeField] private TMP_Text[] shopItemName;
    // 각각 할당되어야 함 리스트 안에 있는 아이템이라
    [SerializeField] private Image[] selectedItemImage;

    [SerializeField] private Button exitButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button cancelButton;

    private void Start()
    {
        exitButton.onClick.AddListener(() =>
        {
            shop_Base.SetActive(false);
        });

        cancelButton.onClick.AddListener(CancelButton);

        if (Input.GetKeyDown(KeyCode.Escape)) // 편의성
        {
            CancelButton();
            Cursor.lockState = CursorLockMode.Locked;
        }

        ShopItemListGenerate();
    }


    public void CancelButton()
    {
        purchaseFrame.SetActive(false);
    }

    private void ShopItemListGenerate()
    {
        for (int i = 0; i < itemManager.items.Count; i++)
        {
            if (i == itemManager.items[i].index)
            {
                shopItemName[i].text = itemManager.items[i].itemName;
                selectedItemImage[i].sprite = itemManager.items[i].itemImage;
                priceText[i].text = itemManager.items[i].itemPrice.ToString();
            }
        }
    }
}
