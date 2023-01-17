using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public enum State { Idle, BackChasing, Chasing, Dead}
    State currentState;
    public enum Type { Jelly, TurtleShell, Slime, Dragon} // 몬스터 타입
    [SerializeField]
    private Type enemyType; // 타입 나누기위한 변수
    [SerializeField]
    public BoxCollider area;
    [SerializeField]
    private GameObject[] itemPrefab;
    [SerializeField]
    private Transform homPos;
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float followRange = 6.0f;
    [SerializeField]
    private float attackRange = 1.5f;
    public GameObject bullet;
    public NavMeshAgent navMeshAgent;
    private EnemyHP enemyHP;
    private Player player;
    public Animator anim;
    public Rigidbody rigid;
    public bool isAttack;
    private bool dead;
    public bool isDamage;
    private float attackDistance = 0.5f; // 공격 사거리
    private float attackTime = 1; // 공격 딜레이
    private float nextAttackTime; // 다음 공격 가능한 시간
    private float myColiisionRadius; // 자신의 충돌 범위
    private float targetCollisionRadius; // 타겟 충돌 범위

    Vector3 firstPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        rigid = GetComponent<Rigidbody>();
        enemyHP = GetComponent<EnemyHP>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        myColiisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = player.GetComponent<CharacterController>().radius;
    }

    private void Start()
    {
        currentState = State.Idle;
        firstPos = transform.position;
        StartCoroutine("UpdatePath");
    }
    
    private void Update()
    {
        if (player == null)
            return;

        if(Time.time > nextAttackTime && enemyType != Type.Dragon)
        {
            // (목표 위치 - 자신의 위치) 제곱을 한 수
            float toTarget = (player.transform.position - transform.position).sqrMagnitude;

            if(toTarget < Mathf.Pow(attackDistance + myColiisionRadius + targetCollisionRadius, 2))
            {
                nextAttackTime = Time.time + attackTime;
            }
        }
        Targeting();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("AttackCollision"))
        { 
            TargettingIMG.Instance.Damageing(player.playerDamage);
        }
        
    }
    private void Targeting()
    {
        if (player == null)
            return;

        if (!dead && enemyType != Type.Dragon)
        {
            float targetRadius = 0; //SphereCast() 반지름 조정할 변수 
            float targetRange = 0; //SphereCast() 길이 조정할 변수 

            switch (enemyType)
            {
                case Type.Jelly:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.TurtleShell:
                    targetRadius = 0.8f;
                    targetRange = 6f;
                    break;
                case Type.Slime:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,  // 레이로 쏴서 Player Layer 찍힌 타겟이면 앞 방향으로 따라가기. 
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StopCoroutine(Attack());
                StartCoroutine(Attack());
            }
        }
    }

    public void Movement()
    {
        if (player == null)
            return;

        if (currentState == State.Chasing )
        {
            GameManager.Instance.statusMgr.GSHP(-5);
            if (GameManager.Instance.statusMgr.currentHp <= 0)
            {
                isAttack = false;
                StopCoroutine(Attack());
                player.PlayerDIe();
            }
        }
    }
    public IEnumerator Attack()
    {
        currentState = State.Chasing;
        isAttack = true;
        //isDamage = true;
        if(enemyType != Type.Slime)
        {
            Animation("onAttack");
        }

        switch (enemyType)
        {
            case Type.Jelly: // 일반 몬스터 A타입
                yield return new WaitForSeconds(0.2f);
                transform.LookAt(player.transform.position);
                area.enabled = true;  // AttackCollision 활성화

                yield return new WaitForSeconds(1f);
                area.enabled = false; // AttackCollision 비활성화

                yield return new WaitForSeconds(1f);
                break;
            case Type.TurtleShell: // 돌격형 몬스터 B타입
                yield return new WaitForSeconds(0.1f);
                transform.LookAt(player.transform.position);
                rigid.AddForce(transform.forward * 0.2f, ForceMode.Impulse);
                area.enabled = true; // AttackCollision 활성화

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                area.enabled = false; // AttackCollision 비활성화

                yield return new WaitForSeconds(2f);
                break;
            case Type.Slime: // 원거리 몬스터 C타입
                yield return new WaitForSeconds(0.5f);
                transform.LookAt(player.transform.position);
                yield return new WaitForSeconds(1f);
                GameObject instantBullet = Instantiate(bullet, transform.position , transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 3;
                
                yield return new WaitForSeconds(2f);
                break;
        }
        currentState = State.Idle;
        isAttack = false;
        //isDamage = false;
    }
    IEnumerator UpdatePath()
    {
        float rate = 0.25f;

        while(player != null)
        {
            if(GameManager.Instance.statusMgr.currentHp <= 0)
            {
                currentState = State.BackChasing;
                player = null;
            }
            else
            {
                if (currentState == State.Idle && Vector3.Distance(player.transform.position, transform.position) < 6)
                {
                    currentState = State.Chasing;
                }
                else if (currentState == State.Chasing && Vector3.Distance(firstPos, player.transform.position) > 8)
                {
                    currentState = State.BackChasing;
                }
            }

            if(currentState == State.Chasing)
            {
                Vector3 dirToTarget = (player.transform.position - transform.position).normalized;
                Vector3 targetPosition = (player.transform.position - dirToTarget * (myColiisionRadius + targetCollisionRadius + attackDistance / 2));
                if(!dead)
                {
                    navMeshAgent.SetDestination(targetPosition);
                }
            }
            // 스폰된 값과 거리가 멀어지면 자기 자리 찾으러 간다.
            else if(currentState == State.BackChasing)
            {
                Animation("onIdle");
                transform.LookAt(firstPos);
                navMeshAgent.SetDestination(firstPos);
                if(Vector3.Distance(firstPos, transform.position) <= 2f)
                {
                    currentState = State.Idle;
                }
            }
            yield return new WaitForSeconds(rate);
        }
    }
    public IEnumerator OnDie()
    {
         if (enemyHP.CurrentHP <= 0)
         {
            dead = true;
            Animation("onDie");
            enemyHP.hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
            if(enemyType == Type.TurtleShell)
            {
                GameObject.Find("Dungeons/OpenWall").GetComponent<Open>().enemyCount--;
            }
            yield return new WaitForSeconds(0.8f);
            SpawnItem();
            Destroy(gameObject);
            GameManager.Instance.statusMgr.IncreaseEXP(10);
            GameManager.Instance.dataMgr.SaveGameData();
            Debug.Log("경험치 10을 얻었습니다");
        }   
    }
    private void SpawnItem()
    {
        int spawnItem = Random.Range(0, 100);
        if(spawnItem < 30)
        {
            Instantiate(itemPrefab[Random.Range(0, 3)], transform.position, Quaternion.identity);
            Instantiate(itemPrefab[Random.Range(0, 3)], transform.position, Quaternion.identity);
        }
        else if (spawnItem < 60)
        {
            Instantiate(itemPrefab[Random.Range(0, 3)], transform.position, Quaternion.identity);
        }
    }

    void Animation(string animName)
    {
        anim.SetTrigger(animName);
    }
}
