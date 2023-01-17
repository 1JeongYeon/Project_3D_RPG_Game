using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject animationObj;
    [SerializeField] private CameraController cameraCont;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController charCont;
    private Animator anim;
    private GameObject nearObject; // 상점 출입 및 각종 상호작용 위한 변수 작성
    private Vector3 moveDir;

    private float moveSpeed = 8f;
    private float jumpPower = 3f;
    private bool interationDown;

    public string currentMapName; // portal 스크립트에 있는 mapNmae 변수의 값을 저장
    public string currentScene;
    public float playerDamage;
    public bool isDie = false;

    public Vector3 sceneChangeVec;

    public Vector3 mainFeidPos;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        charCont = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        sceneChangeVec = Vector3.zero;

        if (Camera.main == null)
        {
            mainFeidPos = transform.position;
            CreateMainCamera();
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Update()
    {
        if (Camera.main == null)
        {
            CreateMainCamera();
            return;
        }
            
        if(sceneChangeVec != Vector3.zero && 
            SceneManager.GetActiveScene().name.Equals("InGame"))
        {
            if(transform.position == mainFeidPos)
                sceneChangeVec = Vector3.zero;
            transform.position = mainFeidPos;
            return;
        }

        playerDamage = Random.Range(5, 20);
        playerDamage = 100;
        PlayerMove();
        PlayerKeyEvent();
        Interation();
        SceneUpdate();
    }

    void CreateMainCamera()
    {
        Instantiate(cameraCont)
            .GetComponent<CameraController>()
            .SetTarget(transform.GetChild(1).transform);
    }

    #region 캐릭터 관련

    /// <summary>
    /// 플레이어 이동 
    /// </summary>
    void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (!charCont.isGrounded)
        {
            moveDir.y += gravity * Time.deltaTime;
        }
        else
        {
            moveSpeed = z > 0 ? 8.0f : 2.0f;
            moveDir = new Vector3(x, 0, z);
            moveDir = transform.TransformDirection(moveDir);

            // 마우스 보는방향으로 화면 전환
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }
        
        charCont.Move(moveDir * moveSpeed * Time.deltaTime);
        
        OnMovement(x, z);
    }

    /// <summary>
    /// 키 이벤트 발동
    /// </summary>
    void PlayerKeyEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerAnimation("Jump");
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlayerAnimation("normalAttack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            PlayerAnimation("skill_1_Attack");
        }
        interationDown = Input.GetButtonDown("Interation");
    }


    /// <summary>
    /// 플레이어 애니메이션
    /// </summary>
    /// <param name="animationName"></param>
    void PlayerAnimation(string animationName)
    {
        switch (animationName)
        {
            case Contents.playerJump:
                Animation("onJump");
                if (charCont.isGrounded == true)
                {
                    moveDir.y = jumpPower;
                }
                break;
            case Contents.playerNormal_Attack:
                Animation("onAttack");
                break;
            case Contents.playerSkill_1_Attack:
                Animation("onWeaponAttack");
                break;
        }
    }

    /// <summary>
    /// 캐릭터 죽음 처리
    /// </summary>
    public void PlayerDIe()
    {
        if (GameManager.Instance.statusMgr.currentHp <= 0)
        {
            isDie = true;
            Animation("onDie");
        }
    }
    #endregion

    #region 애니매이션 관련부

    public void OnMovement(float horizontal, float vertical)
    {
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
    }

    void Animation(string animName)
    {
        anim.SetTrigger(animName);
    }

    public void OnAttackCollision()
    {
        if (animationObj != null)
            animationObj.SetActive(true);
    }
    #endregion

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("item"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Shop"))
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shop"))
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            nearObject = null;
        }
    }
    void Interation()
    {
        if (interationDown == true && nearObject != null)
        {
            if (nearObject.CompareTag("Shop"))
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
            }
        }
    }
    
    // player의 현재 있는 scene 알기 위함
    public void SceneUpdate()
    {
        if (SceneManager.GetActiveScene().name != currentScene)
        {
            currentScene = SceneManager.GetActiveScene().name;
        }
    }

    public Vector3 SetPosChange
    {
        set
        {
            Debug.Log(value);
            sceneChangeVec = value;
        }
    }
}
