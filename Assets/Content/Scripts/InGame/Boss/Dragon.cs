using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dragon : MonoBehaviour
{
    public enum Animation
    {
        //Idle01,
        Idle02,
        Sleep,
        Walk,
        Run,
        NormalAttack,
        ClawAttack,
        FlameAttack,
        Scream,
        //Fly,
        //FlyFlameAttack,
        //Fly_Down,
        //Hit,
        Die
    }
    
    public enum ActionType
    {
        Sleep,
        Wait,
        Move,
        NorAttack,
        ClawAttack,
        Hit,
        Scream,
        Die
    }

    public enum State 
    {   
        Idle, 
        BackChasing, 
        Chasing,
        Dead 
    }
    // 초기 드래곤 actionType은 자는걸로 해둠
    public ActionType actionType = ActionType.Sleep;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackCollsion;
    [SerializeField] private DigitalRuby.PyroParticles.FireBaseScript fire;
    [SerializeField] private Transform fireParent;

    [SerializeField] private GameObject[] itemPrefab;
    private CombatUI combatUI = null;
    [SerializeField] private Transform homPos;
    private bool isScream = false;

    private string[] animationTriggerNames 
        = { "idle", "sleep", "walk", "run", "normalA", "clawA", "flameA", "scream", "die" };

    private float attackDelayTime = 0f;
    // 공격 이후 딜레이 타임 변수 지정
    private float nextATKDelayTime = 5f;
    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Sleep();
    }

    void Update()
    {
        if (GameManager.Instance.statusMgr.currentHp <= 0)
        {
            player.PlayerDIe();
            return;
        }
        float dis = Vector3.Distance(transform.position, player.transform.position);
        if (actionType == ActionType.Sleep)
        {
            return;
        }
        // 일정 조건일때 걸어오게 하고 플레이어를 쳐다보게 함
        if (dis > 12 && isScream)
        {
            Walk();
            transform.LookAt(player.transform);
        }
        // 가까워지면 공격하는 코루틴 실행후 종료
        else
        {
            StartCoroutine("Attack");
            StopCoroutine("Attack");
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackCollision"))
        {
            TargettingIMG.Instance.Damageing(player.playerDamage);
        }
    }

    IEnumerator Attack()
    {
        attackDelayTime += Time.deltaTime;
        if (attackDelayTime > nextATKDelayTime)
        {
            attackDelayTime = 0;

            Animation[] randAniType = { Animation.Idle02, Animation.NormalAttack, Animation.ClawAttack};
            int rand = Random.Range(1, randAniType.Length);
            // Random 변수 rand를 이용하여 다양한 에니메이션 활성화
            switch (randAniType[rand])
            {
                case Animation.Idle02:
                    Wait();
                    yield return null;
                    break;
                case Animation.NormalAttack:
                    StartCoroutine(NormalAttack());
                    GameManager.Instance.statusMgr.GSHP(-10);
                    Debug.Log("공격당했다!");
                    break;
                case Animation.ClawAttack:
                    StartCoroutine(ClawAttack());
                    GameManager.Instance.statusMgr.GSHP(-20);
                    Debug.Log("치명타를 입었다!");
                    break;
                /*case Animation.FlameAttack:
                    StartCoroutine(FlameAttack());
                    break;*/
            }
        }
        Wait();
    }

    void SetAnimation(int animationIndex)
    {
        animator.SetTrigger(animationTriggerNames[animationIndex]);
    }

    public IEnumerator Kill()
    {
        if (player == null)
            yield return null;
        // 플레이어가 죽었을 때
        if (GameManager.Instance.statusMgr.currentHp <= 0)
        {
            yield return new WaitForSeconds(2f);
            Sleep();
        }
    }

    public void Scream()
    {
        actionType = ActionType.Scream;
        SetAnimation((int)Animation.Scream);
    }

    public void Wait()
    {
        actionType = ActionType.Wait;
        SetAnimation((int)Animation.Idle02);
    }

    void Sleep()
    {
        actionType = ActionType.Sleep;
        SetAnimation((int)Animation.Sleep);
    }
    void Walk()
    {
        actionType = ActionType.Move;
        SetAnimation((int)Animation.Walk);
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * 0.28f);
    }

    IEnumerator NormalAttack()
    {
        nextATKDelayTime = 3f;
        actionType = ActionType.NorAttack;
        SetAnimation((int)Animation.NormalAttack);

        // attackCollsion을 짧은 시간동안 active 하여 공격 순간을 구현한다.
        yield return new WaitForSeconds(0.2f);
        attackCollsion.SetActive(true);

        yield return new WaitForSeconds(1f);
        attackCollsion.SetActive(false);

        yield return new WaitForSeconds(2f);
        transform.LookAt(player.transform);
    }

    IEnumerator ClawAttack()
    {
        nextATKDelayTime = 4f;
        actionType = ActionType.ClawAttack;
        SetAnimation((int)Animation.ClawAttack);

        yield return new WaitForSeconds(0.2f);
        attackCollsion.SetActive(true);

        yield return new WaitForSeconds(3f);
        attackCollsion.SetActive(false);

        yield return new WaitForSeconds(4f);
        transform.LookAt(player.transform);

    }

    //IEnumerator FlameAttack()
    //{
    //    nextATKDelayTime = 8f;
    //    actionType = ActionType.Attack;
    //    SetAnimation((int)Animation.FlameAttack);

    //    yield return new WaitForSeconds(1.5f);
    //    Instantiate(fire.gameObject, fireParent);
    //}

    public void Damage(int damage)
    {
        if (combatUI == null)
        {
            combatUI = GameObject.Find("CombatUI(Clone)").GetComponent<CombatUI>();
        }
        combatUI.DecreaseEnemyHP(damage);
    }

    public IEnumerator Die()
    {
        if (CombatUI.Instance.enemyCurrentHp <= 0)
        {
            yield return null;
        }
        if (CombatUI.Instance.enemyCurrentHp <= 0)
        {
            yield return new WaitForSeconds(0.5f);
            SetAnimation((int)Animation.Die);

            yield return new WaitForSeconds(3f);
            ItemSpawn();
            Destroy(gameObject);
            GameManager.Instance.statusMgr.IncreaseEXP(150);
            GameManager.Instance.dataMgr.SaveGameData();
            Debug.Log("경험치 150을 얻었습니다");
        }
    }

    private void ItemSpawn()
    {
        int spawnItem = Random.Range(0, 100);
        if (spawnItem < 60)
        {
            Instantiate(itemPrefab[Random.Range(0, 3)], transform.position, Quaternion.identity);
        }
    }
    public void BossAni()
    {
        StartCoroutine(Open());
    }
    IEnumerator Open()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("BlueDragon").GetComponent<Dragon>().Scream();
        yield return new WaitForSeconds(3.9f);
        isScream = true;
    }
}
