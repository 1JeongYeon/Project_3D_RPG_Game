using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;  // 무기 교체 중복 실행 방지. (True면 못하게) // 모든 무기들이 알게 static

    [SerializeField]
    private float changeweaponDelayTime;  // 무기 교체 딜레이 시간. 칼을 꺼내기 위해 움직이는 시간. 대략 Weapon_Out 애니메이션 시간.
    [SerializeField]
    private float changeweaponEndDelayTime;  // 무기 교체가 완전히 끝날 때 대략 Weapon_In 애니메이션 시간.
    
    [SerializeField]
    private Blade[] blades;  // 모든 종류의 칼 형 무기를 가지는 배열
     
    //관리 차원에서 이름으로 쉽게 무기 접근이 가능하도록 Dictionary 자료 구조 사용.
    private Dictionary<string, Blade> bladeDictionary = new Dictionary<string, Blade>(); // 아직 무기들 구현 x

    [SerializeField]
    private string currentWeaponType;  // 현재 무기의 타입 (칼, 손 등등)
    public static Transform currentWeapon;  // 현재 무기. static으로 선언하여 여러 스크립트에서 클래스 이름으로 바로 접근할 수 있게 함.
    public static Animator currentWeaponAnim; // 현재 무기의 애니메이션. static으로 선언하여 여러 스크립트에서 클래스 이름으로 바로 접근할 수 있게 함. 아직 에니메이션 별로 없음.

    [SerializeField] // 나중에 무기 종류 많아지면 추가해야함
    private BladeController bladeController; // 칼 일땐 BladeController.cs 활성화, 아니면 BladeController.cs 비활성화

    void Start()
    {
        for (int i = 0; i < blades.Length; i++)
        {
            bladeDictionary.Add(blades[i].bladeName, blades[i]);
        }
    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            /*if (Input.GetKeyDown(KeyCode.Alpha1)) // 1 누르면 '맨손'으로 무기 교체 실행
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손")); // 맨손일땐 무기 없어지고 이속이 빨라지면 좋겠다. // 아직 맨손 기능 x 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // 2 누르면 'Blade'으로 무기 교체 실행
            {
                StartCoroutine(ChangeWeaponCoroutine("BLADE", "The Bloodthirster")); // 일단 피바라기 
            }*/
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        // currentWeaponAnim.SetTrigger("Weapon_Out"); 애니메이션 아직 x

        yield return new WaitForSeconds(changeweaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeweaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "BLADE":
                BladeController.isActivate = false;
                break;
        }
    }
    
    void WeaponChange(string _type, string _name) // type은 일단 blade 하나만
    {
        if (_type == "BLADE")
        {
            bladeController.BladeChange(bladeDictionary[_name]);
        }
    }
}
