using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;
    [SerializeField]
    private Button loadGameButton;
    [SerializeField]
    private Button quitButton;

    public string sceneName = "InGame";

    public static StartSceneUI instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        newGameButton.onClick.AddListener(() =>
        {
            Debug.Log("New Game");
            GameManager.Instance.dataMgr.DeleteGameData();
            SceneManager.LoadScene(sceneName);
            gameObject.SetActive(false);
        });

        loadGameButton.onClick.AddListener(() =>
        {
            Debug.Log("Load Game");
            StartCoroutine(LoadSceneCoroutine());
        });
    }

    IEnumerator LoadSceneCoroutine()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        //여기 while문에다가 operation.progress 이용해 로딩화면을 또 만들어줘도 된다.
        while (!asyncOperation.isDone) 
        {
            yield return null;
        }
        // Canvas는  비활성화
        gameObject.SetActive(false);  
    }
}
