using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    // 체력
    public int maxHp;  // 최대 체력. 유니티 에서 지정
    public int currentHp;

    // 마나
    public int maxMp;  // 최대 마나. 유니티에서 지정
    public int currentMp;

    // 방어력
    public int maxDp;  // 최대 방어력. 유니티 에서 지정
    public int currentDp;

    // 마나 증가량
    [SerializeField]
    private int mpIncreaseSpeed;

    // 마나 재회복 딜레이 시간
    [SerializeField]
    private int mpRechargeTime;
    private int currentMpRechargeTime;

    // 현재 경험치 와 최대 경험치
    public float currentExp;
    public float maxExp;

    // 플레이어 레벨
    public int level = 1;
    // 마나 감소 여부
    private bool mpUsed;

    void Update()
    {
        MPRechargeTime();
        MPRecover();
    }

    int CurrentHP
    { 
        get 
        {
            return currentHp;
        }
        set
        {
            if (value < 0)
            {
                int damage = Mathf.Abs(value);
                currentHp = currentHp - damage;

                if(currentHp <= 0)
                {
                    currentHp = 0;
                }
            }
            else
            {
                currentHp = currentHp + value;
                if (currentHp >= maxHp)
                    currentHp = maxHp;
            }
        } 
    }

    int CurrentMP
    {
        get
        {
            return currentMp;
        }
        set
        {
            if (value < 0)
            {
                currentMp = currentMp - value;
            }
            else
            {
                currentMp = currentMp + value;
            }
        }
    }

    int CurrentDP
    {
        get
        {
            return currentDp;
        }
        set
        {
            if (value < 0)
            {
                currentDp = currentDp - value;
            }
            else
            {
                currentDp = currentDp + value;
            }
        }
    }

    public void GSHP(int damage)
    {
        CurrentHP = damage;

        Debug.Log($"HP: {currentHp} ");
    }

    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < maxHp)
        {
            currentHp += _count;
        }
        else
        {
            currentHp = maxHp;
        }
    }

    public void DecreaseHP(int _count)
    {
        if (currentDp > 0)
        {
            DecreaseDP(_count); // 방어력 부터 깎임
            return;
        }
        currentHp -= _count;

        if (currentHp <= 0)
        {
            Debug.Log("HP가 0이 되었다!");
        }
    }

    

    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < maxDp)
        {
            currentDp += _count;
        }
        else
        {
            currentDp = maxDp;
        }
    }

    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
        {
            Debug.Log("방어도 증발~");
        }
    }

    // 레벨 업 까지 포함
    public void IncreaseEXP(int _count)
    {
        if (currentExp + _count < maxExp)
        {
            currentExp += _count;
        } // level up! / level up 할때 캐릭터 능력치 조정 필요, 던전 레벨제한 등등 
        else if (currentExp + _count >= maxExp)
        {
            currentExp -= maxExp;
            Player_LevelUp();
            Player_maxExp();
        }
        else
        {
            currentExp = 0;
            Player_LevelUp();
            Player_maxExp();
        }
    }

    public void DecreaseEXP(int _count)
    {
        currentExp -= _count; 

        /*if ( 플레이어가 죽었을 때 )
        {
            Debug.Log("경험치가 까입니다 ㅠㅠ");
        }*/
    }

    public void IncreaseMana(int _count)
    {
        if (currentMp + _count < maxMp)
        {
            currentMp += _count;
        }
        else
        {
            currentMp = maxMp;
        }
    }


    public void DecreaseMana(int _count)
    {
        mpUsed = true; // 마나를 썻는지 확인
        currentMpRechargeTime = 0; // 음수방지

        if (currentMp - _count > 0)
        {
            currentMp -= _count;
        }
        else
        {
            currentMp = 0;
        }
    }

    private void MPRechargeTime() // 쉬어야 마나가 차기 때문에~
    {
        if (mpUsed == true) // 마나를 썻을 때
        {
            if (currentMpRechargeTime < mpRechargeTime) // 마나를 쓴 이후 부터 마나가 재충전 될 수 있는 시작점 까지 시간을 재서 마나가 회복될수 있는지 확인
            {
                currentMpRechargeTime++; // 현mp차지 타임이 mp차지타임에 도달할 때 까지 1씩 증가시킴 도달하면 mpused = false
            }
            else
            {
                mpUsed = false;
            }
        }
    }

    private void MPRecover() // 회복 타이밍이 되었을 때
    {
        if (!mpUsed && currentMp < maxMp) // 현재 마나가 최대를 안넘는다면
        {
            currentMp += mpIncreaseSpeed; // 마나 지정한 수치만큼 프레임당 회복
        }
    }

    public int GetCurrentMP() // 현 마나 리턴
    {
        return currentMp;
    }

    public void Player_maxExp()
    {
        maxExp = level * 100;
    }

    public void Player_LevelUp()
    {
        // 추후에 기능 더 추가해도 될듯
        level++; // 레벨 증가
        UIManager.Instance.levelTxt.text = "Level : " + level;
    }
}
