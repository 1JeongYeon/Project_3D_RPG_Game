using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SellItemUI : MonoBehaviour//, IDropHandler
{
    private bool activated;
    // 아이템 팔때 쓰는 ui ===================================================================
    [SerializeField] private GameObject sellItemBase;
    [SerializeField] private GameObject sellPopup_main;
    [SerializeField] private GameObject sellPopup_sub;

    [SerializeField] private TMP_InputField sellItemCountInput;
    [SerializeField] private TMP_InputField if_text;
    [SerializeField] private TMP_Text sellItemName;
    [SerializeField] private TMP_Text sellItemTotalPrice;
    [SerializeField] private TMP_Text text_Preview;

    [SerializeField] private Image sellItemImage;
    [SerializeField] private Button OKbtn;

    private void Start()
    {
        OKbtn.onClick.AddListener(OK);
    }

    private void Update()
    {
        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Return)) 
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape)) 
                Cancel();
        }
    }

    // 아이템 정보를 받아와줌
    public void Call()
    {
        if (ItemShadow.instance.itemShadowSlot != null)
        {
            Debug.Log("sellUI call 함수");

            Slot iss = ItemShadow.instance.itemShadowSlot;

            sellItemBase.SetActive(true);
            SellPopupSub();
            activated = true;
            if_text.text = "";
            sellItemImage.sprite = iss.item.itemImage;
            sellItemName.text = iss.item.itemName;
            sellItemTotalPrice.text = (iss.item.itemPrice * 0.7f).ToString();
            text_Preview.text = iss.itemCount.ToString();
        }
    }

    public void Cancel()
    {
        activated = false;
        ItemShadow.instance.SetColor(0);
        sellItemBase.SetActive(false);
        SellPopupMain();
        ItemShadow.instance.itemShadowSlot = null;
    }

    public void OK()
    {
        ItemShadow.instance.SetColor(0);

        int num;
        if (string.IsNullOrEmpty(sellItemCountInput.text))
        {
            // 숫자가 아닌걸 입력했을때임
            num = 1;

            // 숫자가 있다면
            if (CheckNumber(sellItemCountInput.text))
            {
                num = int.Parse(sellItemCountInput.text);
                if (num > ItemShadow.instance.itemShadowSlot.itemCount)
                {
                    num = ItemShadow.instance.itemShadowSlot.itemCount;
                    Debug.Log(num + ": OK 함수 num 1번");
                }
            }
        }
        else
        {
            num = int.Parse(text_Preview.text);
            Debug.Log(num + ": OK 함수 num 3번");
        }
        StartCoroutine(SellItemCorountine(num));
    }

    IEnumerator SellItemCorountine(int _num)
    {
        // 인벤토리에 추가함 내가 파는 아이템의 70프로 가격만
        int sellprice = (int)(ItemShadow.instance.itemShadowSlot.item.itemPrice * 0.7f);
        // 입력한 _num갯수만큼 for문
        for (int i = 0; i < _num; i++)
        {
            if (ItemShadow.instance.itemShadowSlot.item != null)
            {
                // 1씩 가격이 빠지는 버그(?) 때문에 1추가
                GameManager.Instance.Coin += (sellprice) + 1; 
                Inventory.Instance.coin.text = GameManager.Instance.Coin.ToString();
                // 개수 하나씩 줄임 입력 아이템 수 만큼 반복 
                ItemShadow.instance.itemShadowSlot.SetSlotCount(-1); 
                yield return new WaitForSeconds(0.05f);
            }
        }
        sellItemTotalPrice.text = ((_num * sellprice) + 1).ToString();
        ItemShadow.instance.itemShadowSlot = null;
        sellItemBase.SetActive(false);
        activated = false;
    }

    private bool CheckNumber(string _argString)
    {
        char[] _tempCharArray = _argString.ToCharArray();
        bool isNumber = true;

        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            // 아스키 코드 47 ~ 57 이면 숫자 아니면 문자
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57) 
                continue;

            isNumber = false;
        }
        return isNumber;
    }

    public void SellPopupMain()
    {
        sellItemBase.transform.SetParent(sellPopup_main.transform);
    }

    public void SellPopupSub()
    {
        sellItemBase.transform.SetParent(sellPopup_sub.transform);
    }
}
