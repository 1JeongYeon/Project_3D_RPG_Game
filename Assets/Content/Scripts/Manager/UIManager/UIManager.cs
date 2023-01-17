using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null) 
                _instance = FindObjectOfType<UIManager>();
            return _instance;
        }
    }

    // 필요한 이미지 배열
    [SerializeField]
    public Image[] images_Gauge;
    public Text levelTxt;

    // 각 상태를 대표하는 인덱스 hp 는 체력, mp 는 마나, dp는 방어력, exp는 경험치 정도
    public const int HP = 0, MP = 1, DP = 2, EXP = 3;

    private void Start()
    {
        levelTxt.text = "Level : " + GameManager.Instance.statusMgr.level.ToString();
    }

    private void Update()
    {
        GaugeUpdate();
    }

    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)GameManager.Instance.statusMgr.currentHp / GameManager.Instance.statusMgr.maxHp;
        images_Gauge[MP].fillAmount = (float)GameManager.Instance.statusMgr.currentMp / GameManager.Instance.statusMgr.maxMp;
        images_Gauge[DP].fillAmount = (float)GameManager.Instance.statusMgr.currentDp / GameManager.Instance.statusMgr.maxDp;
        images_Gauge[EXP].fillAmount = GameManager.Instance.statusMgr.currentExp / GameManager.Instance.statusMgr.maxExp;
    }
}
