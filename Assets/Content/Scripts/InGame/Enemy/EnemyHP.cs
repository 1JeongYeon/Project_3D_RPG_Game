using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 70;
    [SerializeField]
    private GameObject hpBarPrefab;
    [SerializeField]
    private Vector3 hpBarOffset = new Vector3(0, 0.5f, 0);
    private float currentHP;
    private Animator anim;
    private SkinnedMeshRenderer meshRenderer;
    private Canvas uiCanvas;
    public Image hpBarImage;
    public int damage = 5;

    private Color color;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    private void Start()
    {
        SetHpBar();
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        color = meshRenderer.material.color;
        currentHP = maxHP;
       
    }

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        hpBar.GetComponent<EnemyHPBar>().SetData(transform, hpBarOffset);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
    }

    public void TakeDamage(float damage)
    {
        if (currentHP <= 0)
            return;

        currentHP -= damage;
        hpBarImage.fillAmount = currentHP / maxHP;

        Debug.Log(damage + "체력 감소함.");
        anim.SetTrigger("onHit");
        StartCoroutine("OnHitColor");

        if(currentHP <= 0)
        {
            StartCoroutine(GetComponent<Enemy>().OnDie());
        }
    }

    private IEnumerator OnHitColor()
    {
        meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        meshRenderer.material.color = color;
    }
}
