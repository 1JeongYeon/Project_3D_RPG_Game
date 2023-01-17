using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeController : MonoBehaviour
{
    // 근접무기 관련 다 여따가 넣기로 함 조율 필요

    [SerializeField]
    private Blade currentBlade; // 현재 장착된 칼 형 타입 무기 (칼) 
    
    private bool isAttack = false;  // 현재 공격 중인지 
    private bool isSwing = false;  // 팔을 휘두르는 중인지. isSwing = True 일 때만 데미지를 적용할 것이다.

    private RaycastHit hitInfo;  // 현재 무기(Hand)에 닿은 것들의 정보.

    public static bool isActivate = true;  // 활성화 여부

    void Update()
    {
        if (isActivate == true)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                // StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentBlade.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentBlade.attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentBlade.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentBlade.attackDelay - currentBlade.attackDelayA - currentBlade.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentBlade.range))
        {
            return true;
        }

        return false;
    }

    public void BladeChange(Blade _blade)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentBlade = _blade;
        WeaponManager.currentWeapon = currentBlade.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentBlade.anim; // 에니메이션 넣어야함

        currentBlade.transform.localPosition = Vector3.zero;
        currentBlade.gameObject.SetActive(true);

        isActivate = true;
    }
}
