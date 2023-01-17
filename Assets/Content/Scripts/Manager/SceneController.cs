using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    GameObject testScene = null;
    void Start()
    {
        if (Instance == null)
            Instance = this;

        if (testScene == null)
        {
            SceneManager.LoadScene("UI_Scene", LoadSceneMode.Additive);
            if (SceneManager.GetSceneByName("UI_Scene").IsValid())
            {
                Debug.Log(this);
            }
            testScene = gameObject;
        }
    }

    public void GoMainFeld()
    {
        GameObject.FindWithTag("Player")
            .GetComponent<Player>()
            .SetPosChange = new Vector3(66.6f, 25f, 41.53f);
        StartCoroutine(SceneChange("InGame"));
    }

    public void GoDungeon1()
    {
        StartCoroutine(SceneChange("Dungeon"));
    }

    public void GoDungeon2()
    {
        StartCoroutine(SceneChange("Dungeon2"));
    }

    IEnumerator SceneChange(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
