using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private float speed; // 텍스트 이동속도
    [SerializeField]
    private float colorSpeed; // 투명도 변환속도
    [SerializeField]
    private float destroyTime;
    public float damage;
    private TextMeshPro tMPro;
    private Color color;

    void Start()
    {
        tMPro = GetComponent<TextMeshPro>();
        color = tMPro.color;
        tMPro.text = damage.ToString();
        Invoke("Destroy", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * colorSpeed);
        tMPro.color = color;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
