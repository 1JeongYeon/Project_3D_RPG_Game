using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataJson : MonoBehaviour
{
    [System.Serializable]
    public class ExpData
    {
        public int[] exp;
    }
    public ExpData ed = new ExpData();

    [SerializeField]
    private TextAsset expJsonTxt;
    // Start is called before the first frame update
    void Start()
    {
        ed = JsonUtility.FromJson<ExpData>(expJsonTxt.text);
    }
}
