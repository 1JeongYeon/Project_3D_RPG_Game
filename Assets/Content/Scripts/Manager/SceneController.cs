using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    void Start()
    {
        if (Instance == null)
            Instance = this;

        

        AddUIScene();
    }

    public void AddUIScene()
    {
        SceneManager.LoadScene("UI_Scene", LoadSceneMode.Additive);
    }

    public void GoMainField()
    {
        GameObject.FindWithTag("Player")
            .GetComponent<Player>()
            .SetPosChange = new Vector3(66.6f, 25f, 41.53f);
        GameManager.Instance.dataMgr.SaveGameData();
        StartCoroutine(SceneChange("InGame", true));
    }

    public void GoDungeon1()
    {
        StartCoroutine(SceneChange("Dungeon"));
    }

    public void GoDungeon2()
    {
        StartCoroutine(SceneChange("Dungeon2"));
    }

    IEnumerator SceneChange(string sceneName, bool isAddUI = false)
    {
        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene(sceneName);
        if (isAddUI)
        {
            AddUIScene();
        }
    }
}
