using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTER {
MONSTER1 = 0, MONSTER2 = 1, MONSTER3
};
public static class Contents 
{
    public static readonly int MONSTER1_MAXCOUNT = 12;
    public static readonly int MONSTER2_MAXCOUNT = 1;
    public static readonly int MONSTER3_MAXCOUNT = 2;

    public static int monster1_spawnCount = 0;
    public static int monster2_spawnCount = 0;
    public static int monster3_spawnCount = 0;

    public static string playerName = "Player";

    // 캐릭터 이벤트 애니메이션 이름
    public const string playerJump = "Jump";
    public const string playerNormal_Attack = "normalAttack";
    public const string playerSkill_1_Attack = "skill_1_Attack";
}
