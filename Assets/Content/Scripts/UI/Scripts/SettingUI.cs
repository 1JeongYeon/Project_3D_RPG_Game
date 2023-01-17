using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingUI : MonoBehaviour
{
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button loadBtn;
    [SerializeField] private Button deleteBtn;

    void Start()
    {
        saveBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.dataMgr.SaveGameData();
        });

        loadBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.dataMgr.LoadGameData();
        });

        deleteBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.dataMgr.DeleteGameData();
        });
    }
}
