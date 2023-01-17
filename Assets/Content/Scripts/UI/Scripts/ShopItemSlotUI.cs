using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlotUI : MonoBehaviour
{
    [SerializeField]
    private Button itemButton;

    public Image purchaseFrameItemImage;

    public TMP_Text purchaseFrameItemPrice;

    public TMP_Text purchaseFrameItemDescript;

    public TMP_Text purchaseFrameItemName;

    


    [SerializeField]
    private Image shopItemImage;

    [SerializeField]
    private TMP_Text shopItemPrice;

    [SerializeField]
    private TMP_Text shopItemName;


    [SerializeField]
    private ItemManager itemManager;

    [SerializeField]
    private GameObject purchaseFrame;
    [SerializeField]
    private ShopItemCountInputNumber shopItemDetailPopup;

    void Start()
    {
        itemButton.onClick.AddListener(() =>
        {
            purchaseFrame.SetActive(true);
            PurchaseFrameItemGenerate();
            Item item = null;
            for (int i = 0; i < itemManager.items.Count; i++)
            {
                if (itemManager.items[i].itemName == purchaseFrameItemName.text)
                {
                    purchaseFrameItemDescript.text = itemManager.items[i].itemDescript;
                    item = itemManager.items[i];
                }
            }
            shopItemDetailPopup.OnSetData(item);
            shopItemDetailPopup.PriceSetting();
        });
    }

    private void PurchaseFrameItemGenerate()
    {
        purchaseFrameItemImage.sprite = shopItemImage.sprite;
        purchaseFrameItemPrice.text = shopItemPrice.text;
        purchaseFrameItemName.text = shopItemName.text;

        for (int i = 0; i < itemManager.items.Count; i++)
        {
            if (itemManager.items[i].itemName == purchaseFrameItemName.text)
            {
                purchaseFrameItemDescript.text = itemManager.items[i].itemDescript;
            }
        }
    }
}
