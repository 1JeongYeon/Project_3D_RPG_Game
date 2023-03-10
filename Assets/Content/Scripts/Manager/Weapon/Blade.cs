using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public string bladeName;  // 너클이나 맨 손 구분. 이름으로 구분할 것이다.
    public float range; // 공격 범위. 팔을 뻗으면 어디까지 공격이 닿을지
    public int damage; // 공격력
    public float workSpeed; // 작업 속도
    public float attackDelay;  // 공격 딜레이. 마우스 클릭하는 순간 마다 공격할 순 없으므로.
    public float attackDelayA;  // 공격 활성화 시점. 공격 애니메이션 중에서 칼이 다 휘둘러졌을 때 공격 데미지가 들어가야 한다.
    public float attackDelayB;  // 공격 비활성화 시점. 칼을 휘두르는 애니메이션이 끝나면 공격 데미지가 들어가면 안된다.

    public Animator anim;  // 애니메이터 컴포넌트
    public BoxCollider boxCollider; // 맞는 콜라이더 범위 정하기
}