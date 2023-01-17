using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] //직렬화
public class Data
{
    public float exp;
    public float maxExp;
    public float curHP;
    public float curMP;
    public float maxHP;
    public float maxMP;

    public int level;
    public int coin;
    
    public string saveSceneName;

    //public Vector3 savePlayerPos;
    //public Vector3 savePlayerRot;

    // 인벤토리 슬롯과 퀵슬롯들 정보도 저장할 것이기 때문에 Slot 타입의 객체를 저장하는 것도 필요한데,
    // 이런 [Serializable]가 아닌 사용자 정의 타입 객체인 Slot 타입의 객체는 이 SaveData 클래스의 멤버 변수가 될 수 없다.
    // Slot 클래스는 [Serializable]가 아니라서 직렬화로 저장될 수가 없기 때문에 List 로 따로 저장했다.
    // 배열이나 List<T>도 직렬화가 가능하다. 단, 다차원 배열, 가변 배열 등의 중첩 타입의 컨테이너는 직렬화를 할 수 없다.
    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();
    // 퀵슬롯 또한
    public List<int> quickSlotArrayNumber = new List<int>();
    public List<string> quickSlotItemName = new List<string>();
    public List<int> quickSlotItemNumber = new List<int>();
}




