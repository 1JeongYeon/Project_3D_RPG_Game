using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage;  // 아이템의 이미지

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private InputNumber theInputNumber;

    private SellItemUI theSellItemUI;

    private ItemEffectDatabase theItemEffectDatabase;


    private bool isAccessibleItem = true; // 아이템 접근가능 여부

    [SerializeField] private bool isQuickSlot;  // 해당 슬롯이 퀵슬롯인지 여부 판단
    [SerializeField] private int quickSlotIndex;  // 퀵슬롯 넘버

    private void Awake()
    {
        theSellItemUI = FindObjectOfType<SellItemUI>();

    }
    void Start()
    {
        theInputNumber = FindObjectOfType<InputNumber>();
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }


    // 아이템 이미지의 투명도 조절
    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 인벤토리에 새로운 아이템 슬롯 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment) //장비템이 아닐때 
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else // 장비템이면 숫자 표시 x 숫자 표시 이미지랑 텍스트 x
        {
            text_Count.text = "0"; // 순서 주의 자식 먼저
            go_CountImage.SetActive(false);
        }
        SetColor(1);
    }

    // 해당 슬롯의 아이템 갯수 업데이트
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 해당 슬롯 하나 삭제
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) // ItemEffectDataBase에서 기능 일부 실행 성능 상승
    {
        StatusManager sm = GameManager.Instance.statusMgr;
        if (eventData.button == PointerEventData.InputButton.Right) // 오른쪽 마우스 누를때 사용
        {
            if (item != null)
            {
                theItemEffectDatabase.UseItem(item); // database에서 아이템 사용 관련 모두 실행

                if (item.itemType == Item.ItemType.Used)
                {
                    // 피가 가득 차있거나 마나가 가득 차있을 때가 아닐때에만 갯수를 줄임
                    if (!(sm.currentHp == sm.maxHp))
                    {
                        SetSlotCount(-1);
                    }
                    else if (!(sm.currentMp == sm.maxMp))
                    {
                        SetSlotCount(-1);
                    }
                    else
                    {
                        Debug.Log("상태가 좋아서 사용할 수 없어요~");
                    }
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작 될때
    {
        if (item != null)
        {
            ItemShadow.instance.itemShadowSlot = this;
            ItemShadow.instance.DragSetImage(itemImage);
            ItemShadow.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) // 드래그 중일 때
    {
        if (item != null)
        {
            ItemShadow.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) // 드래그 끝났을 때
    {
        // 드래그가 끝나는 시점의 raycast를 쐈을때 첫번째로 contact된 값을 가진다.
        RaycastResult result = eventData.pointerCurrentRaycast;
        // 3가지 조건
        switch (result.gameObject.name)
        {
            // 상점에 판매 할때
            case "Image : ItemSlot":
            case "Image : ShopFrame":
                {
                    // 호출해야함 샵에서 파는 UI 그리고 파는 함수 등등
                    if (ItemShadow.instance.itemShadowSlot != null)
                    {
                        theSellItemUI.Call();  
                    }
                }
                break;
            // 버릴때
            case "Image :: BG":
                {
                    if (ItemShadow.instance.itemShadowSlot != null) 
                    {
                        theInputNumber.Call();
                    }
                }
                break;
            // 취소 = 되돌리기
            default:
                {
                    ItemShadow.instance.SetColor(0);
                    ItemShadow.instance.itemShadowSlot = null;
                }
                break;
        }
    }

    public void OnDrop(PointerEventData eventData) // 해당 슬롯에 뭔가가 마우스 드롭 됐을 때 즉 나 자신에게 뭔가 드롭된게 있을 때 호출 
    {
        if (ItemShadow.instance.itemShadowSlot != null)
        {
            ChangeSlot();

            if (isQuickSlot)  // 인벤토리->퀵슬롯 or 퀵슬롯->퀵슬롯
            {
                theItemEffectDatabase.IsActivatedquickSlot(quickSlotIndex);
            }
            else  // 인벤토리->인벤토리. 퀵슬롯->인벤토리
            {
                if (ItemShadow.instance.itemShadowSlot.isQuickSlot)  // 퀵슬롯->인벤토리
                {
                    theItemEffectDatabase.IsActivatedquickSlot(ItemShadow.instance.itemShadowSlot.quickSlotIndex);
                }
            }
        }
    }

    public void ChangeSlot()
    {
        // a슬롯 드래그 한 슬롯 b 슬롯 드래그 당하는 슬롯
        // 바뀌어지는 슬롯 정보들 보관할 변수
        Item _tempItem = item; 
        int _tempItemCount = itemCount;
        // a 슬롯은 ItemShadow에서 참조 중. ChangeSlot()이 호출될때 드롭 위치가 된 슬롯에다가 a 슬롯을 추가
        AddItem(ItemShadow.instance.itemShadowSlot.item, ItemShadow.instance.itemShadowSlot.itemCount);
        // a슬롯에 보관해놨던 b슬롯 추가함
        if (_tempItem != null) //빈슬롯이 아니라면 추가
        {
            ItemShadow.instance.itemShadowSlot.AddItem(_tempItem, _tempItemCount);
        }
        else // 빈슬롯이었다면 드래그 한 슬롯을 비워줌
        {
            ItemShadow.instance.itemShadowSlot.ClearSlot();
        }
    }
    public void SetItemAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if (isAccessibleItem == value) return;

        if (value)
        {
            SetColor(1f);
            go_CountImage.SetActive(false);
        }
        else
        {
            SetColor(0.3f);
            go_CountImage.SetActive(false);
        }

        isAccessibleItem = value;
    }

    public int GetQuickSlotIndex()
    {
        return quickSlotIndex;
    }
}
