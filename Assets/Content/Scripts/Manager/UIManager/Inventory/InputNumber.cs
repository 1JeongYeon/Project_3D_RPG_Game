using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false; // 활성화 되어있는지 아닌지

    [SerializeField]
    private Text text_Preview; // 드랍된 템 갯수 표시
    [SerializeField]
    private Text text_Input; // 유저가 입력한 수
    [SerializeField]
    private InputField if_text; // 활성화 시 전 텍스트 지우고 초기화 해야함 text 말고
                                // inputfield 형식으로 불러와서 덮어씌임 방지

    [SerializeField]
    private GameObject go_Base; // inputfield ui오브젝트가 할당 됨, 입력필드 활성화 비활을 위해 겜오브젝트 형식

    [SerializeField]
    private ActionController thePlayer; // 버린 아이템 만들라고 actioncontroller 참조 할것

    [SerializeField]
    private Transform thePlayerTrans;

    void Update()
    {
        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Return)) OK();

            else if (Input.GetKeyDown(KeyCode.Escape)) Cancel();
        }
    }

    public void Call() // 드래그가 끝날때 호출
    {
        go_Base.SetActive(true);
        activated = true;
        if_text.text = "";
        text_Preview.text = ItemShadow.instance.itemShadowSlot.itemCount.ToString();
    }

    public void Cancel()
    {
        activated = false;
        ItemShadow.instance.SetColor(0);
        go_Base.SetActive(false);
        ItemShadow.instance.itemShadowSlot = null;
    }

    public void OK()
    {
        ItemShadow.instance.SetColor(0);

        int num;
        if (text_Input.text != "") // 숫자가 있다면
        {
            if (CheckNumber(text_Input.text))
            {
                num = int.Parse(text_Input.text);
                if (num > ItemShadow.instance.itemShadowSlot.itemCount)
                {
                    num = ItemShadow.instance.itemShadowSlot.itemCount;
                }
            }
            else
            {
                num = 1;
            }
                
        }
        else
        {
            num = int.Parse(text_Preview.text);

        }
        StartCoroutine(DropItemCorountine(num));
    }

    IEnumerator DropItemCorountine(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            if (ItemShadow.instance.itemShadowSlot.item.itemPrefab != null) // 아이템 팔때 inputnumber base 안끄고 팔면 여기서 null (당연함)
            {
                Transform trans = GameObject.Find("Player").transform; // 자기 자신을 찾고
                Instantiate(ItemShadow.instance.itemShadowSlot.item.itemPrefab,
                trans.position + trans.forward,
                Quaternion.identity); // 아이템 프리팹 생성 플레이어 좀 앞에다가
            }
            ItemShadow.instance.itemShadowSlot.SetSlotCount(-1); // 개수 하나씩 줄임 입력 아이템 수 만큼 반복
            yield return new WaitForSeconds(0.05f);
        }

        ItemShadow.instance.itemShadowSlot = null;
        go_Base.SetActive(false);
        activated = false;
    }

    private bool CheckNumber(string _argString)
    {
        char[] _tempCharArray = _argString.ToCharArray();
        bool isNumber = true;

        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57) continue; // 아스키 코드 47 ~ 57 이면 숫자 아니면 문자
            
            isNumber = false;
        }
        return isNumber;
    }
}