using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    // 싱글톤으로 접근 쉽게 함
    private static CombatUI instance;
    public static CombatUI Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<CombatUI>();
            return instance;
        }
    }

    // 필요한 이미지 배열
    [SerializeField]
    private Image[] enemyImages;

    //[SerializeField]
   //private TMP_Text enemyName;
    [SerializeField]
    private TMP_Text enemyHP_Text;

    // E_ 를 붙여서 적 관련 index로 확인함
    private const int E_HP = 0, E_FACE = 1; // 아직 index를 부여해야할지 고민중
    private Dragon dragon;
    // 적 체력
    public int enemyMaxHp;  // 최대 체력. 유니티 에서 지정
    public int enemyCurrentHp;
    private Color color;


    private void Start()
    {
        dragon = GameObject.FindGameObjectWithTag("Dragon").GetComponent<Dragon>();
        enemyCurrentHp = enemyMaxHp =1000;
    }
    private void Update()
    {
        
    }
    public void EnemyHPSetting(int hp, int mp = 0)
    {
        enemyCurrentHp = hp;
        enemyMaxHp = hp;
    }

    public void IncreaseEnemyHP(int _count)
    {
        if (enemyCurrentHp + _count < enemyMaxHp)
        {
            enemyCurrentHp += _count;
        }
        else
        {
            enemyCurrentHp = enemyMaxHp;
        }
        enemyHP_Text.text = $"{enemyCurrentHp} / {enemyMaxHp}";
    }

    public void DecreaseEnemyHP(int _count)
    {
        if (enemyCurrentHp <= 0)
            return;

        enemyCurrentHp -= _count;
        enemyImages[E_HP].fillAmount = (float)enemyCurrentHp / enemyMaxHp;

        enemyHP_Text.text = $"{enemyCurrentHp} / {enemyMaxHp}";

        if (enemyCurrentHp <= 0)
        {
            StartCoroutine(dragon.Die());
            Debug.Log("보스가 죽었다~ (보스 죽는 함수 실행)");
        }
    }
}

