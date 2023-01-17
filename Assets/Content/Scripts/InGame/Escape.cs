using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(SceneController.Instance != null)
            {
                SceneController.Instance.GoMainFeld();
            }
        }
    }
}
