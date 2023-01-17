using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private Button mailbtn;
    [SerializeField] private Button armorbtn;
    [SerializeField] private Button inventorybtn;
    [SerializeField] private Text nickNameText;
    [SerializeField] private Text levelText;

    [SerializeField] private GameObject mainScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
